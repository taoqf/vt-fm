using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace SystemTestingPlugin.Models
{
    /// <summary>
    /// 查询信息实体
    /// </summary>
    public class DataSearchInfoModel:PropertyModelBase
    {
        /// <summary>
        /// 模型Id
        /// </summary>
        private string modelId;
        /// <summary>
        /// 模型Id
        /// </summary>
        public string ModelId
        {
            get
            {
                return modelId;
            }
            set
            {
                if (modelId != value)
                {
                    modelId = value;
                    RaisePropertyChanged("ModelId");
                }
            }
        }
        private string systemId;

        public string SystemId
        {
            get
            {
                return systemId;
            }
            set
            {
                if (systemId != value)
                {
                    systemId = value;
                    RaisePropertyChanged("SystemId");
                }
            }
        }
        private string configsystemId;

        public string ConfigsystemId
        {
            get { return configsystemId; }
            set
            {
                if (configsystemId != value)
                {
                    configsystemId = value;
                    RaisePropertyChanged("ConfigsystemId");
                }
            }
        }
        private string spaceId;

        public string SpaceId
        {
            get { return spaceId; }
            set
            {
                if (spaceId != value)
                {
                    spaceId = value;
                    RaisePropertyChanged("SpaceId");
                }
            }
        }
        private string tableName;

        public string TableName
        {
            get { return tableName; }
            set
            {
                if (tableName != value)
                {
                    tableName = value;
                    RaisePropertyChanged("TableName");
                }
            }
        }
        private string channelId;

        public string ChannelId
        {
            get { return channelId; }
            set
            {
                channelId = value;
                RaisePropertyChanged("ChannelId");
            }
        }
        private DataTable resultDataTable;

        public DataTable ResultDataTable
        {
            get { return resultDataTable; }
            set
            {
                if (resultDataTable != value)
                {
                    resultDataTable = value;
                    RaisePropertyChanged("ResultDataTable");
                }
            }
        }
        private string jsonData;

        public string JsonData
        {
            get { return jsonData; }
            set
            {
                if (jsonData != value)
                {
                    jsonData = value;
                    RaisePropertyChanged("JsonData");
                }
                
            }
        }

        private string conditionStr=string.Empty;

        public string ConditionStr
        {
            get { return conditionStr; }
            set
            {
                if (conditionStr != value)
                {
                    conditionStr = value;
                    RaisePropertyChanged("ConditionStr");
                }
            }
        }
        /// <summary>
        /// 验证信息
        /// </summary>
        private string vertifyMsg;
        /// <summary>
        /// 验证信息
        /// </summary>
        public string VertifyMsg
        {
            get { return vertifyMsg; }
            set
            {
                vertifyMsg = value;
                RaisePropertyChanged("VertifyMsg");
            }
        }
        /// <summary>
        /// 数据路径
        /// </summary>
        private string dataPath = string.Empty;
        /// <summary>
        /// 数据路径
        /// </summary>
        public string DataPath
        {
            get
            {
                return dataPath;
            }
            set
            {
                if (dataPath != value)
                {
                    dataPath = value;
                    RaisePropertyChanged("DataPath");
                }
            }
        }
        private bool emptyFlag = true;

        public bool EmptyFlag
        {
            get { return emptyFlag; }
            set
            {
                emptyFlag = value;
                RaisePropertyChanged("EmptyFlag");
            }
        }
        private object gridSelectedValue;

        public object GridSelectedValue
        {
            get { return gridSelectedValue; }
            set
            {
                gridSelectedValue = value;
                RaisePropertyChanged("GridSelectedValue");
            }
        }
        /// <summary>
        /// 引用表
        /// </summary>
        private DataTable refDataTable;
        /// <summary>
        /// 引用表
        /// </summary>
        public DataTable RefDataTable
        {
            get
            {
                if (refDataTable == null)
                    refDataTable = new DataTable();
                return refDataTable;
            }
            set
            {
                refDataTable = value;
                RaisePropertyChanged("RefDataTable");
            }
        }
    }
}
