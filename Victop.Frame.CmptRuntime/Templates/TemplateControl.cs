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

        protected override void OnInitialized(EventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                VicGridNormal gridGrid1 = this.FindName("grid1") as VicGridNormal;
                PresentationBlockModel blockModel = new PresentationBlockModel();
                blockModel.BlockName = "grid1";

                VicGridNormal gridGrid1_1 = this.FindName("grid1_1") as VicGridNormal;
                PresentationBlockModel blockModel1 = new PresentationBlockModel();
                blockModel1.BlockName = "grid1_1";
                gridGrid1_1.DataContext = blockModel1;
                gridGrid1.DataContext = blockModel;
            }
            base.OnInitialized(e);
        }
    }
}
