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
using Victop.Frame.Templates.ModelTemplate;
using Victop.Wpf.Controls;

namespace Victop.Frame.Templates.LeftCondtionTemplates
{
    /// <summary>
    /// UCModelCondition.xaml 的交互逻辑
    /// </summary>
    public partial class UCModelCondition : UserControl
    {
        #region 委托及事件
        /// <summary>
        /// Combox委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ComboxCheckedDelegate(object sender, EventArgs e);
        /// <summary>
        /// 本阶段按钮选中事件
        /// </summary>
        public event ComboxCheckedDelegate rdoBtnBenjieduanChecked;
        /// <summary>
        /// 未确认按钮选中事件
        /// </summary>
        public event ComboxCheckedDelegate rdoBtnUnConfirmChecked;
        /// <summary>
        /// 确认按钮选中事件
        /// </summary>
        public event ComboxCheckedDelegate rdoBtnConfirmChecked; 
        #endregion

        #region 属性改变通知
        #region 属性通知事件
        /// <summary>
        /// 属性通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region 属性改变通知
        /// <summary>
        /// 属性改变通知
        /// </summary>
        /// <param name="propetyName"></param>
        public void RaisePropertyChanged(string propetyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propetyName));
            }
        }
        #endregion
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCModelCondition()
        {
            InitializeComponent();
        }

        #endregion

        #region 变量
        /// <summary>
        /// 查询条件实体
        /// </summary>
        private ModelDataShowTModel searchModel;
        private DataTable dataSource;
        private string displayMember;
        private string selectedValue;
        #endregion

        #region 属性
        #region 查询条件实体
        /// <summary>
        /// 查询条件实体
        /// </summary>
        public ModelDataShowTModel SearchModel
        {
            get
            {
                if (searchModel==null)
                {
                    searchModel = new ModelDataShowTModel();
                }
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

        /// <summary>
        /// 下拉列表数据源
        /// </summary>
        public DataTable DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }
        /// <summary>
        /// 显示列名
        /// </summary>
        public string DisplayMember
        {
            get { return displayMember; }
            set { displayMember = value; }
        }
        /// <summary>
        /// 绑定列
        /// </summary>
        public string SelectedValue
        {
            get { return selectedValue; }
            set { selectedValue = value; }
        }

        #endregion

      

        #endregion

        #region 三个RadioButton选中事件
        #region 本阶段
        /// <summary>
        /// 本阶段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoBtnBenjieduan_Checked(object sender, RoutedEventArgs e)
        {
            SearchModel.RdoBtnDocStatus = "0";
            if (rdoBtnBenjieduanChecked != null)
            {
                rdoBtnBenjieduanChecked(sender, e);
            }
        }
        #endregion

        #region 未确认
        /// <summary>
        /// 未确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoBtnUnConfirm_Checked(object sender, RoutedEventArgs e)
        {
            SearchModel.RdoBtnDocStatus = "1";
            if (rdoBtnUnConfirm != null)
            {
                rdoBtnUnConfirmChecked(sender, e);
            }
        }
        #endregion

        #region 确认
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoBtnConfirm_Checked(object sender, RoutedEventArgs e)
        {
            SearchModel.RdoBtnDocStatus = "2";
            if (rdoBtnConfirmChecked != null)
            {
                rdoBtnConfirmChecked(sender, e);
            }
        }
        #endregion 
        #endregion

        #region 界面加载
        /// <summary>
        /// 界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VictGridSearch_Loaded(object sender, RoutedEventArgs e)
        {
            //开始日期
            Binding binding = new Binding();
            binding.Source = SearchModel;
            binding.Path = new PropertyPath("StartDate");
            BindingOperations.SetBinding(datePickerStartDate, VicDatePickerNormal.SelectedDateProperty, binding);
            //结束日期
            Binding binding2 = new Binding();
            binding2.Source = SearchModel;
            binding2.Path = new PropertyPath("EndDate");
            BindingOperations.SetBinding(datePickerEndDate, VicDatePickerNormal.SelectedDateProperty, binding2);
            //下拉列表
            Binding binding3 = new Binding();
            binding3.Source = SearchModel;
            binding3.Path = new PropertyPath("CombSqlFilter");
            BindingOperations.SetBinding(cmboxSqlFilter, VicComboBoxNormal.SelectedValueProperty, binding3);
            //文本框
            Binding binding4 = new Binding();
            binding4.Source = SearchModel;
            binding4.Path = new PropertyPath("TboxSqlFilter");
            BindingOperations.SetBinding(txtSqlFilter, VicTextBoxNormal.TextProperty, binding4);

            //设置下拉列表数据源
            cmboxSqlFilter.DisplayMemberPath = DisplayMember;
            cmboxSqlFilter.SelectedValuePath = SelectedValue;
            cmboxSqlFilter.ItemsSource = DataSource.DefaultView;
        } 
        #endregion
    }
}
