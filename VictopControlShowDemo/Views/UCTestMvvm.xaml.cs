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
using Victop.Wpf.Controls;

namespace VictopControlShowDemo.Views
{
    /// <summary>
    /// UCTestMvvm.xaml 的交互逻辑
    /// </summary>
    public partial class UCTestMvvm : UserControl
    {
        public UCTestMvvm()
        {
            InitializeComponent();
        }

        DataTable dt = new DataTable();
        private void gridbody_Loaded(object sender, RoutedEventArgs e)
        {
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            dt.Columns.Add("sex");
            dt.Columns.Add("isnew");
            dt.Columns.Add("date");
            dt.Columns.Add("teacher");
            dt.Columns.Add("name1");
            dt.Columns.Add("sex1");
            dt.Columns.Add("isnew1");
            dt.Columns.Add("teacher1");
            for (int i = 0; i < 5; i++)
            {
                DataRow dr = dt.NewRow();
                dr["id"] = i;
                if (i % 2 == 0)
                {
                    dr["isnew"] = 1;
                    dr["isnew1"] = 0;
                }
                else
                {
                    dr["isnew"] = 0;
                    dr["isnew1"] = 1;
                }
                dr["name"] = "刘" + i;
                dr["sex"] = "男";
                dr["teacher"] = "张" + i;
                dr["date"] = DateTime.Now;
                dr["name1"] = "刘" + i;
                dr["sex1"] = "女";
                dr["teacher1"] = "张" + i;
                dt.Rows.Add(dr);
            }
            dgrid.ItemsSource = dt.DefaultView;

            VicDataGridNumericUpDownColumn column1 = new VicDataGridNumericUpDownColumn();
            column1.Binding = new Binding("id");
            column1.Header = "ID";

            VicDataGridTextColumn column2 = new VicDataGridTextColumn();
            column2.Binding = new Binding("name");
            column2.Header = "姓名";

            VicDataGridComboBoxColumn column3 = new VicDataGridComboBoxColumn();
            column3.ItemsSource = new List<string> { "男", "女" };
            column3.SelectedItemBinding = new Binding("sex");
            column3.Header = "性别";

            VicDataGridCheckBoxColumn column4 = new VicDataGridCheckBoxColumn();
            column4.IsConverterEnable = true;
            column4.Binding = new Binding("isnew");
            column4.Header = "是否新生";

            VicDataGridDatePickerColumn column5 = new VicDataGridDatePickerColumn();
            column5.Binding = new Binding("date");
            column5.Header = "日期";

            VicDataGridTextBoxDataRefColumn column6 = new VicDataGridTextBoxDataRefColumn();
            column6.Binding = new Binding("teacher");
            column6.DataReferenceClick += column6_DataReferenceClick;
            column6.Header = "老师";



            DataGridTextColumn column7 = new DataGridTextColumn();
            column7.Binding = new Binding("name1");
            column7.Header = "姓名1";

            DataGridComboBoxColumn column8 = new DataGridComboBoxColumn();
            column8.ItemsSource = new List<string> { "男", "女" };
            column8.SelectedItemBinding = new Binding("sex1");
            column8.Header = "性别1";

            DataGridCheckBoxColumn column9 = new DataGridCheckBoxColumn();
            column9.Binding = new Binding("isnew1") { Converter = new CheckboxBoolConverter() };
            column9.Header = "是否新生1";


            DataGridTemplateColumn column10 = new DataGridTemplateColumn();
            column10.CellTemplate = this.FindResource("DeptNameTemplate") as DataTemplate;
            column10.CellEditingTemplate = this.FindResource("EditingDeptNameTemplate") as DataTemplate;
            column10.Header = "老师1";


            dgrid.Columns.Add(column1);
            dgrid.Columns.Add(column2);
            dgrid.Columns.Add(column3);
            dgrid.Columns.Add(column4);
            dgrid.Columns.Add(column5);
            dgrid.Columns.Add(column6);
            dgrid.Columns.Add(column7);
            dgrid.Columns.Add(column8);
            dgrid.Columns.Add(column9);
            dgrid.Columns.Add(column10);
            dgrid.ReadXmlFileLayOutInfo(this.GetType().Name);

            //DataGridCheckBoxColumn column2 = new DataGridCheckBoxColumn();
            //column2.Binding = new Binding("fid") { Converter =new CheckboxBoolConverter(),Mode= BindingMode.TwoWay};
            //column2.Header = "FID";


            //VicDataGridComboBoxColumn column3 = new VicDataGridComboBoxColumn();
            //DataTable dtable = new DataTable();
            //dtable.Columns.Add("name");
            //dtable.Columns.Add("idname");
            //for (int j = 0; j < 5; j++)
            //{
            //    DataRow drow = dtable.NewRow();
            //    drow["name"] = "bbb" + j;
            //    drow["idname"] = j;
            //    dtable.Rows.Add(drow);
            //}
            //column3.ItemsSource = dtable.DefaultView;
            //column3.DisplayMemberPath = "name";
            //column3.SelectedValuePath = "idname";
            ////column3.SelectedItemBinding = new Binding("idname");
            //column3.SelectedValueBinding = new Binding("idname");
            //column3.Header = "姓名";

            //DataGridTemplateColumn column3 = new DataGridTemplateColumn();

            //FrameworkElementFactory fef = new FrameworkElementFactory(typeof(ComboBox));
            //Binding bing = new Binding();
            //bing.Path = new PropertyPath("idname");
            //fef.SetValue(ComboBox.ItemsSourceProperty, new List<string> { "0", "1", "2", "3", "4" });
            //fef.SetBinding(ComboBox.SelectedItemProperty, bing);
            //DataTemplate dtemplate = new DataTemplate();
            //dtemplate.VisualTree = fef;
            //column3.CellTemplate = dtemplate;

        }

        void column6_DataReferenceClick(object sender, RoutedEventArgs e)
        {
            VicTextBox tbox = (VicTextBox)sender;
            MessageBox.Show(tbox.VicText.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int i = dt.Rows.Count;
        }

        private void vtbDeptName_VicTextBoxClick(object sender, RoutedEventArgs e)
        {
            VicTextBox tbox = (VicTextBox)sender;
            MessageBox.Show(tbox.VicText.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
        }
    }
}
