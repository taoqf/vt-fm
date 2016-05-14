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

namespace SystemTestingPlugin.ActionMgrs
{
    /// <summary>
    /// UCActionHeader.xaml 的交互逻辑
    /// </summary>
    public partial class UCActionHeader : TemplateControl
    {
        private DataRow headerRow;
        public UCActionHeader()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public DataRow HeaderRow
        {
            get
            {
                return headerRow;
            }

            set
            {
                if (headerRow != value)
                {
                    headerRow = value;
                    RaisePropertyChanged(() => HeaderRow);
                }
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (ParentControl != null)
            {
                Dictionary<string, object> messageDic = new Dictionary<string, object>();
                messageDic.Add("MessageType", "Update");
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("rowid", headerRow["_id"].ToString());
                messageDic.Add("MessageContent", contentDic);
                ParentControl.Excute(messageDic);

            }
        }
        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            if (ParentControl != null)
            {
                Dictionary<string, object> messageDic = new Dictionary<string, object>();
                messageDic.Add("MessageType", "Up");
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("rowid", headerRow["_id"].ToString());
                contentDic.Add("itemobject", ParentControl);
                messageDic.Add("MessageContent", contentDic);
                ParentControl.Excute(messageDic);
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            if (ParentControl != null)
            {
                Dictionary<string, object> messageDic = new Dictionary<string, object>();
                messageDic.Add("MessageType", "Down");
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("rowid", headerRow["_id"].ToString());
                contentDic.Add("itemobject", ParentControl);
                messageDic.Add("MessageContent", contentDic);
                ParentControl.Excute(messageDic);
            }
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            if (ParentControl != null)
            {
                Dictionary<string, object> messageDic = new Dictionary<string, object>();
                messageDic.Add("MessageType", "Show");
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("state", true);
                messageDic.Add("MessageContent", contentDic);
                ParentControl.Excute(messageDic);
            }
        }
    }
}
