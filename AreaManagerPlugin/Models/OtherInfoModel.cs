using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace AreaManagerPlugin.Models
{
    /// <summary>
    /// 其他信息实体
    /// </summary>
    public class OtherInfoModel:PropertyModelBase
    {
        private string messageType;

        public string MessageType
        {
            get { return messageType; }
            set
            {
                if (messageType != value)
                {
                    messageType = value;
                    RaisePropertyChanged("MessageType");
                }
            }
        }

        private string otherConditionData;

        public string OtherConditionData
        {
            get { return otherConditionData; }
            set
            {
                if (otherConditionData != value)
                {
                    otherConditionData = value;
                    RaisePropertyChanged("OtherConditionData");
                }
            }
        }
        private string otherResultData;

        public string OtherResultData
        {
            get { return otherResultData; }
            set
            {
                if (otherConditionData != value)
                {
                    otherResultData = value;
                    RaisePropertyChanged("OtherResultData");
                }
            }
        }
    }
}
