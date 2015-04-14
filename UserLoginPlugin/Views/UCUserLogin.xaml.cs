using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
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

namespace UserLoginPlugin.Views
{
    /// <summary>
    /// UCUserLogin.xaml 的交互逻辑
    /// </summary>
    public partial class UCUserLogin : UserControl
    {
        public UCUserLogin()
        {
            InitializeComponent();
            this.Loaded += UCUserLogin_Loaded;
        }

        void UCUserLogin_Loaded(object sender, RoutedEventArgs e)
        {
            tm.Interval = TimeSpan.FromSeconds(0.2);
        }  
        public DispatcherTimer tm = new DispatcherTimer();
    }

    public class ScrollintTextControl : Label
    {
        /// <summary>
        /// 定时器
        /// </summary>
        Timer MarqueeTimer = new Timer();
        /// <summary>
        /// 滚动文字源
        /// </summary>
        String _TextSource = "滚动文字源";
        /// <summary>
        /// 输出文本
        /// </summary>
        String _OutText = string.Empty;
        /// <summary>
        /// 文字的滚动速度
        /// </summary>
        double _RunSpeed = 500;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScrollintTextControl()
        {
            MarqueeTimer.Interval = _RunSpeed;//文字移动的速度
            MarqueeTimer.Enabled = true;      //开启定时触发事件
            MarqueeTimer.Elapsed += new ElapsedEventHandler(MarqueeTimer_Elapsed);//绑定定时事件
            this.Loaded += new RoutedEventHandler(ScrollingTextControl_Loaded);//绑定控件Loaded事件
        }

        void ScrollingTextControl_Loaded(object sender, RoutedEventArgs e)
        {
            _TextSource = SetContent;
            _OutText = _TextSource;
        }

        void MarqueeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (string.IsNullOrEmpty(_OutText)) return;
            _OutText = _OutText.Substring(1) + _OutText[0];
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SetContent = _OutText;
            }));
        }
        /// <summary>
        /// 滚动的速度
        /// </summary>
        [Description("文字滚动的速度")]　//显示在属性设计视图中的描述
        public double RunSpeed
        {
            get { return _RunSpeed; }
            set
            {
                _RunSpeed = value;
                MarqueeTimer.Interval = _RunSpeed;
            }
        }
        /// <summary>
        /// 滚动文字源
        /// </summary>
        [Description("文字滚动的速度")]
        public string TextSource
        {
            get { return _TextSource; }
            set
            {
                _TextSource = value;
                _OutText = _TextSource;
            }
        }
        private string SetContent
        {
            get { return Content.ToString(); }
            set
            {
                Content = value;
            }
        }
    }
}
