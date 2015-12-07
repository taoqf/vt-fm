using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Victop.Frame.CoreLibrary.MongoModel
{
    /// <summary>
    /// Mongo模型定义实体
    /// </summary>
    public class MongoModelInfoModel
    {
        /// <summary>
        /// 模型定义表结构集合
        /// </summary>
        private List<MongoModelInfoOfTablesModel> modelTables;
        /// <summary>
        /// 模型定义表结构集合
        /// </summary>
        [JsonProperty(PropertyName = "tables")]
        public List<MongoModelInfoOfTablesModel> ModelTables
        {
            get
            {
                if (modelTables == null)
                    modelTables = new List<MongoModelInfoOfTablesModel>();
                return modelTables;
            }
            set { modelTables = value; }
        }
        /// <summary>
        /// 模型定义关系集合
        /// </summary>
        private List<MongoModelInfoOfRelationModel> modelRelation;
        /// <summary>
        /// 模型定义关系集合
        /// </summary>
        [JsonProperty(PropertyName = "relation")]
        public List<MongoModelInfoOfRelationModel> ModelRelation
        {
            get
            {
                if (modelRelation == null)
                    modelRelation = new List<MongoModelInfoOfRelationModel>();
                return modelRelation;
            }
            set { modelRelation = value; }
        }
        /// <summary>
        /// 模型定义引用集合
        /// </summary>
        private List<MongoModelInfoOfRefModel> modelRef;
        /// <summary>
        /// 模型定义引用集合
        /// </summary>
        [JsonProperty(PropertyName = "ref")]
        public List<MongoModelInfoOfRefModel> ModelRef
        {
            get
            {
                if (modelRef == null)
                    modelRef = new List<MongoModelInfoOfRefModel>();
                return modelRef;
            }
            set { modelRef = value; }
        }
        /// <summary>
        /// 数据引用
        /// </summary>
        private List<MongoModelInfoOfClientRefModel> modelClientRef;
        [JsonProperty(PropertyName = "clientRef")]
        public List<MongoModelInfoOfClientRefModel> ModelClientRef
        {
            get
            {
                if (modelClientRef == null)
                    modelClientRef = new List<MongoModelInfoOfClientRefModel>();
                return modelClientRef;
            }
            set { modelClientRef = value; }
        }

        private string modelVersion;
        /// <summary>
        /// 模板版本
        /// </summary>
        [JsonProperty(PropertyName = "version")]
        public string ModelVersion
        {
            get
            {
                return modelVersion;
            }
            set
            {
                modelVersion = value;
            }
        }
        /// <summary>
        /// 模型设置
        /// </summary>
        private MongoModelInfoOfSettingModel modelSetting;
        /// <summary>
        /// 模型设置
        /// </summary>
        [JsonProperty(PropertyName = "setting")]
        public MongoModelInfoOfSettingModel ModelSetting
        {
            get
            {
                if (modelSetting == null)
                    modelSetting = new MongoModelInfoOfSettingModel();
                return modelSetting;
            }
            set { modelSetting = value; }
        }
    }
}
