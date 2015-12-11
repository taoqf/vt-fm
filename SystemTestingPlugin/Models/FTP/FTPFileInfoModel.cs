using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace SystemTestingPlugin.Models
{
    /// <summary>
    /// FTP文件信息实体
    /// </summary>
    public class FTPFileInfoModel : PropertyModelBase
    {
        /// <summary>
        /// 文件名
        /// </summary>
        private string fileName;
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    RaisePropertyChanged(()=> FileName);
                }
            }
        }
        /// <summary>
        /// 文件大小
        /// </summary>
        private string fileSize;
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize
        {
            get
            {
                return fileSize;
            }
            set
            {
                if (fileSize != value)
                {
                    fileSize = value;
                    RaisePropertyChanged(()=> FileSize);
                }
            }
        }
        /// <summary>
        /// 文件类型
        /// </summary>
        private string fileType;
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType
        {
            get
            {
                return fileType;
            }
            set
            {
                if (fileType != value)
                {
                    fileType = value;
                    RaisePropertyChanged(()=> FileType);
                }
            }
        }
        /// <summary>
        /// 最近修改时间
        /// </summary>
        private DateTime lastModifyTime;
        /// <summary>
        /// 最近修改时间
        /// </summary>
        public DateTime LastModifyTime
        {
            get
            {
                return lastModifyTime;
            }
            set
            {
                if (lastModifyTime != value)
                {
                    lastModifyTime = value;
                    RaisePropertyChanged(()=> LastModifyTime);
                }
            }
        }
        /// <summary>
        /// 文件拥有者
        /// </summary>
        private string fileOwner;
        /// <summary>
        /// 文件拥有者
        /// </summary>
        public string FileOwner
        {
            get
            {
                return fileOwner;
            }
            set
            {
                if (fileOwner != value)
                {
                    fileOwner = value;
                    RaisePropertyChanged(()=> FileOwner);
                }
            }
        }
        /// <summary>
        /// 文件组
        /// </summary>
        private string fileGroup;
        /// <summary>
        /// 文件组
        /// </summary>
        public string FileGroup
        {
            get
            {
                return fileGroup;
            }
            set
            {
                if (fileGroup != value)
                {
                    fileGroup = value;
                    RaisePropertyChanged(()=> FileGroup);
                }
            }
        }
        /// <summary>
        /// 是否为目录
        /// </summary>
        private bool isDirectory;
        /// <summary>
        /// 是否为目录
        /// </summary>
        public bool IsDirectory
        {
            get
            {
                return isDirectory;
            }
            set
            {
                if (isDirectory != value)
                {
                    isDirectory = value;
                    RaisePropertyChanged(()=> IsDirectory);
                }
            }
        }
        private string flags;

        public string Flags
        {
            get
            {
                return flags;
            }
            set
            {
                if (flags != value)
                {
                    flags = value;
                    RaisePropertyChanged(()=> Flags);
                }
            }
        }
    }
}
