﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Victop.Frame.Component
{
    /// <summary>
    /// UnitDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class UnitDataGrid : UserControl
    {
        public delegate void UnitDelegate(object sender, DataRow dr);

        public event UnitDelegate SelectedItemChanged;
        /// <summary>
        /// 模型Id
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                return (DataTable)GetValue(ModelIdProperty);
            }
            set
            {
                SetValue(ModelIdProperty, value);
                if (DataSource != null)
                {
                    vicgrid.ItemsSource = DataSource.DefaultView;
                }
            }
        }
        #region 依赖属性
        public static readonly DependencyProperty ModelIdProperty = DependencyProperty.Register("DataSource", typeof(DataTable), typeof(UnitDataGrid));
        #endregion
        public UnitDataGrid()
        {
            InitializeComponent();
            vicgrid.SelectionChanged += vicgrid_SelectionChanged;
        }

        void vicgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(sender, (vicgrid.SelectedItem as DataRowView).Row);
            }
        }
    }
}
