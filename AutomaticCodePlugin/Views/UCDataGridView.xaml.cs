using System.Windows;
using Victop.Frame.CmptRuntime;
using System.Reflection;
using System.IO;

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
            //FeiDaoFSM = new BaseStateMachine("DataGridViewRules", Assembly.GetExecutingAssembly(), this);
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

            //Stream pvdStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("UCDataGridView");
            //StreamReader sr = new StreamReader(pvdStream);
            //string pvdStr = sr.ReadToEnd();
            //if (InitVictopUserControl(pvdStr))
            //{
            //    MainPBlock = GetPresentationBlockModel("masterPBlock");
            //}
            //FeiDaoFSM.Do("Load", sender);
        }
    }
}
