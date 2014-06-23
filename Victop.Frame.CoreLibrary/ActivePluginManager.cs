﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成
//     如果重新生成代码，将丢失对此文件所做的更改。
// </auto-generated>
//------------------------------------------------------------------------------
namespace Victop.Frame.CoreLibrary
{
	using Victop.Frame.CoreLibrary.Models;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// 活动插件管理
	/// </summary>
	/// <remarks>活动插件管理</remarks>
	public class ActivePluginManager
	{
		/// <summary>
		/// 添加插件
		/// </summary>
		public virtual bool AddPlugin(ActivePluginInfo pluginInfo)
		{
            if (pluginInfo == null)
                return false;
            if (CoreDataCollection.ActivePluginList.ContainsKey(pluginInfo.ObjectId))
            {
                CoreDataCollection.ActivePluginList.Remove(pluginInfo.ObjectId);
                CoreDataCollection.ActivePluginList.Add(pluginInfo.ObjectId, pluginInfo);
            }
            else
            {
                CoreDataCollection.ActivePluginList.Add(pluginInfo.ObjectId, pluginInfo);
            }
            return true;
		}

		/// <summary>
		/// 移除插件
		/// </summary>
		public virtual bool RemovePlugin(ActivePluginInfo pluginInfo)
		{
            if (pluginInfo == null)
                return false;
            if (CoreDataCollection.ActivePluginList.ContainsKey(pluginInfo.ObjectId))
            {
                CoreDataCollection.ActivePluginList.Remove(pluginInfo.ObjectId);
                return true;
            }
            return false;
		}

		/// <summary>
		/// 获取插件
		/// </summary>
		public virtual ActivePluginInfo GetPlugin(ActivePluginInfo pluginInfo)
		{
            ActivePluginInfo activPluginInfo = null;
            if (activPluginInfo != null && CoreDataCollection.ActivePluginList.Count > 0)
            {
                activPluginInfo = CoreDataCollection.ActivePluginList.Values.FirstOrDefault(it => it.BusinessKey == pluginInfo.BusinessKey);
            }
            return activPluginInfo;
		}
        /// <summary>
        /// 获取所有活动插件
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ActivePluginInfo> GetActivePlugins()
        {
            return CoreDataCollection.ActivePluginList;
        }

	}
}

