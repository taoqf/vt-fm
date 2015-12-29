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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Victop.Frame.CmptRuntime;

namespace FeidaoUserLoginPlugin.Views
{
    /// <summary>
    /// UCCountDown.xaml 的交互逻辑
    /// </summary>
    public partial class UCCountDown : TemplateControl
    {
        #region 字段
        Storyboard stdStart;
        Storyboard stdStart2;
        Storyboard stdStart3;
        Storyboard stdStart4;
        Storyboard stdStart5;
        Storyboard stdStart6;
    
        private string sencond="0";
        private string sencondex="0";
        private string minute = "0";
        private string minuteEx = "0";
        private string hour="0";
        private string hourEx="0";
        private string kbDay = "0";
        private string hundredDay = "0";
        private string tenDay = "0";
        private string numDay = "0";

        public string NumDay
        {
            get
            {
                return numDay;
            }
            set
            {
                if (numDay != value)
                {
                    numDay = value;
                    RaisePropertyChanged(() => NumDay);
                }
            }
        }
        public string TenDay
        {
            get
            {
                return tenDay;
            }
            set
            {
                if (tenDay != value)
                {
                    tenDay = value;
                    RaisePropertyChanged(() => TenDay);
                }
            }
        }
        public string HundredDay
        {
            get
            {
                return hundredDay;
            }
            set
            {
                if (hundredDay != value)
                {
                    hundredDay = value;
                    RaisePropertyChanged(() => HundredDay);
                }
            }
        }
        public string KbDay
        {
            get
            {
                return kbDay;
            }
            set
            {
                if (kbDay != value)
                {
                    kbDay = value;
                    RaisePropertyChanged(() => KbDay);
                }
            }
        }
        public string HourEx
        {
            get
            {
                return hour;
            }
            set
            {
                if (hour != value)
                {
                    hour = value;
                    //stdStart6 = (Storyboard)this.Resources["Storyboard6"];
                    //stdStart6.Begin();
                    RaisePropertyChanged(() => HourEx);
                }
            }
        }
        public string Hour
        {
            get
            {
                return hour;
            }
            set
            {
                if (hour != value)
                {
                    //stdStart5 = (Storyboard)this.Resources["Storyboard5"];
                    //stdStart5.Begin();
                    hour = value;
                    RaisePropertyChanged(() => Hour);
                }
            }
        }
        public string MinuteEx
        {
            get
            {
                return minuteEx;
            }
            set
            {
                if (minuteEx != value)
                {
                    stdStart4 = (Storyboard)this.Resources["Storyboard4"];
                    stdStart4.Begin();
                    minuteEx = value;
                    RaisePropertyChanged(() => MinuteEx);
                }
            }
        }
        public string Minute
        {
            get {
                return minute;
            }
            set {
                if (minute!=value)
                {
                    stdStart3 = (Storyboard)this.Resources["Storyboard3"];
                    stdStart3.Begin();
                    minute = value;
                    RaisePropertyChanged(() => Minute);
                }
            }
        }
        public string SencondEx
        {
            get
            {
                return sencondex;
            }
            set
            {
                if (sencondex != value)
                {
                    //TODO:启动动画
                    stdStart2 = (Storyboard)this.Resources["Storyboard2"];
                    stdStart2.Begin();
                    sencondex = value;
                    RaisePropertyChanged(() => SencondEx);
                }
            }
        }
        public string Sencond
        {
            get
            {
                return sencond;
            }
            set
            {
                if (sencond != value)
                {
                    stdStart = (Storyboard)this.Resources["Storyboard1"];
                    stdStart.Begin();  
                    sencond = value;
                    RaisePropertyChanged(() => Sencond);
                }
            }
        }
        private DispatcherTimer dTimer = new DispatcherTimer();
        private DateTime endDate = new DateTime(2025, 1, 1, 0, 0, 0);
        #endregion
        public UCCountDown()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Loaded += UCCountDown_Loaded;
        }

        void UCCountDown_Loaded(object sender, RoutedEventArgs e)
        {
            dTimer.Tick += new EventHandler(dTimer_Tick);
            dTimer.Interval = new TimeSpan(0, 0, 1);
            NewMethod();
            dTimer.Start();
           
        }

        private void dTimer_Tick(object sender, EventArgs e)
        {
            NewMethod();
        }

        private void NewMethod()
        {
            TimeSpan ts = endDate - DateTime.Now;
            Sencond = ts.Seconds >= 10 ? ts.Seconds.ToString().Substring(1) : ts.Seconds.ToString();
            SencondEx = ts.Seconds >= 10 ? ts.Seconds.ToString().Substring(0, 1) : "0";
            Minute = ts.Minutes >= 10 ? ts.Minutes.ToString().Substring(1) : ts.Minutes.ToString();
            MinuteEx = ts.Minutes >= 10 ? ts.Minutes.ToString().Substring(0, 1) : "0";
            Hour = ts.Hours >= 10 ? ts.Hours.ToString().Substring(1) : ts.Hours.ToString();
            HourEx = ts.Hours >= 10 ? ts.Hours.ToString().Substring(0, 1) : "0";
            string DayStr = ts.Days.ToString();
            if(DayStr.Length==4)
            {
                KbDay = DayStr.Substring(0,1);
                HundredDay = DayStr.Substring(1, 1);
                TenDay = DayStr.Substring(2,1);
                NumDay = DayStr.Substring(3,1);
            }
            if (DayStr.Length == 3)
            {
                KbDay = "0";
                HundredDay = DayStr.Substring(0, 1);
                TenDay = DayStr.Substring(1, 1);
                NumDay = DayStr.Substring(2, 1);
            }
            if (DayStr.Length == 2)
            {
                KbDay = "0";
                HundredDay = "0";
                TenDay = DayStr.Substring(0, 1);
                NumDay = DayStr.Substring(1, 1);
            }
            if (DayStr.Length == 1)
            {
                KbDay = "0";
                HundredDay = "0";
                TenDay = "0";
                NumDay = DayStr.Substring(0, 1);
            }
        }
    }
}
