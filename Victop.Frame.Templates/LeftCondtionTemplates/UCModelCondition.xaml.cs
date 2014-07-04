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
using Victop.Frame.Templates.ModelTemplate;

namespace Victop.Frame.Templates.LeftCondtionTemplates
{
    /// <summary>
    /// UCModelCondition.xaml 的交互逻辑
    /// </summary>
    public partial class UCModelCondition : UserControl
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
        #endregion

        #region 属性
        #region 查询条件实体
        /// <summary>
        /// 查询条件实体
        /// </summary>
        public ModelDataShowTModel SearchModel
        {
            get { return searchModel; }
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

        private void rdoBtnBenjieduan_Checked(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        private void rdoBtnUnConfirm_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rdoBtnConfirm_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
