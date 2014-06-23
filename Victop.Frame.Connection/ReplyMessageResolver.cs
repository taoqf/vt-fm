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
		public virtual ReplyMessage DataReplyMessage
		{
			get;
			set;
		}

		/// <summary>
		/// 应答消息状态部分
		/// </summary>
		public virtual ReplyMessage MessageStatusReplyMessage
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
            if (ReplyToDataChannel(DataReplyMessage,messageInfo))
            {
                MessageStatusReplyMessage = new ReplyMessage()
                {
                    MessageId = messageInfo.MessageId,
                    DataChannelId = messageInfo.MessageId,
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
            return MessageStatusReplyMessage;
		}

		/// <summary>
		/// 应答消息数据部分保存到数据通道
		/// </summary>
        private bool ReplyToDataChannel(ReplyMessage replyMessageInfo,RequestMessage messageInfo)
        {
            //与数据通道连接，将MessageInfo中的数据部分存储到数据通道中，
            //通道标识使用消息Id
            ReplyMessage ReplyMessage = new ReplyMessage();
            DataCreateManager dataCreateManager = new DataCreateManager();
            bool result = false;
            try
            {
                ReplyMessage = dataCreateManager.SendReplyMessage(replyMessageInfo, messageInfo);
                //保存成功后DataChannelId=MessageId
                if (string.IsNullOrEmpty(ReplyMessage.DataChannelId)) 
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }
            catch (Exception)
            {

                return false;
            }
            return result;
        }

		/// <summary>
		/// 应答状态部分返回到消息管理器(丢弃)
		/// </summary>
		public virtual void ReplyToMessageManager(ReplyMessage messageInfo)
		{
			throw new System.NotImplementedException(); //TODO:方法实现
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
                Dictionary<string,BaseResourceInfo> resourceInfoDic= baseResourceManager.GetAllBaseResource();
                foreach (string key in resourceInfoDic.Keys)
                {
                    RegisterServerInfo registerServerInfo = new RegisterServerInfo();
                    string messageType = JsonHelper.ReadJsonString(resourceInfoDic[key].ResourceXml, "messageType");
                    RegisterServerManager serverManager = new RegisterServerManager();
                    registerServerInfo=serverManager.GetServer(GalleryManager.GetCurrentGalleryId().ToString(), messageType);
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
        public string GetDataXmlByDataChannelId(string channelId)
        {
            DataCreateManager dataManager = new DataCreateManager();
            string dataXml = dataManager.GetXmlData(channelId);
            return dataXml;
        }

	}
}

