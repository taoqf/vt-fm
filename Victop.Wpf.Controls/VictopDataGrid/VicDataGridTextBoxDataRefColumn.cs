using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Victop.Wpf.Controls
{
    public class VicDataGridTextBoxDataRefColumn : DataGridBoundColumn
    {
        #region 字段
        private static Style _defaultEditingElementStyle;
        private static Style _defaultElementStyle;
        #endregion

        #region 构造函数
        static VicDataGridTextBoxDataRefColumn()
        {
            //设置未编辑样式
            ElementStyleProperty.OverrideMetadata(typeof(VicDataGridTextBoxDataRefColumn), new FrameworkPropertyMetadata(DefaultElementStyle));
            //设置编辑样式
            EditingElementStyleProperty.OverrideMetadata(typeof(VicDataGridTextBoxDataRefColumn), new FrameworkPropertyMetadata(DefaultEditingElementStyle));
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
                    Style style = new Style(typeof(VicTextBox));
                    style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
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
                    Style style = new Style(typeof(VicTextBox));

                    style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
                    style.Setters.Add(new Setter(UIElement.IsHitTestVisibleProperty, false));
                    style.Setters.Add(new Setter(UIElement.FocusableProperty, false));
                    style.Setters.Add(new Setter(VicTextBox.HideDataRefButtonProperty, true));
                    style.Setters.Add(new Setter(VicTextBox.BackgroundProperty, new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
                    style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0d)));

                    style.Seal();
                    _defaultElementStyle = style;
                }

                return _defaultElementStyle;
            }
        }
        #endregion

        #region 事件
        public delegate void OnDataReferenceClick(object sender, RoutedEventArgs e);
        /// <summary>
        /// 按钮单击事件
        /// </summary>
        public event OnDataReferenceClick DataReferenceClick;

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
            return GenerateVicTextBox(true, cell);
        }

        /// <summary>
        /// 获取一个绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的只读元素。
        /// </summary>
        /// <param name="cell"> 将包含生成的元素的单元格。</param>
        /// <param name="dataItem">由包含目标单元格的行表示的数据项。</param>
        /// <returns> 绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的新只读元素。</returns>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            VicTextBox generateTextBox = GenerateVicTextBox(false, cell);
            generateTextBox.HideDataRefButton = true;
            return generateTextBox;
        }

        /// <summary>
        ///  获取一个绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的元素。
        /// </summary>
        /// <param name="isEditing">是否是编辑元素</param>
        /// <param name="cell">将包含生成的元素的单元格。</param>
        /// <returns>绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的新元素。</returns>
        private VicTextBox GenerateVicTextBox(bool isEditing, DataGridCell cell)
        {
            VicTextBox vicTextBox = (cell != null) ? (cell.Content as VicTextBox) : null;
            if (vicTextBox == null)
            {
                vicTextBox = new VicTextBox();
                vicTextBox.VicTextBoxClick += vicTextBox_VicTextBoxClick;
            }
            if (isEditing)
                vicTextBox.HideDataRefButton = false;
            else
                vicTextBox.HideDataRefButton = true;
            ApplyStyle(isEditing, true, vicTextBox);
            ApplyBinding(vicTextBox, VicTextBox.VicTextProperty);
            return vicTextBox;
        }

        void vicTextBox_VicTextBoxClick(object sender, RoutedEventArgs e)
        {
            DataReferenceClick(sender, e);
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
            BindingBase binding = Binding;
            if (binding != null)
            {
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
