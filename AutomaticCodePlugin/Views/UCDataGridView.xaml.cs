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
using Victop.Frame.CmptRuntime;
using AutomaticCodePlugin.FSM;

namespace AutomaticCodePlugin.Views
{
    /// <summary>
    /// UCDataGridView.xaml 的交互逻辑
    /// </summary>
    public partial class UCDataGridView : TemplateControl
    {
        public UCDataGridView()
        {
            InitializeComponent();
            FeiDaoFSM = new DataGridViewStateMachine(this);
            DataContext = this;
            this.Loaded += UCDataGridView_Loaded;
        }
        PresentationBlockModel mainPBlock;
        public PresentationBlockModel MainPBlock
        {
            get
            {
                return mainPBlock;
            }
            set
            {
                mainPBlock = value;
                RaisePropertyChanged(() => MainPBlock);
            }
        }
        private void UCDataGridView_Loaded(object sender, RoutedEventArgs e)
        {
            if (InitVictopUserControl(Properties.Resources.masterPVDString))
            {
                MainPBlock = GetPresentationBlockModel("masterPBlock");
            }
            FeiDaoFSM.Do("Load", sender);
        }
    }
}
