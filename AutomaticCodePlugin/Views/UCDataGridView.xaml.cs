using System.Windows;
using Victop.Frame.CmptRuntime;
using System.Reflection;
using System.IO;
using Victop.Frame.PublicLib.Helpers;
using System.Data;

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
#if DEBUG
            SpaceId = "feidao";
#endif
            FeiDaoMachine = new BaseBusinessMachine("DataGridView", this);
            BusinessModel = 1;
            BrowserLoadComplate += UCDataGridView_BrowserLoadComplate;
            DataContext = this;
        }

        private void UCDataGridView_BrowserLoadComplate()
        {
            FeiDaoMachine.Do("afterinit", this);
            MainPBlock = GetPresentationBlockModel("masterPBlock");
        }
    }
}
