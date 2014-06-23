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
    /// 可用服务资源列表
    /// </summary>
    /// <remarks>基础资源信息</remarks>
    public class BaseResourceInfo
    {
        private string resourceName;
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get { return resourceName; }
            set { resourceName = value; }
        }
        private string resourceVersion;
        /// <summary>
        /// 资源版本
        /// </summary>
        public virtual string ResourceVersion
        {
            get { return resourceVersion; }
            set { resourceVersion = value; }
        }
        private List<string> receiptMessageType;
        /// <summary>
        /// 处理消息列表
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
        private ResourceEnum serverStatus;
        /// <summary>
        /// 服务状态
        /// </summary>
        public virtual ResourceEnum ServerStatus
        {
            get { return serverStatus; }
            set { serverStatus = value; }
        }
        private GalleryEnum galleryId;
        /// <summary>
        /// 通道标识Id
        /// </summary>
        public virtual GalleryEnum GalleryId
        {
            get { return galleryId; }
            set { galleryId = value; }
        }
        private string resourceId;
        /// <summary>
        /// 资源标识Id
        /// </summary>
        public virtual string ResourceId
        {
            get { return resourceId; }
            set { resourceId = value; }
        }
        private string resourceXml;
        /// <summary>
        /// 资源结构
        /// </summary>
        public string ResourceXml
        {
            get { return resourceXml; }
            set { resourceXml = value; }
        }

        private List<MenuInfo> resourceMnenus;
        /// <summary>
        /// 菜单资源集合
        /// </summary>
        public List<MenuInfo> ResourceMnenus
        {
            get { return resourceMnenus; }
            set { resourceMnenus = value; }
        }

    }
}

