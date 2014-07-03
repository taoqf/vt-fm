using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Victop.Frame.DataChannel;
using Victop.Frame.MessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Wpf.Controls;


namespace Victop.Frame.Templates.ModelTemplate
{
    /// <summary>
    /// UCModelDataShowWinT.xaml 的交互逻辑
    /// </summary>
    public partial class UCModelDataShowT : UserControl
    {
        #region 委托
        public delegate void newBill();
        public delegate void dgridDouble();

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性改变通知
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region 变量
        private string doccode;
        private ModelDataShowTModel searchModel;
        private string DataChannelId;
        private DataTable dtData;
        public event newBill newBillClick;
        private bool IsBoundColumn = false;
        public event dgridDouble dgridDoubleClick;
        private DataTable dtTable;
        /// <summary>
        /// 是否初次加载（默认“是”，“是”则执行初始化命令，“否”则跳过初始化）
        /// </summary>
        bool IsFirstLoad = true;  //是否初次加载（默认“是”，“是”则执行初始化命令，“否”则跳过初始化）
        #endregion

        #region 属性

        #region 查询条件实体类
        /// <summary>
        /// 查询条件实体
        /// </summary>
        public ModelDataShowTModel SearchModel
        {
            get
            {
                if (searchModel == null)
                    searchModel = new ModelDataShowTModel();
                // searchModel.StartDate = DateTime.Now.AddDays(-7);

                return searchModel;
            }
            set
            {
                if (searchModel != value)
                {
                    searchModel = value;
                    RaisePropertyChanged("SearchModel");
                }
            }
        }

        #endregion

        #region SystemId
        /// <summary>
        /// SystemId
        /// </summary>
        public string SystemId
        {
            get { return (string)GetValue(SystemIdProperty); }
            set { SetValue(SystemIdProperty, value); }
        }
        #endregion

        #region FormId
        /// <summary>
        /// FormId
        /// </summary>
        public string FormId
        {
            get { return (string)GetValue(FormIdProperty); }
            set { SetValue(FormIdProperty, value); }
        }
        #endregion

        #region ModelId
        /// <summary>
        /// ModelId
        /// </summary>
        public string ModelId
        {
            get { return (string)GetValue(ModelIdProperty); }
            set { SetValue(ModelIdProperty, value); }
        }
        #endregion

        #region 是否显示新单
        /// <summary>
        /// 是否显示新单按钮        
        /// </summary>
        public string IsShowNewBill
        {
            get
            {
                if (((string)GetValue(IsShowNewBillProperty)).ToLower() == "true")
                {
                    return "Visible";
                }
                else
                {
                    return "Collapsed";
                }
            }
            set { SetValue(IsShowNewBillProperty, value); }
        }
        #endregion

        #region dgrid数据源
        public DataTable DtData
        {
            get { return dtData; }
            set { dtData = value; }
        }
        #endregion

        #region 依赖属性
        public static readonly DependencyProperty SystemIdProperty = DependencyProperty.Register("SystemId", typeof(string), typeof(UCModelDataShowT));
        public static readonly DependencyProperty FormIdProperty = DependencyProperty.Register("FormId", typeof(string), typeof(UCModelDataShowT));
        public static readonly DependencyProperty ModelIdProperty = DependencyProperty.Register("ModelId", typeof(string), typeof(UCModelDataShowT));
        public static readonly DependencyProperty IsShowNewBillProperty = DependencyProperty.Register("IsShowNewBill", typeof(string), typeof(UCModelDataShowT));
        #endregion

        /// <summary>
        /// 单据号
        /// </summary>
        public string Doccode
        {
            get { return doccode; }
            set { doccode = value; }
        }
        #endregion

        #region 无参构造函数
        public UCModelDataShowT()
        {
            InitializeComponent();
        }
        #endregion

        #region 私有方法
        #region 界面初始化
        /// <summary>
        /// 界面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Binding binding = new Binding();
            binding.Source = SearchModel;
            binding.Path = new PropertyPath("StartDate");
            BindingOperations.SetBinding(datePickerStartDate, VicDatePickerNormal.SelectedDateProperty, binding);

            Binding binding2 = new Binding();
            binding2.Source = SearchModel;
            binding2.Path = new PropertyPath("EndDate");
            BindingOperations.SetBinding(datePickerEndDate, VicDatePickerNormal.SelectedDateProperty, binding2);

            Binding binding3 = new Binding();
            binding3.Source = SearchModel;
            binding3.Path = new PropertyPath("TboxSqlFilter");
            BindingOperations.SetBinding(txtSqlFilter, VicTextBox.TextProperty, binding3);

            Binding binding4 = new Binding() { Mode= BindingMode.TwoWay,UpdateSourceTrigger= UpdateSourceTrigger.PropertyChanged};
            binding4.Source = SearchModel;
            binding4.Path = new PropertyPath("CombSqlFilter");
            BindingOperations.SetBinding(cmboxSqlFilter, VicComboBoxNormal.SelectedValueProperty, binding4);

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                GetDataFromServerByModel();
            }
        }

        #endregion

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromServerByModel();
        }
        #endregion

        #region 打印
        /// <summary>
        /// 打印
        /// </summary>
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("打印成功！");
        }
        #endregion

        #region 确认方法
        /// <summary>
        /// 确认方法
        /// </summary>
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            //-----------------------------------------待修改
            string currentDoccode = "";
            if (dtData != null && dtData.Rows.Count > 0 && dgrid.SelectedIndex != -1)
            {
                DataRow currentChosedDr = dtData.Rows[dtData.Rows.IndexOf(((DataRowView)dgrid.SelectedItem).Row)];
                if (currentChosedDr["docstatus"].ToString() == "0") //未确认
                {
                    //currentDoccode = currentChosedDr["doccode"].ToString(); //单据号
                    //DoEJBSample(currentDoccode);
                    //GetDataFromServer();
                    currentChosedDr["docstatus"] = "10";  //单据状态(已确认未收款)
                    currentChosedDr["postname"] = "zhao";  //确认人员
                    currentChosedDr["postdate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //确认日期
                    //currentChosedDr["modifyname"] = "zhao";  //修改人
                    currentChosedDr["modifydate"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //修改日期
                }
                else
                {
                    MessageBox.Show("您选择的单子的状态已确认，请勿重复操作！");
                    return;
                }
            }
            else
            {
                MessageBox.Show("请选择一条有效的数据！");
                return;
            }
            try
            {
                SaveModelData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("确认失败：" + ex.Message);
                LoggerHelper.Error("", ex);
            }
        }
        #endregion

        #region 清除按钮方法，重置控件
        /// <summary>
        /// 清除按钮方法，重置控件
        /// </summary>
        private void btnEliminate_Click(object sender, RoutedEventArgs e)
        {
            SetCmBoxSqlFilterDataSouce();
            searchModel.StartDate = DateTime.Now.AddDays(-70);//------待修改
            searchModel.EndDate = DateTime.Now;
            searchModel.TboxSqlFilter = string.Empty;
            rdoBtnBenjieduan.IsChecked = true;
            GetDataFromServerByModel();
        }
        #endregion

        #region 新单方法
        /// <summary>
        /// 新单按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewBill_Click(object sender, RoutedEventArgs e)
        {
            if (IsShowNewBill == "Visible" && newBillClick != null)
            {
                newBillClick();
            }
        }
        #endregion

        #region 刷新数据方法
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefreshData_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromServerByModel();
        }
        #endregion

        #region 绑定模糊查询下拉列表
        /// <summary>
        /// 绑定模糊查询下拉列表
        /// </summary>
        private void SetCmBoxSqlFilterDataSouce()
        {
            if (DtData == null) return;
            #region 设置下拉列表绑定列
            if (!IsBoundColumn)
            {
                dtTable = new DataTable();
                dtTable.Columns.Add("columncaption");//列中文名
                dtTable.Columns.Add("columnid");//列英文名。
                foreach (DataColumn column in DtData.Columns)
                {
                    DataRow dr = dtTable.NewRow();
                    if (string.IsNullOrEmpty(column.Caption))
                    {
                        dr[0] = column.ColumnName;
                    }
                    else
                    {
                        dr[0] = column.Caption;
                    }
                    dr[1] = column.ColumnName;
                    dtTable.Rows.Add(dr);
                }
            }
            #endregion 
        }

        #region 将数据绑定到下拉列表上
        /// <summary>
        /// 将数据绑定到combox上。
        /// </summary>
        private void SetDataToCmBox()
        {
            cmboxSqlFilter.DisplayMemberPath = "columncaption";//columncaption
            cmboxSqlFilter.SelectedValuePath = "columnid";//columnid
            cmboxSqlFilter.SelectedIndex = 0;
            cmboxSqlFilter.ItemsSource = dtTable.DefaultView;
        }
        #endregion
        #endregion

        #region Dgrid双击方法，弹出销售换货明细界面
        /// <summary>
        /// Dgrid双击方法，弹出销售出库明细界面
        /// </summary>
        private void dgrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtData != null && dtData.Rows.Count > 0 && dgrid.SelectedIndex != -1)
            {
                this.Doccode = dtData.Rows[dtData.Rows.IndexOf(((DataRowView)dgrid.SelectedItem).Row)]["doccode"].ToString();
                if (dgridDoubleClick != null)
                {
                    dgridDoubleClick();
                }
            }
        }
        #endregion

        #region 三个radioButton
        private void rdoBtnBenjieduan_Checked(object sender, RoutedEventArgs e)
        {
            SearchModel.RdoBtnDocStatus = "0";
        }

        private void rdoBtnUnConfirm_Checked(object sender, RoutedEventArgs e)
        {
            SearchModel.RdoBtnDocStatus = "1";
        }

        private void rdoBtnConfirm_Checked(object sender, RoutedEventArgs e)
        {
            SearchModel.RdoBtnDocStatus = "2";
        }
        #endregion

        #region 过账逻辑,更改库存
        /// <summary>
        /// 调用EJB示例
        /// </summary>
        private void DoEJBSample()
        {
            //过账;
            //DataParams parameters = new DataParams();
            //parameters.ControlId = FormId;//设置ControlId
            //parameters.DocCode = Doccode;//设置单据编号
            //parameters.SystemId = SystemId;//设置系统ID
            //parameters.FormId = FormId;//设置功能号            
            ////使用GetDataByMethodName方法以反射方式调用EJB方法
            ////第一个参数为要调用的方法名称
            ////第二个参数为普通操作参数
            ////第三个参数为数据引用操作参数
            ////返回为后台EJB操作后的结果
            //DataResult result1 = PluginControl.GetDataByMethodName("DoEJB", parameters);
            //MessageBox.Show(result1.Message);
        }
        #endregion

        #region 模型取数
        #region 模型取数
        /// <summary>
        /// 模型取数
        /// </summary>
        private void GetDataFromServerByModel()
        {
            PluginMessage pm = new PluginMessage();
            pm.SendMessage("", OrganizeModelRequestMessage(), new WaitCallback(SearchData));
        }
        #endregion

        #region 组织模型取数消息
        /// <summary>
        /// 组织模型取数消息
        /// </summary>
        /// <returns></returns>
        private string OrganizeModelRequestMessage()
        {

            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "DataChannelService.getFormBusiDataAsync");
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("openType", null);
            contentDic.Add("bzsystemid", SystemId);
            contentDic.Add("formid", FormId);
            contentDic.Add("dataSetID", null);
            contentDic.Add("reportID", null);
            contentDic.Add("modelId", ModelId);
            contentDic.Add("fieldName", null);
            contentDic.Add("masterOnly", false);
            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
            paramsDic.Add("isdata", "1");
            Dictionary<string, string> sqlstrDic = new Dictionary<string, string>();
            sqlstrDic.Add("1", GetSqlFilterConditionData());
            //sqlstrDic.Add("1", " 1=1");
            sqlstrDic.Add("2", " 1=1");
            contentDic.Add("whereArr", sqlstrDic);
            contentDic.Add("masterParam", null);
            contentDic.Add("deltaXml", null);
            contentDic.Add("shareFlag", null);
            contentDic.Add("treeStr", null);
            contentDic.Add("saveType", null);
            contentDic.Add("doccode", null);
            string content = JsonHelper.ToJson(contentDic);
            messageDic.Add("MessageContent", content);
            string str = JsonHelper.ToJson(messageDic);
            return JsonHelper.ToJson(messageDic);
        }
        #endregion

        #region 获取数据，并设置界面数据
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="message"></param>
        private void SearchData(object message)
        {
            DataChannelId = JsonHelper.ReadJsonString(message.ToString(), "DataChannelId");
            DataOperation operateData = new DataOperation();
            DataSet ds = operateData.GetData(DataChannelId);
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new WaitCallback(UpdateTableList), ds);

        }
        #endregion

        #region 并设置界面数据
        /// <summary>
        /// 设置界面数据
        /// </summary>
        /// <param name="state"></param>
        private void UpdateTableList(object ds)
        {
            if (ds != null)
            {
                dtData = ((DataSet)ds).Tables["1"];
                dgrid.ItemsSource = dtData.DefaultView;
                //设置界面数据。
                SetCmBoxSqlFilterDataSouce();
                SetDataToCmBox();
                IsBoundColumn = true;
            }
        }

        #endregion

        #endregion

        #region 模型保存

        #region 模型保存请求
        /// <summary>
        /// 模型保存
        /// </summary>
        private void SaveModelData()
        {
            try
            {
                PluginMessage pluginMessage = new PluginMessage();
                pluginMessage.SendMessage("", OrganizeModelSaveMessage(), new WaitCallback(SaveDataSuccess));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// 组织模型保存消息
        /// </summary>
        /// <returns></returns>
        private string OrganizeModelSaveMessage()
        {
            Dictionary<string, object> messageDic = new Dictionary<string, object>();
            messageDic.Add("MessageType", "DataChannelService.saveBusiData");
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("DataChannelId", DataChannelId);
            contentDic.Add("bzsystemid", SystemId);
            contentDic.Add("modelId", ModelId);
            contentDic.Add("formid", FormId);
            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
            string content = JsonHelper.ToJson(contentDic);
            messageDic.Add("MessageContent", content);
            return JsonHelper.ToJson(messageDic);
        }
        #endregion

        #region 保存请求成功后取数
        /// <summary>
        /// 保存请求成功后，根据返回结构向数据通道请求数据
        /// </summary>
        /// <param name="message"></param>
        private void SaveDataSuccess(object message)
        {
            try
            {
                //dataChannelId = JsonHelper.ReadJsonString(message.ToString(), "DataChannelId");
                DataOperation operateData = new DataOperation();
                DataSet ds = operateData.GetData(DataChannelId);
                if (ds != null)
                {
                    DtData = ds.Tables["1"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        #endregion

        #region 筛选条件
        /// <summary>
        /// 筛选条件
        /// </summary>       
        private string GetSqlFilterConditionData()//查询的逻辑：筛选条件都可为空
        {
            try
            {
                string sqlstr = "1=1";
                if (SearchModel.StartDate != null && SearchModel.EndDate != null)
                {
                    DateTime starttime = SearchModel.StartDate;
                    DateTime endtime = SearchModel.EndDate;
                    if (starttime > endtime) { MessageBox.Show("时间区间有误！"); return "1=0"; }
                    else
                    {
                        sqlstr += "and docdate >=cast('" + searchModel.StartDate.ToString("yyyy-MM-dd") + "' as VARCHAR(10)) and docdate <cast('" + searchModel.EndDate.AddDays(1).ToString("yyyy-MM-dd") + "' as VARCHAR(10))";
                    }
                }
                if (SearchModel.CombSqlFilter != null)
                {
                    sqlstr += " and " + SearchModel.CombSqlFilter + " like '%" + SearchModel.TboxSqlFilter + "%'";
                }
                if (SearchModel.RdoBtnDocStatus == "0") { }//本阶段
                else if (SearchModel.RdoBtnDocStatus == "1")//未确认
                { sqlstr += " and docstatus=0"; }
                else if (SearchModel.RdoBtnDocStatus == "2")//已确认
                {
                    sqlstr += " and docstatus=1";
                }
                return sqlstr;
            }
            catch (Exception ex) { return "1=0"; }
        }

        #endregion

        #endregion
    }
}
