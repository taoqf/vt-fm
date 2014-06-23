﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.DataChannel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Victop.Frame.CoreLibrary.Interfaces;

	/// <summary>
	/// 数据操作
	/// </summary>
	/// <remarks>数据操作类</remarks>
	public class DataOperation
	{
		/// <summary>
		/// 根据通道号获取数据集
		/// </summary>
		/// <param name="channelId">通道号</param>
		public virtual DataSet GetData(string channelId)
		{
            DataChannelManager dataChannelManager = new DataChannelManager();
            Hashtable hashData = dataChannelManager.GetData(channelId);
            ChannelData channelData = hashData["Data"] as ChannelData;
            return channelData.DataInfo;
           
		}
    }
}

