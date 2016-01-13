using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;

namespace Victop.Frame.CmptRuntime.AtomicOperation
{
    /// <summary>
    /// 系统原子操作
    /// </summary>
    public class SystemAtOperation
    {
        private TemplateControl MainView;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainView"></param>
        public SystemAtOperation(TemplateControl mainView)
        {
            MainView = mainView;
        }
        /// <summary>
        /// 设置分组
        /// </summary>
        /// <param name="groupName">分组信息</param>
        public void SetFocus(string groupName)
        {
            MainView.FeiDaoFSM.SetFocus(groupName);
        }
        /// <summary>
        /// 插入事实
        /// </summary>
        /// <param name="oav">oav事实</param>
        public void InsertFact(OAVModel oav)
        {
            MainView.FeiDaoFSM.InsertFact(oav);
        }
        /// <summary>
        /// 移除事实
        /// </summary>
        /// <param name="oav">oav事实</param>
        public void RemoveFact(OAVModel oav)
        {
            MainView.FeiDaoFSM.RemoveFact(oav);
        }
        /// <summary>
        /// 修改事实
        /// </summary>
        /// <param name="oav">oav事实</param>
        public void UpdateFact(OAVModel oav)
        {
            if (oav.AtrributeValue != null)
            MainView.FeiDaoFSM.UpdateFact(oav);
        }
        /// <summary>
        /// 系统输出
        /// </summary>
        /// <param name="consoleText">输出内容</param>
        public void SysConsole(string consoleText)
        {
            Console.WriteLine(consoleText);
        }
        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="content">输出内容</param>
        public void SysFeiDaoLog(string content)
        {
            LoggerHelper.Info(content);
        }
        /// <summary>
        /// 设置警戒条件
        /// </summary>
        /// <param name="se">状体转移实体</param>
        /// <param name="oav">oav警戒值</param>
        /// <param name="oavmsg">oav消息内容</param>
        public void SetActionGuard(StateTransitionModel se, OAVModel oav, OAVModel oavmsg)
        {
            if (oav.AtrributeValue != null && (bool)oav.AtrributeValue)
                se.ActionGuard = true;
            else
            {
                string msg = oavmsg.AtrributeValue != null ? oavmsg.AtrributeValue.ToString() : "条件不满足！";
                VicMessageBoxNormal.Show(msg);
                se.ActionGuard = false;
            }
        }
        /// <summary>
        /// 执行页面动作
        /// </summary>
        /// <param name="pageTrigger">动作名称</param>
        /// <param name="paramInfo">事件触发元素</param>
        public void ExcutePageTrigger(string pageTrigger, object paramInfo)
        {
            MainView.ParentControl.FeiDaoFSM.Do(pageTrigger, paramInfo);
        }
        /// <summary>
        /// 执行组件动作
        /// </summary>
        /// <param name="compntName">组件名</param>
        /// <param name="compntTrigger">动作名称</param>
        /// <param name="paramInfo">事件触发元素</param>
        public void ExcuteComponentTrigger(string compntName, string compntTrigger, object paramInfo)
        {
            TemplateControl tc = MainView.FindName(compntName) as TemplateControl;
            if (tc != null)
                tc.FeiDaoFSM.Do(compntTrigger, paramInfo);
        }
        /// <summary>
        /// 获取页面参数值
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">oav载体</param>
        public void ParamsPageGet(string paramName, OAVModel oav)
        {
            if (MainView.ParentControl.ParamDict.ContainsKey(paramName))
            {
                oav.AtrributeValue = MainView.ParentControl.ParamDict[paramName];
            }
        }
        /// <summary>
        /// 获取Dictionary中参数值
        /// </summary>
        /// <param name="oavDic">存储dic类型的oav</param>
        /// <param name="paramName">参数名</param>
        /// <param name="oav">接收oav</param>
        public void ParamsGetByDictionary(OAVModel oavDic, string paramName, OAVModel oav)
        {
            Dictionary<string, object> dic = oavDic.AtrributeValue as Dictionary<string, object>;
            if (dic != null && dic.ContainsKey(paramName))
            {
                oav.AtrributeValue = dic[paramName];
            }
        }
    }
}
