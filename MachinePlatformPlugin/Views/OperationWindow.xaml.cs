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
using Victop.Wpf.Controls;
using MachinePlatformPlugin.Models;

namespace MachinePlatformPlugin.Views
{
    /// <summary>
    /// IssueLog.xaml 的交互逻辑
    /// </summary>
    public partial class OperationWindow : VicWindowNormal
    {
        private static CabinetInfoModel cabinetInfoModel;

        public static CabinetInfoModel CabinetInfoModel
        {
            get { return OperationWindow.cabinetInfoModel; }
            set { OperationWindow.cabinetInfoModel = value; }
        }
        public OperationWindow(CabinetInfoModel infoModel)
        {
            InitializeComponent();
            CabinetInfoModel = infoModel;
        }
    }
}
