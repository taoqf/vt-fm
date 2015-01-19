using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Victop.Frame.CoreLibrary;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls;
namespace Victop.Frame.Adapter
{
    public class NotificationPoolManager
    {
        private static ObjectPool<string, ICollection<string>> notificationPool;
        /// <summary>
        /// 通知池
        /// </summary>
        public static ObjectPool<string, ICollection<string>> NotificationPool
        {
            get
            {
                if (notificationPool == null)
                {
                    notificationPool = new ObjectPool<string, ICollection<string>>();
                }
                return notificationPool;
            }
        }
        /// <summary>
        /// 保存消息数据
        /// </summary>
        public virtual bool SaveMessageData(RequestMessage requestMessage)
        {
            bool flag = false;
            try
            {
                if (requestMessage != null)
                {
                    LinkedList<String> notificationList = (LinkedList<String>)NotificationPool.Get(requestMessage.ToId);
                    if (notificationList == null)
                    {
                        notificationList = new LinkedList<String>();
                    }
                    string result = JsonHelper.ReadJsonString(requestMessage.MessageContent, "result");
                    notificationList.AddLast(result);
                    ICollection<String> iCollection = NotificationPool.Add(requestMessage.ToId, notificationList);
                    if (iCollection != null)
                    {
                        flag = true;
                        ActivePluginManager pluginManager = new ActivePluginManager();
                        ActivePluginInfo pluginInfo = new ActivePluginInfo();
                        pluginInfo = pluginManager.GetPlugin(pluginInfo);
                        if (pluginInfo != null)
                        {
                            IPlugin pluginInstance = pluginInfo.PluginInstance as IPlugin;
                            pluginInstance.Init();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 获取消息数据
        /// </summary>
        public virtual LinkedList<String> GetMessageData(string messageId)
        {
            LinkedList<String> message = new LinkedList<String>();
            try
            {
                if (!string.IsNullOrEmpty(messageId))
                {
                    message = (LinkedList<String>)NotificationPool.Get(messageId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return message;
        }

        /// <summary>
        /// 移除消息数据
        /// </summary>
        public virtual bool RemoveMessageData(string messageId)
        {
            bool flag = false;
            try
            {
                if (!string.IsNullOrEmpty(messageId))
                {
                    LinkedList<String> message = (LinkedList<String>)NotificationPool.Remove(messageId);
                    if (message != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        /// <summary>
        /// 根据消息id创建消息池,用于存储通信器返回数据
        /// </summary>
        public virtual bool CreateMessagePool(string messageId)
        {
            bool flag = false;
            try
            {
                if (!string.IsNullOrEmpty(messageId))
                {
                    LinkedList<String> message = null;
                    message = (LinkedList<String>)NotificationPool.Get(messageId);
                    if (message != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }
        /// <summary>
        /// 清理通知缓存池
        /// </summary>
        public virtual void ClearNotificationPool()
        {
            while (true)
            {
                try
                {
                    foreach (string id in NotificationPool.PoolMap.Keys)
                    {
                        DateTime lastDate = NotificationPool.GetLastDate(id);
                        if (null == lastDate)
                        {
                            continue;
                        }
                        DateTime nowDate = DateTime.Now;
                        long timeout = (nowDate.Ticks - lastDate.Ticks) / 10000;
                        if (TimeSet.CLEAR_TIMEOUT < timeout)
                        {
                            notificationPool.Remove(id);
                        }
                        else
                        {
                            break;
                        }
                        try
                        {
                            Thread.Sleep((int)TimeSet.CLEAR_SLEEP_TIME);
                        }
                        catch (ThreadInterruptedException ex)
                        {
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
