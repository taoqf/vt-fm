using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace SystemTestingPlugin.Models
{
    /// <summary>
    /// 表结构实体
    /// </summary>
    public class TableStructModel:PropertyModelBase
    {
        /// <summary>
        /// 表名称
        /// </summary>
        private string tableName;
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                if (tableName != value)
                {
                    tableName = value;
                    RaisePropertyChanged(()=> TableName);
                }
            }
        }
        /// <summary>
        /// 表标题
        /// </summary>
        private string tableTitle;
        /// <summary>
        /// 表标题
        /// </summary>
        public string TableTitle
        {
            get
            {
                return tableTitle;
            }
            set
            {
                if (TableTitle != value)
                {
                    tableTitle = value;
                    RaisePropertyChanged(()=> TableTitle);
                }
            }
        }
        /// <summary>
        /// 表字段
        /// </summary>
        private string tableFields;
        /// <summary>
        /// 表字段
        /// </summary>
        public string TableFields
        {
            get
            {
                return tableFields;
            }
            set
            {
                if (tableFields != value)
                {
                    tableFields = value;
                    RaisePropertyChanged(()=> TableFields);
                }
            }
        }
    }
}
