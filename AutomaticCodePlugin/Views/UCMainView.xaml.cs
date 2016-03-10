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
        public UCMainView(Dictionary<string, object> paramDict, int showType)
        {
            InitializeComponent();
            FeiDaoMachine = new BaseBusinessMachine("MainView", this);
            BrowserLoadComplate += UCMainView_BrowserLoadComplate;
            ParamDict = paramDict;
            ShowType = showType;
            this.DataContext = this;
        }

        private void UCMainView_BrowserLoadComplate()
        {
            //FeiDaoMachine.Do("init", this);
        }

        private void dgridProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //FeiDaoFSM.Do("SelectRow");
        }
    }
}
