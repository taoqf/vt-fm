﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.ServerManagerCenter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Victop.Frame.CoreLibrary.Models;
    using Victop.Frame.PublicLib.Helpers;

	/// <summary>
	/// 服务管理路由器:区分查询服务及启动服务
	/// </summary>
	/// <remarks>服务路由器</remarks>
	public class ServerRouter
	{
		/// <summary>
		/// 确定发往启动器或判定器
        /// <param name="messageInfo">消息体</param>
        /// <param name="ramifyFlag">判定标识</param>
		/// </summary>
        public virtual ReplyMessage ServerRamify(RequestMessage messageInfo, RamifyEnum ramifyFlag)
		{
            ReplyMessage replyMessage = null;
            if (messageInfo != null && !string.IsNullOrEmpty(messageInfo.MessageType))
            {
                switch (ramifyFlag)
                {
                    case RamifyEnum.RUN:
                        string replyContent = ServerInitiator.GetInstance().Run(messageInfo);
                        try
                        {
                            Dictionary<string, object> replyDic = JsonHelper.ToObject<Dictionary<string, object>>(replyContent);
                            replyMessage = new ReplyMessage()
                            {
                                MessageId = messageInfo.MessageId,
                                ReplyContent = replyDic["ReplyContent"].ToString(),
                                ReplyAlertMessage = replyDic["ReplyContent"].ToString(),
                            };
                            if (replyDic["ReplyMode"].Equals("0"))
                            {
                                replyMessage.ReplyMode = (CoreLibrary.Enums.ReplyModeEnum)0;
                            }
                            else
                            {
                                replyMessage.ReplyMode = (CoreLibrary.Enums.ReplyModeEnum)1;
                            }
                        }
                        catch (Exception ex)
                        {
                            replyMessage = new ReplyMessage()
                            {
                                MessageId = messageInfo.MessageId,
                                ReplyMode = (CoreLibrary.Enums.ReplyModeEnum)0,
                                ReplyAlertMessage=ex.Message
                            };
                        }
                        break;
                    case RamifyEnum.CHECK:
                        ServerJudgeManager serverJudgeManager = new ServerJudgeManager();
                        RegisterServerInfo registerServerInfo = null;
                        if (serverJudgeManager.JudgeServerInfo(messageInfo, out registerServerInfo))
                        {
                            replyMessage = new ReplyMessage();
                            replyMessage.MessageId = messageInfo.MessageId;
                            replyMessage.ReplyContent = JsonHelper.ToJson(registerServerInfo);
                        }
                        break;
                    default:
                        break;
                }
            }
            return replyMessage;
		}

	}
}

