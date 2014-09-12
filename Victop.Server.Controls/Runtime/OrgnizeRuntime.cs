using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Server.Controls.Runtime
{
    public class OrgnizeRuntime
    {
        public static void InitCompnt(ComponentModel runtime)
        {
            RebulidViews(runtime);
            RebuildPlugins(runtime);
        }

        private static void RebulidViews(ComponentModel runtime)
        {
            foreach (DefinViewsModel item in runtime.CompntDefin.Views)
            {
                Dictionary<string, object> viewDic = JsonHelper.ToObject<Dictionary<string, object>>(runtime.CompntSettings.ViewSetting["views"].ToString());
                if (viewDic.ContainsKey(item.ViewName))
                {
                    item.ModelId = JsonHelper.ReadJsonString(viewDic[item.ViewName].ToString(), "modelid");
                    item.DataFileName = JsonHelper.ReadJsonString(viewDic[item.ViewName].ToString(), "datafilename");
                    foreach (ViewsBlockModel blockitem in item.Blocks)
                    {
                        Dictionary<string, object> blockDic = JsonHelper.ToObject<Dictionary<string, object>>(JsonHelper.ReadJsonString(viewDic[item.ViewName].ToString(), "blocks"));
                        if (blockDic.ContainsKey(blockitem.BlockName))
                        {
                            blockitem.TableName = JsonHelper.ReadJsonString(blockDic[blockitem.BlockName].ToString(), "tablename");
                        }
                    }
                }
            }
        }

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
    }
}
