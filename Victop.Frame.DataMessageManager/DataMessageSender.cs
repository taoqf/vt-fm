using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Victop.Frame.CoreLibrary.Enums;
using Victop.Frame.MessageManager;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.DataMessageManager
{
    /// <summary>
    /// 数据消息发送者
    /// </summary>
    internal class DataMessageSender
    {
        /// <summary>
        /// 应答消息
        /// </summary>
        private Dictionary<string, object> replyMessageInfo;
        /// <summary>
        /// 同步发送消息
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="messageContent">消息体</param>
        /// <param name="waiteTime">等待时间</param>
        /// <returns></returns>
        public Dictionary<string, object> SendMessage(string messageType, Dictionary<string, object> messageContent, int waiteTime = 16)
        {
            Dictionary<string, object> returnDic;
            new PluginMessage().SendMessage(messageType, messageContent, new WaitCallback(MessageBack), DataFormEnum.JSON);
            if (waiteTime > 0)
            {
                bool flag = false;
                UserTimeThread timeoutThread = new UserTimeThread((int)waiteTime);
                Thread thread = new Thread(new ThreadStart(timeoutThread.Sleep));
                thread.Start();
                while (!timeoutThread.Done)
                {
                    if (replyMessageInfo != null)
                    {
                        returnDic = replyMessageInfo;
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
            return replyMessageInfo;
        }
        private void MessageBack(object message)
        {
            replyMessageInfo = JsonHelper.ToObject<Dictionary<string, object>>(message.ToString());
        }
    }
}
