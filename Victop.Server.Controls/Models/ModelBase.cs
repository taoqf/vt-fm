using System.ComponentModel;

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
        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
