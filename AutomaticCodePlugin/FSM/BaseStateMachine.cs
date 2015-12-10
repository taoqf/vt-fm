using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stateless;
using NRules;
using NRules.Fluent;
using System.Reflection;
using Victop.Server.Controls.Models;

namespace AutomaticCodePlugin.FSM
{
    /// <summary>
    /// 基础状态机
    /// </summary>
    public class BaseStateMachine
    {
        public StateMachine<string, string> FSM;
        public string currentState = "None";
        public ISession session;
        public BaseStateMachine(string groupName)
        {
            CreateRuleRepositoryGroup(groupName);
            FSM = new StateMachine<string, string>(() => currentState, s => currentState = s);
        }
        #region NRules
        private void CreateRuleRepositoryGroup(string groupName)
        {
            RuleRepository fullRepository = new RuleRepository();
            if (!string.IsNullOrEmpty(groupName))
            {
                fullRepository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(it => it.IsTagged(groupName)).To(groupName));
                var sets = fullRepository.GetRuleSets().Where(it => it.Name.Equals(groupName));
                var complier = new RuleCompiler();
                var factory = complier.Compile(sets);
                session = factory.CreateSession();
            }
            else
            {
                fullRepository.Load(x => x.From(Assembly.GetExecutingAssembly()));
                ISessionFactory factory = fullRepository.Compile();
                session = factory.CreateSession();
            }
            session.Events.FactInsertedEvent += Events_FactInsertedEvent;
            session.Events.FactRetractedEvent += Events_FactRetractedEvent;
        }
        /// <summary>
        /// 启动规则引擎
        /// </summary>
        public void Fire()
        {
            session.Fire();
        }
        public void InsertOAV(Object obj)
        {
            session.TryInsert(obj);
        }
        public void RetractOAV(Object obj)
        {
            session.TryRetract(obj);
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
