using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.CoreLibrary.Models;

namespace Victop.Frame.Adapter
{
    public class TaskPoolManager
    {
        private static Hashtable taskTool;

        /// <summary>
        ///  任务回调消息缓冲池，储存来自任务调度器的返回消息
        /// </summary>
        public static Hashtable TaskTool
        {
            get 
            {
                if (taskTool == null)
                {
                    taskTool = Hashtable.Synchronized(new Hashtable());
                }
                return taskTool;
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
                    TaskTool.Add(requestMessage.ReplyToId, requestMessage);
                    flag = true;
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
            RequestMessage message = new RequestMessage();
            try
            {
                if (!string.IsNullOrEmpty(messageId))
                {
                    message = (RequestMessage)TaskTool[messageId];
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
                    TaskTool.Remove(messageId);
                    flag = true;
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

                    RequestMessage requestMessage = (RequestMessage)TaskTool[messageId];
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
    }
}
