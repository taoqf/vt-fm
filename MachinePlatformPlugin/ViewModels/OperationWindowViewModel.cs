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
                });
            }
        }
        /// <summary>
        /// 窗口关闭时
        /// </summary>
        public ICommand windowOperationViewClosingCommand
        {
            get
            {
                return new RelayCommand(() => {
                    OperationWindow.CabinetInfoModel.CabinetCADResultDic = (Dictionary<string,object>)pluginModel.PluginInterface.ParamDict["resultdic"];
                });
            }
        }

        public ICommand btnSaveClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    OperationWindow.CabinetInfoModel.CabinetSelectedDataRow["wt_state"] = "1";
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
            paramDic.Add("row", OperationWindow.CabinetInfoModel.CabinetSelectedDataRow);
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
