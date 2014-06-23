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

namespace Victop.Wpf.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Victop.Wpf.Controls.VictopGrids"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Victop.Wpf.Controls.VictopGrids;assembly=Victop.Wpf.Controls.VictopGrids"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:VicGridForm/>
    ///
    /// </summary>
    public class VicGridForm : Grid
    {
        //public bool ShowBorder
        //{
        //    get { return (bool)GetValue(ShowBorderProperty); }
        //    set { SetValue(ShowBorderProperty, value); }
        //}

        //public static readonly DependencyProperty ShowBorderProperty =
        //DependencyProperty.RegisterAttached("ShowBorder", typeof(bool), typeof(VicGridForm), new PropertyMetadata(OnShowBorderChanged));
        
        static VicGridForm()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VicGridForm), new FrameworkPropertyMetadata(typeof(VicGridForm)));
        }

        public VicGridForm()
        {
            this.Loaded += VicGridForm_Loaded;
           
        }
        /// <summary>
        /// 加边框并隔行换色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VicGridForm_Loaded(object sender, RoutedEventArgs e)
        {
            List<Border> list = new List<Border>();
            for (int rowIndex = 0; rowIndex < this.RowDefinitions.Count; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.ColumnDefinitions.Count; columnIndex++)
                {
                    Border border = new Border();
                    if (rowIndex % 2 == 0)
                    {
                        border.Background = new SolidColorBrush(Colors.White);
                    }
                    else
                    {
                        border.Background = new SolidColorBrush(Color.FromRgb(250, 250, 250));
                    }
                    border.BorderBrush = new SolidColorBrush(Color.FromRgb(216, 216, 218));
                    border.BorderThickness = new Thickness(0.5);
                    Grid.SetRow(border, rowIndex);
                    Grid.SetColumn(border, columnIndex);
                    List<FrameworkElement> items = GetItems(rowIndex, columnIndex);
                    if (items != null&&items.Count>0)
                    {
                        RemoveItems(rowIndex, columnIndex);
                        int columnspan = Grid.GetColumnSpan(items[0]);
                        DockPanel dPanel = new DockPanel();
                        foreach (FrameworkElement item in items)
                        {
                            dPanel.Children.Add(item);
                        }
                        border.Child = dPanel;
                        Grid.SetColumnSpan(border, columnspan);
                        columnIndex = columnIndex + columnspan-1;
                    }
                    list.Add(border);
                }
            }
            this.Children.Clear();
            foreach (Border border in list)
            {
                this.Children.Add(border);
            }
            this.Loaded -= VicGridForm_Loaded;
        }
        /// <summary>
        /// 移除该行该列元素
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        private void RemoveItems(int rowIndex, int columnIndex)
        {
            for (int i = this.Children.Count-1; i >=0; i--)
            {
                FrameworkElement item = this.Children[i] as FrameworkElement;
                int row = Grid.GetRow(item);
                int column = Grid.GetColumn(item);
                if (row == rowIndex && column == columnIndex)
                {
                    this.Children.Remove(item);
                }
            }
        }
        /// <summary>
        /// 获取该行该列元素
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        private  List<FrameworkElement> GetItems(int rowIndex, int columnIndex)
        {
            List<FrameworkElement> items=new List<FrameworkElement>();
            for (int i = 0; i < this.Children.Count; i++)
            {
                FrameworkElement item = this.Children[i] as FrameworkElement;
                int row = Grid.GetRow(item);
                int column = Grid.GetColumn(item);
                if (row == rowIndex && column == columnIndex)
                {
                    items.Add(item);
                }
            }
            return items;
        }
        #region 暂无使用(使用属性设置边框和隔行换色)
        private static void OnShowBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            var grid = d as Grid;
            if ((bool)e.OldValue)
            {
                grid.Loaded -= (s, arg) => { };
            }
            if ((bool)e.NewValue)
            {
                grid.Loaded += (s, arg) =>
                {
                    //这种做法自动将控件移动到Border里面来
                    var controls = grid.Children;
                    var count = controls.Count;

                    for (int i = 0; i < count; i++)
                    {
                        var item = controls[i] as FrameworkElement;

                        int row = Grid.GetRow(item);
                        int column = Grid.GetColumn(item);
                        int rowspan = Grid.GetRowSpan(item);
                        int columnspan = Grid.GetColumnSpan(item);

                        Border border = new Border();
                        if (row % 2 == 0)
                        {
                            border.Background = new SolidColorBrush(Colors.White);
                        }
                        else
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(250, 250, 250));
                        }
                        border.BorderBrush = new SolidColorBrush(Color.FromRgb(216, 216, 218));
                        border.BorderThickness = new Thickness(1);
                        border.Padding = new Thickness(1);
                        Grid.SetRow(border, row);
                        Grid.SetColumn(border, column);
                        Grid.SetRowSpan(border, rowspan);
                        Grid.SetColumnSpan(border, columnspan);

                        grid.Children.RemoveAt(i);
                        border.Child = item;
                        grid.Children.Insert(i, border);
                    }
                };
                }
        }
        #endregion
    }
}
