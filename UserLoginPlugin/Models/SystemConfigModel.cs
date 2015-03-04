using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Victop.Server.Controls.Models;

namespace UserLoginPlugin.Models
{
    public class SystemConfigModel:ModelBase
    {
        #region 系统配置
        private string appName;
        /// <summary>应用程序名称 </summary>
        public string AppName
        {
            get
            {
                return appName;
            }
            set
            {
                if (appName!=value)
                {
                    appName = value;
                    RaisePropertyChanged("AppName");
                }
            }
        }

        private string mode;
        /// <summary> 模式</summary>
        public string Mode
        {
            get
            {
                return mode;
            }
            set
            {
                if (mode != value)
                {
                    mode = value;
                    RaisePropertyChanged("Mode");
                }
            }
        }

        private string comLink;
        /// <summary> 连接器状态</summary>
        public string ComLink
        {
            get
            {
                return comLink;
            }
            set
            {
                if (comLink != value)
                {
                    comLink = value;
                    RaisePropertyChanged("ComLink");
                }
            }
        }

        private string autoSearch;
        /// <summary>是否自动检索 </summary>
        public string AutoSearch
        {
            get
            {
                return autoSearch;
            }
            set
            {
                if (autoSearch != value)
                {
                    autoSearch = value;
                    RaisePropertyChanged("AutoSearch");
                }
            }
        }

        private string startPoint;
        /// <summary> 起始端口</summary>
        public string StartPoint
        {
            get
            {
                return startPoint;
            }
            set
            {
                if (startPoint != value)
                {
                    startPoint = value;
                    RaisePropertyChanged("StartPoint");
                }
            }
        }

        private string endPoint;
        /// <summary>结束端口 </summary>
        public string EndPoint
        {
            get
            {
                return endPoint;
            }
            set
            {
                if (endPoint != value)
                {
                    endPoint = value;
                    RaisePropertyChanged("EndPoint");
                }
            }
        }

        private string broadCastTime;
        /// <summary>广播次数 </summary>
        public string BroadCastTime
        {
            get
            {
                return broadCastTime;
            }
            set
            {
                if (broadCastTime != value)
                {
                    broadCastTime = value;
                    RaisePropertyChanged("BroadCastTime");
                }
            }
        }

        #endregion

        #region 日志配置
        private string debug;
        /// <summary>是否调试状态 </summary>
        public string Debug
        {
            get
            {
                return debug;
            }
            set
            {
                if (debug != value)
                {
                    debug = value;
                    RaisePropertyChanged("Debug");
                }
            }
        }

        private string clean;
        /// <summary>是否清理 </summary>
        public string Clean
        {
            get
            {
                return clean;
            }
            set
            {
                if (clean != value)
                {
                    clean = value;
                    RaisePropertyChanged("Clean");
                }
            }
        }

        private string unit;
        /// <summary>清理单位 </summary>
        public string Unit
        {
            get
            {
                return unit;
            }
            set
            {
                if (unit != value)
                {
                    unit = value;
                    RaisePropertyChanged("Unit");
                }
            }
        }

        private string num;
        /// <summary> 日志文件个数</summary>
        public string Num
        {
            get
            {
                return num;
            }
            set
            {
                if (num != value)
                {
                    num = value;
                    RaisePropertyChanged("Num");
                }
            }
        }
        #endregion

        #region 云服务通信设置
        private string cloudServerIP;
        /// <summary>云服务地址 </summary>
        public string CloudServerIP
        {
            get
            {
                return cloudServerIP;
            }
            set
            {
                if (cloudServerIP != value)
                {
                    cloudServerIP = value;
                    RaisePropertyChanged("CloudServerIP");
                }
            }
        }

        private string cloudServerPort;
        /// <summary>云服务端口 </summary>
        public string CloudServerPort
        {
            get
            {
                return cloudServerPort;
            }
            set
            {
                if (cloudServerPort != value)
                {
                    cloudServerPort = value;
                    RaisePropertyChanged("CloudServerPort");
                }
            }
        }

        private string cloudRouterIP;
        /// <summary>云服务路由地址 </summary>
        public string CloudRouterIP
        {
            get
            {
                return cloudRouterIP;
            }
            set
            {
                if (cloudRouterIP != value)
                {
                    cloudRouterIP = value;
                    RaisePropertyChanged("CloudRouterIP");
                }
            }
        }

        private string cloudRouterPort;
        /// <summary>云服务路由端口 </summary>
        public string CloudRouterPort
        {
            get
            {
                return cloudRouterPort;
            }
            set
            {
                if (cloudRouterPort != value)
                {
                    cloudRouterPort = value;
                    RaisePropertyChanged("CloudRouterPort");
                }
            }
        }
        #endregion

        #region 云服务协助通讯设置
        private string cloudHostIP;
        /// <summary>云服务备用地址 </summary>
        public string CloudHostIP
        {
            get
            {
                return cloudHostIP;
            }
            set
            {
                if (cloudHostIP != value)
                {
                    cloudHostIP = value;
                    RaisePropertyChanged("CloudHostIP");
                }
            }
        }

        private string cloudHostPort;
        /// <summary>云服务备用端口 </summary>
        public string CloudHostPort
        {
            get
            {
                return cloudHostPort;
            }
            set
            {
                if (cloudHostPort != value)
                {
                    cloudHostPort = value;
                    RaisePropertyChanged("CloudHostPort");
                }
            }
        }
        #endregion

        #region 接入平台通讯配置 企业soa
        private string enterpriseServerIP;
        /// <summary>企业云服务器地址 </summary>
        public string EnterpriseServerIP
        {
            get
            {
                return enterpriseServerIP;
            }
            set
            {
                if (enterpriseServerIP != value)
                {
                    enterpriseServerIP = value;
                    RaisePropertyChanged("EnterpriseServerIP");
                }
            }
        }

        private string enterpriseServerPort;
        /// <summary>企业云服务器端口 </summary>
        public string EnterpriseServerPort
        {
            get
            {
                return enterpriseServerPort;
            }
            set
            {
                if (enterpriseServerPort != value)
                {
                    enterpriseServerPort = value;
                    RaisePropertyChanged("EnterpriseServerPort");
                }
            }
        }

        private string enterpriseRouterIP;
        /// <summary> 企业云路由地址</summary>
        public string EnterpriseRouterIP
        {
            get
            {
                return enterpriseRouterIP;
            }
            set
            {
                if (enterpriseRouterIP != value)
                {
                    enterpriseRouterIP = value;
                    RaisePropertyChanged("EnterpriseRouterIP");
                }
            }
        }

        private string enterpriseRouterPort;
        /// <summary> 企业云路由端口</summary>
        public string EnterpriseRouterPort
        {
            get
            {
                return enterpriseRouterPort;
            }
            set
            {
                if (enterpriseRouterPort != value)
                {
                    enterpriseRouterPort = value;
                    RaisePropertyChanged("EnterpriseRouterPort");
                }
            }
        }

        private string enterpriseLan;
        /// <summary> 企业Lan</summary>
        public string EnterpriseLan
        {
            get
            {
                return enterpriseLan;
            }
            set
            {
                if (enterpriseLan != value)
                {
                    enterpriseLan = value;
                    RaisePropertyChanged("EnterpriseLan");
                }
            }
        }


        private string enterpriseIsNeedRouter;
        /// <summary>是否需要路由 </summary>
        public string EnterpriseIsNeedRouter
        {
            get
            {
                return enterpriseIsNeedRouter;
            }
            set
            {
                if (enterpriseIsNeedRouter != value)
                {
                    enterpriseIsNeedRouter = value;
                    RaisePropertyChanged("EnterpriseIsNeedRouter");
                }
            }
        }
        #endregion

        #region p2p连接信息
        private string p2pServerIP;
        /// <summary> P2P服务地址</summary>
        public string P2PServerIP
        {
            get
            {
                return p2pServerIP;
            }
            set
            {
                if (p2pServerIP != value)
                {
                    p2pServerIP = value;
                    RaisePropertyChanged("P2PServerIP");
                }
            }
        }

        private string p2pServerPort;
        /// <summary> P2P服务端口</summary>
        public string P2PServerPort
        {
            get
            {
                return p2pServerPort;
            }
            set
            {
                if (p2pServerPort != value)
                {
                    p2pServerPort = value;
                    RaisePropertyChanged("P2PServerPort");
                }
            }
        }
        #endregion

        #region 用户信息
        private string userName;
        /// <summary> 用户名</summary>
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

        private string passWord;
        /// <summary> 密码</summary>
        public string PassWord
        {
            get
            {
                return passWord;
            }
            set
            {
                if (passWord != value)
                {
                    passWord = value;
                    RaisePropertyChanged("PassWord");
                }
            }
        }

        private string clientId;
        /// <summary> 客户端ID</summary>
        public string ClientId
        {
            get
            {
                return clientId;
            }
            set
            {
                if (clientId != value)
                {
                    clientId = value;
                    RaisePropertyChanged("ClientId");
                }
            }
        }
        private string clientNo;
        /// <summary>
        /// 客户端编号
        /// </summary>
        public string ClientNo
        {
            get { return clientNo; }
            set { clientNo = value; }
        }
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
                    RaisePropertyChanged("ProductId");
                }
            }
        }
        #endregion

    }
}
