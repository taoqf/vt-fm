﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.MessageManager
{
    using PublicLib.Managers;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Victop.Frame.Connection;
    using Victop.Frame.CoreLibrary.Enums;
    using Victop.Frame.CoreLibrary.Models;

    /// <summary>
    /// 插件消息线程管理 【线程池】
    /// </summary>
    /// <remarks>插件消息线程管理</remarks>
    public class PluginMessageThreadManager
	{
        private static PluginMessageThreadManager instance = null;
  
        public static PluginMessageThreadManager GetInstance()
        {
            if (instance == null)
            {
                instance = new PluginMessageThreadManager();
                //开启线程判断是否超时。
                PluginMessageManager pluginMessageManager = new PluginMessageManager();
                Thread thread = new Thread(new ThreadStart(pluginMessageManager.CheckPluginMessageValid));
                thread.IsBackground = true;
                //if (!ConfigManager.GetAttributeOfNodeByName("Client", "Debug").Equals("1"))
                //{
                //    thread.Start();
                //}
            }
            return instance;
        }
    
		/// <summary>
		/// 线程工作
		/// </summary>
		public virtual void DoWork(RequestMessage messageInfo)
		{
            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("messageInfo", messageInfo);
            ThreadPool.QueueUserWorkItem(new WaitCallback(SendMessage), paramDic);
		}

        /// <summary>
        /// 请求消息体发送
        /// </summary>
        /// <param name="messageInfo">请求消息体</param>
        private void SendMessage(object paramDic)
        {
            Dictionary<string, object> MessageDic = (Dictionary<string, object>)paramDic;
            RequestMessageSend messageSend = new RequestMessageSend();
            RequestMessage message = (RequestMessage)(MessageDic["messageInfo"]);
            if (string.IsNullOrEmpty(message.MessageType)||string.IsNullOrEmpty(message.MessageContent))
            {
                return;
            }
            messageSend.SendMessage(message);
        }
	}
}

