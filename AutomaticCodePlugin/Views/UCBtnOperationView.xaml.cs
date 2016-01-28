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
            FeiDaoMachine = new BaseBusinessMachine("BtnOpView", this);
            BusinessModel = 1;
            this.BrowserLoadComplate += UCBtnOperationView_BrowserLoadComplate;
        }

        private void UCBtnOperationView_BrowserLoadComplate()
        {
            FeiDaoMachine.Do("afterinit", this);
        }
        private void searchBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FeiDaoMachine.Do("search", sender);
        }

        private void addBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //FeiDaoMachine.Do("add", sender);
        }
    }
}
