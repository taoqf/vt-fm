using AutomaticCodePlugin.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace AutomaticCodePlugin.Views
{
    /// <summary>
    /// UCMainView.xaml 的交互逻辑
    /// </summary>
    public partial class UCMainView : TemplateControl
    {
        #region 私有字段
        PresentationBlockModel mainPBlock;
        MainStateMachine mainFsm;
        #endregion
        #region 公共属性
        public PresentationBlockModel MainPBlock
        {
            get
            {
                return mainPBlock;
            }
            set
            {
                mainPBlock = value;
                RaisePropertyChanged("MainPBlock");
            }
        }
        #endregion
        public UCMainView()
        {
            InitializeComponent();
            DataContext = this;
            mainFsm = new MainStateMachine(this);
        }

        private void mainView_Loaded(object sender, RoutedEventArgs e)
        {
            mainFsm.MainLoad();
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            mainFsm.Search();
        }
    }
}
