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
using Victop.Frame.CmptRuntime;

namespace FeidaoUserLoginPlugin.Views
{
    /// <summary>
    /// UCFeidaoCountdown.xaml 的交互逻辑
    /// </summary>
    public partial class UCFeidaoCountdown : TemplateControl
    {
        public UCFeidaoCountdown()
        {
            InitializeComponent();
            
        }
        DoubleAnimation da = new DoubleAnimation();
        bool b = false;
        private void TemplateControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            //da.To = 0d;
            if(b==false)
            {
                da.To = 180d;
                b = true;
            }
            else if(b==true)
            {
                da.To = 0d;
                b = false;
            }
            this.axr.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
        }
    }
}
