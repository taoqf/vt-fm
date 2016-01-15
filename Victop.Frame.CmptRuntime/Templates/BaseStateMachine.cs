using System;
using Stateless;
using System.Reflection;
using Victop.Frame.PublicLib.Helpers;
using System.Windows;
using System.IO;
using org.drools.dotnet;
using org.drools.dotnet.compiler;
using org.drools.dotnet.rule;
using org.drools.@base;
using System.Collections.Generic;
using Victop.Server.Controls.Models;

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
        private string currentState = "none";
        private StateTransitionModel stateModel;
        private org.drools.FactHandle stateHandle;
        private StateDefineModel stateDefModel;
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
            string pvdPath = string.Format("{0}.PVD.{1}.json", pluginAssembly.GetName().Name, groupName);
            Stream pvdStream = pluginAssembly.GetManifestResourceStream(pvdPath);
            if (pvdStream != null)
            {
                string pvdStr = FileHelper.ReadText(pvdStream);
                MainView.InitVictopUserControl(pvdStr);
            }
            CreateDroolsSession(groupName, pluginAssembly);
            CreateStateDefin(groupName, pluginAssembly);
            stateModel = new StateTransitionModel() { ActionName = "none", ActionSourceElement = mainView, MainView = mainView };
            stateHandle = dSession.assertObject(stateModel);
            FeiDaoFSM = new StateMachine<string, string>(() => currentState, s => currentState = s);
            InitStateConfig();
            FeiDaoFSM.OnTransitioned((x) => OnTransitioned(x));
            Do("beforeinit", MainView);
        }

        private void InitStateConfig()
        {
            if (stateDefModel != null)
            {
                List<string> configFromList = new List<string>();
                foreach (var item in stateDefModel.DefTransitions)
                {
                    if (item.InfoFrom.Equals(item.InfoTo))
                    {
                        FeiDaoFSM.Configure(item.InfoFrom)
                            .PermitReentry(item.InfoName);
                    }
                    else
                    {
                        FeiDaoFSM.Configure(item.InfoFrom)
                            .Permit(item.InfoName, item.InfoTo);
                    }
                    if (!configFromList.Contains(item.InfoFrom))
                    {
                        FeiDaoFSM.Configure(item.InfoFrom)
                            .OnEntry((x) => OnFeiDaoEntry(x))
                            .OnExit((x) => OnFeiDaoExit(x));
                        configFromList.Add(item.InfoFrom);
                    }
                }
            }
        }

        private void OnTransitioned(StateMachine<string, string>.Transition x)
        {
            stateModel.ActionDestination = x.Destination;
            stateModel.ActionSource = x.Source;
            stateModel.ActionTrigger = x.Trigger;
            dSession.modifyObject(stateHandle, stateModel);
            LoggerHelper.InfoFormat("{0} OnTransitioned：Destination:{1},Source:{2},Trigger:{3}", MainView.GetType().FullName, x.Destination, x.Source, x.Trigger);
            if (stateDefModel.DefEvents[x.Trigger] != null && stateDefModel.DefEvents[x.Trigger].Count > 0)
            {
                ExecuteRule(stateDefModel.DefEvents[x.Trigger]);
            }
        }
        private bool OnFeiDaoGuard(string triggerName)
        {
            StateTransitionInfoModel InfoModel = stateDefModel.DefTransitions.Find(it => it.InfoName.Equals(triggerName) && it.InfoFrom.Equals(currentState));
            if (InfoModel != null)
            {
                if (stateDefModel.DefStates[InfoModel.InfoTo].StateGuard != null && stateDefModel.DefStates[InfoModel.InfoTo].StateGuard.Count > 0)
                {
                    ExecuteRule(stateDefModel.DefStates[InfoModel.InfoTo].StateGuard);
                    return stateModel.ActionGuard;
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 状态离开
        /// </summary>
        /// <param name="x"></param>
        protected void OnFeiDaoExit(StateMachine<string, string>.Transition x)
        {
            LoggerHelper.InfoFormat("{0} OnFeiDaoExit：Destination:{1},Source:{2},Trigger:{3}", MainView.GetType().FullName, x.Destination, x.Source, x.Trigger);
            if (stateDefModel.DefStates[x.Source].StateExit != null && stateDefModel.DefStates[x.Source].StateExit.Count > 0)
            {
                ExecuteRule(stateDefModel.DefStates[x.Source].StateExit);
            }
        }
        /// <summary>
        /// 状态进入
        /// </summary>
        /// <param name="x"></param>
        protected void OnFeiDaoEntry(StateMachine<string, string>.Transition x)
        {
            LoggerHelper.InfoFormat("{0} OnFeiDaoEntry：Destination:{1},Source:{2},Trigger:{3}", MainView.GetType().FullName, x.Destination, x.Source, x.Trigger);
            if (stateDefModel.DefStates[x.Destination].StateEntry != null && stateDefModel.DefStates[x.Destination].StateEntry.Count > 0)
            {
                ExecuteRule(stateDefModel.DefStates[x.Destination].StateEntry);
            }
            if (stateDefModel.DefStates[x.Destination].StateDo != null && stateDefModel.DefStates[x.Destination].StateDo.Count > 0)
            {
                ExecuteRule(stateDefModel.DefStates[x.Destination].StateDo);
            }
            if (stateDefModel.DefStates[x.Destination].StateDone != null && stateDefModel.DefStates[x.Destination].StateDone.Count > 0)
            {
                ExecuteRule(stateDefModel.DefStates[x.Destination].StateDone);
            }
        }

        private void ExecuteRule(List<string> ListStr)
        {
            foreach (var item in ListStr)
            {
                dSession.setFocus(item);
                dSession.fireAllRules();
            }
        }
        /// <summary>
        /// 设置分组信息
        /// </summary>
        /// <param name="groupName"></param>
        public void SetFocus(string groupName)
        {
            dSession.setFocus(groupName);
        }
        /// <summary>
        /// 插入事实
        /// </summary>
        /// <param name="oav"></param>
        public void InsertFact(OAVModel oav)
        {
            dSession.assertObject(oav);
        }
        /// <summary>
        /// 修改事实
        /// </summary>
        /// <param name="oav"></param>
        public void UpdateFact(OAVModel oav)
        {
            dSession.modifyObject(dSession.getFactHandle(oav), oav);
        }
        /// <summary>
        /// 移除事实
        /// </summary>
        /// <param name="oav"></param>
        public void RemoveFact(OAVModel oav)
        {
            dSession.retractObject(dSession.getFactHandle(oav));
        }

        /// <summary>
        /// 执行动作
        /// </summary>
        /// <param name="triggerName">动作名称</param>
        /// <param name="triggerSource">动作触发源</param>
        public void Do(string triggerName, object triggerSource)
        {
            if (FeiDaoFSM.CanFire(triggerName) && OnFeiDaoGuard(triggerName))
            {
                stateModel.ActionName = triggerName;
                stateModel.ActionSourceElement = triggerSource as FrameworkElement;
                stateModel.ActionTrigger = triggerName;
                dSession.modifyObject(stateHandle, stateModel);
                FeiDaoFSM.Fire(triggerName);
            }
            else
            {
                LoggerHelper.DebugFormat("{0} currentState:{1} can not  trigger:{2}", MainView.GetType().FullName, currentState, triggerName);
            }
        }
        #region 状态定义文件管理
        private void CreateStateDefin(string defFileName, Assembly pluginAssembly)
        {
            string fullName = string.Format("{0}.FSM.{1}.json", pluginAssembly.GetName().Name, defFileName);
            Stream manifestResourceStream = pluginAssembly.GetManifestResourceStream(fullName);
            if (manifestResourceStream != null)
            {
                StreamReader reader = new StreamReader(manifestResourceStream);
                string temp = reader.ReadToEnd();
                stateDefModel = JsonHelper.ToObject<StateDefineModel>(temp);
            }
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
