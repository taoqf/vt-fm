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
using Victop.Frame.CmptRuntime;
using Victop.Frame.DataMessageManager;

namespace SystemTestingPlugin.ActionMgrs
{
    /// <summary>
    /// UCActionDataReference.xaml 的交互逻辑
    /// </summary>
    public partial class UCActionDataReference : TemplateControl
    {
        DataMessageOperation messageOp = new DataMessageOperation();
        private DataTable resultDt;
        public UCActionDataReference()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public DataTable ResultDt
        {
            get
            {
                return resultDt;
            }

            set
            {
                resultDt = value;
                RaisePropertyChanged(() => ResultDt);
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (dgridResult.SelectedValue != null)
            {
                if (ParentControl != null)
                {
                    Dictionary<string, object> messageDic = new Dictionary<string, object>();
                    messageDic.Add("MessageType", "DataReference");
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("BackData", dgridResult.SelectedValue);
                    messageDic.Add("MessageContent", contentDic);
                    ParentControl.Excute(messageDic);
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            GetProductInfo();
        }

        private void GetProductInfo()
        {
            string MessageType = "MongoDataChannelService.findBusiData";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "18");
            contentDic.Add("modelid", "feidao-model-pub_product-0001");
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                string channelId = returnDic["DataChannelId"].ToString();
                ResultDt = messageOp.GetData(channelId, "[\"pub_product\"]").Tables["dataArray"];
            }
        }
    }
}
