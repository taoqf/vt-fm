using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.DataMessageManager;
using Victop.Server.Controls.Models;

namespace Victop.Frame.CmptRuntime.AtomicOperation
{
    /// <summary>
    /// 自定义服务原子操作
    /// </summary>
    public class CustomServiceAtOperation
    {
        /// <summary>
        /// 服务参数集合
        /// </summary>
        Dictionary<string, Dictionary<string, object>> serviceParamList = new Dictionary<string, Dictionary<string, object>>();
        /// <summary>
        /// 数据消息管理
        /// </summary>
        DataMessageOperation messageOp = new DataMessageOperation();
        /// <summary>
        /// 状态机
        /// </summary>
        BaseStateMachine FeiDaoFSM = null;
        /// <summary>
        /// 自定义服务原子操作
        /// </summary>
        /// <param name="fsm">状态机</param>
        public CustomServiceAtOperation(BaseStateMachine fsm)
        {
            FeiDaoFSM = fsm;
        }
        /// <summary>
        /// 服务参数设置
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="paramValue">参数值</param>
        public void ServiceParamSet(string serviceName, string paramName, object paramValue)
        {
            if (!string.IsNullOrEmpty(serviceName) && !string.IsNullOrEmpty(paramName))
            {
                if (serviceParamList.ContainsKey(serviceName))
                {
                    if (serviceParamList[serviceName].ContainsKey(paramName))
                    {
                        serviceParamList[serviceName][paramName] = paramValue;
                    }
                    else
                    {
                        serviceParamList[serviceName].Add(paramName, paramValue);
                    }
                }
                else
                {
                    serviceParamList.Add(serviceName, new Dictionary<string, object>());
                    serviceParamList[serviceName].Add(paramName, paramValue);
                }
            }
        }
        /// <summary>
        /// 发送服务消息
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public void SendServiceMessage(string serviceName)
        {
            if (!string.IsNullOrEmpty(serviceName) && serviceParamList.ContainsKey(serviceName))
            {
                Dictionary<string, object> resultDic = messageOp.SendMessage(serviceName, serviceParamList[serviceName]);
                if (resultDic != null)
                {
                    OAVModel oavCode = new OAVModel(serviceName, "code", resultDic["ReplyMode"]);
                    OAVModel oavMessage = new OAVModel(serviceName, "message", resultDic["ReplyAlertMessage"]);
                    OAVModel oavContent = new OAVModel(serviceName, "content", resultDic["ReplyContent"]);
                    FeiDaoFSM.InsertFact(oavCode);
                    FeiDaoFSM.InsertFact(oavMessage);
                    FeiDaoFSM.InsertFact(oavContent);
                }
                serviceParamList.Remove(serviceName);
            }
        }
    }
}
