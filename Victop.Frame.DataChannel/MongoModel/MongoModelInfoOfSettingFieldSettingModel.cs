using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.DataChannel.MongoModel
{
    public class MongoModelInfoOfSettingFieldSettingModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        private string fieldSettingName;
        /// <summary>
        /// 表名
        /// </summary>
        [JsonProperty(PropertyName="name")]
        public string FieldSettingName
        {
            get { return fieldSettingName; }
            set { fieldSettingName = value; }
        }
        /// <summary>
        /// 字段集合
        /// </summary>
        private List<MongoModelInfoOfSettingFieldSettingFieldModel> fieldSettingFields;
        /// <summary>
        /// 字段集合
        /// </summary>
        [JsonProperty(PropertyName="fields")]
        public List<MongoModelInfoOfSettingFieldSettingFieldModel> FieldSettingFields
        {
            get
            {
                if (fieldSettingFields == null)
                    fieldSettingFields = new List<MongoModelInfoOfSettingFieldSettingFieldModel>();
                return fieldSettingFields;
            }
            set { fieldSettingFields = value; }
        }
    }
}
