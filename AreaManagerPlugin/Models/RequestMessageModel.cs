using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AreaManagerPlugin.Models
{
    public class RequestMessageModel
    {
        /// <summary>
        /// 打开方式
        /// </summary>
        public string openType { get; set; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string fieldName { get; set; }
        /// <summary>
        /// 订单编码
        /// </summary>
        public string doccode { get; set; }
        /// <summary>
        /// formid
        /// </summary>
        public string formid { get; set; }
        /// <summary>
        /// 系统标识
        /// </summary>
        public string bzsystemid { get; set; }
        /// <summary>
        /// 模型标识
        /// </summary>
        public string modelId { get; set; }
        /// <summary>
        /// 数据集标识
        /// </summary>
        public string dataSetID { get; set; }
        /// <summary>
        /// 报表Id
        /// </summary>
        public string reportID { get; set; }
        /// <summary>
        /// 仅主档
        /// </summary>
        public bool masterOnly { get; set; }
        /// <summary>
        /// Where条件
        /// </summary>
        public string whereArr { get; set; }
        /// <summary>
        /// 数据参数
        /// </summary>
        public Dictionary<string, object> dataparam { get; set; }
        /// <summary>
        /// 主档参数
        /// </summary>
        public string masterParam { get; set; }
        /// <summary>
        /// 具体参数内容
        /// </summary>
        public Dictionary<string, object> deltaXml { get; set; }
        /// <summary>
        /// 共享标识
        /// </summary>
        public string shareFlag { get; set; }
        /// <summary>
        /// 树字符串
        /// </summary>
        public string treeStr { get; set; }
        /// <summary>
        /// 保存类型
        /// </summary>
        public string saveType { get; set; }


    }
}
