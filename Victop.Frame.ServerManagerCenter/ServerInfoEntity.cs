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

	/// <summary>
	/// 服务信息实体类
	/// </summary>
	/// <remarks>服务信息实体类</remarks>
	public class ServerInfoEntity
	{
		/// <summary>
		/// 服务请求消息
		/// </summary>
		public virtual RequestMessage ServerMessage
		{
			get;
			set;
		}

		/// <summary>
		/// 消息回调
		/// </summary>
		public virtual string MessageCallBack
		{
			get;
			set;
		}

		/// <summary>
		/// 服务信息标识(消息id+服务名称)
		/// </summary>
		public virtual string ServerInfoKey
		{
			get;
			set;
		}

	}
}

