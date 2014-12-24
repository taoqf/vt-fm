using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace SystemTestingPlugin.Models
{
    /// <summary>
    /// 编码服务实体
    /// </summary>
    public class CodeServiceInfoModel:PropertyModelBase
    {
        private string systemId;

        public string SystemId
        {
            get
            {
                return systemId;
            }
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
        private string pName;

        public string PName
        {
            get { return pName; }
            set
            {
                if (pName != value)
                {
                    pName = value;
                    RaisePropertyChanged("PName");
                }
            }
        }

        private string setInfo;

        public string SetInfo
        {
            get { return setInfo; }
            set
            {
                if (setInfo != value)
                {
                    setInfo = value;
                    RaisePropertyChanged("SetInfo");
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
