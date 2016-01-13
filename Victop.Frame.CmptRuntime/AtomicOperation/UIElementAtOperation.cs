using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Victop.Frame.CmptRuntime.AtomicOperation
{
    /// <summary>
    /// UI元素原子操作
    /// </summary>
    public class UIElementAtOperation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainView"></param>
        public UIElementAtOperation(TemplateControl mainView)
        {
            MainView = mainView;
        }
        #region 私有字段
       /// <summary>
       /// 页面/组件基类
       /// </summary>
        private TemplateControl MainView;
        #endregion
        /// <summary>
        /// 设置按钮文本
        /// </summary>
        /// <param name="btnName">按钮名称</param>
        /// <param name="btnContent">按钮内容</param>
        public void SetButtonText(string btnName, string btnContent)
        {
            Button btn = MainView.FindName(btnName) as Button;
            if (btn != null)
                btn.Content = btnContent;
        }
    }
}
