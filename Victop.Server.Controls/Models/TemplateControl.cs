using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Victop.Server.Controls.Models
{
    public class TemplateControl : UserControl, INotifyPropertyChanged
    {
        #region 公用属性
        /// <summary>
        /// 系统Id
        /// </summary>
        public string SystemId
        {
            get { return (string)GetValue(SystemIdProperty); }
            set { SetValue(SystemIdProperty, value); }
        }
        /// <summary>
        /// 应用Id
        /// </summary>
        public string FormId
        {
            get { return (string)GetValue(FormIdProperty); }
            set { SetValue(FormIdProperty, value); }
        }
        /// <summary>
        /// 模型Id
        /// </summary>
        public string ModelId
        {
            get { return (string)GetValue(ModelIdProperty); }
            set { SetValue(ModelIdProperty, value); }
        }
        /// <summary>
        /// 主档名称
        /// </summary>
        public string MasterName
        {
            get { return (string)GetValue(MasterNameProperty); }
            set { SetValue(MasterNameProperty, value); }
        }
        /// <summary>
        /// 装配数据路径
        /// </summary>
        public string FitDataPath
        {
            get { return (string)GetValue(FitDataPathProperty); }
            set { SetValue(FitDataPathProperty, value); }
        }
        #endregion

        #region 依赖属性
        public static readonly DependencyProperty SystemIdProperty = DependencyProperty.Register("SystemId", typeof(string), typeof(TemplateControl));
        public static readonly DependencyProperty FormIdProperty = DependencyProperty.Register("FormId", typeof(string), typeof(TemplateControl));
        public static readonly DependencyProperty ModelIdProperty = DependencyProperty.Register("ModelId", typeof(string), typeof(TemplateControl));
        public static readonly DependencyProperty MasterNameProperty = DependencyProperty.Register("MasterName", typeof(string), typeof(TemplateControl));
        public static readonly DependencyProperty FitDataPathProperty = DependencyProperty.Register("FitDataPath", typeof(string), typeof(TemplateControl));
        #endregion

        #region 属性通知事件
        /// <summary>
        /// 属性通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region 属性改变通知
        /// <summary>
        /// 属性改变通知
        /// </summary>
        /// <param name="propetyName"></param>
        public void RaisePropertyChanged(string propetyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propetyName));
            }
        }
        #endregion
    }
}
