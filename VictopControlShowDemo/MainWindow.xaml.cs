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
using System.Xml;
using Victop.Wpf.Controls;
using VictopControlShowDemo.Views;
namespace VictopControlShowDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AddTabItem_Click(object sender, RoutedEventArgs e)
        {
            //Victop.Wpf.Controls.TabItem item = new Victop.Wpf.Controls.TabItem();
            //item.Header = "你好";
            //UCTestMvvm user = new UCTestMvvm();

            //ScrollViewer sc = new ScrollViewer();
            //sc.Content = user;
            //sc.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            //item.Content = sc;
            //tControl.Items.Add(item);
            //tControl.SelectedItem = item;
            string text = dPicker.SelectedDate.ToString();
            string text1 = dPicker1.SelectedDate.ToString();
        }

        private void testGridForm_Click(object sender, RoutedEventArgs e)
        {
            TestVicGridForm gridForm = new TestVicGridForm();
            gridForm.Show();
        }

        private void testTreeView_Click(object sender, RoutedEventArgs e)
        {
            TestVicTreeView treeView = new TestVicTreeView();
            treeView.Show();
        }

        private void testGridToolbar_Click(object sender, RoutedEventArgs e)
        {
            TestGridToolbar gridToolbar = new TestGridToolbar();
            gridToolbar.Show();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            LoadResource();
            Loadlanguage();
        }
        private void LoadResource()
        {
            ResourceDictionary resource = new ResourceDictionary();
            string path = "theme/SkinDefault/Resources.xaml";
            resource.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + path, UriKind.RelativeOrAbsolute);
            //将资源字典合并到当前资源中
            if (Application.Current.Resources.MergedDictionaries.Count > 0 && Application.Current.Resources.MergedDictionaries[0] != null)
            {
                Application.Current.Resources.MergedDictionaries[0] = resource;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(resource);
            }
        }
        private void Loadlanguage()
        {
            ResourceDictionary resource = new ResourceDictionary();
            string path = "language/zh-cn.language";
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.BaseDirectory + path);
            XmlNodeList nodeList = doc.GetElementsByTagName("resource");
            foreach (XmlNode node in nodeList)
            {
                if (resource.Contains(node.Attributes["id"].Value))
                    continue;
                resource.Add(node.Attributes["id"].Value, node.InnerText);
            }
            //将资源字典合并到当前资源中  
            if (Application.Current.Resources.MergedDictionaries.Count > 1 && Application.Current.Resources.MergedDictionaries[1] != null)
            {
                Application.Current.Resources.MergedDictionaries[1] = resource;
            }
            else
            {
                Application.Current.Resources.MergedDictionaries.Add(resource);
            }
        }

        private void testDataGrid_Click(object sender, RoutedEventArgs e)
        {
            TestDataGrid test = new TestDataGrid();
            test.Show();
        }

        private void tbox_VicTextBoxClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("aa");
        }

        private void ShowWindow_Click(object sender, RoutedEventArgs e)
        {
            PluginShellWindow window = new PluginShellWindow();
            UCTestMvvm control = new UCTestMvvm();
            if (control.MinHeight > 0)
            {
                window.Height = control.MinHeight + 90;
            }
            if (control.MinWidth > 0)
            {
                window.Width = control.MinWidth;
            }
            window.gridBody.Children.Add(control);
            window.Show();
        }
    }
}
