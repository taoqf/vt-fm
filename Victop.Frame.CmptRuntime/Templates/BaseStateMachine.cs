﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stateless;
using NRules;
using NRules.Fluent;
using System.Reflection;
using Victop.Server.Controls.Models;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 基础状态机
    /// </summary>
    public class BaseStateMachine
    {
        /// <summary>
        /// 状态机实例
        /// </summary>
        public StateMachine<string, string> FeiDaoFSM;
        /// <summary>
        /// 界面实例
        /// </summary>
        public TemplateControl MainView;
        private string currentState = "None";
        private ISession session;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="groupName">规则分组名</param>
        /// <param name="pluginAssembly">插件程序集</param>
        public BaseStateMachine(string groupName, Assembly pluginAssembly)
        {
            CreateRuleRepositoryGroup(groupName, pluginAssembly);
            FeiDaoFSM = new StateMachine<string, string>(() => currentState, s => currentState = s);
        }
        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="triggerName">动作名称</param>
        public void Do(string triggerName)
        {
            if (FeiDaoFSM.CanFire(triggerName))
            {
                FeiDaoFSM.Fire(triggerName);
            }
            else
            {
                LoggerHelper.DebugFormat("currentState:{0} can not  trigger:{1}", currentState, triggerName);
            }
        }
        #region NRules
        private void CreateRuleRepositoryGroup(string groupName, Assembly pluginAssembly)
        {
            RuleRepository fullRepository = new RuleRepository();
            if (!string.IsNullOrEmpty(groupName))
            {
                fullRepository.Load(x => x.From(pluginAssembly).Where(it => it.IsTagged(groupName) || it.IsTagged("feidao")).To(groupName));
                var sets = fullRepository.GetRuleSets().Where(it => it.Name.Equals(groupName));
                var complier = new RuleCompiler();
                var factory = complier.Compile(sets);
                session = factory.CreateSession();
            }
            else
            {
                fullRepository.Load(x => x.From(pluginAssembly));
                ISessionFactory factory = fullRepository.Compile();
                session = factory.CreateSession();
            }
            session.Events.FactInsertedEvent += Events_FactInsertedEvent;
            session.Events.FactRetractedEvent += Events_FactRetractedEvent;
        }
        /// <summary>
        /// 启动规则引擎
        /// </summary>
        public void Fire(params OAVModel[] oavs)
        {
            foreach (var item in oavs)
            {
                session.TryInsert(item);
            }
            session.Fire();
            foreach (var item in oavs)
            {
                session.TryRetract(item);
            }
        }
        private void Events_FactRetractedEvent(object sender, NRules.Diagnostics.WorkingMemoryEventArgs e)
        {
            Console.WriteLine("FactRetracted:" + e.Fact.Type.FullName);
            if (e.Fact.Type.Name.Equals("OAVModel"))
            {
                OAVModel oav = e.Fact.Value as OAVModel;
                Console.WriteLine("ObjectName:{0},AtrributeName:{1}", oav.ObjectName, oav.AtrributeName);
            }
        }

        private void Events_FactInsertedEvent(object sender, NRules.Diagnostics.WorkingMemoryEventArgs e)
        {
            Console.WriteLine("FactInserted:" + e.Fact.Type.FullName);
            if (e.Fact.Type.Name.Equals("OAVModel"))
            {
                OAVModel oav = e.Fact.Value as OAVModel;
                Console.WriteLine("ObjectName:{0},AtrributeName:{1}", oav.ObjectName, oav.AtrributeName);
            }
        }
        #endregion
    }
}
