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
    using Victop.Frame.CoreLibrary;
    using Victop.Frame.CoreLibrary.Enums;
    using Victop.Frame.CoreLibrary.Models;
    using Victop.Frame.DataChannel;
    using Victop.Frame.PublicLib.Helpers;
    /// <summary>
    /// 应答消息解析器
    /// </summary>
    /// <remarks>应答消息解析器</remarks>
    public class ReplyMessageResolver
    {
        /// <summary>
        /// 应答消息数据部分
        /// </summary>
        private ReplyMessage DataReplyMessage
        {
            get;
            set;
        }

        /// <summary>
        /// 应答消息状态部分
        /// </summary>
        private ReplyMessage MessageStatusReplyMessage
        {
            get;
            set;
        }

        /// <summary>
        /// 分解应答消息体的数据部分和状态部分
        /// </summary>
        public virtual ReplyMessage ResolveReplyMessage(ReplyMessage replyMessageInfo, RequestMessage messageInfo)
        {
            DataReplyMessage = new ReplyMessage()
            {
                MessageId = messageInfo.MessageId,
                ReplyContent = replyMessageInfo.ReplyContent
            };
            string channelId = string.Empty;
            try
            {
                if (ReplyToDataChannel(DataReplyMessage, messageInfo, out channelId))
                {
                    MessageStatusReplyMessage = new ReplyMessage()
                    {
                        MessageId = messageInfo.MessageId,
                        DataChannelId = channelId,
                        ReplyMode = ReplyModeEnum.SYNCH
                    };
                    return MessageStatusReplyMessage;
                }
                else
                {
                    MessageStatusReplyMessage = new ReplyMessage()
                    {
                        MessageId = messageInfo.MessageId,
                        ReplyMode = ReplyModeEnum.CAST
                    };
                }
            }
            catch (Exception ex)
            {
                MessageStatusReplyMessage = new ReplyMessage()
                {
                    MessageId = messageInfo.MessageId,
                    ReplyMode = ReplyModeEnum.CAST,
                    ReplyAlertMessage=ex.Message
                };
            }
            return MessageStatusReplyMessage;
        }

        /// <summary>
        /// 应答消息数据部分保存到数据通道
        /// </summary>
        private bool ReplyToDataChannel(ReplyMessage replyMessageInfo, RequestMessage messageInfo, out string channelId)
        {
            channelId = string.Empty;
            //与数据通道连接，将MessageInfo中的数据部分存储到数据通道中，
            //通道标识使用消息Id
            ReplyMessage ReplyMessage = new ReplyMessage();
            DataCreateManager dataCreateManager = new DataCreateManager();
            bool result = false;
            ReplyMessage = dataCreateManager.SendReplyMessage(replyMessageInfo, messageInfo);
            channelId = ReplyMessage.DataChannelId;
            if (string.IsNullOrEmpty(channelId))
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 更新核心类库服务注册表
        /// </summary>
        private bool RefreshRegisterServerList(List<RegisterServerInfo> registerServerList)
        {
            RegisterServerManager registerServerManager = new RegisterServerManager();
            bool result = false;
            try
            {
                foreach (RegisterServerInfo item in registerServerList)
                {
                    result = registerServerManager.RegisterServer(item);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }

        /// <summary>
        /// 更新菜单资源信息表
        /// </summary>
        public virtual void RefreshBaseResouceList(ReplyMessage messageInfo)
        {
            BaseResourceManager baseResourceManager = new BaseResourceManager();
            BaseResourceInfo baseResourceInfo = new BaseResourceInfo() { GalleryId = GalleryManager.GetCurrentGalleryId(), ResourceXml = messageInfo.ReplyContent };
            bool result = baseResourceManager.AddResouce(baseResourceInfo);
            if (result)
            {
                //TODO:提取基础资源信息xml中的服务部分，形成一个List集合
                List<RegisterServerInfo> serverInfoList = new List<RegisterServerInfo>();
                Dictionary<string, BaseResourceInfo> resourceInfoDic = baseResourceManager.GetAllBaseResource();
                foreach (string key in resourceInfoDic.Keys)
                {
                    RegisterServerInfo registerServerInfo = new RegisterServerInfo();
                    string messageType = JsonHelper.ReadJsonString(resourceInfoDic[key].ResourceXml, "messageType");
                    RegisterServerManager serverManager = new RegisterServerManager();
                    registerServerInfo = serverManager.GetServer(GalleryManager.GetCurrentGalleryId().ToString(), messageType);
                    if (registerServerInfo != null)
                    {
                        serverInfoList.Add(registerServerInfo);
                    }

                }
                RefreshRegisterServerList(serverInfoList);
            }
        }
        /// <summary>
        /// 根据数据通道id获取数据的xml
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public string GetCurdDataByDataChannelId(string channelId, bool masterFlag = true)
        {
            DataCreateManager dataManager = new DataCreateManager();
            string curdData = dataManager.GetCurdData(channelId, masterFlag);
            return curdData;
        }
        /// <summary>
        /// 提交数据集变更
        /// </summary>
        /// <param name="channelId">通道号Id</param>
        /// <returns></returns>
        public bool CommitDataSetChange(string channelId)
        {
            DataCreateManager createManger = new DataCreateManager();
            return createManger.CommitChannelData(channelId);
        }

    }
}

