using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace SystemTestingPlugin.Models
{
    /// <summary>
    /// 下载信息实体
    /// </summary>
    public class DownLoadInfoModel:PropertyModelBase
    {
        /// <summary>
        /// 文件Id
        /// </summary>
        private string fieldId;
        /// <summary>
        /// 文件Id
        /// </summary>
        public string FieldId
        {
            get
            {
                return fieldId;
            }
            set
            {
                if (fieldId != value)
                {
                    fieldId = value;
                    RaisePropertyChanged(()=> FieldId);
                }
            }
        }
        /// <summary>
        /// 下载存储地址
        /// </summary>
        private string downLoadPath;
        /// <summary>
        ///下载存储地址
        /// </summary>
        public string DownLoadPath
        {
            get
            {
                return downLoadPath;
            }
            set
            {
                if (downLoadPath != value)
                {
                    downLoadPath = value;
                    RaisePropertyChanged(()=> DownLoadPath);
                }
            }
        }
        /// <summary>
        /// 产品Id
        /// </summary>
        private string productId;
        /// <summary>
        /// 产品Id
        /// </summary>
        public string ProductId
        {
            get
            {
                return productId;
            }
            set
            {
                if (productId != value)
                {
                    productId = value;
                    RaisePropertyChanged(()=> ProductId);
                }
            }
        }
        /// <summary>
        /// 下载结果
        /// </summary>
        private string downLoadResult;
        /// <summary>
        /// 下载结果
        /// </summary>
        public string DownLoadResult
        {
            get
            {
                return downLoadResult;
            }
            set
            {
                if (downLoadResult != value)
                {
                    downLoadResult = value;
                    RaisePropertyChanged(()=> DownLoadResult);
                }
            }
        }
    }
}
