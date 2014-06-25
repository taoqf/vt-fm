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
using Victop.Frame.CoreLibrary.Enums;

	/// <summary>
	/// 核心数据集合(静态类)
	/// </summary>
	/// <remarks>核心数据集合(静态类)</remarks>
	internal static class CoreDataCollection
	{
        private static Dictionary<string, BaseResourceInfo> baseResourceList;
		/// <summary>
		/// 基础资源集合
        /// 主键值:通道标识
		/// </summary>
		public static Dictionary<string,BaseResourceInfo> BaseResourceList
		{
            get
            {
                if (baseResourceList == null)
                    baseResourceList = new Dictionary<string, BaseResourceInfo>();
                return baseResourceList;
            }
            set
            {
                baseResourceList = value;
            }
		}

        private static Dictionary<string, CloudGalleryInfo> cloudGalleryList;
		/// <summary>
		/// 云通道集合
        /// 主键值：通道标识
		/// </summary>
		public static Dictionary<string,CloudGalleryInfo> CloudGalleryList
		{
            get
            {
                if (cloudGalleryList == null)
                    cloudGalleryList = new Dictionary<string, CloudGalleryInfo>();
                return cloudGalleryList;
            }
            set
            {
                cloudGalleryList = value;
            }
		}
        private static Dictionary<string, ActivePluginInfo> activePluginList;
		/// <summary>
		/// 活动插件列表
        /// 主键值：插件对象标识
		/// </summary>
        public static Dictionary<string, ActivePluginInfo> ActivePluginList
        {
            get
            {
                if (activePluginList == null)
                    activePluginList = new Dictionary<string, ActivePluginInfo>();
                return activePluginList;
            }
            set
            {
                activePluginList = value;
            }
        }
        private static Dictionary<string, Dictionary<string, RegisterServerInfo>> registerServerList;
		/// <summary>
		/// 注册服务列表
        /// 主键值：云通道标识
        /// 每个通道中的键值对唯一标识用服务名称
		/// </summary>
        public static Dictionary<string, Dictionary<string, RegisterServerInfo>> RegisterServerList
		{
            get
            {
                if (registerServerList == null)
                    registerServerList = new Dictionary<string, Dictionary<string, RegisterServerInfo>>();
                return registerServerList;
            }
            set
            {
                registerServerList = value;
            }
		}


        private static GalleryEnum currentGalleryId = GalleryEnum.ENTERPRISE;
        /// <summary>
        /// 当前通道标识
        /// </summary>
        public static GalleryEnum CurrentGalleryId
        {
            get { return currentGalleryId; }
            set { currentGalleryId = value; }
        }
	}
}

