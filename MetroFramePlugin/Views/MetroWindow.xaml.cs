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
using Victop.Wpf.Controls;

namespace MetroFramePlugin.Views
{
    /// <summary>
    /// MetroWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MetroWindow : VicMetroWindow
    {
        public MetroWindow()
        {
            InitializeComponent();
            //this.gridTitleState.MouseLeftButtonDown += gridTitleState_MouseLeftButtonDown;
        }
        int i = 0;
        void gridTitleState_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            i += 1;
            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);

            timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };

            timer.IsEnabled = true;

            if (i % 2 == 0)
            {

                timer.IsEnabled = false;
                i = 0;
                if (CanResize == false)
                {
                    return;
                }

                else if (CanResize == true && WindowState == WindowState.Maximized)
                {
                    ChangeWindowState(WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);

                }
                else if (CanResize == true)
                {
                    ChangeWindowState(WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized);

                }

            }
            else
            {
                if (CanResize == false)
                {
                    return;
                }

                else if (CanResize == true && WindowState == WindowState.Maximized)
                {
                    //WindowState = WindowState.Normal;
                    DragMove();

                }
                else if (CanResize == true)
                {
                    DragMove();
                }
            }
        }
        
    }
}
