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

namespace MetroFramePlugin.Views
{
    /// <summary>
    /// UCPersonPluginContainer.xaml 的交互逻辑
    /// </summary>
    public partial class UCPersonPluginContainer : UserControl
    {
        public UCPersonPluginContainer()
        {
            InitializeComponent();
        }
        //private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        //{
        //    Canvas.SetLeft(thumb, Canvas.GetLeft(thumb) + e.HorizontalChange);
        //    Canvas.SetTop(thumb, Canvas.GetTop(thumb) + e.VerticalChange);
        //}
        private void thumb_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //left.Text = Canvas.GetLeft(thumb).ToString();
            //top.Text = Canvas.GetTop(thumb).ToString();
            //width.Text = thumb.Width.ToString();
            //height.Text = thumb.Height.ToString();
        }
    }
}
