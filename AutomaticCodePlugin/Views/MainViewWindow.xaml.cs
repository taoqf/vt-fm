using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutomaticCodePlugin.Views
{
    /// <summary>
    /// MainViewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainViewWindow : Window
    {
        public MainViewWindow(Dictionary<string,object> paramDict)
        {
            InitializeComponent();
            UCMainView mainView = new UCMainView(paramDict);
            this.Content = mainView;
        }
    }
}
