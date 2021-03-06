﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 视图层Block实体
    /// </summary>
    public class ViewsBlockModel : ICloneable
    {
        private string blockName;
        /// <summary>
        /// Block名称
        /// </summary>
        [JsonProperty(PropertyName = "blockname")]
        public string BlockName
        {
            get
            {
                return blockName;
            }
            set
            {
                blockName = value;
            }
        }
        private string datasetType;
        /// <summary>
        /// 数据集类型
        /// </summary>
        [JsonProperty(PropertyName = "datasettype")]
        public string DatasetType
        {
            get
            {
                return datasetType;
            }
            set
            {
                datasetType = value;
            }
        }
        private string superiors;
        /// <summary>
        /// 上级Block的名称
        /// </summary>
        [JsonProperty(PropertyName = "superiors")]
        public string Superiors
        {
            get
            {
                return superiors;
            }
            set
            {
                superiors = value;
            }
        }
        private string tableName;
        /// <summary>
        /// 表名
        /// </summary>
        [JsonProperty(PropertyName = "tablename")]
        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                tableName = value;
            }
        }
        private ViewsBlockConditionModel conditions;
        /// <summary>
        /// 条件(检索条件、排序及分页)
        /// </summary>
        [JsonProperty(PropertyName = "conditions")]
        public ViewsBlockConditionModel Conditions
        {
            get
            {
                if (conditions == null)
                    conditions = new ViewsBlockConditionModel();
                return conditions;
            }
            set
            {
                conditions = value;
            }
        }
        /// <summary>
        /// Block所属View
        /// </summary>
        [JsonIgnore]
        public DefinViewsModel ViewModel
        {
            get;
            set;
        }
        /// <summary>
        /// 视图标识
        /// </summary>
        [JsonIgnore]
        public string ViewId
        {
            get;
            set;
        }
        [JsonIgnore]
        private DataSet blockDataSet;
        /// <summary>
        /// 块数据集
        /// </summary>
        [JsonIgnore]
        public DataSet BlockDataSet
        {
            get
            {
                if (blockDataSet == null)
                    blockDataSet = new DataSet();
                return blockDataSet;
            }
            set
            {
                blockDataSet = value;
            }
        }
        /// <summary>
        /// 块路径
        /// </summary>
        [JsonIgnore]
        public List<object> BlockDataPath
        {
            get;
            set;
        }
        /// <summary>
        /// 当前选择行
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, object> CurrentRow
        {
            get;
            set;
        }
        /// <summary>
        /// 数据块锁
        /// </summary>
        [JsonIgnore]
        public bool BlockLock
        {
            get;
            set;
        }
        /// <summary>
        /// 设置当前选择行
        /// </summary>
        /// <param name="dr"></param>
        internal void SetCurrentRow(DataRow dr)
        {
            if (CurrentRow == null)
            {
                CurrentRow = new Dictionary<string, object>();
            }
            CurrentRow.Clear();
            if (dr != null)
            {
                foreach (DataColumn item in dr.Table.Columns)
                {
                    CurrentRow.Add(item.ColumnName, dr[item.ColumnName]);
                }
            }
            RebuildPath();
        }
        /// <summary>
        /// 设置当前实体
        /// 3.0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        internal void SetCurrentEntity<T>(T entity)
        {
            if (CurrentRow == null)
            {
                CurrentRow = new Dictionary<string, object>();
            }
            CurrentRow.Clear();
            if (entity != null)
            {
                string entityStr = JsonHelper.ToJson(entity);
                CurrentRow = JsonHelper.ToObject<Dictionary<string, object>>(entityStr);
            }
            RebuildPath();
        }
        private void RebuildPath()
        {
            ViewId = ViewModel.ViewId;
            RebuildDataPath(ViewModel, this);
        }

        /// <summary>
        /// 重建Block的DataPath
        /// </summary>
        /// <param name="definView">View定义</param>
        /// <param name="blockModel">Block实体</param>
        private void RebuildDataPath(DefinViewsModel definView, ViewsBlockModel blockModel)
        {
            for (int i = 0; i < definView.ViewBlocks.Count; i++)
            {
                ViewsBlockModel block = definView.ViewBlocks[i];
                if (block.Superiors.Equals(blockModel.BlockName))
                {
                    if (block.BlockLock && block.BlockDataPath != null && block.BlockDataPath.Count > 0)
                    {
                        RebuildDataPath(definView, block);
                    }
                    else
                    {
                        Dictionary<string, object> pathDic = new Dictionary<string, object>();
                        pathDic.Add("key", "_id");
                        pathDic.Add("value", (blockModel.CurrentRow == null || !blockModel.CurrentRow.ContainsKey("_id")) ? Guid.NewGuid().ToString() : blockModel.CurrentRow["_id"]);
                        if (block.BlockDataPath == null)
                        {
                            block.BlockDataPath = new List<object>();
                        }
                        else
                        {
                            block.BlockDataPath.Clear();
                        }
                        foreach (var item in blockModel.BlockDataPath)
                        {
                            block.BlockDataPath.Add(item);
                        }
                        if (!blockModel.DatasetType.Equals("row"))
                        {
                            block.BlockDataPath.Add(pathDic);
                        }
                        block.ViewId = blockModel.ViewId;
                        if (block.DatasetType.Equals("table"))
                        {
                            block.BlockDataPath.Add(block.TableName);
                        }
                        RebuildDataPath(definView, block);
                    }
                }
            }
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public ViewsBlockModel Copy(string blockName)
        {
            this.BlockLock = true;
            ViewsBlockModel BlockModel = this.Clone() as ViewsBlockModel;
            BlockModel.BlockName = blockName;
            BlockModel.BlockDataPath = new List<object>();
            return BlockModel;
        }
    }
}
