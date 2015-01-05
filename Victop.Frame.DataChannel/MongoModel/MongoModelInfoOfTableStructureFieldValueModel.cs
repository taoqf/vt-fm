using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.DataChannel.MongoModel
{
    /// <summary>
    /// 表字段值实体
    /// </summary>
    class MongoModelInfoOfTableStructureFieldValueModel
    {
        /// <summary>
        /// 值类型
        /// </summary>
        private string valueType;
        /// <summary>
        /// 值类型
        /// </summary>
        [JsonProperty(PropertyName="type")]
        public string ValueType
        {
            get { return valueType; }
            set { valueType = value; }
        }
        /// <summary>
        /// 字段默认值
        /// </summary>
        private object valueDef;
        /// <summary>
        /// 字段默认值
        /// </summary>
        [JsonProperty(PropertyName = "def")]
        public object ValueDef
        {
            get { return valueDef; }
            set { valueDef = value; }
        }
        /// <summary>
        /// 查询的标识 值为0，返回结果不包含该字段,值为1，返回结果包含该字段
        /// </summary>
        private int valueSelectFlag = 1;
        /// <summary>
        /// 查询的标识 值为0，返回结果不包含该字段,值为1，返回结果包含该字段
        /// </summary>
        [JsonProperty(PropertyName = "selectFlag")]
        public int ValueSelectFlag
        {
            get { return valueSelectFlag; }
            set { valueSelectFlag = value; }
        }
        /// <summary>
        /// 使用服务器时间标识 0:不使用服务器时间,1:使用服务器时间
        /// </summary>
        private int valueServerDateFlag = 0;
        /// <summary>
        /// 使用服务器时间标识 0:不使用服务器时间,1:使用服务器时间
        /// </summary>
        [JsonProperty(PropertyName = "serverDateFlag")]
        public int ValueServerDateFlag
        {
            get { return valueServerDateFlag; }
            set { valueServerDateFlag = value; }
        }
    }
}
