using System;
using System.Collections.Generic;
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

namespace Victop.Frame.Templates.LeftCondtionTemplates
{
    /// <summary>
    /// UCTreeViewCondtionT.xaml 的交互逻辑
    /// </summary>
    public partial class UCTreeViewCondtionT : UserControl
    {
        #region TreeView委托及事件
        // <summary>
        /// TreeView委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void TreeViewDelegate(object sender, EventArgs e);
        /// <summary>
        /// TreeView加载事件。
        /// </summary>
        public event TreeViewDelegate TreeViewLoad;
        public event TreeViewDelegate TreeViewSelectedItemChanged;
        #endregion

        #region 无参构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCTreeViewCondtionT()
        {
            InitializeComponent();
        } 
        #endregion

        #region TreeView加载事件
        /// <summary>
        /// TreeView加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tviewEmployee_Loaded(object sender, RoutedEventArgs e)
        {
            if (TreeViewLoad != null)
            {
                TreeViewLoad(sender, e);
            }
        } 
        #endregion

        #region TreeView选择项改变事件
        /// <summary>
        /// TreeView选择项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tviewEmployee_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TreeViewSelectedItemChanged != null)
            {
                TreeViewSelectedItemChanged(sender, e);
            }
        } 
        #endregion
    }
}
