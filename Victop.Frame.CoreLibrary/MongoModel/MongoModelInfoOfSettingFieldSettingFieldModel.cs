using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// field实体
    /// </summary>
    public class MongoModelInfoOfSettingFieldSettingFieldModel
    {
        /// <summary>
        /// 字段
        /// </summary>
        private string fieldKey;
        /// <summary>
        /// 字段
        /// </summary>
        [JsonProperty(PropertyName="key")]
        public string FieldKey
        {
            get { return fieldKey; }
            set { fieldKey = value; }
        }
        /// <summary>
        /// 字段标题
        /// </summary>
        private string fieldLabel;
        /// <summary>
        /// 字段标题
        /// </summary>
        [JsonProperty(PropertyName = "label")]
        public string FieldLabel
        {
            get { return fieldLabel; }
            set { fieldLabel = value; }
        }
    }
}
