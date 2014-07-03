using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace Victop.Frame.Templates.ModelTemplate
{
    public class ModelDataShowT:ModelBase
    {
        private DateTime startDate = DateTime.Now.AddDays(-7);
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (startDate != value)
                {
                    startDate = value;
                    RaisePropertyChanged("StartDate");
                }
            }
        }
        private DateTime endDate = DateTime.Now;
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (endDate != value)
                {
                    endDate = value;
                    RaisePropertyChanged("EndDate");
                }
            }
        }
        private string combSqlFilter;
        /// <summary>
        /// 下拉列表
        /// </summary>
        public string CombSqlFilter
        {
            get { return combSqlFilter; }
            set
            {
                if (combSqlFilter != value)
                {
                    combSqlFilter = value;
                    RaisePropertyChanged("CombSqlFilter");
                }
            }
        }
        private string tboxSqlFilter;
        /// <summary>
        /// 查询条件值
        /// </summary>
        public string TboxSqlFilter
        {
            get { return tboxSqlFilter; }
            set
            {
                if (tboxSqlFilter != value)
                {
                    tboxSqlFilter = value;
                    RaisePropertyChanged("TboxSqlFilter");
                }
            }
        }
        private string rdoBtnDocStatus;
        /// <summary>
        /// 凭据状态
        /// </summary>
        public string RdoBtnDocStatus
        {
            get { return rdoBtnDocStatus; }
            set
            {
                if (rdoBtnDocStatus != value)
                {
                    rdoBtnDocStatus = value;
                    RaisePropertyChanged("RdoBtnDocStatus");
                }
            }
        }
    }
}
