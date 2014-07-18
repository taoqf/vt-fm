﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Victop.Frame.CoreLibrary.Models;
    using Victop.Frame.Adapter;
    using Victop.Frame.CoreLibrary.Interfaces;
    using Victop.Frame.CoreLibrary.Enums;
    using Victop.Frame.PublicLib.Helpers;
    using Victop.Frame.CoreLibrary;
    using Victop.Frame.PublicLib.Managers;

    /// <summary>
    /// 消息转发器
    /// </summary>
    /// <remarks>消息转发器</remarks>
    public class MessageTransmitManager
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        public virtual ReplyMessage SendRmoteMessage(RequestMessage messageInfo)
        {
            IAdapter adapter = new MessageManager();
            try
            {
                if (messageInfo.MessageType.Equals("LoginService.userLoginNew"))
                {
                    ReplyMessage replyMessage = UserLoginSubmit(adapter, messageInfo);
                    return replyMessage;
                }
                else if (messageInfo.MessageType.Equals("LoginService.getCurrentLinker"))
                {
                    ReplyMessage replyMessage = AnonymousLoginSubmit(adapter, messageInfo);
                    return replyMessage;
                }
                else
                {
                    ReplyMessage replyMessage = MessageSubmit(adapter, messageInfo);
                    return replyMessage;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Info(messageInfo, ex);
                ReplyMessage replyMessage = new ReplyMessage()
                {
                    MessageId = messageInfo.MessageId,
                    ReplyContent = ex.Message
                };

                return replyMessage;
            }
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="messageInfo"></param>
        /// <returns></returns>
        private ReplyMessage UserLoginSubmit(IAdapter adapter, RequestMessage messageInfo)
        {
            CloudGalleryInfo currentGallery = new GalleryManager().GetGallery(GalleryManager.GetCurrentGalleryId().ToString());
            Dictionary<string, string> contentDic = JsonHelper.ToObject<Dictionary<string, string>>(messageInfo.MessageContent);
            if (contentDic.ContainsKey("clientId"))
            {
                contentDic["clientId"] = currentGallery.ClientId;
            }
            else
            {
                contentDic.Add("clientId", currentGallery.ClientId);
            }
            if (contentDic.ContainsKey("channelID"))
            {
                if (string.IsNullOrEmpty(currentGallery.ClientInfo.ChannelId))
                {
                    currentGallery.ClientInfo.ChannelId = Guid.NewGuid().ToString();
                }
                contentDic["channelID"] = currentGallery.ClientInfo.ChannelId;
            }
            else
            {
                if (string.IsNullOrEmpty(currentGallery.ClientInfo.ChannelId))
                {
                    currentGallery.ClientInfo.ChannelId = Guid.NewGuid().ToString();
                }
                contentDic.Add("channelID", currentGallery.ClientInfo.ChannelId);
            }
            messageInfo.MessageContent = JsonHelper.ToJson(contentDic);
            ReplyMessage replyMessage = adapter.SubmitRequest(messageInfo);
            if (replyMessage.ReplyMode == ReplyModeEnum.SYNCH || replyMessage.ReplyMode == ReplyModeEnum.BREAK)
            {
                if (JsonHelper.ReadJsonString(replyMessage.ReplyContent, "code").Equals("0"))
                {
                    replyMessage.MessageId = messageInfo.MessageId;
                    replyMessage.ReplyMode = (ReplyModeEnum)0;
                    replyMessage.ReplyAlertMessage = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "msg");
                }
                else
                {
                    currentGallery.ClientInfo.LinkRouterAddress = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "routerAddress");
                    currentGallery.ClientInfo.LinkServerAddress = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "linkInfo");
                    currentGallery.ClientInfo.SessionId = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "sessionID");
                    currentGallery.ClientInfo.UserName = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "userName");
                    currentGallery.ClientInfo.UserPwd = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "userpw");
                    currentGallery.ClientInfo.UserCode = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "usercode");
                    messageInfo.MessageContent = replyMessage.ReplyContent;
                    replyMessage = ConnectLinkSubmit(adapter, messageInfo);
                }
            }
            return replyMessage;
        }

        private ReplyMessage AnonymousLoginSubmit(IAdapter adapter, RequestMessage messageInfo)
        {
            ReplyMessage replyMessage = adapter.SubmitRequest(messageInfo);
            if (replyMessage.ReplyMode == ReplyModeEnum.SYNCH || replyMessage.ReplyMode == ReplyModeEnum.BREAK)
            {
                if (!string.IsNullOrEmpty(replyMessage.ReplyContent))
                {
                    //解析ReplyContent内容完善当前通道的用户登录信息
                    CloudGalleryInfo currentGallery = new GalleryManager().GetGallery(GalleryManager.GetCurrentGalleryId().ToString());
                    currentGallery.ClientInfo.LinkRouterAddress = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "routerAddress");
                    currentGallery.ClientInfo.LinkServerAddress = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "address");
                    currentGallery.ClientInfo.SessionId = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "sessionID");
                    currentGallery.ClientInfo.ChannelId = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "channelID");
                    messageInfo.MessageContent = replyMessage.ReplyContent;
                }
            }
            replyMessage.MessageId = messageInfo.MessageId;
            return replyMessage;
        }
        /// <summary>
        /// 注册连接器
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="messageInfo"></param>
        /// <returns></returns>
        private ReplyMessage ConnectLinkSubmit(IAdapter adapter, RequestMessage messageInfo)
        {
            messageInfo.MessageType = "LinkService.registAsync";
            ReplyMessage replyMessage = adapter.SubmitRequest(messageInfo);
            if (replyMessage.ReplyMode == ReplyModeEnum.SYNCH || replyMessage.ReplyMode == ReplyModeEnum.BREAK)
            {
                if (!string.IsNullOrEmpty(replyMessage.ReplyContent))
                {
                    BaseResourceInfo baseResourceInfo = new BaseResourceInfo();
                    baseResourceInfo.GalleryId = GalleryManager.GetCurrentGalleryId();
                    Dictionary<string, string> replyContentDic = JsonHelper.ToObject<Dictionary<string, string>>(replyMessage.ReplyContent);
                    baseResourceInfo.ResourceXml = replyMessage.ReplyContent;
                    baseResourceInfo.ResourceMnenus = JsonHelper.ToObject<List<MenuInfo>>(JsonHelper.ReadJsonString(replyContentDic["menu"], "result"));
                    BaseResourceManager baseResourceManager = new BaseResourceManager();
                    bool result = baseResourceManager.AddResouce(baseResourceInfo);
                    if (result)
                    {
                        replyMessage.ReplyContent = string.Empty;
                        replyMessage.ReplyAlertMessage = replyContentDic["msg"];
                    }
                }
            }
            else
            {
                replyMessage.ReplyMode = (ReplyModeEnum)0;
                replyMessage.ReplyAlertMessage = "注册连接器失败";
            }
            replyMessage.MessageId = messageInfo.MessageId;
            return replyMessage;
        }
        /// <summary>
        /// 常规消息
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="messageInfo"></param>
        /// <returns></returns>
        private ReplyMessage MessageSubmit(IAdapter adapter, RequestMessage messageInfo)
        {
            MessageOrganizeManager organizeManager = new MessageOrganizeManager();
            DataOperateEnum saveDataFlag = DataOperateEnum.NONE;
            string DataChannelId = JsonHelper.ReadJsonString(messageInfo.MessageContent, "DataChannelId");
            messageInfo = organizeManager.OrganizeMessage(messageInfo, out saveDataFlag);
            ReplyMessage replyMessage = adapter.SubmitRequest(messageInfo);
            if (!(replyMessage.ReplyMode == (ReplyModeEnum)0))
            {
                ReplyMessageResolver replyMessageResolver = new ReplyMessageResolver();
                switch (saveDataFlag)
                {
                    case DataOperateEnum.SAVE:
                        replyMessage = replyMessageResolver.ResolveReplyMessage(replyMessage, messageInfo);
                        break;
                    case DataOperateEnum.COMMIT:
                        bool result = replyMessageResolver.CommitDataSetChange(DataChannelId);
                        break;
                    case DataOperateEnum.NONE:
                    default:
                        break;
                }
                replyMessage.MessageId = messageInfo.MessageId;
            }
            return replyMessage;
        }
        private bool UpdateBaseResourceByGalleryId(BaseResourceInfo resourceInfo)
        {
            return true;
        }
    }
}

