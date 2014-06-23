using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Victop.Frame.CoreLibrary.Models;

namespace Victop.Frame.Adapter
{
    public class KickoutPoolManager
    {
        private static ObjectPool<string, RequestMessage> kickoutPool;
        /// <summary>
        /// 特殊消息缓存池(中断消息)
        /// </summary>
        public static ObjectPool<string, RequestMessage> KickoutPool
        {
            get
            {
                if (kickoutPool == null)
                {
                    kickoutPool = new ObjectPool<string, RequestMessage>();
                }
                return kickoutPool;
            }
        }
        /// <summary>
        /// 保存消息数据
        /// </summary>
        public virtual bool SaveMessageData(RequestMessage reqeustMessage)
        {
            bool flag = false;
            try
            {
                if (reqeustMessage!=null)
                {
                    RequestMessage rMessage = KickoutPool.Add(reqeustMessage.ToId, reqeustMessage);
                    if (rMessage != null)
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
        /// 获取消息数据
        /// </summary>
        public virtual RequestMessage GetMessageData(string messageId)
        {
            RequestMessage requestMessage = new RequestMessage();
            try
            {
                if (!string.IsNullOrEmpty(messageId))
                {
                    requestMessage = KickoutPool.Get(messageId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return requestMessage;
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
                    RequestMessage requestMessage = KickoutPool.Remove(messageId);
                    if (requestMessage != null)
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
                    RequestMessage requestMessage = null;
                    requestMessage = KickoutPool.Get(messageId);
                    if (requestMessage != null)
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
        /// 清理特殊消息缓冲池
        /// </summary>
        public virtual void ClearKickoutPool()
        {
            while (true)
            {
                try
                {
                    foreach (string id in KickoutPool.PoolMap.Keys)
                    {
                        DateTime lastDate = KickoutPool.GetLastDate(id);
                        if (null == lastDate)
                        {
                            continue;
                        }
                        DateTime nowDate = DateTime.Now;
                        long timeout = (nowDate.Ticks - lastDate.Ticks) / 10000;
                        if (TimeSet.CLEAR_TIMEOUT < timeout)
                        {
                            KickoutPool.Remove(id);
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
