using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using Victop.Frame.MessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Wpf.Controls;

namespace PortalFramePlugin.Views
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
        private DataTable gridDataTable;
        public NotificationCenter()
        {
            InitializeComponent();
            string notifyTimeStr = ConfigurationManager.AppSettings["notifyTime"];
            double notifyTime = Convert.ToDouble(notifyTimeStr);
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(notifyTime);
            _timer.Tick += timer_Tick;
            _timer.Start();

            _timerClose = new DispatcherTimer();
            _timerClose.Interval = TimeSpan.FromSeconds(notifyTime);
            _timerClose.Tick += timerClose_Tick;
            Rect workingRectangle = SystemParameters.WorkArea;
            _screenWidth = workingRectangle.Width;
            this.Left = _screenWidth;
            this.Top = SystemParameters.WorkArea.Bottom - this.Height;

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
            GetNotifyMessage();
            if (gridDataTable != null && gridDataTable.Rows.Count > 0)
            {
                _timer.Stop();
                _timerClose.Start();
                _storyboardShow.Begin();
            }
        }
        /// <summary>
        /// 定时刷新，隐藏消息中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void GetNotifyMessage()
        {
            PluginMessage pluginMessage = new PluginMessage();
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "TaskNotifyService.GetNoifyInfo");
            Dictionary<string, string> contentDic = new Dictionary<string, string>();
            contentDic.Add("ObjectId", Guid.NewGuid().ToString());
            messageDic.Add("MessageContent", JsonHelper.ToJson(contentDic));
            pluginMessage.SendMessage("", JsonHelper.ToJson(messageDic), new WaitCallback(SaveDataSuccess));
        }
        private void SaveDataSuccess(object message)
        {
            gridDataTable = new DataTable();
            gridDataTable.Columns.Add("消息类型");
            gridDataTable.Columns.Add("消息内容");
            gridDataTable.Columns.Add("状态");
            Dictionary<string, object> replyDic = JsonHelper.ToObject<Dictionary<string, object>>(message.ToString());
            replyDic = JsonHelper.ToObject<Dictionary<string, object>>(replyDic["ReplyContent"].ToString());
            if (replyDic != null && replyDic.Keys.Count > 0)
            {
                //TODO:
            }
            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new WaitCallback(UpdateDataGridSource), gridDataTable);
        }
        private void UpdateDataGridSource(object ItemSource)
        {
            DataTable dt = (DataTable)ItemSource;
            datagridInfo.ItemsSource = dt.DefaultView;
        }
    }
}
