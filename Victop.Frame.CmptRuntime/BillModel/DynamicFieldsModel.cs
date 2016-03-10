using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace Victop.Frame.CmptRuntime
{
    public class DynamicFieldsModel : PropertyModelBase
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        private string fieldName;
        /// <summary>
        /// 字段名称
        /// </summary>
        [JsonProperty(PropertyName = "fieldname")]
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        private string fieldType;
        /// <summary>
        /// 字段类型
        /// </summary>
        [JsonProperty(PropertyName = "fieldtype")]
        public string FieldType
        {
            get { return fieldType; }
            set { fieldType = value; }
        }

        /// <summary>
        /// 字段默认值
        /// </summary>
        private object fieldValue;
        /// <summary>
        /// 字段默认值
        /// </summary>
        [JsonProperty(PropertyName = "fieldvalue")]
        public object FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }
    }
}
