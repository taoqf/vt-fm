using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;

namespace SystemTestingPlugin.ActionMgrs
{
    /// <summary>
    /// UCActionDataLoadEdit.xaml 的交互逻辑
    /// </summary>
    public partial class UCActionDataLoadEdit : TemplateControl
    {
        private DataTable detailDt;
        private string actionNo;
        private ActionElement elementModel;
        private ActionElement stageModel;
        private ObservableCollection<ActionElementRelation> stageRelationModelList;

        public ActionElement ElementModel
        {
            get
            {
                if (elementModel == null)
                    elementModel = new ActionElement() { ActionNo = actionNo };
                return elementModel;
            }

            set
            {
                if (elementModel != value)
                {
                    elementModel = value;
                    RaisePropertyChanged(() => ElementModel);
                }
            }
        }

        public ActionElement StageModel
        {
            get
            {
                if (stageModel == null)
                    StageModel = new ActionElement();
                return stageModel;
            }

            set
            {
                if (stageModel != value)
                {
                    stageModel = value;
                    RaisePropertyChanged(() => StageModel);
                }
            }
        }
        public ObservableCollection<ActionElementRelation> StageRelationModelList
        {
            get
            {
                if (stageRelationModelList == null)
                    stageRelationModelList = new ObservableCollection<ActionElementRelation>();
                return stageRelationModelList;
            }

            set
            {
                if (stageRelationModelList != value)
                {
                    stageRelationModelList = value;
                    RaisePropertyChanged(() => StageRelationModelList);
                }
            }
        }
        #region 窗体管理
        VicWindowNormal elementRefWindow;
        #endregion
        public UCActionDataLoadEdit(string actionNo, DataTable detailDt)
        {
            InitializeComponent();
            this.DataContext = this;
            this.detailDt = detailDt;
            this.Loaded += UCActionDataLoadEdit_Loaded;
        }

        private void UCActionDataLoadEdit_Loaded(object sender, RoutedEventArgs e)
        {
            //根据target_no筛选出窄表数据转换成实体
            DataRow elementDr = detailDt.Select("target_no='tt'").FirstOrDefault();
            if (elementDr != null)
            {
                ElementModel.Id = elementDr["_id"].ToString();
                ElementModel.ActionNo = elementDr["action_no"].ToString();
                ElementModel.TargetNo = elementDr["target_no"].ToString();
                ElementModel.TargetName = elementDr["target_name"].ToString();
                ElementModel.TargetValue = elementDr["target_value"].ToString();
            }
            DataRow stageDr = detailDt.Select("target_no='abc'").FirstOrDefault();
            if (stageDr != null)
            {
                StageModel.Id = stageDr["_id"].ToString();
                StageModel.ActionNo = stageDr["action_no"].ToString();
                StageModel.TargetNo = stageDr["target_no"].ToString();
                StageModel.TargetName = stageDr["target_name"].ToString();
                StageModel.TargetValue = stageDr["target_value"].ToString();
                if (!string.IsNullOrEmpty(StageModel.TargetValue))
                {
                    StageRelationModelList = JsonHelper.ToObject<ObservableCollection<ActionElementRelation>>(StageModel.TargetValue);
                }
            }
        }

        private void vtboxName_VicTextBoxClick(object sender, RoutedEventArgs e)
        {
            elementRefWindow = new VicWindowNormal() { Title = "要素数据引用", Width = 400, Height = 300, ShowInTaskbar = false, WindowStartupLocation = WindowStartupLocation.CenterOwner };
            elementRefWindow.Content = new UCActionDataReference() { ParentControl = this };
            elementRefWindow.Owner = XamlTreeHelper.GetParentObject<Window>(vtboxName);
            elementRefWindow.SetResourceReference(VicWindowNormal.StyleProperty, "WindowMessageSkin");
            elementRefWindow.ShowDialog();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //将实体更改的内容转换到DataRow中
            DataRow elementDr = detailDt.Select("target_no='tt'").First();
            if (elementDr != null)
            {
                elementDr["target_value"] = ElementModel.TargetValue;
            }
            DataRow stageDr = detailDt.Select("target_no='abc'").FirstOrDefault();
            if (stageDr != null)
            {
                stageDr["target_value"] = JsonHelper.ToJson(StageRelationModelList);
            }
            //TODO:

            if (ParentControl != null)
            {
                Dictionary<string, object> messageDic = new Dictionary<string, object>();
                messageDic.Add("MessageType", "Save");
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("rowid", ElementModel.Id);
                messageDic.Add("MessageContent", contentDic);
                ParentControl.Excute(messageDic);
            }
        }

        public override void Excute(Dictionary<string, object> paramDic)
        {
            if (paramDic != null)
            {
                string messageType = paramDic.ContainsKey("MessageType") ? paramDic["MessageType"].ToString() : string.Empty;
                Dictionary<string, object> contentDic = (Dictionary<string, object>)paramDic["MessageContent"];
                switch (messageType)
                {
                    case "DataReference":
                        ElementModel.TargetValue = contentDic["BackData"].ToString();
                        elementRefWindow.Close();
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            StageRelationModelList.Add(new ActionElementRelation() { });
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgrid.SelectedItem != null)
            {
                StageRelationModelList.Remove((ActionElementRelation)dgrid.SelectedItem);
            }
        }
    }
}
