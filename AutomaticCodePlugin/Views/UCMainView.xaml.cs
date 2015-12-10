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
        PresentationBlockModel mainPBlock;
        public UCMainView()
        {
            InitializeComponent();
            this.DataContext = this;
            FeiDaoFSM = new MainStateMachine();
            FeiDaoFSM.MainView = this;
        }
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
        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            FeiDaoFSM.Do("SearchBtnClick");
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            FeiDaoFSM.Do("AddRow");
        }

        private void dgridProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FeiDaoFSM.Do("SelectRow");
        }

        private void mainView_Loaded(object sender, RoutedEventArgs e)
        {
            FeiDaoFSM.Do("ViewLoad");
        }
    }
}
