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
    using System.Net;
    using System.Text.RegularExpressions;

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
                if (messageInfo.MessageType.Equals("LoginService.userLogin"))
                {
                    ReplyMessage replyMessage = UserLoginSubmit(adapter, messageInfo);
                    return replyMessage;
                }
                else
                {
                    ReplyMessage replyMessage = MessageSubmit(adapter, messageInfo);
                    if (replyMessage.ReplyMode == ReplyModeEnum.ROUTER)
                    {
                        RequestMessage userLoginMsg = new RequestMessage();
                        userLoginMsg.MessageType = "LoginService.userLogin";
                        CloudGalleryInfo currentGallery = new GalleryManager().GetGallery(GalleryManager.GetCurrentGalleryId().ToString());
                        Dictionary<string, object> loginContent = new Dictionary<string, object>();
                        loginContent.Add("userCode", currentGallery.ClientInfo.UserCode);
                        loginContent.Add("userpw", currentGallery.ClientInfo.UserPwd);
                        loginContent.Add("logintypenew", GetUserLoginForm(currentGallery.ClientInfo.UserCode));
                        userLoginMsg.MessageContent = JsonHelper.ToJson(loginContent);
                        ReplyMessage loginReplyMessage = UserLoginSubmit(adapter, userLoginMsg);
                        if (loginReplyMessage.ReplyMode != ReplyModeEnum.CAST)
                        {
                            replyMessage = MessageSubmit(adapter, messageInfo);
                        }
                        else
                        {
                            replyMessage = loginReplyMessage;
                            replyMessage.ReplyMode = ReplyModeEnum.CAST;
                        }
                    }
                    return replyMessage;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Info(messageInfo, ex);
                ReplyMessage replyMessage = new ReplyMessage()
                {
                    MessageId = messageInfo.MessageId,
                    ReplyContent = ex.InnerException == null ? ex.Message : ex.InnerException.Message
                };

                return replyMessage;
            }
        }
        /// <summary>
        /// 获取登录方式
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private string GetUserLoginForm(string userName)
        {
            if (Regex.IsMatch(userName, @"^[1]+[3,4,5,7,8]+\d{9}"))
            {
                return "phone";
            }
            if (Regex.IsMatch(userName, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
            {
                return "email";
            }
            return "usercode";
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
            if (!contentDic.ContainsKey("spaceid"))
            {
                contentDic.Add("spaceid", currentGallery.ClientId);
            }
            if (!contentDic.ContainsKey("logintypenew"))
            {
                contentDic.Add("logintypenew", "usercode");
            }
            if (!contentDic.ContainsKey("userip"))
            {
                string ipreg = @"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))";
                string hostname = Dns.GetHostName();
                bool getIpFlag = false;
                IPHostEntry localhost = Dns.GetHostEntry(hostname);
                foreach (IPAddress item in localhost.AddressList)
                {
                    if (Regex.IsMatch(item.ToString(), ipreg))
                    {
                        contentDic.Add("userip", item.ToString());
                        getIpFlag = true;
                        break;
                    }
                }
                if (!getIpFlag)
                {
                    contentDic.Add("userip", localhost.AddressList.Last().ToString());
                }
            }
            messageInfo.MessageContent = JsonHelper.ToJson(contentDic);
            ReplyMessage replyMessage = adapter.SubmitRequest(messageInfo);
            if (replyMessage.ReplyMode == (ReplyModeEnum)0)
            {
                replyMessage.MessageId = messageInfo.MessageId;
            }
            else
            {
                currentGallery.ClientInfo.SessionId = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "sessionID");
                currentGallery.ClientInfo.UserName = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "user_name");
                currentGallery.ClientInfo.UserCode = replyMessage.ReplyContent.Contains("userCode") ? JsonHelper.ReadJsonString(replyMessage.ReplyContent, "userCode") : JsonHelper.ReadJsonString(replyMessage.ReplyContent, "userCode");
                messageInfo.MessageContent = replyMessage.ReplyContent;
                replyMessage = GetLoginUserMenuSubmit(adapter, messageInfo);
            }
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
                        replyMessage = replyMessageResolver.ResolveReplyMessage(replyMessage, messageInfo);
                        break;
                    case DataOperateEnum.COMMIT:
                        bool result = replyMessageResolver.CommitDataSetChange(DataChannelId);
                        break;
                    case DataOperateEnum.NONE:
                    default:
                        break;
                }
            }
            if (string.IsNullOrEmpty(replyMessage.ReplyAlertMessage))
            {
                replyMessage.ReplyAlertMessage = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "Result");
            }
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
            string afterLoginStr = ConfigurationManager.AppSettings["afterlogin"];
            contentDic.Add("systemid", JsonHelper.ReadJsonString(afterLoginStr, "systemid"));
            contentDic.Add("clientType", "3");
            contentDic.Add("configsystemid", JsonHelper.ReadJsonString(afterLoginStr, "configsystemid"));
            string userCode = messageInfo.MessageContent.Contains("usercode") ? JsonHelper.ReadJsonString(messageInfo.MessageContent, "usercode") : JsonHelper.ReadJsonString(messageInfo.MessageContent, "userCode");
            contentDic.Add("userCode", userCode);
            messageInfo.MessageContent = JsonHelper.ToJson(contentDic);
            DataOperateEnum saveDataFlag = DataOperateEnum.NONE;
            MessageOrganizeManager organizeManager = new MessageOrganizeManager();
            messageInfo = organizeManager.OrganizeMessage(messageInfo, out saveDataFlag);
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
                    List<MenuInfo> menuInfo = JsonHelper.ToObject<List<MenuInfo>>(JsonHelper.ReadJsonString(replyMessage.ReplyContent, "menus"));
                    baseResourceInfo.ResourceMnenus = menuInfo;
                    BaseResourceManager baseResourceManager = new BaseResourceManager();
                    bool result = baseResourceManager.AddResouce(baseResourceInfo);
                    #endregion
                    #region 用户信息管理
                    string userInfoStr = JsonHelper.ReadJsonString(replyMessage.ReplyContent, "user");
                    Dictionary<string, object> userInfoList = JsonHelper.ToObject<Dictionary<string, object>>(userInfoStr);
                    CloudGalleryInfo currentGallery = new GalleryManager().GetGallery(GalleryManager.GetCurrentGalleryId().ToString());
                    try
                    {
                        if (userInfoList != null && userInfoList.Count > 0)
                        {
                            currentGallery.ClientInfo.UserId = userInfoList["_id"].ToString();
                            currentGallery.ClientInfo.UserImg = (userInfoList.ContainsKey("avatar_path") && userInfoList["avatar_path"] != null) ? userInfoList["avatar_path"].ToString() : string.Empty;
                            currentGallery.ClientInfo.UserFullInfo = userInfoList;
                            if (userInfoList.ContainsKey("pub_user_connect") && userInfoList["pub_user_connect"] != null)
                            {
                                currentGallery.ClientInfo.RoleList = JsonHelper.ToObject<List<UserRoleInfo>>(userInfoList["pub_user_connect"].ToString());
                            }
                        }
                        else
                        {
                            currentGallery.ClientInfo.UserId = string.Empty;
                            currentGallery.ClientInfo.UserImg = string.Empty;
                            currentGallery.ClientInfo.UserFullInfo = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerHelper.ErrorFormat("用户信息:{0}", userInfoStr);
                        currentGallery.ClientInfo.UserId = string.Empty;
                        currentGallery.ClientInfo.UserImg = string.Empty;
                        currentGallery.ClientInfo.UserFullInfo = null;
                    }
                    #endregion
                }
            }
            return replyMessage;
        }
    }
}

