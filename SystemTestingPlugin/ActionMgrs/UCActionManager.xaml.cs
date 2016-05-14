using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Victop.Frame.PublicLib.Helpers;
using Victop.Wpf.Controls;

namespace SystemTestingPlugin.ActionMgrs
{
    /// <summary>
    /// UCActionManager.xaml 的交互逻辑
    /// </summary>
    public partial class UCActionManager : TemplateControl
    {
        DataMessageOperation messageOp = new DataMessageOperation();
        VicButtonNormal addBtn = new VicButtonNormal() { Content = "+", Width = 300, Height = 150, Name = "btnAddItem", FontSize = 80 };
        private string channelId = string.Empty;
        private string modelId = "feidao-model-data_block_action-0001_2.1";
        private string masterTableName = "data_block_action";
        private string detailTableName = "action_detailed";
        private string sortField = "fzno";
        public UCActionManager()
        {
            InitializeComponent();
            addBtn.Click += AddBtn_Click;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DrawingActionInfo()
        {
            lboxActionList.Items.Clear();
            lboxActionList.Items.Add(addBtn);
            if (!string.IsNullOrEmpty(channelId))
            {
                DataTable masterDt = messageOp.GetData(channelId, CreateDataPath("")).Tables["dataArray"];
                masterDt.DefaultView.Sort = "fzno asc";
                if (masterDt != null && masterDt.Rows.Count > 0)
                {
                    foreach (DataRow item in masterDt.Rows)
                    {
                        DataTable detailDt = messageOp.GetData(channelId, CreateDataPath(item["_id"].ToString())).Tables["dataArray"];
                        lboxActionList.Items.Insert(lboxActionList.Items.Count - 1, new UCActionDataLoad(item, detailDt) { ParentControl = this });
                    }
                }
            }
        }

        public UCActionManager(Dictionary<string, object> paramDict, int showType)
        {
            InitializeComponent();
            ParamDict = paramDict;
            ShowType = showType;
        }


        public override void Excute(Dictionary<string, object> paramDic)
        {
            if (paramDic != null)
            {
                string messageType = paramDic.ContainsKey("MessageType") ? paramDic["MessageType"].ToString() : string.Empty;
                Dictionary<string, object> contentDic = (Dictionary<string, object>)paramDic["MessageContent"];
                switch (messageType)
                {
                    case "Up":
                        UpdateSort(contentDic, true);
                        break;
                    case "Down":
                        UpdateSort(contentDic, false);
                        break;
                    case "Delete":
                        break;
                    case "Save":
                        if (contentDic.ContainsKey("rowid"))
                        {
                            messageOp.SaveData(channelId, CreateDataPath(contentDic["rowid"].ToString()));
                            SendSaveDataMessage();
                        }
                        break;
                }
            }
        }

        private void UpdateSort(Dictionary<string, object> contentDic, bool asc)
        {
            var itemobject = contentDic["itemobject"];
            int itemIndex = lboxActionList.Items.IndexOf(itemobject);
            lboxActionList.SelectedIndex = itemIndex;
            if (asc ? (itemIndex != 0) : (itemIndex < lboxActionList.Items.Count-2))
            {
                lboxActionList.Items.Remove(itemobject);
                lboxActionList.Items.Insert(asc ? itemIndex - 1 : itemIndex + 1, itemobject);
                TemplateControl sourcetc = (TemplateControl)itemobject;
                TemplateControl destinationtc = (TemplateControl)lboxActionList.Items[itemIndex];
                DataRow sourceRow = (DataRow)sourcetc.ParamDict["masterRow"];
                DataRow destinationRow = (DataRow)destinationtc.ParamDict["masterRow"];
                int indexNo = (int)sourceRow[sortField];
                sourceRow[sortField] = destinationRow[sortField];
                destinationRow[sortField] = indexNo;
                messageOp.SaveData(channelId, CreateDataPath());
                if (SendSaveDataMessage())
                {
                    GetActionInfo();
                    DrawingActionInfo();
                    lboxActionList.SelectedIndex = asc ? itemIndex - 1 : itemIndex + 1;
                }
                else
                {

                }
            }
        }

        private bool SendSaveDataMessage()
        {
            string messageType = "MongoDataChannelService.saveBusiData";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "18");
            contentDic.Add("modelid", modelId);
            contentDic.Add("DataChannelId", channelId);
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(messageType, contentDic);
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void mainView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                if (!lboxActionList.Items.Contains(addBtn))
                {
                    lboxActionList.Items.Add(addBtn);
                }
                if (string.IsNullOrEmpty(channelId))
                {
                    GetActionInfo();
                }
                DrawingActionInfo(); 
            }
        }
        private void GetActionInfo()
        {
            string MessageType = "MongoDataChannelService.findBusiData";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "18");
            contentDic.Add("modelid", modelId);
            Dictionary<string, object> conditionDic = new Dictionary<string, object>();
            Dictionary<string, object> sortDic = new Dictionary<string, object>();
            sortDic.Add(sortField, 1);
            conditionDic.Add("sort", sortDic);
            contentDic.Add("condition", conditionDic);
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(MessageType, contentDic);
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                channelId = returnDic["DataChannelId"].ToString();
            }
            else
            {
                VicMessageBoxNormal.Show("获取动作列表失败");
            }
        }
        private string CreateDataPath(string rowValue = "")
        {
            List<object> dataPathList = new List<object>();
            dataPathList.Add(masterTableName);
            if (!string.IsNullOrEmpty(rowValue))
            {
                Dictionary<string, object> rowDic = new Dictionary<string, object>();
                rowDic.Add("key", "_id");
                rowDic.Add("value", rowValue);
                dataPathList.Add(rowDic);
                dataPathList.Add(detailTableName);
            }
            return JsonHelper.ToJson(dataPathList);
        }
    }
}
