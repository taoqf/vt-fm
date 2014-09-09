using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using Victop.Frame.DataChannel;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.SyncOperation;
using Victop.Server.Controls.Models;

namespace DataCruisePlugin.Models
{
    public class BlockModel : ModelBase
    {
        /// <summary>
        /// 实体定义
        /// </summary>
        private EntityDefinitionModel entityDefModel;

        public EntityDefinitionModel EntityDefModel
        {
            get
            {
                return entityDefModel;
            }
            set
            {
                entityDefModel = value;
                RaisePropertyChanged("EntityDefModel");
            }
        }
        /// <summary>
        /// viewid
        /// </summary>
        private string viewId;
        /// <summary>
        /// viewid
        /// </summary>
        public string ViewId
        {
            get
            {
                return viewId;
            }
            set
            {
                viewId = value;
                RaisePropertyChanged("ViewId");
            }
        }
        /// <summary>
        /// 路径
        /// </summary>
        private List<Object> dataPath;
        /// <summary>
        /// 路径
        /// </summary>
        public List<Object> DataPath
        {
            get
            {
                if (dataPath == null)
                {
                    dataPath = new List<object>();
                }
                return dataPath;
            }
            set
            {
                if (dataPath != value)
                {
                    dataPath = value;
                    if (ParentBlock == null || string.IsNullOrEmpty(ParentBlock.TableId))
                    {
                        SendFindDataMessage();
                    }
                    else
                    {
                        GetDataByPath();
                    }
                    RaisePropertyChanged("DataPath");
                }
            }
        }
        /// <summary>
        /// 表标识
        /// </summary>
        private string tableId;
        /// <summary>
        /// 表标识
        /// </summary>
        public string TableId
        {
            get { return tableId; }
            set
            {
                tableId = value;
                RaisePropertyChanged("TableId");
            }
        }

        /// <summary>
        /// 表名
        /// </summary>
        private string tableName;
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                tableName = value;
                RaisePropertyChanged("TableName");
            }
        }
        /// <summary>
        /// 数据集
        /// </summary>
        private DataTable blockDt;
        /// <summary>
        /// 数据集
        /// </summary>
        public DataTable BlockDt
        {
            get
            {
                if (blockDt == null)
                    blockDt = new DataTable();
                return blockDt;
            }
            set
            {
                blockDt = value;
                RaisePropertyChanged("BlockDt");
            }
        }
        /// <summary>
        /// 数据集类型
        /// </summary>
        private string dataSetType;
        /// <summary>
        /// 数据集类型
        /// </summary>
        public string DataSetType
        {
            get
            {
                return dataSetType;
            }
            set
            {
                dataSetType = value;
                RaisePropertyChanged("DataSetType");
            }
        }
        /// <summary>
        /// 父级block
        /// </summary>
        private BlockModel parentBlock;
        /// <summary>
        /// 父级Block
        /// </summary>
        public BlockModel ParentBlock
        {
            get
            {
                if (parentBlock == null)
                    parentBlock = new BlockModel();
                return parentBlock;
            }
            set
            {
                parentBlock = value;
                RaisePropertyChanged("ParentBlock");
            }
        }
        /// <summary>
        /// 当前选择行
        /// </summary>
        private Dictionary<string, object> currentRow;
        /// <summary>
        /// 当前选择行
        /// </summary>
        public Dictionary<string, object> CurrentRow
        {
            get
            {
                if (currentRow == null)
                {
                    currentRow = new Dictionary<string, object>();
                    currentRow.Add("_id", Guid.NewGuid());
                }
                return currentRow;
            }
            set
            {
                currentRow = value;
                RaisePropertyChanged("CurrentRow");
            }
        }
        /// <summary>
        /// 重建DataPath
        /// </summary>
        public void RebuildDataPath(ObservableCollection<BlockModel> fullBlocks,ObservableCollection<EntityDefinitionModel> fullEntityModels)
        {
            if (this.dataPath == null || this.dataPath.Count == 0||string.IsNullOrEmpty(fullEntityModels.First(it => it.TableName == this.TableName).HostTable))
            {
                List<object> pathDic = new List<object>();
                pathDic.Add(this.tableName);
                this.DataPath = pathDic;
            }
            foreach (BlockModel item in fullBlocks.Where(it => it.ParentBlock.TableName == this.TableName))
            {
                List<object> pathList = this.DataPath;
                pathList.Add(this.CurrentRow);
                pathList.Add(item.TableName);
                item.ViewId = item.ParentBlock.ViewId;
                item.DataPath = pathList;
                item.RebuildDataPath(fullBlocks, fullEntityModels);
            }
        }
        #region 私有方法
        /// <summary>
        /// 发送检索数据消息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private void SendFindDataMessage()
        {
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "800");
            contentDic.Add("configsystemid", "101");
            contentDic.Add("spaceid", "tbs");
            contentDic.Add("modelid", string.Format("table::{0}", this.TableName));
            if (this.EntityDefModel.RefCondtions != null && this.EntityDefModel.RefCondtions.Count > 0)
            {
                List<object> conList = new List<object>();
                foreach (string item in this.EntityDefModel.RefCondtions.Keys)
                {
                    conList.Add(this.EntityDefModel.RefCondtions[item]);
                }
                contentDic.Add("tablecondition", conList);
            }
            string messageType = "MongoDataChannelService.findBusiData";
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> returnDic = messageOp.SendMessage(messageType, contentDic, "JSON");
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                this.ViewId = returnDic["DataChannelId"].ToString();
                DataOperation dataOp = new DataOperation();
                BlockDt = dataOp.GetData(this.ViewId, JsonHelper.ToJson(this.DataPath), CreateStructDataTable(this.EntityDefModel));
            }
            else
            {
                BlockDt = new DataTable(this.TableName);
            }
            SetCurrentRow();
        }
        /// <summary>
        /// 依据路径获取数据集
        /// </summary>
        /// <returns></returns>
        private void GetDataByPath()
        {
            DataOperation dataOp = new DataOperation();
            BlockDt = dataOp.GetData(this.ViewId, JsonHelper.ToJson(this.DataPath), null);
            SetCurrentRow();
        }
        /// <summary>
        /// 设置当前选择行
        /// </summary>
        private void SetCurrentRow()
        {
            if (BlockDt != null && BlockDt.Rows.Count > 0)
            {
                DataRow dr = BlockDt.Rows[0];
                Dictionary<string, object> drDic = new Dictionary<string, object>();
                drDic.Add("key","_id");
                drDic.Add("value", dr["_id"]);
                CurrentRow = drDic;
            }
            else
            {
                CurrentRow = null;
            }
        }


        /// <summary>
        /// 创建结构表
        /// </summary>
        /// <param name="entityFields"></param>
        /// <returns></returns>
        private DataTable CreateStructDataTable(EntityDefinitionModel entityModel)
        {
            List<EntityFieldModel> entityFields = entityModel.Fields as List<EntityFieldModel>;
            DataTable structDt = new DataTable(entityModel.TableName);
            foreach (EntityFieldModel item in entityFields)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = item.Field;
                if (item.Field.Equals("_id"))
                {
                    dc.ReadOnly = true;
                    dc.DefaultValue = Guid.NewGuid();
                }
                dc.Caption = item.FieldTitle;
                switch (item.FieldType)
                {
                    case "date":
                        dc.DataType = typeof(DateTime);
                        break;
                    case "int":
                        dc.DataType = typeof(Int32);
                        break;
                    case "long":
                        dc.DataType = typeof(Int64);
                        break;
                    case "bool":
                        dc.DataType = typeof(Boolean);
                        break;
                    case "string":
                    default:
                        dc.DataType = typeof(String);
                        break;
                }
                structDt.Columns.Add(dc);
            }
            return structDt;
        }
        #endregion
    }
}
