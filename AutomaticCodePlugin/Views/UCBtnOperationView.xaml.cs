using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Victop.Frame.CmptRuntime;

namespace AutomaticCodePlugin.Views
{
    /// <summary>
    /// UCBtnOperationView.xaml 的交互逻辑
    /// </summary>
    public partial class UCBtnOperationView : TemplateControl
    {
        public UCBtnOperationView()
        {
            InitializeComponent();
            FeiDaoFSM = new BaseStateMachine("BtnOpViewRules", Assembly.GetExecutingAssembly(), this);
            this.Loaded += UCBtnOperationView_Loaded;
        }

        private void UCBtnOperationView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            FeiDaoFSM.Do("Load", sender);
        }

        private void searchBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FeiDaoFSM.Do("Search", sender);
        }

        private void addBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FeiDaoFSM.Do("Add", sender);
        }
    }
}
