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
using System.Windows.Shapes;
using Victop.Wpf.Controls;

namespace SystemTestingPlugin.Views
{
    /// <summary>
    /// AreaWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AreaWindow : Window
    {
        public AreaWindow(Dictionary<string, object> paramDict)
        {
            InitializeComponent();
            UCAreaWindowData ucData = new UCAreaWindowData(paramDict);
            Content = ucData;
        }
    }
}
