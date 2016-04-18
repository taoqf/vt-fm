using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
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
        Dictionary<string, List<dynamic>> serviceOAVList = new Dictionary<string, List<dynamic>>();
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
        /// <param name="waiteTime">等待时间</param>
        public void SendServiceMessage(string serviceName, int waiteTime = 60)
        {
            if (!string.IsNullOrEmpty(serviceName) && serviceParamList.ContainsKey(serviceName))
            {
                Dictionary<string, object> resultDic = messageOp.SendSyncMessage("js_custom_func", serviceParamList[serviceName], waiteTime);
                if (resultDic != null)
                {
                    //已调用过先清除OAV,防止重复添加
                    if (serviceOAVList.ContainsKey(serviceName))
                    {
                        try
                        {
                            foreach (dynamic oav in serviceOAVList[serviceName])
                            {
                                MainView.FeiDaoMachine.RemoveFact(oav);
                            }
                            serviceOAVList[serviceName].Clear();
                        }
                        catch (Exception ex)
                        {
                            serviceOAVList[serviceName].Clear();
                            LoggerHelper.Error("删除服务OAV失败!服务为：" + serviceName + "。错误原因" + ex.ToString());
                        }
                    }
                    else
                    {
                        serviceOAVList.Add(serviceName, new List<dynamic>());
                    }
                    dynamic oavCode = MainView.FeiDaoMachine.InsertFact(serviceName, "code", resultDic["ReplyMode"]);
                    dynamic oavMessage = MainView.FeiDaoMachine.InsertFact(serviceName, "message", resultDic["ReplyAlertMessage"]);
                    dynamic oavContent = MainView.FeiDaoMachine.InsertFact(serviceName, "content", resultDic["ReplyContent"]);
                    //oav存入集合
                    serviceOAVList[serviceName].Add(oavCode);
                    serviceOAVList[serviceName].Add(oavMessage);
                    serviceOAVList[serviceName].Add(oavContent);
                }
                serviceParamList.Remove(serviceName);
            }
        }

        /// <summary>
        /// 发送http请求
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        public void SendHttpRequest(string requestUrl,object oav)
        {
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(requestUrl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "GET"; //请求方式GET或POST
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Authorization", "Basic YWRtaW46YWRtaW4=");

                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, Encoding.UTF8);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                dynamic o1= oav;
                o1.v = content;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "服务连接");
            }
        }
    }
}
