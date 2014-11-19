using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using MachinePlatformPlugin.Views;
using GalaSoft.MvvmLight.Command;
using Victop.Server.Controls.Models;
using Victop.Frame.SyncOperation;
using Victop.Frame.PublicLib.Helpers;
using System.Windows.Controls;
using Victop.Wpf.Controls;
using System.Windows;
using System.Data;
using System.ComponentModel;

namespace MachinePlatformPlugin.ViewModels
{
    /// <summary>
    /// 操作窗口ViewModel
    /// </summary>
    public class OperationWindowViewModel : ModelBase
    {
        #region 字段
        /// <summary>
        /// 主窗口
        /// </summary>
        private OperationWindow windowOperationView;
        /// <summary>
        /// 插件实体
        /// </summary>
        private PluginModel pluginModel;
        /// <summary>
        /// Tab集合
        /// </summary>
        private object tabList;
        #endregion
        #region 属性
        public object TabList
        {
            get
            {
                return tabList;
            }
            set
            {
                if (tabList != value)
                {
                    tabList = value;
                    RaisePropertyChanged("TabList");
                }
            }
        }
        #endregion
        #region 命令
        /// <summary>
        /// 窗口加载
        /// </summary>
        public ICommand windowOperationViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    windowOperationView = (OperationWindow)x;
                    TabList = CreateTabContent();
                    windowOperationView.Closing += windowOperationView_Closing;
                });
            }
        }

        void windowOperationView_Closing(object sender, CancelEventArgs e)
        {
            if (pluginModel.PluginInterface.ParamDict["row"] != null)
            {
                Dictionary<string, object> rowDic = JsonHelper.ToObject<Dictionary<string, object>>(JsonHelper.ToJson(pluginModel.PluginInterface.ParamDict["row"]));
                foreach (string item in rowDic.Keys)
                {
                    if (OperationWindow.CabinetInfoModel.CabinetSelectedDataRow.Table.Columns.Contains(item) && rowDic[item] != null)
                    {
                        OperationWindow.CabinetInfoModel.CabinetSelectedDataRow[item] = rowDic[item];
                    }
                }
            }
            OperationWindow.CabinetInfoModel.CabinetCADResultDic = JsonHelper.ToObject<Dictionary<string, object>>(JsonHelper.ToJson(pluginModel.PluginInterface.ParamDict["resultdic"]));
            foreach (string item in OperationWindow.CabinetInfoModel.CabinetCADResultDic.Keys)
            {
                if (OperationWindow.CabinetInfoModel.CabinetSelectedDataRow.Table.Columns.Contains(item) && OperationWindow.CabinetInfoModel.CabinetCADResultDic[item] != null)
                {
                    OperationWindow.CabinetInfoModel.CabinetSelectedDataRow[item] = OperationWindow.CabinetInfoModel.CabinetCADResultDic[item];
                }
            }
        }

        public ICommand btnSaveClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //OperationWindow.CabinetInfoModel.CabinetSelectedDataRow["wt_state"] = "1";
                });
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 启动CAD插件
        /// </summary>
        /// <returns></returns>
        private PluginModel RunCADPlugin()
        {
            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add("systemid", OperationWindow.CabinetInfoModel.SystemId);
            paramDic.Add("configsystemid", OperationWindow.CabinetInfoModel.ConfigSystemId);
            paramDic.Add("spaceid", OperationWindow.CabinetInfoModel.SpaceId);
            Dictionary<string, object> rowDic = new Dictionary<string, object>();
            if (OperationWindow.CabinetInfoModel.CabinetSelectedDataRow != null)
            {
                foreach (DataColumn item in OperationWindow.CabinetInfoModel.CabinetSelectedDataRow.Table.Columns)
                {
                    rowDic.Add(item.ColumnName, OperationWindow.CabinetInfoModel.CabinetSelectedDataRow[item.ColumnName]);
                }
            }
            paramDic.Add("caddata", OperationWindow.CabinetInfoModel.CabinetCADOperationData);
            paramDic.Add("row", rowDic);
            OperationWindow.CabinetInfoModel.CabinetCADResultDic["file_name"] = OperationWindow.CabinetInfoModel.CabinetSelectedDataRow["file_name"];
            OperationWindow.CabinetInfoModel.CabinetCADResultDic["file_type"] = OperationWindow.CabinetInfoModel.CabinetSelectedDataRow["file_type"];
            OperationWindow.CabinetInfoModel.CabinetCADResultDic["file_path"] = OperationWindow.CabinetInfoModel.CabinetSelectedDataRow["file_path"];
            paramDic.Add("resultdic", OperationWindow.CabinetInfoModel.CabinetCADResultDic);
            PluginOperation pluginOp = new PluginOperation();
            return pluginOp.StratPlugin(OperationWindow.CabinetInfoModel.CabinetCADName, paramDic);

        }
        /// <summary>
        /// 创建Tab区域
        /// </summary>
        /// <returns></returns>
        private object CreateTabContent()
        {
            pluginModel = RunCADPlugin();
            if (!string.IsNullOrEmpty(pluginModel.ObjectId))
            {
                windowOperationView.Title = pluginModel.PluginInterface.PluginTitle;
                return pluginModel.PluginInterface.StartControl;
            }
            else
            {
                windowOperationView.Hide();
                VicMessageBoxNormal.Show("CAD" + pluginModel.ErrorMsg, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                windowOperationView.Close();
                return null;
            }
        }
        #endregion
    }
}
