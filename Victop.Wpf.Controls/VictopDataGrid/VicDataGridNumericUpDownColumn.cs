using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Victop.Wpf.Controls
{
    public class VicDataGridNumericUpDownColumn : DataGridBoundColumn
    {
        #region 字段
        private static Style _defaultEditingElementStyle;
        private static Style _defaultElementStyle;
        private double _minimum = (double)VicNumericUpDown.MinimumProperty.DefaultMetadata.DefaultValue;
        private double _maximum = (double)VicNumericUpDown.MaximumProperty.DefaultMetadata.DefaultValue;
        private double _interval = (double)VicNumericUpDown.IntervalProperty.DefaultMetadata.DefaultValue;
        private string _stringFormat = (string)VicNumericUpDown.StringFormatProperty.DefaultMetadata.DefaultValue;
        private bool _hideUpDownButtons = (bool)VicNumericUpDown.HideUpDownButtonsProperty.DefaultMetadata.DefaultValue;
        #endregion

        #region 构造函数
        static VicDataGridNumericUpDownColumn()
        {
            //设置未编辑样式
            ElementStyleProperty.OverrideMetadata(typeof(VicDataGridNumericUpDownColumn), new FrameworkPropertyMetadata(DefaultElementStyle));
            //设置编辑样式
            EditingElementStyleProperty.OverrideMetadata(typeof(VicDataGridNumericUpDownColumn), new FrameworkPropertyMetadata(DefaultEditingElementStyle));
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
                    Style style = new Style(typeof(VicNumericUpDown));
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
                    Style style = new Style(typeof(VicNumericUpDown));

                    style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
                    style.Setters.Add(new Setter(UIElement.IsHitTestVisibleProperty, false));
                    style.Setters.Add(new Setter(UIElement.FocusableProperty, false));
                    style.Setters.Add(new Setter(VicNumericUpDown.HideUpDownButtonsProperty, true));
                    style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0d)));

                    style.Seal();
                    _defaultElementStyle = style;
                }

                return _defaultElementStyle;
            }
        }
        /// <summary>获取或设置最小值</summary>
        public double Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        /// <summary>获取或设置最大值</summary>
        public double Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        /// <summary>获取或设置间隔，该间隔指定数值增长的步长</summary>
        public double Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        /// <summary>获取或设置一个字符串，该字符串指定如果绑定值显示为字符串，应如何设置该绑定的格式</summary>
        public string StringFormat
        {
            get { return _stringFormat; }
            set { _stringFormat = value; }
        }

        /// <summary>获取或设置一个bool型值，该值指定是否隐藏上下按钮</summary>
        public bool HideUpDownButtons
        {
            get { return _hideUpDownButtons; }
            set { _hideUpDownButtons = value; }
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
            return GenerateVicNumericUpDown(true, cell);
        }

        /// <summary>
        /// 获取一个绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的只读元素。
        /// </summary>
        /// <param name="cell"> 将包含生成的元素的单元格。</param>
        /// <param name="dataItem">由包含目标单元格的行表示的数据项。</param>
        /// <returns> 绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的新只读元素。</returns>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            VicNumericUpDown generateNumericUpDown = GenerateVicNumericUpDown(false, cell);
            generateNumericUpDown.HideUpDownButtons = true;
            return generateNumericUpDown;
        }

        /// <summary>
        ///  获取一个绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的元素。
        /// </summary>
        /// <param name="isEditing">是否是编辑元素</param>
        /// <param name="cell">将包含生成的元素的单元格。</param>
        /// <returns>绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的新元素。</returns>
        private VicNumericUpDown GenerateVicNumericUpDown(bool isEditing, DataGridCell cell)
        {
            VicNumericUpDown numericUpDown = (cell != null) ? (cell.Content as VicNumericUpDown) : null;
            if (numericUpDown == null)
            {
                numericUpDown = new VicNumericUpDown();
            }

            ApplyStyle(isEditing, true, numericUpDown);
            ApplyBinding(numericUpDown, VicNumericUpDown.ValueProperty);

            numericUpDown.Minimum = Minimum;
            numericUpDown.Maximum = Maximum;
            numericUpDown.StringFormat = StringFormat;
            numericUpDown.Interval = Interval;
            numericUpDown.InterceptArrowKeys = true;
            numericUpDown.InterceptMouseWheel = true;
            numericUpDown.Speedup = true;
            numericUpDown.HideUpDownButtons = HideUpDownButtons;

            return numericUpDown;
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
