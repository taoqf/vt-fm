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
        #endregion
        public CompntDataGrid()
        {
            InitializeComponent();
            comRunTime.CompntDefin = JsonHelper.ToObject<CompntDefinModel>(Properties.Resources.CompntDataGrid_Def);
            comRunTime.CompntSettings = JsonHelper.ToObject<CompntSettingModel>(Properties.Resources.CompntDataGrid_Setting);
            OrgnizeRuntime.InitCompnt(comRunTime);
        }
        public void GetData()
        {
            foreach (DefinViewsModel item in comRunTime.CompntDefin.Views)
            {
                string viewId = SendMessage(item.ModelId);
                if (!string.IsNullOrEmpty(viewId))
                {
                    ViewsBlockModel rootBlock = item.Blocks.Find(it => it.Superiors.Equals("root"));
                    rootBlock.ViewId = viewId;
                    rootBlock.BlockDataPath = new List<object>() { rootBlock.TableName };
                    DataOperation dataOp = new DataOperation();
                    rootBlock.BlockDt = dataOp.GetData(rootBlock.ViewId, JsonHelper.ToJson(rootBlock.BlockDataPath));
                    if(rootBlock.BlockDt!=null&&rootBlock.BlockDt.Rows.Count>0)
                    {
                        rootBlock.CurrentRow = new Dictionary<string, object>();
                        foreach (DataColumn col in rootBlock.BlockDt.Columns)
                        {
                            rootBlock.CurrentRow.Add(col.ColumnName, rootBlock.BlockDt.Rows[0][col]);
                        }
                    }
                }
            }
            firstplugin.DataSource = comRunTime.CompntDefin.Views.Find(it=>it.ViewName.Equals("firstview")).Blocks.Find(it => it.BlockName.Equals("tom")).BlockDt;
        }
        private string SendMessage(string modelId)
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
    }
}
