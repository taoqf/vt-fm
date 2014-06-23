using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.CoreLibrary.Models;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.Adapter
{
    public class OtherPoolManager
    {
        private static ObjectPool<string, ICollection<RequestMessage>> otherPool;

        /// <summary>
        /// 其他消息缓冲池，储存非特定消息类型的消息
        /// </summary>
        public static ObjectPool<string, ICollection<RequestMessage>> OtherPool
        {
            get
            {
                if (otherPool == null)
                {
                    otherPool = new ObjectPool<string, ICollection<RequestMessage>>();
                }
                return otherPool;
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
                if (reqeustMessage != null)
                {
                    // 接收消息为通知消息，存入通知缓存队列
                    LinkedList<RequestMessage> otherList = (LinkedList<RequestMessage>)OtherPool.Get(reqeustMessage.MessageType);
                    if (otherList == null)
                    {
                        otherList = new LinkedList<RequestMessage>();
                    }
                    otherList.AddLast(reqeustMessage);
                    ICollection<RequestMessage> iCollection = OtherPool.Add(reqeustMessage.MessageType, otherList);
                    if (iCollection == null)
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
        public virtual LinkedList<RequestMessage> GetMessageData(string messageId)
        {
            LinkedList<RequestMessage> message = new LinkedList<RequestMessage>();
            try
            {
                if (!string.IsNullOrEmpty(messageId))
                {
                    message = (LinkedList<RequestMessage>)OtherPool.Get(messageId);
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
                    LinkedList<RequestMessage> message = (LinkedList<RequestMessage>)OtherPool.Remove(messageId);
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
                    LinkedList<RequestMessage> message = null;
                    message = (LinkedList<RequestMessage>)OtherPool.Get(messageId);
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
    }
}
