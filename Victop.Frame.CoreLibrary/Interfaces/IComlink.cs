﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.CoreLibrary.Interfaces
{
    using Victop.Frame.CoreLibrary.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Victop.Frame.CoreLibrary.AbsClasses;

	/// <summary>
	/// 通信接口
	/// </summary>
	/// <remarks>通信器接口</remarks>
	public interface IComlink 
	{
		/// <summary>
		/// 初始化通信器
		/// </summary>
		void Init(Base baseI);

		/// <summary>
		/// 获取通信器状态
		/// </summary>
		int GetStatus();

		/// <summary>
		/// 发送消息
		/// </summary>
		ReplyMessage SendMessage(RequestMessage messageInfo);

		/// <summary>
		/// 启动本地服务
		/// </summary>
		string StartLocalServer(string ip, string port);

		/// <summary>
		/// 重置通信器
		/// </summary>
		void Reset();

		/// <summary>
		/// 停止通信器
		/// </summary>
		void Stop();

		/// <summary>
		/// 检查回调节点
		/// </summary>
		bool CheckCallBackEndPoint(string target);

		/// <summary>
		/// 转移节点
		/// </summary>
		bool TransferEndPoint(string sender, string target);

	}
}

