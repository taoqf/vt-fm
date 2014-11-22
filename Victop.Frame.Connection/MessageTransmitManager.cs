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
    using System.Configuration;

    /// <summary>
    /// 消息转发器
    /// </summary>
    /// <remarks>消息转发器</remarks>
    public class MessageTransmitManager
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        public virtual ReplyMessage SendRmoteMessage(RequestMessage messageInfo, DataFormEnum dataForm)
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
                    ReplyMessage replyMessage = MessageSubmit(adapter, messageInfo, dataForm);
                    return replyMessage;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Info(messageInfo, ex);
                ReplyMessage replyMessage = new ReplyMessage()
                {
                    MessageId = messageInfo.MessageId,
                    ReplyContent = ex.InnerException.Message
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
            DataFormEnum dataForm = ConfigurationManager.AppSettings["SystemType"].Equals("JSON") ? DataFormEnum.JSON : DataFormEnum.DATASET;
            switch (dataForm)
            {
                case DataFormEnum.DATASET:
                    if (contentDic.ContainsKey("clientId"))
                    {
                        contentDic["clientId"] = currentGallery.ClientId;
                    }
                    else
                    {
                        contentDic.Add("clientId", currentGallery.ClientId);
                    }
                    break;
                case DataFormEnum.JSON:
                    if (contentDic.ContainsKey("clientId"))
                    {
                        contentDic.Remove("clientId");
                        contentDic.Add("spaceId", currentGallery.ClientId);
                    }
                    if (contentDic.ContainsKey("spaceId"))
                    {
                        contentDic["spaceId"] = currentGallery.ClientId;
                    }
                    else
                    {
                        contentDic.Add("spaceId", currentGallery.ClientId);
                    }
                    if (contentDic.ContainsKey("usercode"))
                    {
                        contentDic.Add("userCode", contentDic["usercode"]);
                        contentDic.Remove("usercode");
                    }
                    break;
                default:
                    break;
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
            if (replyMessage.ReplyMode == (ReplyModeEnum)0)
            {
                replyMessage.MessageId = messageInfo.MessageId;
                replyMessage.ReplyAlertMessage = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "msg");
            }
            else
            {
                currentGallery.ClientInfo.LinkRouterAddress = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "routerAddress");
                currentGallery.ClientInfo.LinkServerAddress = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "linkInfo");
                currentGallery.ClientInfo.SessionId = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "sessionID");
                currentGallery.ClientInfo.UserName = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "userName");
                currentGallery.ClientInfo.UserPwd = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "userpw");
                currentGallery.ClientInfo.UserCode = replyMessage.ReplyContent.Contains("usercode") ? JsonHelper.ReadJsonString(replyMessage.ReplyContent, "usercode") : JsonHelper.ReadJsonString(replyMessage.ReplyContent, "userCode");
                messageInfo.MessageContent = replyMessage.ReplyContent;
                if (dataForm == DataFormEnum.DATASET)
                {
                    replyMessage = ConnectLinkSubmit(adapter, messageInfo);
                }
                else
                {
                    replyMessage = GetLoginUserMenuSubmit(adapter, messageInfo);
                    //replyMessage.ReplyAlertMessage = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "msg");
                }
            }
            return replyMessage;
        }
        /// <summary>
        /// 匿名登陆
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="messageInfo"></param>
        /// <returns></returns>
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
            if (replyMessage.ReplyMode == (ReplyModeEnum)0)
            {
                replyMessage.ReplyAlertMessage = "注册连接器失败";
            }
            else
            {
                if (!string.IsNullOrEmpty(replyMessage.ReplyContent))
                {
                    BaseResourceInfo baseResourceInfo = new BaseResourceInfo();
                    baseResourceInfo.GalleryId = GalleryManager.GetCurrentGalleryId();
                    Dictionary<string, string> replyContentDic = JsonHelper.ToObject<Dictionary<string, string>>(replyMessage.ReplyContent);
                    baseResourceInfo.ResourceXml = replyMessage.ReplyContent;
                    List<object> menuObj = JsonHelper.ToObject<List<object>>(JsonHelper.ReadJsonString(replyContentDic["menu"], "result"));
                    if (menuObj != null)
                    {
                        foreach (object item in menuObj)
                        {
                            MenuInfo menuInfo = JsonHelper.ToObject<MenuInfo>(item.ToString());
                            if (menuInfo == null || baseResourceInfo.ResourceMnenus.Exists(it => it.Id == menuInfo.Id))
                                continue;
                            baseResourceInfo.ResourceMnenus.Add(menuInfo);
                        }
                    }
                    BaseResourceManager baseResourceManager = new BaseResourceManager();
                    bool result = baseResourceManager.AddResouce(baseResourceInfo);
                    if (result)
                    {
                        replyMessage.ReplyContent = string.Empty;
                        replyMessage.ReplyAlertMessage = replyContentDic["msg"];
                    }
                }
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
        private ReplyMessage MessageSubmit(IAdapter adapter, RequestMessage messageInfo, DataFormEnum dataForm)
        {
            MessageOrganizeManager organizeManager = new MessageOrganizeManager();
            DataOperateEnum saveDataFlag = DataOperateEnum.NONE;
            string DataChannelId = JsonHelper.ReadJsonString(messageInfo.MessageContent, "DataChannelId");
            messageInfo = organizeManager.OrganizeMessage(messageInfo, dataForm, out saveDataFlag);
            string DataSource = ConfigurationManager.AppSettings.Get("DataSource").ToLower();
            ReplyMessage replyMessage = new ReplyMessage();
            switch (DataSource)
            {
                case "local":
                    SimulatedDataManager simDataManager = new SimulatedDataManager();
                    replyMessage = simDataManager.SubmitRequest(messageInfo);
                    break;
                case "remote":
                default:
                    replyMessage = adapter.SubmitRequest(messageInfo);
                    break;
            }
            if (!(replyMessage.ReplyMode == (ReplyModeEnum)0))
            {
                ReplyMessageResolver replyMessageResolver = new ReplyMessageResolver();
                switch (saveDataFlag)
                {
                    case DataOperateEnum.SAVE:
                        replyMessage = replyMessageResolver.ResolveReplyMessage(replyMessage, messageInfo, dataForm);
                        break;
                    case DataOperateEnum.COMMIT:
                        bool result = replyMessageResolver.CommitDataSetChange(DataChannelId);
                        break;
                    case DataOperateEnum.NONE:
                    default:
                        break;
                }
            }
            replyMessage.ReplyAlertMessage = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "Result");
            replyMessage.MessageId = messageInfo.MessageId;
            return replyMessage;
        }

        /// <summary>
        /// 获取登陆用户信息的菜单
        /// </summary>
        /// <param name="adapter"></param>
        /// <param name="messageInfo"></param>
        /// <returns></returns>
        private ReplyMessage GetLoginUserMenuSubmit(IAdapter adapter, RequestMessage messageInfo)
        {
            messageInfo.MessageType = "MongoDataChannelService.afterLogin";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "101");
            contentDic.Add("client_type_val", "1");
            contentDic.Add("configsystemid", "101");
            string userCode = messageInfo.MessageContent.Contains("usercode") ? JsonHelper.ReadJsonString(messageInfo.MessageContent, "usercode") : JsonHelper.ReadJsonString(messageInfo.MessageContent, "userCode");
            contentDic.Add("userCode", userCode);
            messageInfo.MessageContent = JsonHelper.ToJson(contentDic);
            DataOperateEnum saveDataFlag = DataOperateEnum.NONE;
            MessageOrganizeManager organizeManager = new MessageOrganizeManager();
            messageInfo = organizeManager.OrganizeMessage(messageInfo, DataFormEnum.JSON, out saveDataFlag);
            ReplyMessage replyMessage = adapter.SubmitRequest(messageInfo);
            if (replyMessage.ReplyMode == (ReplyModeEnum)0)
            {
                replyMessage.ReplyAlertMessage = "获取当前登录用户菜单失败";
            }
            else
            {
                if (!string.IsNullOrEmpty(replyMessage.ReplyContent))
                {
                    #region 菜单处理
                    BaseResourceInfo baseResourceInfo = new BaseResourceInfo();
                    baseResourceInfo.GalleryId = GalleryManager.GetCurrentGalleryId();
                    baseResourceInfo.ResourceXml = replyMessage.ReplyContent;
                    List<MenuInfo> menuInfo = JsonHelper.ToObject<List<MenuInfo>>(JsonHelper.ReadJsonString(replyMessage.ReplyContent, "menu"));
                    if (menuInfo != null)
                    {
                        foreach (MenuInfo item in menuInfo)
                        {
                            if (string.IsNullOrEmpty(item.parent_id))
                            {
                                item.ParentMenu = "0";
                            }
                            else
                            {
                                item.ParentMenu = item.parent_id;
                            }
                            item.Id = item._id;
                            item.MenuId = item._id;
                            item.MenuName = item.menu_name;
                            item.BzSystemId = item.systemid;
                            item.HomeId = item.authority_code;
                        }
                    }
                    baseResourceInfo.ResourceMnenus = menuInfo;
                    BaseResourceManager baseResourceManager = new BaseResourceManager();
                    bool result = baseResourceManager.AddResouce(baseResourceInfo); 
                    #endregion
                    #region 用户信息管理
                    string userInfoStr = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "userInfo");
                    List<Dictionary<string, object>> userInfoList = JsonHelper.ToObject<List<Dictionary<string, object>>>(userInfoStr);
                    CloudGalleryInfo currentGallery = new GalleryManager().GetGallery(GalleryManager.GetCurrentGalleryId().ToString());
                    try
                    {
                        if (userInfoList != null && userInfoList.Count > 0)
                        {
                            currentGallery.ClientInfo.UserId = userInfoList[0]["_id"].ToString();
                            currentGallery.ClientInfo.UserImg = userInfoList[0]["staff_picture"].ToString();
                        }
                        else
                        {
                            currentGallery.ClientInfo.UserId = string.Empty;
                            currentGallery.ClientInfo.UserImg = string.Empty;
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.ErrorFormat("用户信息:{0}", userInfoStr);
                        currentGallery.ClientInfo.UserId = string.Empty;
                        currentGallery.ClientInfo.UserImg = string.Empty;
                    }
                    #endregion
                }
            }
            return replyMessage;
        }
    }
}

