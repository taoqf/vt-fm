using Newtonsoft.Json;

namespace Victop.Server.Controls.Models
{
    public class CustomerMessageModel
    {
        /// <summary>
        /// 打开方式
        /// </summary>
        [JsonProperty(PropertyName="openType")]
        public string OpenType { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        [JsonProperty(PropertyName = "fieldName")]
        public string FieldName { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        [JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        [JsonProperty(PropertyName="doccode")]
        public string DocCode { get; set; }
        /// <summary>
        /// FormId
        /// </summary>
        [JsonProperty(PropertyName="formid")]
        public string FormId { get; set; }
        /// <summary>
        /// 系统标识
        /// </summary>
        [JsonProperty(PropertyName = "bzsystemid")]
        public string SystemId { get; set; }
        /// <summary>
        /// 数据类型标识
        /// </summary>
        [JsonProperty(PropertyName="modelId")]
        public string ModelId { get; set; }
        /// <summary>
        /// 属性字符串
        /// </summary>
        [JsonProperty(PropertyName="treeStr")]
        public string TreeStr { get; set; }
        /// <summary>
        /// 保存类型
        /// </summary>
        [JsonProperty(PropertyName = "saveType")]
        public string SaveType { get; set; }
        /// <summary>
        /// 执行用户
        /// </summary>
        [JsonProperty(PropertyName="runUser")]
        public string RunUser { get; set; }
        /// <summary>
        /// 共享标识
        /// </summary>
        [JsonProperty(PropertyName="shareFlag")]
        public string ShareFlag { get; set; }
        /// <summary>
        /// 数据表标识
        /// </summary>
        [JsonProperty(PropertyName = "dataSetID")]
        public string DataSetIds { get; set; }
        /// <summary>
        /// 内容xml
        /// </summary>
        [JsonProperty(PropertyName = "deltaXml")]
        public object DeltaXml { get; set; }
        /// <summary>
        /// 报表标识
        /// </summary>
        [JsonProperty(PropertyName = "reportID")]
        public string ReportId { get; set; }
        /// <summary>
        /// 主档标识
        /// </summary>
        [JsonProperty(PropertyName="masterOnly")]
        public bool MasterOnly { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        [JsonProperty(PropertyName="whereArr")]
        public string WhereArr { get; set; }
        /// <summary>
        /// 数据参数
        /// </summary>
        [JsonProperty(PropertyName="dataparam")]
        public object DataParam { get; set; }
        /// <summary>
        /// 主档参数
        /// </summary>
        [JsonProperty(PropertyName = "masterParam")]
        public object MasterParam { get; set; }
    }
}
