using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.DataMessageManager;
using Victop.Server.Controls.Models;
using Victop.Frame.PublicLib.Helpers;

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
        /// 服务OAV集合
        /// </summary>
        Dictionary<string, List<OAVModel>> serviceOAVList = new Dictionary<string, List<OAVModel>>();
        /// <summary>
        /// 数据消息管理
        /// </summary>
        DataMessageOperation messageOp = new DataMessageOperation();
        /// <summary>
        /// 状态机
        /// </summary>
        TemplateControl MainView;
        /// <summary>
        /// 自定义服务原子操作
        /// </summary>
        /// <param name="mainView">实例</param>
        public CustomServiceAtOperation(TemplateControl mainView)
        {
            MainView = mainView;
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
                    //已调用过先清除OAV,防止重复添加
                    if (serviceOAVList.ContainsKey(serviceName))
                    {
                        try
                        {
                            foreach (OAVModel oav in serviceOAVList[serviceName])
                            {
                                MainView.FeiDaoFSM.RemoveFact(oav);
                            }
                            serviceOAVList[serviceName].Clear();
                        }
                        catch (Exception ex)
                        {
                            serviceOAVList[serviceName].Clear();
                            LoggerHelper.Error("删除服务OAV失败!服务为：" + serviceName +"。错误原因"+ ex.ToString());
                        }
                    }
                    else
                    {
                        serviceOAVList.Add(serviceName, new List<OAVModel>());
                    }
                    OAVModel oavCode = new OAVModel(serviceName, "code", resultDic["ReplyMode"]);
                    OAVModel oavMessage = new OAVModel(serviceName, "message", resultDic["ReplyAlertMessage"]);
                    OAVModel oavContent = new OAVModel(serviceName, "content", resultDic["ReplyContent"]);
                    MainView.FeiDaoFSM.InsertFact(oavCode);
                    MainView.FeiDaoFSM.InsertFact(oavMessage);
                    MainView.FeiDaoFSM.InsertFact(oavContent);
                    //oav存入集合
                    serviceOAVList[serviceName].Add(oavCode);
                    serviceOAVList[serviceName].Add(oavMessage);
                    serviceOAVList[serviceName].Add(oavContent);
                    
                }
                serviceParamList.Remove(serviceName);
            }
        }
    }
}
