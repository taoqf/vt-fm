using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Victop.Frame.CmptRuntime;

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
            FeiDaoFSM = new BaseStateMachine("MainViewRules", Assembly.GetExecutingAssembly(), this);
            ParamDict = paramDict;
            ShowType = showType;
            this.Loaded += mainView_Loaded;
        }
        private void dgridProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //FeiDaoFSM.Do("SelectRow");
        }

        private void mainView_Loaded(object sender, RoutedEventArgs e)
        {
            FeiDaoFSM.Do("Load", sender);
        }
    }
}
