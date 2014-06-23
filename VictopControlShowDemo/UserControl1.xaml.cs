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

namespace VictopControlShowDemo
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public Victop.Wpf.Controls.TabControl MainTabCtrl;

        public UserControl1()
        {
            InitializeComponent();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //Button btn = new Button();
            //btn.Width = 100;
            //btn.Height = 100;
            //btn.Content = "你好";
            //grid.Children.Add(btn);
        }
        private void imgtrade_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Victop.Wpf.Controls.TabItem item = new Victop.Wpf.Controls.TabItem();
            item.Header = "你好";
            Button btn = new Button();
            btn.Width = 50;
            btn.Height = 30;
            item.Content = btn;
            MainTabCtrl.Items.Add(item);
            MainTabCtrl.SelectedItem = item;
        }

        private void btnAdd1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
