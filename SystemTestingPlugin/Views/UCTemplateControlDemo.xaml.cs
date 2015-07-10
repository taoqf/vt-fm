using Victop.Frame.CmptRuntime;
using Victop.Frame.PublicLib.Helpers;

namespace SystemTestingPlugin.Views
{
    /// <summary>
    /// UCTemplateControlDemo.xaml 的交互逻辑
    /// </summary>
    public partial class UCTemplateControlDemo : TemplateControl
    {
        public UCTemplateControlDemo()
        {
            DefinModel = JsonHelper.ToObject<CompntDefinModel>(FileHelper.ReadFitData("newcmpnt"));
            InitializeComponent();
        }
    }
}
