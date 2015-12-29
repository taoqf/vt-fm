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
using Victop.Frame.CmptRuntime;

namespace FeidaoUserLoginPlugin.Views
{
    /// <summary>
    /// UCCurrentTime.xaml 的交互逻辑
    /// </summary>
    public partial class UCCurrentTime : TemplateControl
    {
        #region 字段&属性
        private string yearArea;
        private string year2Area;
        private string year3Area;
        private string year4Area;
        private string monthArea;
        private string month2Area;
        private string dayArea;
        private string day2Area;
        private string timeArea;
        private string time2Area;

        public string Time2Area
        {
            get
            {
                return time2Area;
            }
            set
            {
                if (time2Area != value)
                {
                    time2Area = value;
                    RaisePropertyChanged(() => Time2Area);
                }
            }
        }
        public string TimeArea
        {
            get
            {
                return timeArea;
            }
            set
            {
                if (timeArea != value)
                {
                    timeArea = value;
                    RaisePropertyChanged(() => TimeArea);
                }
            }
        }
        public string Day2Area
        {
            get
            {
                return day2Area;
            }
            set
            {
                if (day2Area != value)
                {
                    day2Area = value;
                    RaisePropertyChanged(() => Day2Area);
                }
            }
        }
        public string DayArea
        {
            get
            {
                return dayArea;
            }
            set
            {
                if (dayArea != value)
                {
                    dayArea = value;
                    RaisePropertyChanged(() => DayArea);
                }
            }
        }
        public string Month2Area
        {
            get
            {
                return month2Area;
            }
            set
            {
                if (month2Area != value)
                {
                    month2Area = value;
                    RaisePropertyChanged(() => Month2Area);
                }
            }
        }
        public string MonthArea
        {
            get
            {
                return monthArea;
            }
            set
            {
                if (monthArea != value)
                {
                    monthArea = value;
                    RaisePropertyChanged(() => MonthArea);
                }
            }
        }
        public string Year4Area
        {
            get
            {
                return year4Area;
            }
            set
            {
                if (year4Area != value)
                {
                    year4Area = value;
                    RaisePropertyChanged(() => Year4Area);
                }
            }
        }
        public string Year3Area
        {
            get
            {
                return year3Area;
            }
            set
            {
                if (year3Area != value)
                {
                    year3Area = value;
                    RaisePropertyChanged(() => Year3Area);
                }
            }
        }
        public string Year2Area
        {
            get
            {
                return year2Area;
            }
            set
            {
                if (year2Area != value)
                {
                    year2Area = value;
                    RaisePropertyChanged(() => Year2Area);
                }
            }
        }
        public string YearArea
        {
            get
            {
                return yearArea;
            }
            set
            {
                if (yearArea != value)
                {
                    yearArea = value;
                    RaisePropertyChanged(() => YearArea);
                }
            }
        }
        #endregion
        public UCCurrentTime()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Loaded += UCCurrentTime_Loaded;
        }

        void UCCurrentTime_Loaded(object sender, RoutedEventArgs e)
        {
            getCurrentTime();
        }
        private void getCurrentTime()
        {
            string ts = DateTime.Now.ToString();
            YearArea = ts.Substring(0, 1);
            Year2Area = ts.Substring(1, 1);
            Year3Area = ts.Substring(2, 1);
            Year4Area = ts.Substring(3, 1);
            MonthArea = ts.Substring(5, 1);
            Month2Area = ts.Substring(6, 1);
            DayArea = ts.Substring(8, 1);
            Day2Area = ts.Substring(9, 1);
            TimeArea = ts.Substring(11, 1);
            Time2Area = ts.Substring(12, 1);
        }
    }
}
