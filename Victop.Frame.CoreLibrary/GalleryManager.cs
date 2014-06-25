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
    using Victop.Frame.PublicLib.Managers;

	/// <summary>
	/// 云通道管理
	/// </summary>
	/// <remarks>通道管理</remarks>
	public class GalleryManager
	{
		/// <summary>
		/// 添加通道信息
		/// </summary>
		public virtual bool AddGallery(CloudGalleryInfo galleryInfo)
		{
            if (galleryInfo == null)
                return false;
            if (CoreDataCollection.CloudGalleryList.ContainsKey(galleryInfo.CloudGalleryId.ToString()))
            {
                CoreDataCollection.CloudGalleryList.Remove(galleryInfo.CloudGalleryId.ToString());
                CoreDataCollection.CloudGalleryList.Add(galleryInfo.CloudGalleryId.ToString(), galleryInfo);
            }
            else
            {
                CoreDataCollection.CloudGalleryList.Add(galleryInfo.CloudGalleryId.ToString(), galleryInfo);
            }
            return true;
		}

		/// <summary>
		/// 修改通道信息
		/// </summary>
		public virtual bool UpdateGallery(string cloudId, CloudGalleryInfo galleryInfo)
		{
            if (string.IsNullOrEmpty(cloudId) || galleryInfo == null)
            {
                return false;
            }
            if (CoreDataCollection.CloudGalleryList.ContainsKey(cloudId))
            {
                CoreDataCollection.CloudGalleryList[cloudId] = galleryInfo;
                return true;
            }
            else
            {
                return false;
            }
		}

		/// <summary>
		/// 初始化云通道
		/// </summary>
		public void InitGalleryList()
		{
            GalleryManager galleryManager = new GalleryManager();
            //云通道包括三部分：1、自动发现本地MiniSOA;2、内置飞道云；3、通过配置文件读取企业云
            #region 读取企业云配置
            CloudGalleryInfo EnterPriseGalleryInfo = new CloudGalleryInfo();
            EnterPriseGalleryInfo.CloudGalleryId = GalleryEnum.ENTERPRISE;
            EnterPriseGalleryInfo.CloudAddress = ConfigManager.GetAttributeOfNodeByName("Client", "Server");
            EnterPriseGalleryInfo.RouterAddress = ConfigManager.GetAttributeOfNodeByName("Client","Router");
            EnterPriseGalleryInfo.IsNeedRouter = ConfigManager.GetAttributeOfNodeByName("Client", "IsNeedRouter") == "0" ? true : false;
            EnterPriseGalleryInfo.ClientId = ConfigManager.GetAttributeOfNodeByName("UserInfo", "ClientId");
            galleryManager.AddGallery(EnterPriseGalleryInfo);
            #endregion
            #region 内置飞道云
            CloudGalleryInfo victopGalleryInfo = new CloudGalleryInfo();
            victopGalleryInfo.CloudGalleryId = GalleryEnum.VICTOP;
            victopGalleryInfo.CloudAddress = "192.168.1.1:9999";
            victopGalleryInfo.RouterAddress = "192.168.1.1:9527";
            victopGalleryInfo.IsNeedRouter = true;
            galleryManager.AddGallery(victopGalleryInfo);
            #endregion
            #region 本地MiniSOA
            while (AutoDiscover.GetInstance().IsRun)
            {
                if (AutoDiscover.GetInstance().LocalServerList.Count > 0)
                {
                    foreach (string item in AutoDiscover.GetInstance().LocalServerList)
                    {
                        CloudGalleryInfo localGalleryInfo = new CloudGalleryInfo();
                        localGalleryInfo.CloudGalleryId = GalleryEnum.LOCALSOA;
                        localGalleryInfo.CloudAddress = item;
                        localGalleryInfo.IsNeedRouter = false;
                        galleryManager.AddGallery(localGalleryInfo);
                    }

                }
            }
            #endregion
        }

		/// <summary>
		/// 刷新云通道集合
		/// </summary>
		public virtual void RefreshGalleryList()
		{
			throw new System.NotImplementedException(); //TODO:方法实现
		}

		/// <summary>
		/// 获取通道信息
		/// </summary>
		public virtual CloudGalleryInfo GetGallery(string cloudId)
		{
            if (string.IsNullOrWhiteSpace(cloudId))
                return null;
            if (CoreDataCollection.CloudGalleryList.ContainsKey(cloudId))
            {
                return CoreDataCollection.CloudGalleryList[cloudId];
            }
            else
            {
                return CoreDataCollection.CloudGalleryList.Values.FirstOrDefault();
            }
		}
        /// <summary>
        /// 获取所有通道信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, CloudGalleryInfo> GetAllGalleryInfo()
        {
            return CoreDataCollection.CloudGalleryList;
        }
        /// <summary>
        /// 获取当前通道标识
        /// </summary>
        /// <returns></returns>
        public static GalleryEnum GetCurrentGalleryId()
        {
            return CoreDataCollection.CurrentGalleryId;
        }
        /// <summary>
        /// 修改当前通道标识
        /// </summary>
        /// <param name="galleryId"></param>
        public void SetCurrentGalleryId(GalleryEnum galleryId)
        {
            CoreDataCollection.CurrentGalleryId = galleryId;
        }
	}
}

