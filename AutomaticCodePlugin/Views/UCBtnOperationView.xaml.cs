using AutomaticCodePlugin.FSM;
using Victop.Frame.CmptRuntime;

namespace AutomaticCodePlugin.Views
{
    /// <summary>
    /// UCBtnOperationView.xaml 的交互逻辑
    /// </summary>
    public partial class UCBtnOperationView : TemplateControl
    {
        /// <summary>
        /// 查询按钮点击事件
        /// </summary>
        public TemplateDelegateEvent SearchBtnClick;
        /// <summary>
        /// 添加按钮点击事件
        /// </summary>
        public TemplateDelegateEvent AddBtnClick;
        public UCBtnOperationView()
        {
            InitializeComponent();
            FeiDaoFSM = new BtnOpViewStateMachine();
            FeiDaoFSM.MainView = this;
            this.Loaded += UCBtnOperationView_Loaded;
        }

        private void UCBtnOperationView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            FeiDaoFSM.Do("Load");
        }

        private void searchBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FeiDaoFSM.Do("Search");
        }

        private void addBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FeiDaoFSM.Do("Add");
        }
    }
}
