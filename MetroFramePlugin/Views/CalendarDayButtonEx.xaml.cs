using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// CalendarDayButtonEx.xaml 的交互逻辑
    /// </summary>
    public partial class CalendarDayButtonEx : UserControl, INotifyPropertyChanged
    {
        #region 成员属性
        private static Brush DefaultLunarBrush = new SolidColorBrush(Color.FromRgb(153, 153, 153));
        private string _lunar;
        [Description("农历")]
        [Category("Common Properties")]
        public string Lunar
        {
            get { return _lunar; }
            set
            {
                _lunar = value;
                if (value.Length > 3)
                {
                    _lunar = value.Substring(0, 3) + "...";
                }
                OnPropertyChanged("Lunar");
            }
        }


        private bool _isThisMonth = true;
        [Description("是不是当前月")]
        [Category("Common Properties")]
        public bool IsThisMonth
        {
            get { return _isThisMonth; }
            set
            {
                _isThisMonth = value;
                if (_isThisMonth)
                {
                    if (!IsToday)
                    {
                        ThisMonth = SolarDateTime.Month;
                        if (IsWeekend || IsFestival)
                        {
                            if (IsWeekend)
                            {
                                SolarBrush = Brushes.Red;
                            }
                            if (IsFestival)
                            {
                                LunarBrush = Brushes.Red;
                            }
                        }
                        else
                        {
                            SolarBrush = Brushes.Black;
                            LunarBrush = DefaultLunarBrush;
                        }
                    }
                }
                else
                {
                    SolarBrush = _notThisMonthBrush;
                    LunarBrush = _notThisMonthBrush;
                }
                OnPropertyChanged("IsThisMonth");
            }
        }

        private Brush _notThisMonthBrush = new SolidColorBrush(Color.FromRgb(191, 191, 191));
        [Description("不是当前月的颜色")]
        [Category("Common Properties")]
        public Brush NotThisMonthBrush
        {
            get { return _notThisMonthBrush; }
            set
            {
                _notThisMonthBrush = value;
                OnPropertyChanged("NotThisMonthBrush");
            }
        }

        private Brush _solarBrush;

        public Brush SolarBrush
        {
            get { return _solarBrush; }
            set
            {
                _solarBrush = value;
                OnPropertyChanged("SolarBrush");
            }
        }


        private Brush _lunarBrush = DefaultLunarBrush;

        public Brush LunarBrush
        {
            get { return _lunarBrush; }
            set
            {
                _lunarBrush = value;

                OnPropertyChanged("LunarBrush");
            }
        }

        private Brush _todayBrush;

        public Brush TodayBrush
        {
            get { return _todayBrush; }
            set
            {
                _todayBrush = value;
                OnPropertyChanged("TodayBrush");
            }
        }

        private DateTime _solarDateTime;

        public DateTime SolarDateTime
        {
            get { return _solarDateTime; }
            set
            {
                _solarDateTime = value;
                OnPropertyChanged("SolarDateTime");
            }
        }

        private bool _isToday = false;
        /// <summary>
        /// 是不是今天
        /// </summary>
        public bool IsToday
        {
            get { return _isToday; }
            set
            {
                _isToday = value;
                if (value)
                {
                    TodayBrush = new SolidColorBrush(Color.FromRgb(255, 187, 0));
                    SolarBrush = Brushes.White;
                    LunarBrush = Brushes.White;
                }
                else
                {
                    TodayBrush = Brushes.Transparent;
                }
                OnPropertyChanged("IsToday");
            }
        }

        private bool _isWeekend = false;
        [Description("是不是周末")]
        [Category("Common Properties")]
        /// <summary>
        /// 是不是周末
        /// </summary>
        public bool IsWeekend
        {
            get { return _isWeekend; }
            set
            {
                _isWeekend = value;

                if (IsThisMonth)
                {
                    if (_isWeekend)
                    {
                        SolarBrush = Brushes.Red;
                        if (IsFestival)
                        {
                            LunarBrush = Brushes.Red;
                        }
                        else
                        {
                            LunarBrush = DefaultLunarBrush;
                        }
                    }
                    else
                    {
                        SolarBrush = Brushes.Black;
                        if (IsFestival)
                        {
                            LunarBrush = Brushes.Red;
                        }
                        else
                        {
                            LunarBrush = DefaultLunarBrush;
                        }
                    }
                }
                else
                {
                    SolarBrush = _notThisMonthBrush;
                    LunarBrush = _notThisMonthBrush;
                }
                OnPropertyChanged("IsWeekend");
            }
        }

        private bool _isFestival = false;
        /// <summary>
        /// 是不是节日
        /// </summary>
        public bool IsFestival
        {
            get { return _isFestival; }
            set
            {
                _isFestival = value;
                if (!IsToday)
                {


                    if (IsThisMonth)
                    {
                        if (_isFestival)
                        {
                            LunarBrush = Brushes.Red;
                            if (IsWeekend)
                            {
                                SolarBrush = Brushes.Red;
                            }
                            else
                            {
                                SolarBrush = Brushes.Black;
                            }
                        }
                        else
                        {
                            LunarBrush = DefaultLunarBrush;
                            if (IsWeekend)
                            {
                                SolarBrush = Brushes.Red;
                            }
                            else
                            {
                                SolarBrush = Brushes.Black;
                            }
                        }
                    }
                    else
                    {
                        SolarBrush = _notThisMonthBrush;
                        LunarBrush = _notThisMonthBrush;
                    }
                }
                OnPropertyChanged("IsFestival");
            }
        }

        private int _thisMonth;
        /// <summary>
        /// 当前月
        /// </summary>
        public int ThisMonth
        {
            get { return _thisMonth; }
            set
            {
                _thisMonth = value;
                OnPropertyChanged("ThisMonth");
            }
        }

        private bool _isHoliday = false;
        /// <summary>
        /// 是不是放假
        /// </summary>
        public bool IsHoliday
        {
            get { return _isHoliday; }
            set
            {
                _isHoliday = value;
                if (value)
                {
                    if (IsThisMonth)
                    {
                        HolidayOpacity = 1;
                    }
                    else
                    {
                        HolidayOpacity = 0.5;
                    }
                }
                else
                {
                    HolidayOpacity = 0;
                }
                OnPropertyChanged("IsHoliday");
            }
        }


        private Double _holidayOpacity = 0;
        public Double HolidayOpacity
        {
            get { return _holidayOpacity; }
            set
            {
                _holidayOpacity = value;
                OnPropertyChanged("HolidayOpacity");
            }
        }

        private bool _isWorkday = false;

        public bool IsWorkday
        {
            get { return _isWorkday; }
            set
            {
                _isWorkday = value;
                if (value)
                {
                    if (IsThisMonth)
                    {
                        WorkdayOpacity = 1;
                    }
                    else
                    {
                        WorkdayOpacity = 0.5;
                    }
                }
                else
                {
                    WorkdayOpacity = 0;
                }
                OnPropertyChanged("IsWorkday");
            }
        }
        private Double _workdayOpacity = 0;
        public Double WorkdayOpacity
        {
            get { return _workdayOpacity; }
            set
            {
                _workdayOpacity = value;
                OnPropertyChanged("WorkdayOpacity");
            }
        }
        #endregion
        public CalendarDayButtonEx()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focusable = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
        }
    }
}
