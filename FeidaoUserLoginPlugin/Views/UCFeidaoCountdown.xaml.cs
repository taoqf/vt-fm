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
        bool isGo = true;
        private bool isNoClick = true;
        private int count=0;
        private Storyboard look3DStoryboard;
        private Storyboard lookOpposite3DStoryboard;
        private Storyboard look3D1Storyboard;
        private Storyboard look3D2Storyboard;
        DispatcherTimer timer = new DispatcherTimer();
        public UCFeidaoCountdown()
        {
            InitializeComponent();
            look3DStoryboard = (Storyboard)this.Resources["begin3DStoryboard"];
            lookOpposite3DStoryboard = (Storyboard)this.Resources["beginOpposite3DStoryboard"];
            look3D1Storyboard = (Storyboard)this.Resources["begin3D1Storyboard"];
            look3D2Storyboard = (Storyboard)this.Resources["beginOpposite3D1Storyboard"];
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            count++;
            if (isNoClick)
            { 
                if (count %2 == 0)
                {
                    look3DStoryboard.Begin();
                    lookOpposite3DStoryboard.Stop();
                }
                else
                {
                    lookOpposite3DStoryboard.Begin();
                    look3DStoryboard.Stop();
                }
            
            }
           
        }
        private void UCFeidaoCountdown_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            isNoClick=false;
            look3DStoryboard.Stop();
            if (isGo)
            {
                look3D1Storyboard.Begin();
                look3D2Storyboard.Stop();
                isGo = false;
            }
            else
            {
                look3D1Storyboard.Stop();
                look3D2Storyboard.Begin();
                isGo = true;
            }
        }
     
    }
}
