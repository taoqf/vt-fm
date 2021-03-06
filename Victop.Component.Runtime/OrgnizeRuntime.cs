﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Component.Runtime
{
    public class OrgnizeRuntime
    {
        /// <summary>
        /// 初始化组件运行时
        /// </summary>
        /// <param name="runtime"></param>
        public static bool InitCompnt(ComponentModel runtime)
        {
            try
            {
                if (runtime.CompntSettings != null)
                {
                    RebulidViews(runtime);
                    RebuildPlugins(runtime);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 重建Views
        /// </summary>
        /// <param name="runtime">2015-02-13 马晓宝</param>
        private static void RebulidViews(ComponentModel runtime)
        {
            Dictionary<string, object> viewDic = JsonHelper.ToObject<Dictionary<string, object>>(runtime.CompntSettings.ViewSetting["views"].ToString());
            foreach (DefinViewsModel item in runtime.CompntDefin.Views)
            {              
                if (viewDic.ContainsKey(item.ViewName))
                {
                    item.ModelId = JsonHelper.ReadJsonString(viewDic[item.ViewName].ToString(), "modelid");
                    item.DataFileName = JsonHelper.ReadJsonString(viewDic[item.ViewName].ToString(), "datafilename");

                    Dictionary<string, object> blockDic = JsonHelper.ToObject<Dictionary<string, object>>(JsonHelper.ReadJsonString(viewDic[item.ViewName].ToString(), "blocks"));
                     
                    foreach (ViewsBlockModel blockitem in item.Blocks)
                    {
                        if (blockDic.ContainsKey(blockitem.BlockName))
                        {
                            blockitem.TableName = JsonHelper.ReadJsonString(blockDic[blockitem.BlockName].ToString(), "tablename");
                            #region 12-13新增
                            blockitem.ViewModel = item;
                            #endregion
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 重建插件
        /// </summary>
        /// <param name="runtime"></param>
        private static void RebuildPlugins(ComponentModel runtime)
        {
            runtime.RuntimeParams = new Dictionary<string, Dictionary<string, object>>();
            foreach (DefinPluginsModel item in runtime.CompntDefin.Plugins)
            {
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                if (runtime.CompntSettings.PluginsSetting.ContainsKey(item.PluginName))
                {
                    foreach (object paramitem in item.Params)
                    {
                        paramDic.Add(paramitem.ToString(), JsonHelper.ReadJsonString(runtime.CompntSettings.PluginsSetting[item.PluginName].ToString(), paramitem.ToString()));
                    }
                }
                runtime.RuntimeParams.Add(item.PluginName, paramDic);
            }
            Dictionary<string, object> compntDic = new Dictionary<string, object>();
            foreach (string item in runtime.CompntSettings.CompoSetting.Keys)
            {
                if (runtime.CompntDefin.CompoSetting.Contains(item))
                {
                    compntDic.Add(item, runtime.CompntSettings.CompoSetting[item]);
                }
                if (runtime.CompntDefin.ParamMap.Exists(it => it.CompoParam.Equals(item)))
                {
                    DefinParamMapModel paramModel = runtime.CompntDefin.ParamMap.First(it => it.CompoParam.Equals(item));
                    if (runtime.RuntimeParams.ContainsKey(paramModel.PluginName))
                    {
                        runtime.RuntimeParams[paramModel.PluginName][paramModel.ParamName] = runtime.CompntSettings.CompoSetting[item];
                    }
                }
            }
            runtime.RuntimeParams.Add("root", compntDic);
        }
        /// <summary>
        /// 绑定插件
        /// </summary>
        /// <param name="runtime"></param>
        /// <param name="pluginName"></param>
        public static void BindingPlugin(ComponentModel runtime, string pluginName)
        {
            DefinPluginsModel pluginModel = runtime.CompntDefin.Plugins.FirstOrDefault(it => it.PluginName.Equals(pluginName));
            if (pluginModel != null)
            {
                foreach (DefinViewsModel item in runtime.CompntDefin.Views)
                {
                    foreach (ViewsBlockModel blockitem in item.Blocks)
                    {
                        if (blockitem.BlockName.Equals(pluginModel.DataBlock))
                        {
                            pluginModel.PluginBlock = blockitem;
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 重建Block的DataPath
        /// </summary>
        /// <param name="runtime"></param>
        public static void RebuildAllDataPath(ComponentModel runtime)
        {
            foreach (DefinViewsModel item in runtime.CompntDefin.Views)
            {
                ViewsBlockModel blockmodel = item.Blocks.Find(it => it.Superiors.Equals("root"));
                if (blockmodel.BlockLock && blockmodel.BlockDataPath != null && blockmodel.BlockDataPath.Count > 0)
                {
                    RebuildDataPath(item, blockmodel);
                }
                else
                {
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
        /// <param name="view"></param>
        public static void RebuildViewDataPath(ComponentModel runtime, DefinViewsModel view)
        {
            ViewsBlockModel blockmodel = view.Blocks.Find(it => it.Superiors.Equals("root"));
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
                if (blockmodel.DataSetType.Equals("row"))
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
        /// <param name="definView"></param>
        /// <param name="blockModel"></param>
        private static void RebuildDataPath(DefinViewsModel definView, ViewsBlockModel blockModel)
        {
            for (int i = 0; i < definView.Blocks.Count; i++)
            {
                ViewsBlockModel block = definView.Blocks[i];
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
                        if (!blockModel.DataSetType.Equals("row"))
                        {
                            block.BlockDataPath.Add(pathDic);
                        }
                        block.ViewId = blockModel.ViewId;
                        if (block.DataSetType.Equals("table"))
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
