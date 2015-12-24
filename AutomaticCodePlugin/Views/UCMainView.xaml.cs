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
using Victop.Server.Controls;

namespace AutomaticCodePlugin.Views
{
    /// <summary>
    /// UCMainView.xaml 的交互逻辑
    /// </summary>
    public partial class UCMainView : TemplateControl
    {
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
        public UCMainView(Dictionary<string, object> paramDict, int showType)
        {
            InitializeComponent();
            this.DataContext = this;
            FeiDaoFSM = new MainViewStateMachine(this);
            ParamDict = paramDict;
            ShowType = showType;
            ucBtnOp.SearchBtnClick += OnSearchBtnClick;
            ucBtnOp.AddBtnClick += OnAddBtnClick;
        }

        private void OnAddBtnClick(object sender, Dictionary<string, object> paramDic)
        {
            FeiDaoFSM.Do("Add");
        }

        private void OnSearchBtnClick(object sender, Dictionary<string, object> paramDic)
        {
            FeiDaoFSM.Do("Search");
        }

        private void dgridProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //FeiDaoFSM.Do("SelectRow");
        }

        private void mainView_Loaded(object sender, RoutedEventArgs e)
        {
            FeiDaoFSM.Do("Load");
        }
    }
}
