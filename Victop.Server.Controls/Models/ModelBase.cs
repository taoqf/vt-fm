using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Victop.Server.Controls.Models
{
    /// <summary>
    /// Model基类
    /// </summary>
    public class ModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// 事件定义
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性改变通知
        /// </summary>
        /// <param name="propertyName"></param>
        [Obsolete("建议使用此方法的另外一个重载")]
        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        /// <summary>
        /// 属性值变化时发生
        /// </summary>
        /// <param name="propertyExpression">属性表达式</param>
        public void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = (propertyExpression.Body as MemberExpression).Member.Name;
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
