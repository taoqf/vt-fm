using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.DataMessageManager;
using System.Collections.ObjectModel;
using Victop.Server.Controls.Models;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 组件定义视图层实体
    /// </summary>
    public class DefinViewsModel : PropertyModelBase
    {
        /// <summary>
        /// 视图名称
        /// </summary>
        [JsonProperty(PropertyName = "viewname")]
        private string viewName;
        /// <summary>
        /// 视图名称
        /// </summary>
        [JsonIgnore]
        public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }
        /// <summary>
        /// 视图模型
        /// </summary>
        [JsonProperty(PropertyName = "modelid")]
        private string modelId;
        /// <summary>
        /// 视图模型
        /// </summary>
        [JsonIgnore]
        public string ModelId
        {
            get { return modelId; }
            set { modelId = value; }
        }
        /// <summary>
        /// 加载数据方式 0不加载, 1带缓存, 2不使用缓存 
        /// </summary>
        [JsonProperty(PropertyName = "loaddata")]
        private int loadData;
        /// <summary>
        /// 加载数据方式 0不加载, 1带缓存, 2不使用缓存 
        /// </summary>
        [JsonIgnore]
        public int LoadData
        {
            get { return loadData; }
            set { loadData = value; }
        }
        /// <summary>
        /// 是否编辑
        /// </summary>
        [JsonProperty(PropertyName = "editable")]
        private bool editAble;
        /// <summary>
        /// 是否编辑
        /// </summary>
        [JsonIgnore]
        public bool EditAble
        {
            get { return editAble; }
            set { editAble = value; }
        }

        /// <summary>
        /// 视图Block
        /// </summary>
        [JsonProperty(PropertyName = "blocks")]
        private ObservableCollection<ViewsBlockModel> viewBlocks;
        /// <summary>
        /// 视图Block
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<ViewsBlockModel> ViewBlocks
        {
            get
            {
                if (viewBlocks == null)
                    viewBlocks = new ObservableCollection<ViewsBlockModel>();
                return viewBlocks;
            }
            set
            {
                if (viewBlocks != value)
                {
                    viewBlocks = value;
                    RaisePropertyChanged("ViewBlocks");
                }
            }
        }
        /// <summary>
        /// 数据通道标识
        /// </summary>
        [JsonIgnore]
        public string ViewId
        {
            get;
            set;
        }
        /// <summary>
        /// SystemId
        /// </summary>
        [JsonProperty(PropertyName = "systemid")]
        private string systemId;
        /// <summary>
        /// SystemId
        /// </summary>
        [JsonIgnore]
        public string SystemId
        {
            get
            {
                return systemId;
            }
            set
            {
                systemId = value;
            }
        }
        /// <summary>
        /// 引用数据的SystemId
        /// </summary>
        [JsonProperty(PropertyName = "refsystemid")]
        private string refSystemId;
        /// <summary>
        /// 引用数据的SystemId
        /// </summary>
        [JsonIgnore]
        public string RefSystemId
        {
            get
            {
                return refSystemId;
            }
            set
            {
                refSystemId = value;
            }
        }

        public void DoRender()
        {
            if (loadData.Equals(1))
            {
                SearchData();
            }
        }
        public void SearchData()
        {
            string MessageType = "MongoDataChannelService.findBusiData";
            DataMessageOperation messageOp = new DataMessageOperation();
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", SystemId);
            contentDic.Add("refsystemid", RefSystemId);
            contentDic.Add("modelid", ModelId);
            List<object> conditionList = new List<object>();
            foreach (ViewsBlockModel item in this.viewBlocks)
            {
                Dictionary<string, object> blockDic = new Dictionary<string, object>();
                blockDic.Add("name", item.TableName);
                List<object> tableConList = new List<object>();
                tableConList.Add(item.Conditions.TableCondition);
                blockDic.Add("tablecondition", tableConList);
                blockDic.Add("sort", item.Conditions.TableSort);
                Dictionary<string, object> pageDic = new Dictionary<string, object>();
                pageDic.Add("size", item.Conditions.PageSize);
                pageDic.Add("index", item.Conditions.PageIndex);
                blockDic.Add("paging", pageDic);
                conditionList.Add(blockDic);
            }
            if (conditionList.Count > 0)
            {
                contentDic.Add("conditions", conditionList);
            }
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                ViewId = returnDic["DataChannelId"].ToString();
            }
        }

        /// <summary>
        /// 获取Block数据
        /// </summary>
        public void GetBlockData(string BlockName)
        {
            if (string.IsNullOrEmpty(ViewId))
            {
                DoRender();
            }
            if (!string.IsNullOrEmpty(BlockName))
            {
                ViewsBlockModel blockModel = ViewBlocks.FirstOrDefault(it => it.BlockName == BlockName);
                if (blockModel != null && blockModel.BlockDataPath != null && blockModel.BlockDataPath.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ViewId))
                    {
                        DataMessageOperation dataOp = new DataMessageOperation();
                        blockModel.BlockDataSet = dataOp.GetData(ViewId, JsonHelper.ToJson(blockModel.BlockDataPath));
                        if (blockModel.BlockDataSet != null && blockModel.BlockDataSet.Tables["dataArray"] != null && blockModel.BlockDataSet.Tables["dataArray"].Rows.Count > 0)
                        {
                            blockModel.SetCurrentRow(blockModel.BlockDataSet.Tables["dataArray"].Rows[0]);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        public void SaveData()
        {
            if (!string.IsNullOrEmpty(ViewId))
            {
                ViewsBlockModel blockmodel = ViewBlocks.FirstOrDefault(it => it.Superiors.Equals("root"));
                if (blockmodel.BlockDataPath != null && blockmodel.BlockDataPath.Count > 0)
                {
                    DataMessageOperation dataOp = new DataMessageOperation();
                    dataOp.SaveData(ViewId, JsonHelper.ToJson(blockmodel.BlockDataPath));
                    SaveBlockData(ViewId, blockmodel);
                }
            }
        }
        private void SaveBlockData(string viewId, ViewsBlockModel blockModel)
        {
            List<ViewsBlockModel> BlockList = ViewBlocks.Where(it => it.Superiors.Equals(blockModel.BlockName)).ToList();
            if (BlockList != null && BlockList.Count > 0)
            {
                foreach (ViewsBlockModel item in BlockList)
                {
                    if (item.BlockDataPath != null && item.BlockDataPath.Count > 0)
                    {
                        DataMessageOperation dataOp = new DataMessageOperation();
                        dataOp.SaveData(viewId, JsonHelper.ToJson(item.BlockDataPath));
                    }
                }
                foreach (ViewsBlockModel item in BlockList)
                {
                    SaveBlockData(viewId, item);
                }
            }
        }
    }
}
