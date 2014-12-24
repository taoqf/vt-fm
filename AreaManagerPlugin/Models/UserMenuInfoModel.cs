using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace AreaManagerPlugin.Models
{
    /// <summary>
    /// 用户信息菜单信息实体
    /// </summary>
    public class UserMenuInfoModel:PropertyModelBase
    {
        private string systemId;

        public string SystemId
        {
            get { return systemId; }
            set
            {
                if (systemId != value)
                {
                    systemId = value;
                    RaisePropertyChanged("SystemId");
                }
            }
        }
        private string configsystemId;

        public string ConfigsystemId
        {
            get { return configsystemId; }
            set
            {
                if (configsystemId != value)
                {
                    configsystemId = value;
                    RaisePropertyChanged("ConfigsystemId");
                }
            }
        }
        private string clientType = "1";

        public string ClientType
        {
            get { return clientType; }
            set
            {
                if (clientType != value)
                {
                    clientType = value;
                    RaisePropertyChanged("ClientType");
                }
            }
        }

        private string userCode;

        public string UserCode
        {
            get { return userCode; }
            set
            {
                if (userCode != value)
                {
                    userCode = value;
                    RaisePropertyChanged("UserCode");
                }
            }
        }
        private string resultData;

        public string ResultData
        {
            get { return resultData; }
            set
            {
                if (resultData != value)
                {
                    resultData = value;
                    RaisePropertyChanged("ResultData");
                }
            }
        }
        /// <summary>
        /// 验证信息
        /// </summary>
        private string vertifyMsg;
        /// <summary>
        /// 验证信息
        /// </summary>
        public string VertifyMsg
        {
            get { return vertifyMsg; }
            set
            {
                vertifyMsg = value;
                RaisePropertyChanged("VertifyMsg");
            }
        }
    }
}
