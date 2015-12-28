using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace MetroFramePlugin.Models
{
    public class UpdateLogModel : PropertyModelBase
    {
        /// <summary>
        /// 日志版本
        /// </summary>
        private string logVersion;
        /// <summary>
        /// 日志版本
        /// </summary>
        public string LogVersion
        {
            get
            {
                return logVersion;
            }

            set
            {
                if (logVersion != value)
                {
                    logVersion = value;
                    RaisePropertyChanged(()=> LogVersion);
                }
            }
        }
        /// <summary>
        /// 日志内容
        /// </summary>
        private string logContent;
        /// <summary>
        /// 日志内容
        /// </summary>
        public string LogContent
        {
            get
            {
                return logContent;
            }

            set
            {
                if (logContent != value)
                {
                    logContent = value;
                    RaisePropertyChanged(()=> LogContent);
                }
            }
        }
        /// <summary>
        /// 日志创建者
        /// </summary>
        private string logCreater;
        /// <summary>
        /// 日志创建者
        /// </summary>
        public string LogCreater
        {
            get
            {
                return logCreater;
            }

            set
            {
                if (logCreater != value)
                {
                    logCreater = value;
                    RaisePropertyChanged(()=> LogCreater);
                }
            }
        }
        /// <summary>
        /// 更新日期时间
        /// </summary>
        private string logDate;
        /// <summary>
        /// 更新日期时间
        /// </summary>
        public string LogDate
        {
            get
            {
                return logDate;
            }

            set
            {
                if (logDate != value)
                {
                    logDate = value;
                    RaisePropertyChanged(()=> LogDate);
                }
            }
        }
    }
}
