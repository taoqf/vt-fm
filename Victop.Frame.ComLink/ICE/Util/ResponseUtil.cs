﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.ComLink.ICE.Util
{
    using slice;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Victop.Frame.CoreLibrary.Enums;
    using Victop.Frame.CoreLibrary.Models;
    using Victop.Frame.PublicLib.Helpers;

	/// <summary>
	/// 应答处理单元
	/// </summary>
	/// <remarks>应答处理单元</remarks>
	public class ResponseUtil
	{
		/// <summary>
		/// 应答消息类成员直接转为ICE应答消息结构体
		/// </summary>
        public static Reply ToReply(ReplyMessage messageInfo)
		{
            Reply rtRely = new Reply();
            string Content = "";
            try
            {
                Dictionary<string, string> map = new Dictionary<string, string>();
                map.Add("replyControl", messageInfo.ReplyControl);
                map.Add("replyContent", messageInfo.ReplyContent);
                Content = JsonHelper.ToJson(map);
                rtRely.replyMode = (int)messageInfo.ReplyMode;
                rtRely.replyContent = Content;
            }
            catch (Exception)
            { }
            return rtRely;
		}

		/// <summary>
		/// ICE应答消息结构体转为应答消息类成员
		/// </summary>
        public static ReplyMessage ToReplyMessage(Reply reply)
		{
            ReplyMessage replyMessage = new ReplyMessage();
            try
            {
                replyMessage.ReplyMode = (ReplyModeEnum)reply.replyMode;
                string Content = reply.replyContent;
                replyMessage.ReplyControl = JsonHelper.ReadJsonString(Content, "replyControl");
                replyMessage.ReplyContent = JsonHelper.ReadJsonString(Content, "replyContent");
            }
            catch (Exception)
            { }
            return replyMessage;
		}

	}
}

