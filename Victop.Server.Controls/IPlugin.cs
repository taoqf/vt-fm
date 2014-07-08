﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Server.Controls
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

	/// <summary>
	/// 插件接口
	/// </summary>
	public interface IPlugin 
	{
		/// <summary>
		/// 插件标题
		/// </summary>
		string PluginTitle { get; }

		/// <summary>
		/// 插件名称
		/// </summary>
		string PluginName { get;}

        /// <summary>
        /// 插件接收消息类型列表
        /// </summary>
        List<string> ServiceReceiptMessageType { get; }
		/// <summary>
		/// 显示类型
        /// 0:窗口
        /// 1:UserControl
		/// </summary>
		int ShowType { get; }

		/// <summary>
		/// 系统插件
		/// </summary>
		int SystemPlugin { get;}

		/// <summary>
		/// 自动初始化
		/// </summary>
		int AutoInit { get;}

        /// <summary>
        /// 起始窗口
        /// </summary>
        Window StartWindow { get; }
        /// <summary>
        /// 起始控件
        /// </summary>
        UserControl StartControl { get; }
        /// <summary>
        /// 初始化方法
        /// </summary>
		void Init();
        /// <summary>
        /// 参数键值对
        /// </summary>
        Dictionary<string, object> ParamDict { get; set; }

	}
}

