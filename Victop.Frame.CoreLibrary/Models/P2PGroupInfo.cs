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

	/// <summary>
	/// p2p客户端信息
	/// </summary>
	/// <remarks>P2P组信息</remarks>
	public class P2PGroupInfo
	{
        private string clientKey;
		/// <summary>
		/// p2pClient标识
		/// </summary>
		public virtual string ClientKey
		{
            get { return clientKey; }
            set { clientKey = value; }
		}
        private string clientDetialId;
		/// <summary>
		/// p2p客户端内容标识
		/// </summary>
		public virtual string ClientDetialId
		{
            get { return clientDetialId; }
            set { clientDetialId = value; }
		}
        private int onlineStatus;
		/// <summary>
		/// 是否在线
		/// </summary>
		public virtual int OnlineStatus
		{
            get { return onlineStatus; }
            set { onlineStatus = value; }
		}

	}
}

