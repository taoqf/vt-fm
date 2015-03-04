using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace SystemTestingPlugin.Models
{
    public class FTPSiteInfoModel : PropertyModelBase
    {
        /// <summary>
        /// 主机地址
        /// </summary>
        private string hostUrl;
        /// <summary>
        /// 主机地址
        /// </summary>
        public string HostUrl
        {
            get
            {
                return hostUrl;
            }
            set
            {
                if (hostUrl != value)
                {
                    hostUrl = value;
                    RaisePropertyChanged("HostUrl");
                }
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        private string userName;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (userName != value)
                {
                    userName = value;
                    RaisePropertyChanged("UserName");
                }
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        private string userPwd;
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPwd
        {
            get
            {
                return userPwd;
            }
            set
            {
                if (userPwd != value)
                {
                    userPwd = value;
                    RaisePropertyChanged("UserPwd");
                }
            }
        }
        /// <summary>
        /// 主机端口
        /// </summary>
        private long hostPort = 21;
        /// <summary>
        /// 主机端口
        /// </summary>
        public long HostPort
        {
            get
            {
                return hostPort;
            }
            set
            {
                if (hostPort != value)
                {
                    hostPort = value;
                    RaisePropertyChanged("HostPort");
                }
            }
        }
        /// <summary>
        /// 远端路径
        /// </summary>
        private string remotePath;
        /// <summary>
        /// 远端路径
        /// </summary>
        public string RemotePath
        {
            get
            {
                return remotePath;
            }
            set
            {
                if (remotePath != value)
                {
                    remotePath = value;
                    RaisePropertyChanged("RemotePath");
                }
            }
        }
        /// <summary>
        /// 本地路径
        /// </summary>
        private string localPath;
        /// <summary>
        /// 本地路径
        /// </summary>
        public string LocalPath
        {
            get
            {
                return localPath;
            }
            set
            {
                if (localPath != value)
                {
                    localPath = value;
                    RaisePropertyChanged("LocalPath");
                }
            }
        }
    }
}
