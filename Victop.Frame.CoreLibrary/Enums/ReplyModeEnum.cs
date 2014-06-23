﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.CoreLibrary.Enums
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// 应答类型枚举
	/// </summary>
	/// <remarks>应答模式</remarks>
	public enum ReplyModeEnum : int
	{
		/// <summary>
		/// 丢弃处理
		/// </summary>
		CAST,
		/// <summary>
		/// 同步处理
		/// </summary>
		SYNCH,
		/// <summary>
		/// 异步处理
		/// </summary>
		ASYNC,
		/// <summary>
		/// 转发处理
		/// </summary>
		ROUTER,
		/// <summary>
		/// 中断阻塞
		/// </summary>
		BREAK,
	}
}
