using System;
using System.Collections.Generic;
using System.Data;
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
using Victop.Frame.PublicLib.Helpers;

namespace SystemTestingPlugin.Views
{
    /// <summary>
    /// UCAreaWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UCAreaWindow : UserControl
    {
        DataTable dt = new DataTable("Class");
        public UCAreaWindow()
        {
            InitializeComponent();
            this.AddColumnsMySelf();
        }

        private void AddColumnsMySelf()
        {
            dt = new DataTable();
            dt.Columns.Add("编号");
            dt.Columns.Add("姓名");
            dt.Columns.Add("性别");
            dt.Columns.Add("是否新生");
            dt.Columns.Add("入学时间");
            dt.Columns.Add("教师姓名");
            dt.Columns.Add("监护人姓名");
            dt.Columns.Add("监护人性别");
            for (int i = 0; i < 50; i++)
            {
                DataRow dr = dt.NewRow();
                dr["编号"] = i;
                if (i % 2 == 0)
                {
                    dr["是否新生"] = 1;
                }
                else
                {
                    dr["是否新生"] = 0;
                }
                dr["姓名"] = "刘" + i;
                dr["性别"] = "男";
                dr["入学时间"] = DateTime.Now;
                dr["教师姓名"] = "张" + i;
                dr["监护人姓名"] = "刘" + i;
                dr["监护人性别"] = "女";
                dt.Rows.Add(dr);
            }
            testGrid1.ItemsSource = dt.DefaultView;
            testGrid2.ItemsSource = dt.DefaultView;
            testGrid3.ItemsSource = dt.DefaultView;
        }
    }
}
