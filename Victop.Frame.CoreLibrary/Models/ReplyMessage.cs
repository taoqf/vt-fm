﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.CoreLibrary.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Victop.Frame.CoreLibrary.Enums;

	/// <summary>
	/// 应答消息体
	/// </summary>
	/// <remarks>应答消息体</remarks>
	public class ReplyMessage
	{
        private string messageId;
		/// <summary>
		/// 消息标识
		/// </summary>
		public virtual string MessageId
		{
            get { return messageId; }
            set { messageId = value; }
		}
        private ReplyModeEnum replyMode;
		/// <summary>
		/// 应答类型
		/// </summary>
		public virtual ReplyModeEnum ReplyMode
		{
            get { return replyMode; }
            set { replyMode = value; }
		}

        private string replyControl;
		/// <summary>
		/// 应答消息控制字段
		/// </summary>
		public virtual string ReplyControl
		{
            get { return replyControl; }
            set { replyControl = value; }
		}
        private string replyContent;
		/// <summary>
		/// 应答消息数据字段
		/// </summary>
		public virtual string ReplyContent
		{
            get { return replyContent; }
            set { replyContent = value; }
		}
        private string dataChannelId;
		/// <summary>
		/// 数据通道Id
		/// </summary>
		public virtual string DataChannelId
		{
            get { return dataChannelId; }
            set { dataChannelId = value; }
		}
        private string replyAlertMessage;
		/// <summary>
		/// 应答提示信息
		/// </summary>
		public virtual string ReplyAlertMessage
		{
            get { return replyAlertMessage; }
            set { replyAlertMessage = value; }
		}

        private string targetId;
        /// <summary>
        /// 目标Id
        /// </summary>
        public string TargetId
        {
            get { return targetId; }
            set { targetId = value; }
        }

	}
}

