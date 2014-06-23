using System;
using System.Collections.Generic;
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
    /// TestVicGridForm.xaml 的交互逻辑
    /// </summary>
    public partial class TestVicGridForm : Window
    {
        public TestVicGridForm()
        {
            InitializeComponent();
            AddBorder();
        }
        /// <summary> 添加Border </summary>
         public void AddBorder()
         {
             int rows = MainGrid.RowDefinitions.Count;
             int columns = MainGrid.ColumnDefinitions.Count;
             for (int i = 0; i < rows; i++)
             {
                 if (i != rows - 1)
                 {
                     #region
 
                     for (int j = 0; j < columns; j++)
                     {
                         Border border = null;
                         if (j == columns - 1)
                         {
                             border = new Border()
                             {
                                 BorderBrush = new SolidColorBrush(Colors.Green),
                                 BorderThickness = new Thickness(2.5, 2.5, 2.5, 0)
                             };
                         }
                         else
                         {
                             border = new Border()
                             {
                                 BorderBrush = new SolidColorBrush(Colors.Green),
                                 BorderThickness = new Thickness(2.5, 2.5, 0, 0)                                
                             };
                         }
                         Grid.SetRow(border, i);
                         Grid.SetColumn(border, j);
                         MainGrid.Children.Add(border);
                     }
                     #endregion
                 }
                 else
                 {
                     for (int j = 0; j < columns; j++)
                     {
                         Border border = null;
                         if (j + 1 != columns)
                         {
                             border = new Border
                             {
                                 BorderBrush = new SolidColorBrush(Colors.Green),
                                 BorderThickness = new Thickness(2.5, 2.5, 0, 2.5)
                             };
                         }
                         else
                         {
                             border = new Border
                             {
                                 BorderBrush = new SolidColorBrush(Colors.Green),
                                 BorderThickness = new Thickness(2.5, 2.5, 2.5, 2.5)
                             };
                         }
                         Grid.SetRow(border, i);
                         Grid.SetColumn(border, j);
                         MainGrid.Children.Add(border);
                     }
                 }
             }
         }
    }
}
