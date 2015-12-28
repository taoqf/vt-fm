using System;
using Stateless;
using System.Reflection;
using Victop.Frame.PublicLib.Helpers;
using System.Windows;
using System.IO;
using org.drools.dotnet;
using org.drools.dotnet.compiler;
using org.drools.dotnet.rule;

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
            CreateDroolsSession(groupName, pluginAssembly);
            stateModel = new StateTransitionModel() { ActionName = "None", ActionSourceElement = mainView, MainView = mainView };
            //nSession.Insert(stateModel);
            stateHandle = dSession.assertObject(stateModel);
            FeiDaoFSM = new StateMachine<string, string>(() => currentState, s => currentState = s);
            FeiDaoFSM.OnTransitioned((x) => OnTransitioned(x));

        }

        private void OnTransitioned(StateMachine<string, string>.Transition x)
        {
            stateModel.ActionDestination = x.Destination;
            stateModel.ActionSource = x.Source;
            stateModel.ActionTrigger = x.Trigger;
            dSession.modifyObject(stateHandle, stateModel);
            Console.WriteLine("{0}:OnTransitioned",MainView.Name);
            Console.WriteLine("CurrentState:{0}", FeiDaoFSM.State);
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
