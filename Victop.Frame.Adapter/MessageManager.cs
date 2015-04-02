﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.Adapter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Victop.Frame.CoreLibrary;
    using Victop.Frame.CoreLibrary.AbsClasses;
    using Victop.Frame.CoreLibrary.Enums;
    using Victop.Frame.CoreLibrary.Interfaces;
    using Victop.Frame.CoreLibrary.Models;
    using Victop.Frame.PublicLib.Helpers;

    /// <summary>
    /// 消息管理器
    /// </summary>
    /// <remarks>消息管理器</remarks>
    public class MessageManager : Base, IAdapter
    {
        /// <summary>
        /// 创建一个预设的消息对象，预设消息ID，发送者身份
        /// </summary>
        public virtual RequestMessage CreateMessage(MessageTargetEnum messageTarget)
        {
            RequestMessage message = new RequestMessage();
            try
            {
                GalleryManager galleryManager = new GalleryManager();
                CloudGalleryInfo cloudGallyInfo = galleryManager.GetGallery(GalleryManager.GetCurrentGalleryId().ToString());
                LoginUserInfo loginUserInfo = cloudGallyInfo.ClientInfo;
                //message.MessageId = Guid.NewGuid().ToString();
                message.FromRole = "WPF客户端";
                message.FromId = loginUserInfo.UserCode;
                message.CurrentSenderId = String.IsNullOrWhiteSpace(loginUserInfo.ChannelId) ? Guid.NewGuid().ToString() : loginUserInfo.ChannelId;
                message.SessionId = loginUserInfo.SessionId;
                switch (messageTarget)
                {
                    case MessageTargetEnum.LINK:
                        message.SetTargetAddress(loginUserInfo.LinkServerAddress.Split(':')[0], loginUserInfo.LinkServerAddress.Split(':')[1]);
                        if (cloudGallyInfo.IsNeedRouter)
                        {
                            message.SetRouterAddress(loginUserInfo.LinkRouterAddress.Split(':')[0], loginUserInfo.LinkRouterAddress.Split(':')[1]);
                        }
                        break;
                    case MessageTargetEnum.MAIN:
                        if (cloudGallyInfo.IsNeedRouter)
                        {
                            message.SetRouterAddress(cloudGallyInfo.RouterAddress.Split(':')[0], cloudGallyInfo.RouterAddress.Split(':')[1]);
                        }
                        message.SetTargetAddress(cloudGallyInfo.CloudAddress.Split(':')[0], cloudGallyInfo.CloudAddress.Split(':')[1]);
                        break;
                    case MessageTargetEnum.NORMAL:
                        message.SetTargetAddress(loginUserInfo.LinkServerAddress.Split(':')[0], loginUserInfo.LinkServerAddress.Split(':')[1]);
                        if (cloudGallyInfo.IsNeedRouter)
                        {
                            message.SetRouterAddress(loginUserInfo.LinkRouterAddress.Split(':')[0], loginUserInfo.LinkRouterAddress.Split(':')[1]);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return message;
        }

        /// <summary>
        /// 创建一个预设的消息对象，预设消息ID，发送者身份，传入消息类型
        /// </summary>
        public virtual RequestMessage CreateMessage(MessageTargetEnum messageTarget, string messageType)
        {
            RequestMessage message = CreateMessage(messageTarget);
            message.MessageType = messageType;
            return message;
        }

        /// <summary>
        /// 创建一个预设的消息对象，预设消息ID，发送者身份，传入消息类型，消息控制参数，消息本体
        /// </summary>
        public virtual RequestMessage CreateMessage(MessageTargetEnum messageTarget, string messageType, string messageControl, string messageContent)
        {
            RequestMessage message = CreateMessage(messageTarget, messageType);
            message.MessageControl = messageControl;
            message.MessageContent = messageContent;
            return message;
        }
        public virtual RequestMessage CreateMessage(MessageTargetEnum messageTarget, RequestMessage messageInfo)
        {
            string messageId = messageInfo.MessageId;
            string messageToRole = messageInfo.ToRole;
            messageInfo = CreateMessage(messageTarget, messageInfo.MessageType, messageInfo.MessageControl, messageInfo.MessageContent);
            messageInfo.MessageId = messageId;
            messageInfo.ToRole = messageToRole;
            return messageInfo;
        }
        /// <summary>
        /// 获取消息
        /// </summary>
        public virtual RequestMessage GetReplyMessage(string messageId)
        {
            throw new System.NotImplementedException(); //TODO:方法实现
        }

        /// <summary>
        /// 依据消息类型获取消息清单
        /// </summary>
        public virtual LinkedList<RequestMessage> FindMessageList(string messageType)
        {
            throw new System.NotImplementedException(); //TODO:方法实现
        }

        /// <summary>
        /// 提交信息,直接返回应答消息
        /// </summary>
        public virtual ReplyMessage SubmitRequest(RequestMessage message)
        {
            #region 获取推送消息
            if (message.MessageType == "TaskNotifyService.GetNoifyInfo")
            {
                ReplyMessage repMessage = new ReplyMessage();
                repMessage.MessageId = message.MessageId;
                repMessage.ReplyMode = ReplyModeEnum.ASYNC;
                repMessage.ReplyContent = JsonHelper.ToJson(NotificationPoolManager.NotificationPool.PoolMap);
                //NotificationPoolManager notifyMgr = new NotificationPoolManager();
                //notifyMgr.ClearNotificationPool();
                return repMessage;
            }
            #endregion

            return SubmitRequest(message, 10000);
        }

        /// <summary>
        /// 发送消息,模拟同步
        /// </summary>
        public virtual ReplyMessage SubmitRequest(RequestMessage message, long time)
        {
            if (message.MessageType == "LoginService.userLoginNew" || message.MessageType == "LoginService.getCurrentLinker")
            {
                message = CreateMessage(MessageTargetEnum.MAIN, message);
            }
            else if (message.MessageType == "LinkService.registAsync")
            {
                message = CreateMessage(MessageTargetEnum.LINK, message);
            }
            else
            {
                message = CreateMessage(MessageTargetEnum.NORMAL, message);
            }
            MessagePoolManager messagePoolManager = new MessagePoolManager();
            ReplyMessage replyMessage = FrameInit.GetInstance().ComlinkObject.SendMessage(message);
            LoggerHelper.InfoFormat("发送消息:{0}", JsonHelper.ToJson(message));
            switch (replyMessage.ReplyMode)
            {
                case ReplyModeEnum.ASYNC:
                    if (time > 0)
                    {
                        bool flag = false;
                        TimeoutThread timeoutThread = new TimeoutThread((int)time);
                        Thread thread = new Thread(new ThreadStart(timeoutThread.Sleep));
                        thread.Start();
                        string messageID = message.MessageId;
                        while (!timeoutThread.Done)
                        {
                            RequestMessage returnMessage = GetMessage(messageID);
                            if (returnMessage != null)
                            {
                                replyMessage.ReplyControl = returnMessage.MessageControl;
                                replyMessage.ReplyContent = returnMessage.MessageContent;
                                LoggerHelper.InfoFormat("接收信息:{0}", returnMessage.MessageContent);
                                string replyCode = JsonHelper.ReadJsonString(returnMessage.MessageContent, "code");
                                if (!string.IsNullOrEmpty(replyCode))
                                {
                                    long codeInt = Convert.ToInt32(replyCode);
                                    replyMessage.ReplyMode = (ReplyModeEnum)codeInt;
                                    replyMessage.ReplyAlertMessage = JsonHelper.ReadJsonString(returnMessage.MessageContent, "msg");
                                }
                                else
                                {
                                    replyMessage.ReplyMode = ReplyModeEnum.BREAK;
                                }
                                replyMessage.MessageId = messageID;
                                messagePoolManager.RemoveMessageData(messageID);
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
                            replyMessage.MessageId = message.MessageId;
                            replyMessage.ReplyMode = (ReplyModeEnum)0;
                        }
                    }
                    break;
                case ReplyModeEnum.BREAK:
                    break;
                case ReplyModeEnum.CAST:
                    replyMessage.MessageId = message.MessageId;
                    replyMessage.ReplyMode = (ReplyModeEnum)0;
                    replyMessage.ReplyAlertMessage = "不可识别的消息类型";
                    break;
                case ReplyModeEnum.ROUTER:
                    break;
                case ReplyModeEnum.SYNCH:
                    replyMessage.MessageId = message.MessageId;
                    replyMessage.ReplyMode = (ReplyModeEnum)0;
                    replyMessage.ReplyAlertMessage = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "result");
                    break;
                default:
                    break;
            }
            return replyMessage;
        }


        /// <summary>
        /// 停止适配器
        /// </summary>
        public virtual void StopAdapter()
        {
            throw new System.NotImplementedException(); //TODO:方法实现
        }

        /// <summary>
        /// 异步处理消息
        /// </summary>
        public override void AsyncDoMessage(RequestMessage messageInfo)
        {
            MessagePoolManager messagePoolManager = new MessagePoolManager();
            KickoutPoolManager kickoutPoolManager = new KickoutPoolManager();
            NotificationPoolManager notificationPoolManager = new NotificationPoolManager();
            OtherPoolManager otherPoolManager = new OtherPoolManager();
            TaskPoolManager taskPoolManager = new TaskPoolManager();
            if ("notification".Equals(messageInfo.MessageType))
            {
                string msgControl = messageInfo.MessageControl;
                string kickString = JsonHelper.ReadJsonString(msgControl, "kickout");
                if ("1".Equals(kickString))
                {
                    kickoutPoolManager.SaveMessageData(messageInfo);
                    return;
                }
                notificationPoolManager.SaveMessageData(messageInfo);
            }
            else
            {
                string replyToID = messageInfo.ReplyToId;
                if (null == replyToID || "".Equals(replyToID.Trim()) || "null".Equals(replyToID.Trim()))
                {
                    otherPoolManager.SaveMessageData(messageInfo);

                }
                else if ("victop-task-role".Equals(messageInfo.FromRole) && "reply".Equals(messageInfo.MessageType))
                {
                    messagePoolManager.SaveMessageData(messageInfo);
                }
                else
                {
                    messagePoolManager.SaveMessageData(messageInfo);
                }
            }
        }

        /// <summary>
        /// 同步消息
        /// </summary>
        public override void SynchDoMessage(IResponse response, RequestMessage messageInfo)
        {
            response.Reply("", "ok", ReplyModeEnum.ASYNC);
        }
        /// <summary>
        /// 拒绝消息
        /// </summary>
        /// <param name="response"></param>
        /// <param name="messageInfo"></param>
        public override void RejectedMessage(IResponse response, RequestMessage messageInfo)
        {
            response.Reply("端点拒收此消息", ReplyModeEnum.CAST);
        }
        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public RequestMessage GetMessage(string messageId)
        {
            MessagePoolManager messagePoolManager = new MessagePoolManager();
            RequestMessage message = null;
            if (!string.IsNullOrEmpty(messageId))
            {
                message = messagePoolManager.GetMessageData(messageId);
            }
            return message;
        }

        public override bool Init()
        {
            return true;
        }

        public override void AutoRun()
        {
            throw new NotImplementedException();
        }

        public override void OnExit(ExitTypeEnum exitType)
        {
            return;
        }


        private class TimeoutThread
        {
            private bool done = false;

            public bool Done
            {
                get { return done; }
            }
            private int time = 0;
            public TimeoutThread(int time)
            {
                this.time = time;
            }
            public void Sleep()
            {
                lock (this)
                {
                    if (!done)
                    {
                        try
                        {
                            Monitor.Wait(this, time);
                        }
                        catch (ThreadInterruptedException ex)
                        {
                            throw ex;
                        }
                    }
                }
                Stop();
            }
            public void Stop()
            {
                lock (this)
                {
                    if (!done)
                    {
                        done = true;
                        Monitor.Pulse(this);
                    }
                }
            }
        }
    }
}

