using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Victop.Wpf.Controls;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 用户控件模板类
    /// </summary>
    public class TemplateControl : UserControl
    {
        /// <summary>
        /// 组件定义实体
        /// </summary>
        public CompntDefinModel DefinModel;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitialized(EventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                OrgnizeRuntime.InitCompnt(DefinModel);
                if (DefinModel.CompntViews.Count > 0)
                {
                    foreach (DefinViewsModel item in DefinModel.CompntViews)
                    {
                        item.DoRender();
                        OrgnizeRuntime.RebuildViewDataPath(DefinModel, item);
                    }
                }
            }
            base.OnInitialized(e);
        }
    }
}
