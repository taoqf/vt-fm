using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace VictopPartner
{
    /// <summary>
    /// NotificationCenter.xaml 的交互逻辑
    /// </summary>
    public partial class NotificationCenter : Window
    {
        private readonly double _screenWidth;
        private readonly DispatcherTimer _timer;
        private readonly DispatcherTimer _timerClose;
        private readonly Storyboard _storyboardShow;
        private readonly Storyboard _storyboardHide;
        public NotificationCenter()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(10);
            _timer.Tick += timer_Tick;
            _timer.Start();

            _timerClose = new DispatcherTimer();
            _timerClose.Interval = TimeSpan.FromSeconds(10);
            _timerClose.Tick += timerClose_Tick;

            System.Drawing.Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;

            _screenWidth = workingRectangle.Width;
            this.Left = _screenWidth;
            this.Top = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;

            _storyboardShow = new Storyboard();
            DoubleAnimationUsingKeyFrames doubleAnimation = new DoubleAnimationUsingKeyFrames();
            EasingDoubleKeyFrame easingDoubleKeyFrame = new EasingDoubleKeyFrame(_screenWidth - this.Width, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2)));

            doubleAnimation.KeyFrames.Add(easingDoubleKeyFrame);
            _storyboardShow.Children.Add(doubleAnimation);
            Storyboard.SetTarget(doubleAnimation, this);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));

            _storyboardHide = new Storyboard();
            doubleAnimation = new DoubleAnimationUsingKeyFrames();
            easingDoubleKeyFrame = new EasingDoubleKeyFrame(_screenWidth, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2)));

            doubleAnimation.KeyFrames.Add(easingDoubleKeyFrame);
            _storyboardHide.Children.Add(doubleAnimation);
            Storyboard.SetTarget(doubleAnimation, this);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
        }
        //定时刷新，得到实时告警信息
        private void timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            _timerClose.Start();

            _storyboardShow.Begin();
        }

        private void timerClose_Tick(object sender, EventArgs e)
        {
            _timer.Start();
            _timerClose.Stop();
            _storyboardHide.Begin();
        }


        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _timer.Start();
            _timerClose.Stop();
            _storyboardHide.Begin();
        }
    }
}
