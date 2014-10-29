using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Xml;
using Victop.Server.Controls;
using Victop.Wpf.Controls;

namespace ThemeManagerPlugin.Views
{
    /// <summary>
    /// ThemesWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ThemesWindow : Victop.Wpf.Controls.VicWindowNormal
    {
        Storyboard stdEnd;
        public ThemesWindow()
        {
            InitializeComponent();
            stdEnd = (Storyboard)portalWindow.Resources["end"];
        }

        private void ThemesWindow_OnClosed(object sender, EventArgs e)
        {
            //MessageBoxResult result = VicMessageBoxNormal.Show("确定要退出么？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);
            //if (result == MessageBoxResult.Yes)
            //{
            //    this.stdEnd.Begin();
            //    stdEnd.Completed += (c, d) =>
            //    {
            //        portalWindow.Close();
                   
            //    };  
            //}
        }
        
       
    }

}
