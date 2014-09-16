using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Victop.Server.Controls.Runtime;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.SyncOperation;
using Victop.Frame.DataChannel;
using System.Data;

namespace Victop.Frame.Component
{
    /// <summary>
    /// CompntDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class CompntDataGrid : UserControl
    {
        #region 属性定义
        /// <summary>
        /// 组件运行时
        /// </summary>
        private ComponentModel comRunTime = new ComponentModel();
        /// <summary>
        /// 组件设置
        /// </summary>
        public CompntSettingModel SettingModel;
        DataOperation dataOp = new DataOperation();
        #endregion
        public CompntDataGrid()
        {
            InitializeComponent();
            comRunTime.CompntDefin = JsonHelper.ToObject<CompntDefinModel>(Properties.Resources.CompntDataGrid_Def);
            this.Loaded += CompntDataGrid_Loaded;
        }

        void CompntDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            comRunTime.CompntSettings = SettingModel;
            firstplugin.SelectedItemChanged += firstplugin_SelectedItemChanged;
        }

        void firstplugin_SelectedItemChanged(object sender, DataRow dr)
        {
            DefinPluginsModel pluginModel = (DefinPluginsModel)firstplugin.Tag;
            pluginModel.PluginBlock.CurrentRow.Clear();
            foreach (DataColumn item in dr.Table.Columns)
            {
                pluginModel.PluginBlock.CurrentRow.Add(item.ColumnName, dr[item.ColumnName]);
            }
            OrgnizeRuntime.RebuildAllDataPath(comRunTime);
            DefinPluginsModel spluginModel = (DefinPluginsModel)secondplugin.Tag;
            spluginModel.PluginBlock.BlockDt = dataOp.GetData(spluginModel.PluginBlock.ViewId, JsonHelper.ToJson(spluginModel.PluginBlock.BlockDataPath));
            secondplugin.DataSource = spluginModel.PluginBlock.BlockDt;
            //secondplugin.DoRender();
        }
        /// <summary>
        /// 组件执行
        /// </summary>
        /// <param name="setttingModel"></param>
        public void DoRender(CompntSettingModel setttingModel)
        {
            if (setttingModel == null)
            {
                comRunTime.CompntSettings = SettingModel;
            }
            else
            {
                comRunTime.CompntSettings = setttingModel;
            }
            OrgnizeRuntime.InitCompnt(comRunTime);
            foreach (DefinViewsModel item in comRunTime.CompntDefin.Views)
            {
                string viewId = SendFindMessage(item.ModelId);
                if (!string.IsNullOrEmpty(viewId))
                {
                    item.ViewId = viewId;
                    ViewsBlockModel rootBlock = item.Blocks.Find(it => it.Superiors.Equals("root"));
                    rootBlock.ViewId = viewId;
                    rootBlock.BlockDataPath = new List<object>() { rootBlock.TableName };
                    rootBlock.BlockDt = dataOp.GetData(rootBlock.ViewId, JsonHelper.ToJson(rootBlock.BlockDataPath));
                    if (rootBlock.BlockDt != null && rootBlock.BlockDt.Rows.Count > 0)
                    {
                        rootBlock.CurrentRow = new Dictionary<string, object>();
                        foreach (DataColumn col in rootBlock.BlockDt.Columns)
                        {
                            rootBlock.CurrentRow.Add(col.ColumnName, rootBlock.BlockDt.Rows[0][col]);
                        }
                    }
                }
            }
            foreach (DefinPluginsModel item in comRunTime.CompntDefin.Plugins)
            {
                OrgnizeRuntime.BindingPlugin(comRunTime, item.PluginName);
                if (this.FindName(item.PluginName).GetType().Name.Equals(typeof(UnitDataGrid).Name))
                {
                    UnitDataGrid unitgrid = this.FindName(item.PluginName) as UnitDataGrid;
                    unitgrid.Tag = item;
                    unitgrid.DataSource = item.PluginBlock.BlockDt;
                }
                if (this.FindName(item.PluginName).GetType().Name.Equals(typeof(UnitRowDataPanel).Name))
                {
                    UnitRowDataPanel unitpanel = this.FindName(item.PluginName) as UnitRowDataPanel;
                    unitpanel.Tag = item;
                }
            }
        }
        /// <summary>
        /// 发送查询消息
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private string SendFindMessage(string modelId)
        {
            try
            {
                string MessageType = "MongoDataChannelService.findBusiData";
                MessageOperation messageOp = new MessageOperation();
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("systemid", "100");
                contentDic.Add("configsystemid", "101");
                contentDic.Add("spaceid", "victop_core");
                contentDic.Add("modelid", "table::iteration");
                Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                if (returnDic != null)
                {
                    return returnDic["DataChannelId"].ToString();
                }
                return null;
            }
            catch (Exception ex)
            {
                string temp = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 发送保存消息
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private Dictionary<string, object> SendSaveMessage(DefinViewsModel viewModel)
        {
            DataOperation dataOp = new DataOperation();
            foreach (ViewsBlockModel item in viewModel.Blocks)
            {
                dataOp.SaveData(item.ViewId, JsonHelper.ToJson(item.BlockDataPath));
            }
            string MessageType = "MongoDataChannelService.saveBusiData";
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "100");
            contentDic.Add("configsystemid", "101");
            contentDic.Add("spaceid", "victop_core");
            contentDic.Add("modelid", "table::iteration");
            contentDic.Add("DataChannelId", viewModel.ViewId);
            Dictionary<string, object> resultDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
            return resultDic;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            DoRender(null);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> resultDic = SendSaveMessage(comRunTime.CompntDefin.Views[0]);
            if (resultDic != null)
            {
                MessageBox.Show(JsonHelper.ReadJsonString(resultDic["ReplyContent"].ToString(), "msg"));
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }
    }


    //public class CompntParamModel
    //{
    //    public string Param1
    //    {
    //        get;
    //        set;
    //    }
    //    public string Param2
    //    {
    //        get;
    //        set;
    //    }
    //    public string Param2
    //    {
    //        get;
    //        set;
    //    }
    //}
}
