using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MetroFramePlugin.Models;

namespace MetroFramePlugin.Views
{
    /// <summary>
    /// CalendarEx.xaml 的交互逻辑
    /// </summary>
    public partial class CalendarEx : UserControl
    {
        List<DateTime> listWorkday;
        public CalendarEx()
        {
            InitializeComponent();

            list = new List<HolidayArrangement>();
            // List<DateTime> listHoliday = new List<DateTime>();
            listWorkday = new List<DateTime>();
            //  listHoliday.Add(new DateTime(DateTime.Now.Year, 1, 1));
            //dt[0] = DateTime.Now;
            listWorkday.Add(new DateTime(2014, 1, 26));
            listWorkday.Add(new DateTime(2014, 2, 8));
            listWorkday.Add(new DateTime(2014, 5, 4));
            listWorkday.Add(new DateTime(2014, 9, 28));
            listWorkday.Add(new DateTime(2014, 10, 1));
            list.Add(new HolidayArrangement() { Name = "元旦", Holiday = new DateTime(DateTime.Now.Year, 1, 1), NumberOfDays = 1, Workday = listWorkday });
            list.Add(new HolidayArrangement() { Name = "春节", Holiday = new DateTime(DateTime.Now.Year, 1, 31), NumberOfDays = 7, Workday = listWorkday });
            Dictionary<int, string> year = new Dictionary<int, string>();
            for (int i = 1901; i < 2049; i++)
            {
                year.Add(i, i + "年");
            }
            cbb_year.SelectedValuePath = "Key";
            cbb_year.DisplayMemberPath = "Value";
            cbb_year.ItemsSource = year;



            Dictionary<int, string> month = new Dictionary<int, string>();
            for (int i = 1; i <= 12; i++)
            {
                month.Add(i, i + "月");
            }
            cbb_month.SelectedValuePath = "Key";
            cbb_month.DisplayMemberPath = "Value";
            cbb_month.ItemsSource = month;


            DateTime now = DateTime.Now;
            cbb_year.SelectedValue = now.Year;
            cbb_month.SelectedValue = now.Month;

            cbb_month.SelectionChanged += Combobox_SelectionChanged;
            cbb_year.SelectionChanged += Combobox_SelectionChanged;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    calendarDayButtons[i, j] = new CalendarDayButtonEx();
                }
            }
            InitGrid(grid);


        }
        List<HolidayArrangement> list;
        CalendarDayButtonEx[,] calendarDayButtons = new CalendarDayButtonEx[6, 7];
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //if (calendarDayButtons[0, 0] == null)
            //{
            //    Dictionary<int, string> year = new Dictionary<int, string>();
            //    for (int i = 1901; i < 2049; i++)
            //    {
            //        year.Add(i, i + "年");
            //    }
            //    cbb_year.ItemsSource = year;
            //    cbb_year.SelectedValuePath = "Key";
            //    cbb_year.DisplayMemberPath = "Value";


            //    //int year = ((Dictionary<int, string>)cbb_year.SelectedValue).Key;
            //    Dictionary<int, string> month = new Dictionary<int, string>();
            //    for (int i = 1; i <= 12; i++)
            //    {
            //        month.Add(i, i + "月");
            //    }
            //    cbb_month.ItemsSource = month;
            //    cbb_month.SelectedValuePath = "Key";
            //    cbb_month.DisplayMemberPath = "Value";

            //    DateTime now = DateTime.Now;
            //    cbb_year.SelectedValue = now.Year;
            //    cbb_month.SelectedValue = now.Month;
            //    for (int i = 0; i < 6; i++)
            //    {
            //        for (int j = 0; j < 7; j++)
            //        {
            //            calendarDayButtons[i, j] = new CalendarDayButtonEx();
            //        }
            //    }
            //    InitGrid(grid);
            //}
        }
        /// <summary>
        /// 阴历显示什么
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string LunarShow(DateTime dt, out bool isHoliday)
        {
            ChineseCalendar chineseCalendar = new ChineseCalendar(dt);
            isHoliday = true;
            string res = "";
            if (chineseCalendar.ChineseTwentyFourDay != "")
            {
                res = chineseCalendar.ChineseTwentyFourDay;
            }
            else if (chineseCalendar.ChineseCalendarHoliday != "")
            {
                res = chineseCalendar.ChineseCalendarHoliday;
            }
            else if (chineseCalendar.DateHoliday != "")
            {
                res = chineseCalendar.DateHoliday;
            }
            else
            {
                isHoliday = false;
                res = chineseCalendar.ChineseShortDateString;
            }
            return res;
        }
        private string LunarShow(DateTime dt)
        {
            ChineseCalendar chineseCalendar = new ChineseCalendar(dt);
            string res = "";
            if (chineseCalendar.ChineseTwentyFourDay != "")
            {
                res = chineseCalendar.ChineseTwentyFourDay;
            }
            else if (chineseCalendar.ChineseCalendarHoliday != "")
            {
                res = chineseCalendar.ChineseCalendarHoliday;
            }
            else if (chineseCalendar.DateHoliday != "")
            {
                res = chineseCalendar.DateHoliday;
            }
            else
            {
                res = chineseCalendar.ChineseShortDateString;
            }
            return res;
        }
        /// <summary>
        /// 获取本月节日
        /// </summary>
        private bool GetThisMonthHoliday(int month)
        {
            int day = DateTime.DaysInMonth(DateTime.Now.Year, month);
            for (int i = 1; i <= day; i++)
            {
                foreach (var item in list)
                {
                    if (LunarShow(new DateTime(DateTime.Now.Year, month, i)) == item.Name)
                    {

                    }
                }
            }
            return false;
        }
        public void ShowCalendar(DateTime dt)
        {
            ThisMonth = dt.Month;
            int year = dt.Year;
            int month = dt.Month;
            //string txt = "阳历：" + cc.DateString
            //    + " \n属相：" + cc.AnimalString
            //    + " \n农历：" + cc.ChineseDateString
            //    + " \n时辰：" + cc.ChineseHour
            //    + " \n节气：" + cc.ChineseTwentyFourDay
            //    + " \n节日：" + cc.ChineseCalendarHoliday + " " + cc.DateHoliday
            //    + " \n前一个节气：" + cc.ChineseTwentyFourPrevDay
            //    + " \n后一个节气：" + cc.ChineseTwentyFourNextDay
            //    + " \n干支：" + cc.GanZhiDateString
            //    + " \n星期：" + cc.WeekDayStr
            //    + " \n星宿：" + cc.ChineseConstellation
            //    + " \n星座：" + cc.Constellation;
            DateTime now = DateTime.Now;

            int day = DateTime.DaysInMonth(year, month);
            // 获取这个月1号是星期几0->6 日->六
            int begin = (int)(new DateTime(year, month, 1).DayOfWeek);

            int dayNum = 1;
            string strDate = "";
            int previouDays = -1;
            int previouMonth = month - 1;
            int nextMonth = month + 1;
            if (previouMonth < 1)
            {
                previouDays = DateTime.DaysInMonth(year - 1, 12);
            }
            else
            {
                previouDays = DateTime.DaysInMonth(year, previouMonth);
            }
            bool isHoliday = false;
            DateTime dateTime;
            int count = 1;
            int days = 0;
            bool flag = false;
            previouDays = previouDays - begin + 1;
            for (int column = 0; column < begin; column++, previouDays++)
            {
                if (previouMonth < 1)
                {
                    dateTime = new DateTime(year - 1, 12, previouDays);
                }
                else
                {
                    dateTime = new DateTime(year, previouMonth, previouDays);
                }
                strDate = LunarShow(dateTime, out isHoliday);
                calendarDayButtons[0, column].IsToday = false;
                //  calendarDayButtons[0, column].IsVacation = false;
                calendarDayButtons[0, column].SolarDateTime = dateTime;
                calendarDayButtons[0, column].Lunar = strDate;
                calendarDayButtons[0, column].IsThisMonth = false;
                calendarDayButtons[0, column].IsWorkday = false;
                calendarDayButtons[0, column].IsHoliday = false;
                foreach (var item in listWorkday)
                {
                    if (DateTime.Compare(item,dateTime) == 0)
                    {
                        calendarDayButtons[0, column].IsWorkday = true;
                    }
                }
                if (flag)
                {
                    if (count <= days)
                    {
                        calendarDayButtons[0, column].IsHoliday = true;
                    }
                    else
                    {
                        flag = false;
                        count = 1;
                    }
                }
                else
                {
                    foreach (var item in list)
                    {
                        if (strDate == item.Name)
                        {
                            calendarDayButtons[0, column].IsHoliday = true;
                            flag = true;
                            days = item.NumberOfDays;
                            count++;
                        }
                    }
                }
            }
            int y = 1;
            
            for (int row = 0; row < 6; row++)
            {
                if (row != 0)
                {
                    begin = 0;
                }
                for (int column = begin; column < 7; column++, dayNum++)
                {
                    if (dayNum <= day)
                    {
                        dateTime = new DateTime(year, month, dayNum);
                        strDate = LunarShow(dateTime, out isHoliday);
                        if (dateTime.CompareTo(DateTime.Today) == 0)
                        {
                            calendarDayButtons[row, column].IsToday = true;
                        }
                        else
                        {
                            calendarDayButtons[row, column].IsToday = false;
                        }
                        calendarDayButtons[row, column].SolarDateTime = dateTime;
                        calendarDayButtons[row, column].Lunar = strDate;
                        calendarDayButtons[row, column].IsThisMonth = true;
                        calendarDayButtons[row, column].IsFestival = isHoliday;
                        calendarDayButtons[row, column].IsWorkday = false;
                        calendarDayButtons[row, column].IsHoliday = false;
                        foreach (var item in listWorkday)
                        {
                            if (DateTime.Compare(item, dateTime) == 0)
                            {
                                calendarDayButtons[row, column].IsWorkday = true;
                            }
                        }
                        if (flag)
                        {
                            if (count <= days)
                            {
                                calendarDayButtons[row, column].IsHoliday = true;
                                count++;
                            }
                            else
                            {
                                flag = false;
                                count = 1;
                            }
                        }
                        else
                        {
                            foreach (var item in list)
                            {
                                if (strDate == item.Name)
                                {
                                    calendarDayButtons[row, column].IsHoliday = true;
                                    flag = true;
                                    days = item.NumberOfDays;
                                    count++;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (nextMonth > 12)
                        {
                            dateTime = new DateTime(year + 1, 1, y);
                        }
                        else
                        {
                            dateTime = new DateTime(year, nextMonth, y);
                        }

                        strDate = LunarShow(dateTime, out isHoliday);
                        calendarDayButtons[row, column].IsToday = false;
                        calendarDayButtons[row, column].IsFestival = isHoliday;

                        calendarDayButtons[row, column].SolarDateTime = dateTime;
                        calendarDayButtons[row, column].Lunar = strDate;
                        calendarDayButtons[row, column].IsThisMonth = false;
                        calendarDayButtons[row, column].IsWorkday = false;
                        calendarDayButtons[row, column].IsHoliday = false;
                        foreach (var item in listWorkday)
                        {
                            if (DateTime.Compare(item, dateTime) == 0)
                            {
                                calendarDayButtons[row, column].IsWorkday = true;
                            }
                        }
                        if (flag)
                        {
                            if (count <= days)
                            {
                                calendarDayButtons[row, column].IsHoliday = true;
                                count++;
                            }
                            else
                            {
                                flag = false;
                                count = 1;
                            }
                        }
                        else
                        {
                            foreach (var item in list)
                            {
                                if (strDate == item.Name)
                                {
                                    calendarDayButtons[row, column].IsHoliday = true;
                                    flag = true;
                                    days = item.NumberOfDays;
                                    count++;
                                }
                            }
                        }
                        y++;
                    }
                }
            }
        }

        private void InitHolidayArrangement(DateTime dt)
        {

        }
        private void InitGrid(Grid g)
        {
            for (int row = 0; row < 6; row++)
            {
                for (int column = 0; column < 7; column++)
                {
                    Grid.SetColumn(calendarDayButtons[row, column], column);
                    Grid.SetRow(calendarDayButtons[row, column], row + 1);
                    grid.Children.Add(calendarDayButtons[row, column]);
                    if (column == 0 || column == 6)
                    {
                        calendarDayButtons[row, column].IsWeekend = true;
                    }
                    calendarDayButtons[row, column].MouseDown += CalendarEx_MouseDown;
                }
            }

            ShowCalendar(DateTime.Now);

        }
        /// <summary>
        /// 当前月
        /// </summary>
        int ThisMonth = 0;
        void CalendarEx_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CalendarDayButtonEx calendarDayButtonEx = sender as CalendarDayButtonEx;
            // MessageBox.Show(calendarDayButtonEx.IsThisMonth.ToString() + ThisMonth.ToString());

            if (!calendarDayButtonEx.IsThisMonth)
            {
                int month = (int)cbb_month.SelectedValue;
                if (month == 1 && calendarDayButtonEx.SolarDateTime.Month == 12)
                {
                    cbb_month.SelectedValue = 12;
                    cbb_year.SelectedValue = calendarDayButtonEx.SolarDateTime.Year;
                }
                else if (month == 12 && calendarDayButtonEx.SolarDateTime.Month == 1)
                {
                    cbb_month.SelectedValue = 1;
                    cbb_year.SelectedValue = calendarDayButtonEx.SolarDateTime.Year;
                }
                else if (month > calendarDayButtonEx.SolarDateTime.Month)
                {
                    cbb_month.SelectedValue = (int)cbb_month.SelectedValue - 1;
                }
                else if (month < calendarDayButtonEx.SolarDateTime.Month)
                {
                    cbb_month.SelectedValue = (int)cbb_month.SelectedValue + 1;

                }
                //   Combobox_SelectionChanged(null, null);
            }
        }


        private void Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbb = sender as ComboBox;
            int month = (int)cbb_month.SelectedValue;
            int year = (int)cbb_year.SelectedValue;
            if (cbb.Name == "cbb_month")
            {
                month = ((dynamic)e.AddedItems[0]).Key;
            }
            else
            {
                year = ((dynamic)e.AddedItems[0]).Key;
            }
            ShowCalendar(new DateTime(year, month, 1));
        }

        private void PART_PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if ((int)cbb_month.SelectedValue != 1)
            {
                int month = (int)cbb_month.SelectedValue - 1;
                cbb_month.SelectedValue = month;// (int)cbb_month.SelectedValue - 1;
                // Combobox_SelectionChanged(null, null);
            }
        }

        private void PART_NextButton_Click(object sender, RoutedEventArgs e)
        {
            if ((int)cbb_month.SelectedValue != 12)
            {
                cbb_month.SelectedValue = (int)cbb_month.SelectedValue + 1;
                // Combobox_SelectionChanged(null, null);
            }
        }

        private void btn_today_Click(object sender, RoutedEventArgs e)
        {
            cbb_year.SelectedValue = DateTime.Now.Year;
            cbb_month.SelectedValue = DateTime.Now.Month;
            // Combobox_SelectionChanged(null, null);
        }

        private void cbb_year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbb_year.SelectedValue == null)
            {
                return;
            }
            int year = (int)cbb_year.SelectedValue;
            int month = (int)cbb_month.SelectedValue;
            ShowCalendar(new DateTime(year, month, 1));
        }
    }
}
