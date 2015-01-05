using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.DataChannel.MongoModel
{
    /// <summary>
    /// 表结构实体
    /// </summary>
    public class MongoModelInfoOfTableStructureModel
    {
        /// <summary>
        /// 字段名
        /// </summary>
        private string fieldKey;
        /// <summary>
        /// 字段名
        /// </summary>
        [JsonProperty(PropertyName="key")]
        public string FieldKey
        {
            get { return fieldKey; }
            set { fieldKey = value; }
        }
        /// <summary>
        /// 字段值
        /// </summary>
        private MongoModelInfoOfTableStructureFieldValueModel fieldValue;
        /// <summary>
        /// 字段值
        /// </summary>
        [JsonProperty(PropertyName="value")]
        internal MongoModelInfoOfTableStructureFieldValueModel FieldValue
        {
            get
            {
                if (fieldValue == null)
                    fieldValue = new MongoModelInfoOfTableStructureFieldValueModel();
                return fieldValue;
            }
            set
            {
                fieldValue = value;
            }
        }
    }
}
