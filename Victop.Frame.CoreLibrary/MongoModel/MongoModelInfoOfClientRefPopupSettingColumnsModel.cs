using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    public class MongoModelInfoOfClientRefPopupSettingColumnsModel
    {
        /// <summary>
        /// 列名
        /// </summary>
        private string columnField;
        /// <summary>
        /// 列名
        /// </summary>
        [JsonProperty(PropertyName = "field")]
        public string ColumnField
        {
            get { return columnField; }
            set { columnField = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        private string columnLabel;
        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty(PropertyName="label")]
        public string ColumnLabel
        {
            get { return columnLabel; }
            set { columnLabel = value; }
        }
        /// <summary>
        /// 列类型
        /// </summary>
        private string columnType;
        /// <summary>
        /// 列类型
        /// </summary>
        [JsonProperty(PropertyName="type")]
        public string ColumnType
        {
            get { return columnType; }
            set { columnType = value; }
        }
        /// <summary>
        /// 列宽度
        /// </summary>
        private long columnLength;
        /// <summary>
        /// 列宽度
        /// </summary>
        [JsonProperty(PropertyName = "length")]
        public long ColumnLength
        {
            get { return columnLength; }
            set { columnLength = value; }
        } 
    }
}
