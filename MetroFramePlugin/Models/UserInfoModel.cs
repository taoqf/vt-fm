using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace MetroFramePlugin.Models
{
    /// <summary>
    /// 用户信息实体
    /// </summary>
    public class UserInfoModel : PropertyModelBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        private string userName = string.Empty;
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
        /// 用户角色
        /// </summary>
        private string userRole;
        public string OldRole { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public string UserRole
        {
            get
            {
                return userRole;
            }
            set
            {
                if (userRole != value)
                {
                    userRole = value;
                    RaisePropertyChanged("UserRole");
                }
            }
        }
        private string userImg;
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserImg
        {
            get
            {
                return userImg;
            }
            set
            {
                if (userImg != value)
                {
                    userImg = value;
                    RaisePropertyChanged("UserImg");
                }
            }
        }
        private string clientId;
        /// <summary>
        /// ClientId
        /// </summary>
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
        /// <summary>
        /// 用户账号
        /// </summary>
        private string userCode;

        public string OldUserCode { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserCode
        {
            get
            {
                return userCode;
            }
            set
            {
                if (userCode != value)
                {
                    userCode = value;
                    RaisePropertyChanged("UserCode");
                }
            }
        }
        /// <summary>
        /// 是否登陆
        /// </summary>
        private bool isLogin = false;
        /// <summary>
        /// 是否登陆
        /// </summary>
        public bool IsLogin
        {
            get
            {
                return isLogin;
            }

            set
            {
                if (isLogin != value)
                {
                    isLogin = value;
                    RaisePropertyChanged("IsLogin");
                }
            }
        }

        private string unLockPwd;
        /// <summary>
        /// 解锁密码
        /// </summary>

        public string UnLockPwd
        {
            get
            {
                return unLockPwd;
            }
            set
            {
                if (unLockPwd != value)
                {
                    unLockPwd = value;
                    RaisePropertyChanged("UnLockPwd");
                }
            }
        }
        /// <summary>
        /// 是否为多角色
        /// </summary>
        private bool isMultipleRole;
        /// <summary>
        /// 是否为多角色
        /// </summary>
        public bool IsMultipleRole
        {
            get
            {
                return isMultipleRole;
            }

            set
            {
                if (isMultipleRole != value)
                {
                    isMultipleRole = value;
                    RaisePropertyChanged("IsMultipleRole");
                }
            }
        }
    }
}
