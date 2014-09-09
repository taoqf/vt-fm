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
        public EntityDefinitionModel EntityDefModel;
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
                    dataPath.Add(this.tableName);
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
        public void RebuildDataPath(ObservableCollection<BlockModel> fullBlocks)
        {
            foreach (BlockModel item in fullBlocks.Where(it => it.ParentBlock.TableName == this.TableName))
            {
                List<object> pathList = this.DataPath;
                pathList.Add(this.CurrentRow);
                pathList.Add(item.TableName);
                item.DataPath = pathList;
                item.RebuildDataPath(fullBlocks);
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
            string messageType = "MongoDataChannelService.findBusiData";
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> returnDic = messageOp.SendMessage(messageType, contentDic, "JSON");
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                DataOperation dataOp = new DataOperation();
                BlockDt = dataOp.GetData(this.ViewId, JsonHelper.ToJson(this.DataPath), null);
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
                drDic.Add("_id", dr["_id"]);
                CurrentRow = drDic;
            }
            else
            {
                CurrentRow = null;
            }
        }
        #endregion
    }
}
