using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Victop.Wpf.Controls
{
    public class VicDataGridCheckBoxColumn : DataGridBoundColumn
    {
        #region 字段
        private static Style _defaultEditingElementStyle;
        private static Style _defaultElementStyle;
        private bool isConverterEnable;

        #endregion

        #region 构造函数
        static VicDataGridCheckBoxColumn()
        {
            //设置未编辑样式
            ElementStyleProperty.OverrideMetadata(typeof(VicDataGridCheckBoxColumn), new FrameworkPropertyMetadata(DefaultElementStyle));
            //设置编辑样式
            EditingElementStyleProperty.OverrideMetadata(typeof(VicDataGridCheckBoxColumn), new FrameworkPropertyMetadata(DefaultEditingElementStyle));
        }
        #endregion

        #region 属性
        /// <summary>获取在呈现列为处于编辑模式的单元格显示的元素时使用的样式。</summary>
        public static Style DefaultEditingElementStyle
        {
            get
            {
                if (_defaultEditingElementStyle == null)
                {
                    Style style = new Style(typeof(VicCheckBoxNormal));
                    style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center));
                    style.Seal();
                    _defaultEditingElementStyle = style;
                }
                return _defaultEditingElementStyle;
            }
        }
        /// <summary>获取在呈现列为未处于编辑模式的单元格显示的元素时使用的样式。</summary>
        public static Style DefaultElementStyle
        {
            get
            {
                if (_defaultElementStyle == null)
                {
                    Style style = new Style(typeof(VicCheckBoxNormal));
                    style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center));
                    style.Seal();
                    _defaultElementStyle = style;
                }
                return _defaultElementStyle;
            }
        }
        /// <summary>转换器是否可用</summary>
        public bool IsConverterEnable
        {
            get { return isConverterEnable; }
            set { isConverterEnable = value; }
        }
        #endregion

        #region 方法
        /// <summary>
        ///  获取一个绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的编辑元素。
        /// </summary>
        /// <param name="cell"> 将包含生成的元素的单元格。</param>
        /// <param name="dataItem">由包含目标单元格的行表示的数据项。</param>
        /// <returns>绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的新编辑元素。</returns>
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return GenerateVicCheckBoxNormal(true, cell);
        }

        /// <summary>
        /// 获取一个绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的只读元素。
        /// </summary>
        /// <param name="cell"> 将包含生成的元素的单元格。</param>
        /// <param name="dataItem">由包含目标单元格的行表示的数据项。</param>
        /// <returns> 绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的新只读元素。</returns>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            VicCheckBoxNormal generateCheckBoxNormal = GenerateVicCheckBoxNormal(false, cell);
            return generateCheckBoxNormal;
        }

        /// <summary>
        ///  获取一个绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的元素。
        /// </summary>
        /// <param name="isEditing">是否是编辑元素</param>
        /// <param name="cell">将包含生成的元素的单元格。</param>
        /// <returns>绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的新元素。</returns>
        private VicCheckBoxNormal GenerateVicCheckBoxNormal(bool isEditing, DataGridCell cell)
        {
            VicCheckBoxNormal checkBoxNormal = (cell != null) ? (cell.Content as VicCheckBoxNormal) : null;
            if (checkBoxNormal == null)
            {
                checkBoxNormal = new VicCheckBoxNormal();
                checkBoxNormal.Width = 20;
                checkBoxNormal.HorizontalAlignment = HorizontalAlignment.Center;
            }

            ApplyStyle(isEditing, true, checkBoxNormal);
            ApplyBinding(checkBoxNormal, VicCheckBoxNormal.IsCheckedProperty);

            return checkBoxNormal;
        }
        /// <summary>申请样式</summary>
        internal void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
        {
            Style style = PickStyle(isEditing, defaultToElementStyle);
            if (style != null)
            {
                element.Style = style;
            }
        }
        /// <summary>挑选样式</summary>
        private Style PickStyle(bool isEditing, bool defaultToElementStyle)
        {
            Style style = isEditing ? EditingElementStyle : ElementStyle;
            if (isEditing && defaultToElementStyle && (style == null))
            {
                style = ElementStyle;
            }
            return style;
        }

        /// <summary>申请绑定</summary>
        internal void ApplyBinding(DependencyObject target, DependencyProperty property)
        {
            Binding binding = (Binding)Binding;
            if (binding != null)
            {
                if (binding.Converter == null && IsConverterEnable) binding.Converter = new CheckboxBoolConverter();
                BindingOperations.SetBinding(target, property, binding);
            }
            else
            {
                BindingOperations.ClearBinding(target, property);
            }
        }
        #endregion
    }
}
