using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Victop.Frame.CmptRuntime;

namespace FeidaoUserLoginPlugin.Views
{
    /// <summary>
    /// UCFeidaoCountdown.xaml 的交互逻辑
    /// </summary>
    public partial class UCFeidaoCountdown : TemplateControl
    {
        DoubleAnimation da = new DoubleAnimation();
        bool b = false;
        bool dacomplated = true;
        DispatcherTimer timer = new DispatcherTimer();
        public UCFeidaoCountdown()
        {
            InitializeComponent();
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.Completed += da_Completed;
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;
            timer.Start();

        }

        void da_Completed(object sender, EventArgs e)
        {
            dacomplated = true;
            timer.IsEnabled = true;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            BeginAnimation();
        }

        private void TemplateControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            timer.IsEnabled = false;
            BeginAnimation();
        }
        private void BeginAnimation()
        {
            if (dacomplated)
            {
                if (b == false)
                {
                    da.To = 180d;
                    b = true;
                }
                else if (b == true)
                {
                    da.To = 0d;
                    b = false;
                }
                this.axr.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
                dacomplated = false;
            }
        }
    }
}
