﻿using System;
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
using Victop.Wpf.Controls;

namespace MachinePlatformPlugin.Views
{
    /// <summary>
    /// SelectPersonnel.xaml 的交互逻辑
    /// </summary>
    public partial class SelectPersonnel : VicWindowNormal
    {
        public SelectPersonnel()
        {
            InitializeComponent();
        }
        public SelectPersonnel(object viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
