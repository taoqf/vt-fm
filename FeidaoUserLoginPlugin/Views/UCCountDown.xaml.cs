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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Victop.Frame.CmptRuntime;

namespace FeidaoUserLoginPlugin.Views
{
    /// <summary>
    /// UCCountDown.xaml 的交互逻辑
    /// </summary>
    public partial class UCCountDown : TemplateControl
    {
        #region 字段
        Storyboard stdStart;
        Storyboard stdStart2;
    
        private string sencond="0";
        private string sencondex="0";
        public string SencondEx
        {
            get
            {
                return sencondex;
            }
            set
            {
                if (sencondex != value)
                {
                    //TODO:启动动画
                    stdStart2 = (Storyboard)this.Resources["Storyboard2"];
                    stdStart2.Begin();
                    sencondex = value;
                    RaisePropertyChanged(() => SencondEx);
                }
            }
        }
        public string Sencond
        {
            get
            {
                return sencond;
            }
            set
            {
                if (sencond != value)
                {
                    stdStart = (Storyboard)this.Resources["Storyboard1"];
                    stdStart.Begin();  
                    sencond = value;
                    RaisePropertyChanged(() => Sencond);
                }
            }
        }
        private DispatcherTimer dTimer = new DispatcherTimer();
        private DateTime endDate = new DateTime(2025, 1, 1, 0, 0, 0);
        #endregion
        public UCCountDown()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Loaded += UCCountDown_Loaded;
        }

        void UCCountDown_Loaded(object sender, RoutedEventArgs e)
        {
            dTimer.Tick += new EventHandler(dTimer_Tick);
            dTimer.Interval = new TimeSpan(0, 0, 1);
            dTimer.Start();
           
        }

        private void dTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = endDate - DateTime.Now;
            Sencond = ts.Seconds >= 10 ? ts.Seconds.ToString().Substring(1) : ts.Seconds.ToString();
            SencondEx = ts.Seconds >= 10 ? ts.Seconds.ToString().Substring(0, 1) : "0";
        }
    }
}
