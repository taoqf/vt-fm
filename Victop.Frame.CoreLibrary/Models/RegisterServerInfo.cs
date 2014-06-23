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
	/// 注册服务信息
    /// 云通道标识与服务名称联合唯一
	/// </summary>
	/// <remarks>注册服务信息</remarks>
	public class RegisterServerInfo
	{
        private string cloudGalleryId;
		/// <summary>
		/// 云通道标识
		/// </summary>
		public virtual string CloudGalleryId
		{
            get { return cloudGalleryId; }
            set { cloudGalleryId = value; }
		}
        private ServerTypeEnum serverType;
		/// <summary>
		/// 服务类型
		/// </summary>
		public virtual ServerTypeEnum ServerType
		{
            get { return serverType; }
            set { serverType = value; }
		}
        private List<string> receiptMessageType;
		/// <summary>
		/// 接收消息类型
		/// </summary>
		public virtual List<string> ReceiptMessageType
		{
            get
            {
                if (receiptMessageType == null)
                    receiptMessageType = new List<string>();
                return receiptMessageType;
            }
            set
            {
                receiptMessageType = value;
            }
		}
        private string serverName;
        /// <summary>
        /// 服务名称
        /// </summary>
        public virtual string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }
        private string serverPath;
		/// <summary>
		/// 服务路径
		/// </summary>
		public virtual string ServerPath
		{
            get { return serverPath; }
            set { serverPath = value; }
		}
        private ResourceEnum serverStatus;
		/// <summary>
		/// 服务状态
		/// </summary>
		public virtual ResourceEnum ServerStatus
		{
            get { return serverStatus; }
            set { serverStatus = value; }
		}

	}
}

