using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

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
        
        [JsonProperty(PropertyName = "clientId")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string ClientId { get; set; }
        [JsonProperty(PropertyName="doccode")]
        public string DocCode { get; set; }
        [JsonProperty(PropertyName="formid")]
        public string FormId { get; set; }
        [JsonProperty(PropertyName = "bzsystemid")]
        public string SystemId { get; set; }
        [JsonProperty(PropertyName="modelId")]
        public string ModelId { get; set; }
        [JsonProperty(PropertyName="treeStr")]
        public string TreeStr { get; set; }
        [JsonProperty(PropertyName = "saveType")]
        public string SaveType { get; set; }
        [JsonProperty(PropertyName="runUser")]
        public string RunUser { get; set; }
        [JsonProperty(PropertyName="shareFlag")]
        public string ShareFlag { get; set; }
        [JsonProperty(PropertyName = "dataSetID")]
        public string DataSetIds { get; set; }
        [JsonProperty(PropertyName = "deltaXml")]
        public object DeltaXml { get; set; }
        [JsonProperty(PropertyName = "reportID")]
        public string ReportId { get; set; }
        [JsonProperty(PropertyName="masterOnly")]
        public bool MasterOnly { get; set; }
        [JsonProperty(PropertyName="whereArr")]
        public string WhereArr { get; set; }
        [JsonProperty(PropertyName="dataparam")]
        public object DataParam { get; set; }
        [JsonProperty(PropertyName = "masterParam")]
        public object MasterParam { get; set; }
    }
}
