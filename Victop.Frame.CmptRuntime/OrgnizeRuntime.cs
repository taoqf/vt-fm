using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 组件运行时
    /// </summary>
    public class OrgnizeRuntime
    {
        /// <summary>
        /// 初始化组件
        /// </summary>
        /// <param name="runtime"></param>
        public static void InitCompnt(CompntDefinModel runtime)
        {
            try
            {
                RebulidViews(runtime);
                PresentationBindingViewBlock(runtime);
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("初始化组件异常:{0}", ex.Message);
            }
        }
        /// <summary>
        /// 重建Views
        /// </summary>
        /// <param name="runtime"></param>
        private static void RebulidViews(CompntDefinModel runtime)
        {
            foreach (DefinViewsModel item in runtime.CompntViews)
            {
                foreach (ViewsBlockModel blockitem in item.ViewBlocks)
                {
                    blockitem.ViewModel = item;
                }
            }
        }
        /// <summary>
        /// 展示层Block绑定View层Block
        /// </summary>
        /// <param name="runtime"></param>
        private static void PresentationBindingViewBlock(CompntDefinModel runtime)
        {
            foreach (PresentationBlockModel item in runtime.CompntPresentation.PresentationBlocks)
            {
                DefinViewsModel viewModel = runtime.CompntViews.FirstOrDefault(it => it.ViewName.Equals(item.ViewName));
                if (viewModel != null)
                {
                    item.ViewBlock = viewModel.ViewBlocks.FirstOrDefault(it => it.BlockName.Equals(item.BindingBlock));
                }
            }
        }

        /// <summary>
        /// 重建所有View下所有Block的DataPath
        /// </summary>
        /// <param name="runtime">运行时</param>
        public static void RebuildAllDataPath(CompntDefinModel runtime)
        {
            foreach (DefinViewsModel item in runtime.CompntViews)
            {
                ViewsBlockModel blockmodel = item.ViewBlocks.FirstOrDefault(it => it.Superiors.Equals("root"));
                if (blockmodel.BlockLock && blockmodel.BlockDataPath != null && blockmodel.BlockDataPath.Count > 0)
                {
                    RebuildDataPath(item, blockmodel);
                }
                else
                {
                    if (blockmodel.BlockDataPath == null)
                        blockmodel.BlockDataPath = new List<object>();
                    blockmodel.BlockDataPath.Clear();
                    blockmodel.BlockDataPath.Add(blockmodel.TableName);
                    if (blockmodel != null)
                    {
                        RebuildDataPath(item, blockmodel);
                    }
                }
            }
        }
        /// <summary>
        /// 重构 View下的Block的DataPath
        /// </summary>
        /// <param name="runtime">运行时</param>
        /// <param name="view">View定义</param>
        public static void RebuildViewDataPath(CompntDefinModel runtime, DefinViewsModel view)
        {
            ViewsBlockModel blockmodel = view.ViewBlocks.FirstOrDefault(it => it.Superiors.Equals("root"));
            if (blockmodel.BlockLock && blockmodel.BlockDataPath != null && blockmodel.BlockDataPath.Count > 0)
            {
                RebuildDataPath(view, blockmodel);
            }
            else
            {
                if (blockmodel.BlockDataPath == null)
                    blockmodel.BlockDataPath = new List<object>();
                blockmodel.BlockDataPath.Clear();
                blockmodel.BlockDataPath.Add(blockmodel.TableName);
                if (blockmodel.DatasetType.Equals("row"))
                {
                    Dictionary<string, object> pathDic = new Dictionary<string, object>();
                    pathDic.Add("key", "_id");
                    pathDic.Add("value", (blockmodel.CurrentRow == null || !blockmodel.CurrentRow.ContainsKey("_id")) ? Guid.NewGuid().ToString() : blockmodel.CurrentRow["_id"]);
                    blockmodel.BlockDataPath.Add(pathDic);
                }
                if (blockmodel != null)
                {
                    RebuildDataPath(view, blockmodel);
                }
            }
        }
        /// <summary>
        /// 重建Block的DataPath
        /// </summary>
        /// <param name="definView">View定义</param>
        /// <param name="blockModel">Block实体</param>
        private static void RebuildDataPath(DefinViewsModel definView, ViewsBlockModel blockModel)
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
    }
}
