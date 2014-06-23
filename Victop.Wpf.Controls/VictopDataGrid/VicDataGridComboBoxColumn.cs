using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Victop.Wpf.Controls
{
    public class VicDataGridComboBoxColumn : DataGridComboBoxColumn
    {
        #region 字段
        private static Style _defaultEditingElementStyle;
        private static Style _defaultElementStyle;
        #endregion

        #region 构造函数
        static VicDataGridComboBoxColumn()
        {
            //设置未编辑样式
            ElementStyleProperty.OverrideMetadata(typeof(VicDataGridComboBoxColumn), new FrameworkPropertyMetadata(DefaultElementStyle));
            //设置编辑样式
            EditingElementStyleProperty.OverrideMetadata(typeof(VicDataGridComboBoxColumn), new FrameworkPropertyMetadata(DefaultEditingElementStyle));
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
                    Style style = new Style(typeof(VicComboBoxNormal));
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
                    Style style = new Style(typeof(VicComboBoxNormal));

                    style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
                    style.Setters.Add(new Setter(UIElement.IsHitTestVisibleProperty, false));
                    style.Setters.Add(new Setter(UIElement.FocusableProperty, false));
                    style.Setters.Add(new Setter(VicComboBoxNormal.HideDropDownToggleProperty, true));
                    style.Setters.Add(new Setter(VicComboBoxNormal.BackgroundProperty, new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
                    style.Setters.Add(new Setter(VicComboBoxNormal.BorderBrushProperty, new SolidColorBrush(Color.FromArgb(0, 0, 0, 0))));
                    style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0d)));

                    style.Seal();
                    _defaultElementStyle = style;
                }
                return _defaultElementStyle;
            }
        }


        //public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(VicDataGridComboBoxColumn), new PropertyMetadata(string.Empty));

        //public string DisplayMemberPath
        //{
        //    get { return (string)GetValue(DisplayMemberPathProperty); }
        //    set { SetValue(DisplayMemberPathProperty, value); }
        //}

        //public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(VicDataGridComboBoxColumn), new PropertyMetadata(null));

        //public IEnumerable ItemsSource
        //{
        //    get { return (IEnumerable)GetValue(ItemsSourceProperty); }
        //    set { SetValue(ItemsSourceProperty, value); }
        //}

        //public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register("SelectedValuePath", typeof(string), typeof(VicDataGridComboBoxColumn), new PropertyMetadata(string.Empty));

        //public string SelectedValuePath
        //{
        //    get { return (string)GetValue(SelectedValuePathProperty); }
        //    set { SetValue(SelectedValuePathProperty, value); }
        //}

        ///// <summary>
        /////  获取或设置 System.Windows.Controls.ComboBox 控件的文本框部分中文本的绑定。
        ///// </summary>
        //public virtual BindingBase TextBinding { get; set; }


        ///// <summary>
        ///// 获取或设置当前选定项的绑定。
        ///// </summary>
        //public virtual BindingBase SelectedItemBinding { get; set; }

        ///// <summary>
        ///// 获取或设置通过使用 System.Windows.Controls.DataGridComboBoxColumn.SelectedValuePath而获得的选定项的值。
        ///// </summary>
        //public virtual BindingBase SelectedValueBinding { get; set; }
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
            return GenerateVicComboBoxNormal(true, cell);
        }

        /// <summary>
        /// 获取一个绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的只读元素。
        /// </summary>
        /// <param name="cell"> 将包含生成的元素的单元格。</param>
        /// <param name="dataItem">由包含目标单元格的行表示的数据项。</param>
        /// <returns> 绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的新只读元素。</returns>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            VicComboBoxNormal generateComboBoxNormal = GenerateVicComboBoxNormal(false, cell);
            return generateComboBoxNormal;
        }

        /// <summary>
        ///  获取一个绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的元素。
        /// </summary>
        /// <param name="isEditing">是否是编辑元素</param>
        /// <param name="cell">将包含生成的元素的单元格。</param>
        /// <returns>绑定到列的 System.Windows.Controls.DataGridBoundColumn.Binding 属性值的新元素。</returns>
        private VicComboBoxNormal GenerateVicComboBoxNormal(bool isEditing, DataGridCell cell)
        {
            VicComboBoxNormal comboBoxNormal = (cell != null) ? (cell.Content as VicComboBoxNormal) : null;
            if (comboBoxNormal == null)
            {
                comboBoxNormal = new VicComboBoxNormal();
            }
            ApplyStyle(isEditing, true, comboBoxNormal);
            comboBoxNormal.ItemsSource = ItemsSource;
            ApplyBinding(comboBoxNormal, VicComboBoxNormal.TextProperty, TextBinding);
            ApplyBinding(comboBoxNormal, VicComboBoxNormal.SelectedItemProperty, SelectedItemBinding);
            ApplyBinding(comboBoxNormal, VicComboBoxNormal.SelectedValueProperty, SelectedValueBinding);
            comboBoxNormal.DisplayMemberPath = DisplayMemberPath;
            comboBoxNormal.SelectedValuePath = SelectedValuePath;
            return comboBoxNormal;
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
        internal void ApplyBinding(DependencyObject target, DependencyProperty property, BindingBase Binding)
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

        ////
        //// 摘要:
        ////     在 System.Windows.Controls.DataGridComboBoxColumn.SelectedItemBinding 属性更改时调用。
        ////
        //// 参数:
        ////   oldBinding:
        ////     以前的绑定。
        ////
        ////   newBinding:
        ////     已将列更改为的绑定。
        //protected void OnSelectedItemBindingChanged(BindingBase oldBinding, BindingBase newBinding)
        //{ 
        
        //}
        ////
        //// 摘要:
        ////     在 System.Windows.Controls.DataGridComboBoxColumn.SelectedValueBinding 属性更改时调用。
        ////
        //// 参数:
        ////   oldBinding:
        ////     以前的绑定。
        ////
        ////   newBinding:
        ////     已将列更改为的绑定。
        //protected void OnSelectedValueBindingChanged(BindingBase oldBinding, BindingBase newBinding)
        //{ 
        
        //}
        ////
        //// 摘要:
        ////     在 System.Windows.Controls.DataGridComboBoxColumn.TextBinding 属性更改时调用。
        ////
        //// 参数:
        ////   oldBinding:
        ////     以前的绑定。
        ////
        ////   newBinding:
        ////     已将列更改为的绑定。
        //protected  void OnTextBindingChanged(BindingBase oldBinding, BindingBase newBinding);
    }
}
