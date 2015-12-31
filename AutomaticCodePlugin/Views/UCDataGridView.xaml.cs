using System.Windows;
using Victop.Frame.CmptRuntime;
using System.Reflection;
using System.IO;
using Victop.Frame.PublicLib.Helpers;

namespace AutomaticCodePlugin.Views
{
    /// <summary>
    /// UCDataGridView.xaml 的交互逻辑
    /// </summary>
    public partial class UCDataGridView : TemplateControl
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
        public UCDataGridView()
        {
            InitializeComponent();
            FeiDaoFSM = new BaseStateMachine("DataGridView", Assembly.GetExecutingAssembly(), this);
            DataContext = this;
            this.Loaded += UCDataGridView_Loaded;
        }
        private void UCDataGridView_Loaded(object sender, RoutedEventArgs e)
        {
           if(InitFlag)
            {
                MainPBlock = GetPresentationBlockModel("masterPBlock");
                FeiDaoFSM.Do("afterinit", sender);
            }
        }
    }
}
