using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FeidaoUserLoginPlugin.Views
{
    /// <summary>
    /// UCFeidaoUserLogin.xaml 的交互逻辑
    /// </summary>
    public partial class UCFeidaoUserLogin : UserControl
    {
        public UCFeidaoUserLogin()
        {
            InitializeComponent();
            this.Loaded+=UCFeidaoUserLogin_Loaded;
        }
        void UCFeidaoUserLogin_Loaded(object sender, RoutedEventArgs e)
        {
            tm.Interval = TimeSpan.FromSeconds(0.2);
        }
        public DispatcherTimer tm = new DispatcherTimer();
    }
}
