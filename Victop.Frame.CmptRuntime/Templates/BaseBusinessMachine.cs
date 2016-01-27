﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 基础业务引擎
    /// </summary>
    public class BaseBusinessMachine
    {
        /// <summary>
        /// 界面实例
        /// </summary>
        protected TemplateControl MainView;
        private object businessSope;
        private string GroupName;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="groupName">规则分组名</param>
        /// <param name="mainView">主应用程序</param>
        public BaseBusinessMachine(string groupName, TemplateControl mainView)
        {
            MainView = mainView;
            GroupName = groupName;
            string pvdPath = string.Format("{0}.PVD.{1}.json", mainView.GetType().Assembly.GetName().Name, groupName);
            Stream pvdStream = mainView.GetType().Assembly.GetManifestResourceStream(pvdPath);
            if (pvdStream != null)
            {
                string pvdStr = FileHelper.ReadText(pvdStream);
                MainView.InitVictopUserControl(pvdStr);
            }
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        public void Init()
        {
            string fsmString = string.Empty;
            string ruleString = string.Empty;
            string fullName = string.Format("{0}.FSM.{1}.json", MainView.GetType().Assembly.GetName().Name, GroupName);
            Stream manifestResourceStream = MainView.GetType().Assembly.GetManifestResourceStream(fullName);
            if (manifestResourceStream != null)
            {
                fsmString = FileHelper.ReadText(manifestResourceStream);
            }
            fullName = string.Format("{0}.Rules.{1}.drl", MainView.GetType().Assembly.GetName().Name, GroupName);
            manifestResourceStream = MainView.GetType().Assembly.GetManifestResourceStream(fullName);
            if (manifestResourceStream != null)
            {
                ruleString = FileHelper.ReadText(manifestResourceStream);
            }
            businessSope = MainView.BuiltBrowser.InvokeScript("send_msg", "init", null, ruleString, fsmString);
        }
        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="triggerName">动作名称</param>
        /// <param name="triggerSource">动作触发源</param>
        public void Do(string triggerName, object triggerSource)
        {
            if (MainView.BuiltBrowser != null)
            {
                MainView.BuiltBrowser.InvokeScript("send_msg", "fsm", businessSope, triggerName, triggerSource);
            }
        }
    }
}