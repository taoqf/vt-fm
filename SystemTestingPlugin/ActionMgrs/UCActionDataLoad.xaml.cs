using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Victop.Frame.PublicLib.Helpers;
using Victop.Wpf.Controls;

namespace SystemTestingPlugin.ActionMgrs
{
    /// <summary>
    /// UCActionDataLoad.xaml 的交互逻辑
    /// </summary>
    public partial class UCActionDataLoad : TemplateControl
    {
        private DataTable detailDt;
        private ActionElement elementModel;
        private ActionElement stageModel;
        private ObservableCollection<ActionElementRelation> stageRelationModelList;
        public ActionElement ElementModel
        {
            get
            {
                if (elementModel == null)
                    elementModel = new ActionElement();
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
        #region 窗口管理
        VicWindowNormal editWindow;
        #endregion
        public UCActionDataLoad()
        {
            this.DataContext = this;
        }
        public UCActionDataLoad(DataRow masterDr, DataTable detailDt)
        {
            InitializeComponent();
            this.DataContext = this;
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                actionHeader.HeaderRow = masterDr;
                ParamDict.Add("masterRow", masterDr);
                this.detailDt = detailDt;
                DataRowToModel();
            }
        }

        private void DataRowToModel()
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
        private void EditAction()
        {
            editWindow = new VicWindowNormal() { Title = "编辑数据规范", Width = 400, Height = 300, ShowInTaskbar = false, WindowStartupLocation = WindowStartupLocation.CenterOwner };
            editWindow.Content = new UCActionDataLoadEdit(actionHeader.HeaderRow["action_no"].ToString(), detailDt) { ParentControl = this };
            editWindow.Owner = XamlTreeHelper.GetParentObject<Window>(actionHeader);
            editWindow.SetResourceReference(VicWindowNormal.StyleProperty, "WindowMessageSkin");
            editWindow.ShowDialog();
        }

        public override void Excute(Dictionary<string, object> paramDic)
        {
            if (paramDic != null)
            {
                string messageType = paramDic.ContainsKey("MessageType") ? paramDic["MessageType"].ToString() : string.Empty;
                Dictionary<string, object> contentDic = (Dictionary<string, object>)paramDic["MessageContent"];
                switch (messageType)
                {
                    case "Update":
                        EditAction();
                        break;
                    case "Up":
                    case "Down":
                    case "Delete":
                        if (ParentControl != null)
                        {
                            ParentControl.Excute(paramDic);
                        }
                        break;
                    case "Save":
                        if (ParentControl != null)
                        {
                            contentDic["rowid"] = actionHeader.HeaderRow["_id"].ToString();
                            ParentControl.Excute(paramDic);
                            DataRowToModel();
                            editWindow.Close();
                        }
                        break;
                    case "Show":
                        if (gridContent.Visibility == Visibility.Visible)
                        {
                            gridContent.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            gridContent.Visibility = Visibility.Visible;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
