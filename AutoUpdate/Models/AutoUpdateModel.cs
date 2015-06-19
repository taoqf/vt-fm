using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoUpdate.Models
{
    public class AutoUpdateModel
    {
        /// <summary>
        /// 更新地址
        /// </summary>
        private string updateUrl;
        /// <summary>
        /// 更新地址
        /// </summary>
        public string UpdateUrl
        {
            get { return updateUrl; }
            set { updateUrl = value; }
        }
        /// <summary>
        /// 本地更新时间
        /// </summary>
        private long localUpdateTimestamp;
        /// <summary>
        /// 本地更新时间
        /// </summary>
        public long LocalUpdateTimestamp
        {
            get { return localUpdateTimestamp; }
            set { localUpdateTimestamp = value; }
        }
        /// <summary>
        /// 服务端更新时间
        /// </summary>
        private long serverUpdateTimestamp;
        /// <summary>
        /// 服务端更新时间
        /// </summary>
        public long ServerUpdateTimestamp
        {
            get { return serverUpdateTimestamp; }
            set { serverUpdateTimestamp = value; }
        }
        /// <summary>
        /// 服务端更新版本号
        /// </summary>
        private string serverUpdaterVersion;
        /// <summary>
        /// 服务端更新版本号
        /// </summary>
        public string ServerUpdaterVersion
        {
            get { return serverUpdaterVersion; }
            set { serverUpdaterVersion = value; }
        }
        /// <summary>
        /// 下载文件名称
        /// </summary>
        private string loadingFileName;
        /// <summary>
        /// 下载文件名称
        /// </summary>
        public string LoadingFileName
        {
            get { return loadingFileName; }
            set { loadingFileName = value; }
        }
        /// <summary>
        /// 下载文件所属文件夹
        /// </summary>
        private string loadingFilePath;
        /// <summary>
        /// 下载文件所属文件夹
        /// </summary>
        public string LoadingFilePath
        {
            get { return loadingFilePath; }
            set { loadingFilePath = value; }
        }
        /// <summary>
        /// 当前文件大小
        /// </summary>
        private long loadingFileSize;
        /// <summary>
        /// 当前文件大小
        /// </summary>
        public long LoadingFileSize
        {
            get { return loadingFileSize; }
            set { loadingFileSize = value; }
        }
        /// <summary>
        /// 所有文件大小
        /// </summary>
        private long totalSize;
        /// <summary>
        /// 所有文件大小
        /// </summary>
        public long TotalSize
        {
            get { return totalSize; }
            set { totalSize = value; }
        }
        /// <summary>
        /// 文件总数
        /// </summary>
        private long totalCount;
        /// <summary>
        /// 文件总数
        /// </summary>
        public long TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }
        /// <summary>
        /// 文件集合
        /// </summary>
        private List<string> fileNames;
        /// <summary>
        /// 文件集合
        /// </summary>
        public List<string> FileNames
        {
            get { return fileNames; }
            set { fileNames = value; }
        }
        /// <summary>
        /// 已更新文件数
        /// </summary>
        private long updatedNum;
        /// <summary>
        /// 已更新文件数
        /// </summary>
        public long UpdatedNum
        {
            get { return updatedNum; }
            set { updatedNum = value; }
        }
        /// <summary>
        /// 已更新文件大小
        /// </summary>
        private long updateSize;
        /// <summary>
        /// 已更新文件大小
        /// </summary>
        public long UpdateSize
        {
            get { return updateSize; }
            set { updateSize = value; }
        }

      


    }
}
