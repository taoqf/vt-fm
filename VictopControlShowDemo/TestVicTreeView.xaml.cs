using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VictopControlShowDemo
{
    /// <summary>
    /// TestVicTreeView.xaml 的交互逻辑
    /// </summary>
    public partial class TestVicTreeView : Window
    {
        public TestVicTreeView()
        {
            InitializeComponent();
        }
        DataRowView drv;
        DataTable TreeDt = new DataTable();


        private void tree_Loaded(object sender, RoutedEventArgs e)
        {
            TreeDt.Columns.Add("id");
            TreeDt.Columns.Add("fid");
            TreeDt.Columns.Add("name");
            tree.IDField = "id";
            tree.FIDField = "fid";
            tree.DisplayField = "name";
            //for (int i = 0; i < 5; i++)
            //{
            //    DataRow dr = TreeDt.NewRow();
            //    dr[0] = i;
            //    //dr[1] = 0;
            //    dr[2] = "Root";
            //    TreeDt.Rows.Add(dr);
            //}
            //for (int j = 0; j < 5; j++)
            //{
            //    DataRow dr = TreeDt.NewRow();
            //    dr[0] = j+5;
            //    dr[1] = j;
            //    dr[2] = "Children"+j;
            //    TreeDt.Rows.Add(dr);
            //}
            tree.ItemsSource = TreeDt.DefaultView;
            DataRow dr = TreeDt.NewRow();
            dr["id"] = new Guid();
            dr["fid"] = 0;
            dr["name"] = "aaa";
            TreeDt.Rows.Add(dr);

        }

        private void btnAddBrother_Click(object sender, RoutedEventArgs e)
        {
            if (drv == null)
            {
                DataRow dr1 = TreeDt.NewRow();
                dr1["id"] = 2;
                //dr1["fid"] = 1;
                dr1["name"] = "Page";
                TreeDt.Rows.Add(dr1);
            }
            else
            {
                DataRow dr1 = TreeDt.NewRow();
                dr1["id"] = 21;
                dr1["fid"] = drv["fid"];
                dr1["name"] = "Page";
                TreeDt.Rows.Add(dr1);
            }
            tree.ItemsSource = TreeDt.DefaultView;
        }

        private void btnAddChildren_Click(object sender, RoutedEventArgs e)
        {
            if (drv == null) return;
            DataRow dr1 = TreeDt.NewRow();
            dr1["id"] = 31;
            dr1["fid"] = drv["id"];
            dr1["name"] = "Page";
            TreeDt.Rows.Add(dr1);
        }
        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            drv = (DataRowView)tree.SelectedItem;
        }
    }
}
