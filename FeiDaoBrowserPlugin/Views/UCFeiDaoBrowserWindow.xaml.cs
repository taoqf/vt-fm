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

namespace FeiDaoBrowserPlugin.Views
{
    /// <summary>
    /// UCFeiDaoBrowserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UCFeiDaoBrowserWindow : VicMetroWindow
    {
        public UCFeiDaoBrowserWindow(Dictionary<string, object> paramDict, int showType)
        {
            InitializeComponent();
            UCFeiDaoBrowser mainView = new UCFeiDaoBrowser(paramDict, showType);
            mainView.Uid = Uid;
            this.Content = mainView;
        }
    }
}
