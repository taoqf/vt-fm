﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.MessageManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Victop.Frame.CoreLibrary;
    using Victop.Frame.CoreLibrary.Enums;
    using Victop.Frame.CoreLibrary.Models;
    using Victop.Frame.MessageManager.Enums;
    using Victop.Frame.PublicLib.Helpers;

    /// <summary>
    /// 插件消息格式管理 【消息格式验证】
    /// </summary>
    /// <remarks>插件消息格式管理</remarks>
    public class PluginMessageFormManager
    {
        /// <summary>
        /// 检查插件消息格式
        /// <param name="messageInfo">消息信息</param>
        /// <param name="callBack">消息回调方法</param>
        /// <param name="validTime">有效时间</param>
        /// </summary>
        public virtual void CheckMessageFormat(string messageType,Dictionary<string,object> messageContent,WaitCallback callBack, long validTime, DataFormEnum dataForm)
        {
            PluginMessageManager pluginMessageManager = new PluginMessageManager();
            if (true)//验证成功
            {
                RequestMessage message = new RequestMessage()
                {
                    MessageId = Guid.NewGuid().ToString(),
                    MessageType = messageType,
                    MessageContent = JsonHelper.ToJson(messageContent)
                };
                bool result = pluginMessageManager.InsertPluginMessage(message.MessageId, new PluginMessageInfo()
                {
                    MessageBody = message,
                    MessageEffectiveTime = DateTime.Now.AddSeconds(validTime),
                    MessageId = message.MessageId,
                    CloudGalleryId = GalleryManager.GetCurrentGalleryId().ToString(),
                    MessageCallBack = callBack
                });
                if (result)//插入成功
                {
                    PluginMessageThreadManager.GetInstance().DoWork(message,dataForm);
                }
                else
                {
                    ReturnReplyMessageToPlugin(callBack,MesssageStatusEnum.EXIST);
                }
            }
        }

        private void ReturnReplyMessageToPlugin(WaitCallback callBack,MesssageStatusEnum messageStatus)
        {
            RequestMessage message = CreateMessage(callBack);
            //1.调用自身方法创建对应的消息格式信息。
            ReplyMessage replyMessage=new ReplyMessage();
            switch (messageStatus)
            {
                case MesssageStatusEnum.EXIST:
                    replyMessage = CreateExsitRelyMessage(message.MessageId);
                    break;
                case MesssageStatusEnum.FORMATERROR:
                default:
                    replyMessage = CreateFormatErrorRelyMessage(message.MessageId);
                    break;
            }
            //2.创建组织返回信息格式对象
            ReplyPluginMessageManager replyPluginMessageManager = new ReplyPluginMessageManager();
            //3.调用组织返回消息的方法。
            replyPluginMessageManager.OrganizeReplyMessage(replyMessage);
        }

        /// <summary>
        /// 验证/插入失败 创建消息，并插入消息池中
        /// </summary>
        /// <param name="callBack"></param>
        /// <param name="pluginMessageManager"></param>
        /// <returns></returns>
        private RequestMessage CreateMessage(WaitCallback callBack)
        {
            PluginMessageManager pluginMessageManager = new PluginMessageManager();
            RequestMessage message = new RequestMessage()
            {
                MessageId = Guid.NewGuid().ToString()
            };
            bool result = pluginMessageManager.InsertPluginMessage(message.MessageId, new PluginMessageInfo()
            {
                MessageBody = message,
                MessageEffectiveTime = DateTime.Now.AddSeconds(15),
                MessageId = message.MessageId,
                MessageCallBack = callBack
            });
            return message;
        }

        /// <summary>
        ///验证失败， 创建应答消息对象。
        /// </summary>
        /// <param name="messageInfo"></param>
        /// <returns></returns>
        private ReplyMessage CreateFormatErrorRelyMessage(string messageId)
        {
            ReplyMessage replyMessage = new ReplyMessage();//创建应答消息对象。
            replyMessage.MessageId = messageId;
            replyMessage.ReplyAlertMessage = "消息格式有误";
            replyMessage.ReplyContent = "[{\"ReplyData\":\"失败\"}]";
            return replyMessage;
        }

        /// <summary>
        /// 消息已存在时，创建应答消息对象。
        /// </summary>
        /// <param name="messageInfo"></param>
        /// <returns></returns>
        private ReplyMessage CreateExsitRelyMessage(string messageId)
        {
            ReplyMessage replyMessage = new ReplyMessage();//创建应答消息对象。
            replyMessage.MessageId = messageId;
            replyMessage.ReplyAlertMessage = "消息已存在";
            replyMessage.ReplyContent = "[{\"ReplyData\":\"消息已存在\"}]";
            return replyMessage;
        }

    }
}

