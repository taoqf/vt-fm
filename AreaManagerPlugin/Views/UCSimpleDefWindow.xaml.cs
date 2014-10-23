using System;
using System.Collections.Generic;
using System.Data;
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
using Victop.Frame.DataChannel;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.SyncOperation;

namespace AreaManagerPlugin.Views
{
    /// <summary>
    /// UCSimpleDefWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UCSimpleDefWindow : UserControl
    {
        public UCSimpleDefWindow()
        {
            InitializeComponent();
        }

        private void cboxSimDef_DropDownOpened(object sender, EventArgs e)
        {
            SendFindMessage();
        }
        private void SendFindMessage()
        {
            string MessageType = "MongoDataChannelService.findBusiData";
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "906");
            contentDic.Add("configsystemid", "905");
            //contentDic.Add("spaceid", "tianlong");
            contentDic.Add("modelid", "tl_customer_in_0001");
            Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
            if (returnDic != null)
            {
                string viewId = returnDic["DataChannelId"].ToString();
                DataOperation dataOp = new DataOperation();
                DataSet temp = dataOp.GetData(viewId, "[\"simpleRef\"]");
            }
        }
    }
}
