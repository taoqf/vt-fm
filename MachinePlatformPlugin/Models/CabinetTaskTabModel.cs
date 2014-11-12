using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Victop.Server.Controls.Models;

namespace MachinePlatformPlugin.Models
{
    /// <summary>
    /// 机台任务区Tab模型
    /// </summary>
    public class CabinetTaskTabModel : ModelBase
    {
        /// <summary>
        /// 主Tab名称
        /// </summary>
        private string masterTabName;
        /// <summary>
        /// 主Tab名称
        /// </summary>
        public string MasterTabName
        {
            get
            {
                return masterTabName;
            }
            set
            {
                masterTabName = value;
                RaisePropertyChanged("MasterTabName");
            }
        }
        /// <summary>
        /// 主Tab标题
        /// </summary>
        private string masterTabHeader;
        /// <summary>
        /// 主Tab标题
        /// </summary>
        public string MasterTabHeader
        {
            get
            {
                return masterTabHeader;
            }
            set
            {
                masterTabHeader = value;
                RaisePropertyChanged("MasterTabHeader");
            }
        }
        /// <summary>
        /// 主Tab可见状态
        /// </summary>
        private Visibility masterTabVisibility = Visibility.Collapsed;
        /// <summary>
        /// 主Tab可见状态
        /// </summary>
        public Visibility MasterTabVisibility
        {
            get
            {
                return masterTabVisibility;
            }
            set
            {
                masterTabVisibility = value;
                RaisePropertyChanged("MasterTabVisibility");
            }
        }
        /// <summary>
        /// 明细Tab名称
        /// </summary>
        private string detialTabName;
        /// <summary>
        /// 明细Tab名称
        /// </summary>
        public string DetialTabName
        {
            get
            {
                return detialTabName;
            }
            set
            {
                detialTabName = value;
                RaisePropertyChanged("DetialTabName");
            }
        }
        /// <summary>
        /// 明细Tab标题
        /// </summary>
        private string detialTabHeader;
        /// <summary>
        /// 明细Tab标题
        /// </summary>
        public string DetialTabHeader
        {
            get
            {
                return detialTabHeader;
            }
            set
            {
                detialTabHeader = value;
                RaisePropertyChanged("DetialTabHeader");
            }
        }
        /// <summary>
        /// 明细Tab可见状态
        /// </summary>
        private Visibility detialTabVisibility = Visibility.Collapsed;
        /// <summary>
        /// 明细Tab可见状态
        /// </summary>
        public Visibility DetialTabVisibility
        {
            get { return detialTabVisibility; }
            set
            {
                detialTabVisibility = value;
                RaisePropertyChanged("DetialTabVisibility");
            }
        }
        /// <summary>
        /// 从明细Tab名称
        /// </summary>
        private string subDetialTabName;
        /// <summary>
        /// 从明细Tab名称
        /// </summary>
        public string SubDetialTabName
        {
            get { return subDetialTabName; }
            set
            {
                subDetialTabName = value;
                RaisePropertyChanged("SubDetialTabName");
            }
        }
        /// <summary>
        /// 从明细Tab标题
        /// </summary>
        private string subDetialTabHeader;
        /// <summary>
        /// 从明细Tab标题
        /// </summary>
        public string SubDetialTabHeader
        {
            get { return subDetialTabHeader; }
            set
            {
                subDetialTabHeader = value;
                RaisePropertyChanged("SubDetialTabHeader");
            }
        }
        /// <summary>
        /// 从明细列表可见状态
        /// </summary>
        private Visibility subDetialTabVisibility= Visibility.Collapsed;
        /// <summary>
        /// 从明细列表可见状态
        /// </summary>
        public Visibility SubDetialTabVisibility
        {
            get
            {
                return subDetialTabVisibility;
            }
            set
            {
                subDetialTabVisibility = value;
                RaisePropertyChanged("SubDetialTabVisibility");
            }
        }

    }
}
