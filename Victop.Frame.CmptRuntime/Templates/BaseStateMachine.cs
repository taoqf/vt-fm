using System;
using System.Linq;
using Stateless;
using NRules;
using NRules.Fluent;
using System.Reflection;
using Victop.Server.Controls.Models;
using Victop.Frame.PublicLib.Helpers;
using System.Windows;
using System.IO;
using org.drools.dotnet;
using org.drools.dotnet.compiler;
using org.drools.dotnet.rule;
using org.drools.@base;

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
        protected TemplateControl MainView;
        private string currentState = "None";
        private StateTransitionModel stateModel;
        private org.drools.FactHandle stateHandle;
        /// <summary>
        /// NRules ：Session
        /// </summary>
        private ISession nSession;
        /// <summary>
        /// Drools.net :Session
        /// </summary>
        private WorkingMemory dSession;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="groupName">规则分组名</param>
        /// <param name="pluginAssembly">插件程序集</param>
        /// <param name="mainView">主应用程序</param>
        public BaseStateMachine(string groupName, Assembly pluginAssembly, TemplateControl mainView)
        {
            MainView = mainView;
            //CreateRuleRepositoryGroup(groupName, pluginAssembly);
            CreateDroolsSession(groupName, pluginAssembly);
            stateModel = new StateTransitionModel() { ActionName = "None", ActionSourceElement = mainView, MainView = mainView };
            //nSession.Insert(stateModel);
            stateHandle = dSession.assertObject(stateModel);
            FeiDaoFSM = new StateMachine<string, string>(() => currentState, s => currentState = s);
            FeiDaoFSM.OnTransitioned((x) => OnTransitioned(x));

        }

        private void OnTransitioned(StateMachine<string, string>.Transition x)
        {
            stateModel.ActionDestination = "中文";
            stateModel.ActionSource = x.Source;
            stateModel.ActionTrigger = x.Trigger;
            dSession.modifyObject(stateHandle, stateModel);
            //Console.WriteLine("{0}:OnTransitioned",MainView.Name);
            //Console.WriteLine("CurrentState:{0}", FeiDaoFSM.State);
        }
        /// <summary>
        /// 状态离开
        /// </summary>
        /// <param name="x"></param>
        protected void OnFeidaoExit(StateMachine<string, string>.Transition x)
        {

        }
        /// <summary>
        /// 状态进入
        /// </summary>
        /// <param name="x"></param>
        protected void OnFeidaoEntry(StateMachine<string, string>.Transition x)
        {
            dSession.fireAllRules();
        }

        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="triggerName">动作名称</param>
        /// <param name="triggerSource">动作触发源</param>
        public void Do(string triggerName, object triggerSource)
        {
            if (FeiDaoFSM.CanFire(triggerName))
            {
                stateModel.ActionName = triggerName;
                stateModel.ActionSourceElement = triggerSource as FrameworkElement;
                stateModel.ActionTrigger = triggerName;
                //nSession.Update(stateModel);
                dSession.modifyObject(stateHandle, stateModel);
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
                nSession = factory.CreateSession();
            }
            else
            {
                fullRepository.Load(x => x.From(pluginAssembly));
                ISessionFactory factory = fullRepository.Compile();
                nSession = factory.CreateSession();
            }
            nSession.Events.FactInsertedEvent += Events_FactInsertedEvent;
            nSession.Events.FactRetractedEvent += Events_FactRetractedEvent;
            nSession.Events.RuleFiredEvent += Events_RuleFiredEvent;
        }
        /// <summary>
        /// 启动规则引擎
        /// </summary>
        public void Fire(params OAVModel[] oavs)
        {
            foreach (var item in oavs)
            {
                //nSession.TryInsert(item);
                dSession.assertObject(item);
            }
            //nSession.Fire();
            var filter = new RuleNameEqualsAgendaFilter("group");
            dSession.fireAllRules(filter);
            foreach (var item in oavs)
            {
                //nSession.TryRetract(item);
                dSession.retractObject(dSession.getFactHandle(item));
            }
        }
        private void Events_FactRetractedEvent(object sender, NRules.Diagnostics.WorkingMemoryEventArgs e)
        {
            Console.WriteLine("移除事实:" + e.Fact.Type.FullName);
            if (e.Fact.Type.Name.Equals("OAVModel"))
            {
                OAVModel oav = e.Fact.Value as OAVModel;
                Console.WriteLine("ObjectName:{0},AtrributeName:{1}", oav.ObjectName, oav.AtrributeName);
            }
        }

        private void Events_FactInsertedEvent(object sender, NRules.Diagnostics.WorkingMemoryEventArgs e)
        {
            Console.WriteLine("插入事实:" + e.Fact.Type.FullName);
            if (e.Fact.Type.Name.Equals("OAVModel"))
            {
                OAVModel oav = e.Fact.Value as OAVModel;
                Console.WriteLine("ObjectName:{0},AtrributeName:{1}", oav.ObjectName, oav.AtrributeName);
            }
        }
        private void Events_RuleFiredEvent(object sender, NRules.Diagnostics.AgendaEventArgs e)
        {
            Console.WriteLine("规则执行后:" + e.Rule.Name);
        }
        #endregion

        #region Drools.net
        private void CreateDroolsSession(string ruleFileName, Assembly pluginAssembly)
        {
            string fullName = string.Format("{0}.Rules.{1}.drl", pluginAssembly.GetName().Name, ruleFileName);
            Stream manifestResourceStream = pluginAssembly.GetManifestResourceStream(fullName);
            PackageBuilder packageBuilder = new PackageBuilder();
            packageBuilder.AddPackageFromDrl(manifestResourceStream);
            Package package = packageBuilder.GetPackage();
            RuleBase ruleBase = RuleBaseFactory.NewRuleBase();
            ruleBase.AddPackage(package);
            dSession = ruleBase.NewWorkingMemory();
            dSession.setGlobal("FeiDao", new FeiDaoOperation(MainView));
        }
        #endregion
    }
}
