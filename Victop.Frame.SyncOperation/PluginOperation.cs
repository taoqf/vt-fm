using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Victop.Frame.DataChannel;
using Victop.Frame.MessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls;
using Victop.Server.Controls.Models;

namespace Victop.Frame.SyncOperation
{
    /// <summary>
    /// 插件同步操作类
    /// </summary>
    public class PluginOperation
    {
        private PluginModel PluginInfo;
        /// <summary>
        /// 启动窗口插件
        /// </summary>
        /// <param name="PluginName">插件名称</param>
        /// <param name="paramDic">参数键值对</param>
        /// <param name="waitTime">同步等待时间(秒)</param>
        /// <returns></returns>
        public PluginModel StratPlugin(string PluginName, Dictionary<string, object> paramDic = null, int waitTime = 15)
        {
            PluginModel returnModel = null;
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "PluginService.PluginRun");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("PluginName", PluginName);
            contentDic.Add("PluginPath", "");
            contentDic.Add("PluginParam", JsonHelper.ToJson(paramDic));
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            new PluginMessage().SendMessage(Guid.NewGuid().ToString(), JsonHelper.ToJson(messageDic),new WaitCallback(PluginShow));
            if (waitTime > 0)
            {
                bool flag = false;
                UserTimeThread timeoutThread = new UserTimeThread((int)waitTime);
                Thread thread = new Thread(new ThreadStart(timeoutThread.Sleep));
                thread.Start();
                while (!timeoutThread.Done)
                {
                    if (PluginInfo != null)
                    {
                        returnModel = PluginInfo;
                        lock (this)
                        {
                            timeoutThread.Stop();
                        }
                        flag = true;
                        break;
                    }
                    try
                    {
                        Thread.Sleep(1);
                    }
                    catch (ThreadInterruptedException)
                    {
                        lock (this)
                        {
                            timeoutThread.Stop();
                        }
                    }
                }
                if (!flag)
                {
                }
            }
            return returnModel;
        }
        /// <summary>
        /// 释放插件
        /// </summary>
        /// <param name="ObjectId"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        public bool StopPlugin(string ObjectId, int waitTime = 15)
        {
            bool result = false;
            MessageOperation messageOp = new MessageOperation();
            string messageType = "PluginService.PluginStop";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("ObjectId", ObjectId);
            Dictionary<string, object> resultDic = messageOp.SendMessage(messageType, contentDic, waitTime);
            if (resultDic != null)
            {
                result = resultDic["ReplyMode"].ToString().Equals("0") ? false : true;
            }
            return result;
        }
        /// <summary>
        /// 获取活动插件列表
        /// </summary>
        /// <returns></returns>
        public List<PluginModel> GetActivePluginList()
        {
            List<PluginModel> PluginList = new List<PluginModel>();
            DataOperation PluginOper = new DataOperation();
            List<Dictionary<string, object>> pluginInfo = PluginOper.GetPluginInfo();
            foreach (var item in pluginInfo)
            {
                PluginModel pluginModel = new PluginModel();
                pluginModel.PluginInterface = item["IPlugin"] as IPlugin;
                pluginModel.AppId = item["AppId"].ToString();
                pluginModel.ObjectId = item["ObjectId"].ToString();
                PluginList.Add(pluginModel);
            }
            return PluginList;
        }

        private void PluginShow(object message)
        {
            if (!JsonHelper.ReadJsonString(message.ToString(), "ReplyMode").Equals("0"))
            {
                string messageId = JsonHelper.ReadJsonString(message.ToString(), "MessageId");
                DataOperation PluginOper = new DataOperation();
                List<Dictionary<string, object>> pluginList = PluginOper.GetPluginInfo();
                foreach (var item in pluginList)
                {
                    if (item["ObjectId"].ToString().Equals(messageId))
                    {
                        PluginModel pluginModel = new PluginModel();
                        pluginModel.PluginInterface = item["IPlugin"] as IPlugin;
                        pluginModel.AppId = item["AppId"].ToString();
                        pluginModel.ObjectId = item["ObjectId"].ToString();
                        PluginInfo = pluginModel;
                        break;
                    }
                }
            }
            else
            {
                PluginInfo = new PluginModel() {
                    ErrorMsg = JsonHelper.ReadJsonString(message.ToString(), "ReplyAlertMessage")
                };
            }
        }
    }
}
