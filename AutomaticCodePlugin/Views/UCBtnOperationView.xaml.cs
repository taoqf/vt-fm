using System.ComponentModel;
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
            FeiDaoFSM = new BaseStateMachine("BtnOpView", Assembly.GetExecutingAssembly(), this);
            this.Loaded += UCBtnOperationView_Loaded;
        }

        private void UCBtnOperationView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                FeiDaoFSM.Do("afterinit", sender);
            }
        }

        private void searchBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FeiDaoFSM.Do("search", sender);
        }

        private void addBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FeiDaoFSM.Do("add", sender);
        }
    }
}
