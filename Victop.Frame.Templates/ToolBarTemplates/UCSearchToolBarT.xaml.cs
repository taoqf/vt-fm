using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Victop.Frame.Templates.ToolBarTemplates
{
    /// <summary>
    /// UCSearchToolBarT.xaml 的交互逻辑
    /// </summary>
    public partial class UCSearchToolBarT : UserControl
    {
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

        #region 委托及事件
        /// <summary>
        /// 按钮委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ButtonDelegate(object sender, EventArgs e);
        /// <summary>
        /// 新增按钮点击事件
        /// </summary>
        public event ButtonDelegate btnAddClick;
        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        public event ButtonDelegate btnDeleteClick;
        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        public event ButtonDelegate btnCancelClick;
        /// <summary>
        /// 全消按钮点击事件
        /// </summary>
        public event ButtonDelegate btnCancelAllClick;
        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        public event ButtonDelegate btnSaveClick;
        #endregion

        #region 变量
        /// <summary>
        /// 取消按钮标志
        /// </summary>
        private bool cancelFlag; 
        #endregion

        #region 属性
        /// <summary>
        /// 取消按钮标志
        /// </summary>
        public bool CancelFlag
        {
            get { return cancelFlag; }
            set
            {
                if (cancelFlag != value)
                {
                    cancelFlag = value;
                    RaisePropertyChanged("CancelFlag");
                }
            }
        }
        
        #endregion

        #region 无参构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCSearchToolBarT()
        {
            InitializeComponent();
        } 
        #endregion

        #region 新增按钮单击
        /// <summary>
        /// 新增按钮单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (btnAddClick!=null)
            {
                btnAddClick(sender,e);
            }
        } 
        #endregion

        #region 删除按钮点击
        /// <summary>
        /// 删除按钮点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (btnDeleteClick!=null)
            {
                btnDeleteClick(sender, e);
            }
        } 
        #endregion

        #region 取消按钮单击
        /// <summary>
        /// 取消按钮单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (btnCancelClick!=null)
            {
                btnCancelClick(sender, e);
            }
        } 
        #endregion

        #region 全消按钮单击
        /// <summary>
        /// 全消按钮单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelAll_Click(object sender, RoutedEventArgs e)
        {
            if (btnCancelAllClick!=null)
            {
                btnCancelAllClick(sender, e);
            }
        } 
        #endregion

        #region 保存按钮单击
        /// <summary>
        /// 保存按钮单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (btnSaveClick!=null)
            {
                btnSaveClick(sender, e);
            }
        } 
        #endregion
       
    }
}
