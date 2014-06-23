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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VictopControlShowDemo
{
    /// <summary>
    /// PluginShellWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PluginShellWindow : Window
    {
        public PluginShellWindow()
        {
            InitializeComponent();
        }
        public string str;
        private void gridTitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            gridTitleBar.AllowDrop = true;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void btnMini_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                btnMax.SetResourceReference(StyleProperty, "btnMaxiStyle");
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                btnMax.SetResourceReference(StyleProperty, "btnRenewStyle");
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
