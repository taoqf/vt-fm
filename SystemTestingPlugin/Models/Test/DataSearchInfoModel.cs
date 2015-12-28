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
    public class DataSearchInfoModel : PropertyModelBase
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
                    RaisePropertyChanged(() => ModelId);
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
                    RaisePropertyChanged(() => SystemId);
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
                    RaisePropertyChanged(() => ConfigsystemId);
                }
            }
        }
        private string refSystemId;
        /// <summary>
        /// 引用Id
        /// </summary>
        public string RefSystemId
        {
            get
            {
                return refSystemId;
            }
            set
            {
                if (refSystemId != value)
                {
                    refSystemId = value;
                    RaisePropertyChanged(() => RefSystemId);
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
                    RaisePropertyChanged(() => SpaceId);
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
                    RaisePropertyChanged(() => TableName);
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
                RaisePropertyChanged(() => ChannelId);
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
                    RaisePropertyChanged(() => ResultDataTable);
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
                    RaisePropertyChanged(() => JsonData);
                }

            }
        }

        private string conditionStr = string.Empty;
        /// <summary>
        /// 查询
        /// </summary>
        public string ConditionStr
        {
            get { return conditionStr; }
            set
            {
                if (conditionStr != value)
                {
                    conditionStr = value;
                    RaisePropertyChanged(() => ConditionStr);
                }
            }
        }

        private string sortStr = string.Empty;
        /// <summary>
        /// 排序
        /// </summary>
        public string SortStr
        {
            get
            {
                return sortStr;
            }
            set
            {
                if (sortStr != value)
                {
                    sortStr = value;
                    RaisePropertyChanged(() => SortStr);
                }
            }
        }

        private string pagingStr = string.Empty;
        /// <summary>
        /// 分页
        /// </summary>
        public string PagingStr
        {
            get
            {
                return pagingStr;
            }
            set
            {
                if (pagingStr != value)
                {
                    pagingStr = value;
                    RaisePropertyChanged(() => PagingStr);
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
                RaisePropertyChanged(() => VertifyMsg);
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
                    RaisePropertyChanged(() => DataPath);
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
                RaisePropertyChanged(() => EmptyFlag);
            }
        }
        private object gridSelectedValue;

        public object GridSelectedValue
        {
            get { return gridSelectedValue; }
            set
            {
                gridSelectedValue = value;
                RaisePropertyChanged(() => GridSelectedValue);
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
                RaisePropertyChanged(() => RefDataTable);
            }
        }
        #region 窄表相关
        /// <summary>
        /// 窄表行值
        /// </summary>
        private string narrowRowValue;
        /// <summary>
        /// 窄表行值
        /// </summary>
        public string NarrowRowValue
        {
            get
            {
                return narrowRowValue;
            }
            set
            {
                if (narrowRowValue != value)
                {
                    narrowRowValue = value;
                    RaisePropertyChanged(() => NarrowRowValue);
                }
            }
        }
        /// <summary>
        /// 窄表引用字段
        /// </summary>
        private string narrowRefField;
        /// <summary>
        /// 窄表引用字段
        /// </summary>
        public string NarrowRefField
        {
            get
            {
                return narrowRefField;
            }
            set
            {
                if (narrowRefField != value)
                {
                    narrowRefField = value;
                    RaisePropertyChanged(() => NarrowRefField);
                }
            }
        }
        private object narrowGridSelectedValue;

        public object NarrowGridSelectedValue
        {
            get { return narrowGridSelectedValue; }
            set
            {
                narrowGridSelectedValue = value;
                RaisePropertyChanged(() => NarrowGridSelectedValue);
            }
        }
        #endregion
    }
}
