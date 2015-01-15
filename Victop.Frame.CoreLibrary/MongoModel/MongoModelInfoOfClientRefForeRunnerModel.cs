using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// 引用前导
    /// </summary>
    public class MongoModelInfoOfClientRefForeRunnerModel
    {
        /// <summary>
        /// 引用模型Id
        /// </summary>
        private string refModel;
        /// <summary>
        /// 引用模型Id
        /// </summary>
        [JsonProperty(PropertyName = "refModel")]
        public string RefModel
        {
            get { return refModel; }
            set { refModel = value; }
        }
        /// <summary>
        /// 树标识
        /// </summary>
        private string treeId;
        [JsonProperty(PropertyName = "treeId")]
        public string TreeId
        {
            get { return treeId; }
            set { treeId = value; }
        }
        /// <summary>
        /// 树父级字段
        /// </summary>
        private string treeParentId;
        /// <summary>
        /// 树父级字段
        /// </summary>
        [JsonProperty(PropertyName = "treeParentId")]
        public string TreeParentId
        {
            get { return treeParentId; }
            set { treeParentId = value; }
        }
        /// <summary>
        /// 树展示节点
        /// </summary>
        private string treeDisplay;
        /// <summary>
        /// 树展示节点
        /// </summary>
        [JsonProperty(PropertyName = "treeDisplay")]
        public string TreeDisplay
        {
            get { return treeDisplay; }
            set { treeDisplay = value; }
        }
    }
}
