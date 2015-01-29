using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace SystemTestingPlugin.Models
{
    /// <summary>
    /// 模型定义ClientRef实体
    /// </summary>
    public class MongoModelInfoOfClientRefModel
    {
        /// <summary>
        /// 引用字段
        /// </summary>
        private string clientRefField;
        /// <summary>
        /// 引用字段
        /// </summary>
        [JsonProperty(PropertyName = "refField")]
        public string ClientRefField
        {
            get { return clientRefField; }
            set { clientRefField = value; }
        }
        /// <summary>
        /// 引用表名
        /// </summary>
        private string clientRefTableName;
        /// <summary>
        /// 引用表名
        /// </summary>
        [JsonProperty(PropertyName = "refTableName")]
        public string ClientRefTableName
        {
            get { return clientRefTableName; }
            set { clientRefTableName = value; }
        }
        /// <summary>
        /// 引用字段的正则匹配查询
        /// </summary>
        private string clientRefRegStrategy;
        /// <summary>
        /// 引用字段的正则匹配查询
        /// </summary>
        [JsonProperty(PropertyName = "regStrategy")]
        public string ClientRefRegStrategy
        {
            get { return clientRefRegStrategy; }
            set { clientRefRegStrategy = value; }
        }
        /// <summary>
        /// 数据引用的ModelId
        /// </summary>
        private string clientRefModel;
        [JsonProperty(PropertyName = "refModel")]
        public string ClientRefModel
        {
            get { return clientRefModel; }
            set { clientRefModel = value; }
        }
        /// <summary>
        /// 首选条件
        /// </summary>
        private List<MongoModelInfoOfClientRefConditionModel> clientRefConditionFirst;
        /// <summary>
        /// 首选条件
        /// </summary>
        [JsonProperty(PropertyName = "conditionFirst")]
        public List<MongoModelInfoOfClientRefConditionModel> ClientRefConditionFirst
        {
            get
            {
                if (clientRefConditionFirst == null)
                    clientRefConditionFirst = new List<MongoModelInfoOfClientRefConditionModel>();
                return clientRefConditionFirst;
            }
            set { clientRefConditionFirst = value; }
        }
        /// <summary>
        /// 次要条件
        /// </summary>
        private List<MongoModelInfoOfClientRefConditionModel> clientRefConditionSecond;
        /// <summary>
        /// 次要条件
        /// </summary>
        [JsonProperty(PropertyName = "conditionSecond")]
        public List<MongoModelInfoOfClientRefConditionModel> ClientRefConditionSecond
        {
            get
            {
                if (clientRefConditionSecond == null)
                    clientRefConditionSecond = new List<MongoModelInfoOfClientRefConditionModel>();
                return clientRefConditionSecond;
            }
            set { clientRefConditionSecond = value; }
        }
        /// <summary>
        /// 引用字段与引用数据
        /// </summary>
        private List<MongoModelInfoOfClientRefPropertyModel> clientRefProperty;
        /// <summary>
        /// 引用字段与引用数据
        /// </summary>
        [JsonProperty(PropertyName = "property")]
        public List<MongoModelInfoOfClientRefPropertyModel> ClientRefProperty
        {
            get
            {
                if (clientRefProperty == null)
                    clientRefProperty = new List<MongoModelInfoOfClientRefPropertyModel>();
                return clientRefProperty;
            }
            set { clientRefProperty = value; }
        }
        /// <summary>
        /// 引用装配
        /// </summary>
        private MongoModelInfoOfClientRefPopupSettingModel clientRefPopupSetting;
        /// <summary>
        /// 引用装配
        /// </summary>
        [JsonProperty(PropertyName="popupSetting")]
        public MongoModelInfoOfClientRefPopupSettingModel ClientRefPopupSetting
        {
            get
            {
                if (clientRefPopupSetting == null)
                    clientRefPopupSetting = new MongoModelInfoOfClientRefPopupSettingModel();
                return clientRefPopupSetting;
            }
            set { clientRefPopupSetting = value; }
        }
        /// <summary>
        /// 前导引用
        /// </summary>
        private MongoModelInfoOfClientRefForeRunnerModel clientRefForeRunner;
        /// <summary>
        /// 前导引用
        /// </summary>
        [JsonProperty(PropertyName = "forerunner")]
        public MongoModelInfoOfClientRefForeRunnerModel ClientRefForeRunner
        {
            get
            {
                if (clientRefForeRunner == null)
                    clientRefForeRunner = new MongoModelInfoOfClientRefForeRunnerModel();
                return clientRefForeRunner;
            }
            set { clientRefForeRunner = value; }
        }
    }
}
