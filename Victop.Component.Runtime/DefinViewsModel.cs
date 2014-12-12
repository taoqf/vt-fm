using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Victop.Frame.DataChannel;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.SyncOperation;

namespace Victop.Component.Runtime
{
    /// <summary>
    /// 组件定义-View实体
    /// </summary>
    public class DefinViewsModel
    {
        /// <summary>
        /// View名称
        /// </summary>
        [JsonProperty(PropertyName = "viewname")]
        public string ViewName
        {
            get;
            set;
        }
        /// <summary>
        /// 模型编号
        /// </summary>
        [JsonProperty(PropertyName = "modelid")]
        public string ModelId
        {
            get;
            set;
        }
        /// <summary>
        /// 数据文件名称
        /// </summary>
        [JsonProperty(PropertyName = "datafilename")]
        public string DataFileName
        {
            get;
            set;
        }
        /// <summary>
        /// 块集合
        /// </summary>
        [JsonProperty(PropertyName = "blocks")]
        public List<ViewsBlockModel> Blocks
        {
            get;
            set;
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
        /// 获取Block数据
        /// </summary>
        public void GetBlockData(string BlockName)
        {
            if (!string.IsNullOrEmpty(BlockName))
            {
                ViewsBlockModel blockModel = Blocks.FirstOrDefault(it => it.BlockName == BlockName);
                if (blockModel != null && blockModel.BlockDataPath != null && blockModel.BlockDataPath.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ViewId))
                    {
                        DataOperation dataOp = new DataOperation();
                        blockModel.BlockDt = dataOp.GetData(ViewId, JsonHelper.ToJson(blockModel.BlockDataPath));
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
                ViewsBlockModel blockmodel = Blocks.Find(it => it.Superiors.Equals("root"));
                if (blockmodel.BlockDataPath != null && blockmodel.BlockDataPath.Count > 0)
                {
                    DataOperation dataOp = new DataOperation();
                    dataOp.SaveData(ViewId, JsonHelper.ToJson(blockmodel.BlockDataPath));
                    SaveBlockData(ViewId, blockmodel);
                }
            }
        }
        private void SaveBlockData(string viewId, ViewsBlockModel blockModel)
        {
            List<ViewsBlockModel> BlockList = Blocks.Where(it => it.Superiors.Equals(blockModel.BlockName)).ToList();
            if (BlockList != null && BlockList.Count > 0)
            {
                foreach (ViewsBlockModel item in BlockList)
                {
                    if (item.BlockDataPath != null && item.BlockDataPath.Count > 0)
                    {
                        DataOperation dataOp = new DataOperation();
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
