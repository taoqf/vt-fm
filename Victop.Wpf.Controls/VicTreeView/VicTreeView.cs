using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Victop.Wpf.Controls
{
    public class VicTreeView : TreeView
    {
        private List<System.Windows.Media.Brush> _brushList = new List<System.Windows.Media.Brush> { new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0)) };
        private int _controlCount;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields 避免未使用的私有字段")]
        private int _FilterListViewRowCurrentIndex;
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields 避免未使用的私有字段")]
        private int _FilterListViewRowIndex;
        private DataTable _FilterListViewRowsDown;
        private DataTable _FilterListViewRowsUp;
        private List<FrameworkElementFactory> _FrameworkElementControls = new List<FrameworkElementFactory>();
        private double _imageHeight = 16.0;
        private List<System.Windows.Media.ImageSource> _imageSource = new List<System.Windows.Media.ImageSource>();
        private double _imageWidth = 16.0;
        private bool _isSelectFirstRow = true;
        private ContextMenu _MenuSelect;
        //private CheckBoxCheckedEventHandler CheckBoxCheckedEvent;
        //private CheckBoxThreeStateCheckedEventHandler CheckBoxThreeStateCheckedEvent;
        //private CheckBoxThreeStateClickEventHandler CheckBoxThreeStateClickEvent;
        //private CheckBoxThreeStateUnCheckedEventHandler CheckBoxThreeStateUnCheckedEvent;
        //private CheckBoxUnCheckedEventHandler CheckBoxUnCheckedEvent;
        //private ComboBoxSelectionChangedEventHandler ComboBoxSelectionChangedEvent;
        private DataRow currentRow;
        public static readonly DependencyProperty DisplayFilterProperty = DependencyProperty.Register("DisplayFilter", typeof(bool), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata(false));
        public static readonly DependencyProperty FilterBackgroundProperty = DependencyProperty.Register("FilterBackground", typeof(System.Windows.Media.Color), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata(System.Windows.Media.Color.FromRgb(0xbf, 0xdb, 0xff)));
        public static readonly DependencyProperty FilterExpressionProperty = DependencyProperty.Register("FilterExpression", typeof(string), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata(""));
        private Dictionary<DataRow, System.Windows.Media.Color> FilterItemsOldColor = new Dictionary<DataRow, System.Windows.Media.Color>();
        public static readonly DependencyProperty FilterLevelSeperateProperty = DependencyProperty.Register("FilterLevelSeperate", typeof(string), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata("#"));
        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata(null, new PropertyChangedCallback(Victop.Wpf.Controls.VicTreeView.OnFilterChanged)));
        public static readonly DependencyProperty FilterRowColorProperty = DependencyProperty.Register("FilterRowColor", typeof(System.Windows.Media.Color), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata(System.Windows.Media.Color.FromRgb(0xda, 0xdf, 0xe7)));
        public static readonly DependencyProperty FilterTypeProperty = DependencyProperty.Register("FilterType", typeof(Victop.Wpf.Controls.FilterType), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata(Victop.Wpf.Controls.FilterType.Sql));
        public static readonly DependencyProperty FilterWatermarkProperty = DependencyProperty.Register("FilterWatermark", typeof(string), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata("搜索"));
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline 以内联方式初始化引用类型的静态字段")]
        public static readonly DependencyProperty IsDownSearchProperty = DependencyProperty.Register("IsDownSearch", typeof(bool), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata(true, new PropertyChangedCallback(Victop.Wpf.Controls.VicTreeView.OnIsDownSearchChanged)));
        public static readonly DependencyProperty IsExpandCurrentItemProperty = DependencyProperty.Register("IsExpandCurrentItem", typeof(bool), typeof(Victop.Wpf.Controls.VicTreeView), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsExpandItemChildProperty = DependencyProperty.Register("IsExpandItemChild", typeof(bool), typeof(Victop.Wpf.Controls.VicTreeView), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsShowItemChildActionProperty = DependencyProperty.Register("IsShowItemChildAction", typeof(bool), typeof(Victop.Wpf.Controls.VicTreeView), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty ItemChildCancelSelectHeaderProperty = DependencyProperty.Register("ItemChildCancelSelectHeader", typeof(string), typeof(Victop.Wpf.Controls.VicTreeView), new PropertyMetadata("取消子节点"));
        public static readonly DependencyProperty ItemChildExpandHeaderProperty = DependencyProperty.Register("ItemChildExpandHeader", typeof(string), typeof(Victop.Wpf.Controls.VicTreeView), new PropertyMetadata("展开子节点"));
        public static readonly DependencyProperty ItemChildSelectHeaderProperty = DependencyProperty.Register("ItemChildSelectHeader", typeof(string), typeof(Victop.Wpf.Controls.VicTreeView), new PropertyMetadata("全选子节点"));
        public static readonly DependencyProperty ItemChildUnExpandHeaderProperty = DependencyProperty.Register("ItemChildUnExpandHeader", typeof(string), typeof(Victop.Wpf.Controls.VicTreeView), new PropertyMetadata("折叠子节点"));
        public static readonly DependencyProperty ItemChildUnSelectHeaderProperty = DependencyProperty.Register("ItemChildUnSelectHeader", typeof(string), typeof(Victop.Wpf.Controls.VicTreeView), new PropertyMetadata("反选子节点"));
        public static readonly DependencyProperty PinYinFieldProperty = DependencyProperty.Register("PinYinField", typeof(string), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata(""));
        public static readonly DependencyProperty PinYinSearchRelationProperty = DependencyProperty.Register("PinYinSearchRelation", typeof(string), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata("or"));
        public static readonly DependencyProperty PinYinSearchTypeProperty = DependencyProperty.Register("PinYinSearchType", typeof(bool), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata(true));
        //private RadioButtonCheckedEventHandler RadioButtonCheckedEvent;
        //private RadioButtonUnCheckedEventHandler RadioButtonUnCheckedEvent;
        public static readonly DependencyProperty SetFilterFirstRowSelectedProperty = DependencyProperty.Register("SetFilterFirstRowSelected", typeof(bool), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata(false));
        public static readonly DependencyProperty SetFilterRowColorProperty = DependencyProperty.Register("FilterRowChecked", typeof(bool), typeof(Victop.Wpf.Controls.VicTreeView), new UIPropertyMetadata(false));
        //private TextBoxLostFocusEventHandler TextBoxLostFocusEvent;
        //private TextBoxValueChangedEventHandler TextBoxValueChangedEvent;
        //private MouseLeftButtonUpHandler tvcImageMouseLeftButtonUpEvent;

        public event CheckBoxCheckedEventHandler CheckBoxCheckedEvent
        {
            add { }
            remove { }
            //add
            //{
            //    CheckBoxCheckedEventHandler handler2;
            //    CheckBoxCheckedEventHandler checkBoxCheckedEvent = this.CheckBoxCheckedEvent;
            //    do
            //    {
            //        handler2 = checkBoxCheckedEvent;
            //        CheckBoxCheckedEventHandler handler3 = (CheckBoxCheckedEventHandler)Delegate.Combine(handler2, value);
            //        checkBoxCheckedEvent = Interlocked.CompareExchange<CheckBoxCheckedEventHandler>(ref this.CheckBoxCheckedEvent, handler3, handler2);
            //    }
            //    while (checkBoxCheckedEvent != handler2);
            //}
            //remove
            //{
            //    CheckBoxCheckedEventHandler handler2;
            //    CheckBoxCheckedEventHandler checkBoxCheckedEvent = this.CheckBoxCheckedEvent;
            //    do
            //    {
            //        handler2 = checkBoxCheckedEvent;
            //        CheckBoxCheckedEventHandler handler3 = (CheckBoxCheckedEventHandler)Delegate.Remove(handler2, value);
            //        checkBoxCheckedEvent = Interlocked.CompareExchange<CheckBoxCheckedEventHandler>(ref this.CheckBoxCheckedEvent, handler3, handler2);
            //    }
            //    while (checkBoxCheckedEvent != handler2);
            //}
        }

        public event CheckBoxThreeStateCheckedEventHandler CheckBoxThreeStateCheckedEvent
        {
            add { }
            remove { }
            //add
            //{
            //    CheckBoxThreeStateCheckedEventHandler handler2;
            //    CheckBoxThreeStateCheckedEventHandler checkBoxThreeStateCheckedEvent = this.CheckBoxThreeStateCheckedEvent;
            //    do
            //    {
            //        handler2 = checkBoxThreeStateCheckedEvent;
            //        CheckBoxThreeStateCheckedEventHandler handler3 = (CheckBoxThreeStateCheckedEventHandler)Delegate.Combine(handler2, value);
            //        checkBoxThreeStateCheckedEvent = Interlocked.CompareExchange<CheckBoxThreeStateCheckedEventHandler>(ref this.CheckBoxThreeStateCheckedEvent, handler3, handler2);
            //    }
            //    while (checkBoxThreeStateCheckedEvent != handler2);
            //}
            //remove
            //{
            //    CheckBoxThreeStateCheckedEventHandler handler2;
            //    CheckBoxThreeStateCheckedEventHandler checkBoxThreeStateCheckedEvent = this.CheckBoxThreeStateCheckedEvent;
            //    do
            //    {
            //        handler2 = checkBoxThreeStateCheckedEvent;
            //        CheckBoxThreeStateCheckedEventHandler handler3 = (CheckBoxThreeStateCheckedEventHandler)Delegate.Remove(handler2, value);
            //        checkBoxThreeStateCheckedEvent = Interlocked.CompareExchange<CheckBoxThreeStateCheckedEventHandler>(ref this.CheckBoxThreeStateCheckedEvent, handler3, handler2);
            //    }
            //    while (checkBoxThreeStateCheckedEvent != handler2);
            //}
        }

        public event CheckBoxThreeStateClickEventHandler CheckBoxThreeStateClickEvent
        {
            add { }
            remove { }
            //add
            //{
            //    CheckBoxThreeStateClickEventHandler handler2;
            //    CheckBoxThreeStateClickEventHandler checkBoxThreeStateClickEvent = this.CheckBoxThreeStateClickEvent;
            //    do
            //    {
            //        handler2 = checkBoxThreeStateClickEvent;
            //        CheckBoxThreeStateClickEventHandler handler3 = (CheckBoxThreeStateClickEventHandler)Delegate.Combine(handler2, value);
            //        checkBoxThreeStateClickEvent = Interlocked.CompareExchange<CheckBoxThreeStateClickEventHandler>(ref this.CheckBoxThreeStateClickEvent, handler3, handler2);
            //    }
            //    while (checkBoxThreeStateClickEvent != handler2);
            //}
            //remove
            //{
            //    CheckBoxThreeStateClickEventHandler handler2;
            //    CheckBoxThreeStateClickEventHandler checkBoxThreeStateClickEvent = this.CheckBoxThreeStateClickEvent;
            //    do
            //    {
            //        handler2 = checkBoxThreeStateClickEvent;
            //        CheckBoxThreeStateClickEventHandler handler3 = (CheckBoxThreeStateClickEventHandler)Delegate.Remove(handler2, value);
            //        checkBoxThreeStateClickEvent = Interlocked.CompareExchange<CheckBoxThreeStateClickEventHandler>(ref this.CheckBoxThreeStateClickEvent, handler3, handler2);
            //    }
            //    while (checkBoxThreeStateClickEvent != handler2);
            //}
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        public event CheckBoxThreeStateUnCheckedEventHandler CheckBoxThreeStateUnCheckedEvent
        {
            add { }
            remove { }
            //add
            //{
            //    CheckBoxThreeStateUnCheckedEventHandler handler2;
            //    CheckBoxThreeStateUnCheckedEventHandler checkBoxThreeStateUnCheckedEvent = this.CheckBoxThreeStateUnCheckedEvent;
            //    do
            //    {
            //        handler2 = checkBoxThreeStateUnCheckedEvent;
            //        CheckBoxThreeStateUnCheckedEventHandler handler3 = (CheckBoxThreeStateUnCheckedEventHandler)Delegate.Combine(handler2, value);
            //        checkBoxThreeStateUnCheckedEvent = Interlocked.CompareExchange<CheckBoxThreeStateUnCheckedEventHandler>(ref this.CheckBoxThreeStateUnCheckedEvent, handler3, handler2);
            //    }
            //    while (checkBoxThreeStateUnCheckedEvent != handler2);
            //}
            //remove
            //{
            //    CheckBoxThreeStateUnCheckedEventHandler handler2;
            //    CheckBoxThreeStateUnCheckedEventHandler checkBoxThreeStateUnCheckedEvent = this.CheckBoxThreeStateUnCheckedEvent;
            //    do
            //    {
            //        handler2 = checkBoxThreeStateUnCheckedEvent;
            //        CheckBoxThreeStateUnCheckedEventHandler handler3 = (CheckBoxThreeStateUnCheckedEventHandler)Delegate.Remove(handler2, value);
            //        checkBoxThreeStateUnCheckedEvent = Interlocked.CompareExchange<CheckBoxThreeStateUnCheckedEventHandler>(ref this.CheckBoxThreeStateUnCheckedEvent, handler3, handler2);
            //    }
            //    while (checkBoxThreeStateUnCheckedEvent != handler2);
            //}
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        public event CheckBoxUnCheckedEventHandler CheckBoxUnCheckedEvent
        {
            add { }
            remove { }
            //add
            //{
            //    CheckBoxUnCheckedEventHandler handler2;
            //    CheckBoxUnCheckedEventHandler checkBoxUnCheckedEvent = this.CheckBoxUnCheckedEvent;
            //    do
            //    {
            //        handler2 = checkBoxUnCheckedEvent;
            //        CheckBoxUnCheckedEventHandler handler3 = (CheckBoxUnCheckedEventHandler)Delegate.Combine(handler2, value);
            //        checkBoxUnCheckedEvent = Interlocked.CompareExchange<CheckBoxUnCheckedEventHandler>(ref this.CheckBoxUnCheckedEvent, handler3, handler2);
            //    }
            //    while (checkBoxUnCheckedEvent != handler2);
            //}
            //remove
            //{
            //    CheckBoxUnCheckedEventHandler handler2;
            //    CheckBoxUnCheckedEventHandler checkBoxUnCheckedEvent = this.CheckBoxUnCheckedEvent;
            //    do
            //    {
            //        handler2 = checkBoxUnCheckedEvent;
            //        CheckBoxUnCheckedEventHandler handler3 = (CheckBoxUnCheckedEventHandler)Delegate.Remove(handler2, value);
            //        checkBoxUnCheckedEvent = Interlocked.CompareExchange<CheckBoxUnCheckedEventHandler>(ref this.CheckBoxUnCheckedEvent, handler3, handler2);
            //    }
            //    while (checkBoxUnCheckedEvent != handler2);
            //}
        }

        public event ComboBoxSelectionChangedEventHandler ComboBoxSelectionChangedEvent
        {
            add { }
            remove { }
            //add
            //{
            //    ComboBoxSelectionChangedEventHandler handler2;
            //    ComboBoxSelectionChangedEventHandler comboBoxSelectionChangedEvent = this.ComboBoxSelectionChangedEvent;
            //    do
            //    {
            //        handler2 = comboBoxSelectionChangedEvent;
            //        ComboBoxSelectionChangedEventHandler handler3 = (ComboBoxSelectionChangedEventHandler)Delegate.Combine(handler2, value);
            //        comboBoxSelectionChangedEvent = Interlocked.CompareExchange<ComboBoxSelectionChangedEventHandler>(ref this.ComboBoxSelectionChangedEvent, handler3, handler2);
            //    }
            //    while (comboBoxSelectionChangedEvent != handler2);
            //}
            //remove
            //{
            //    ComboBoxSelectionChangedEventHandler handler2;
            //    ComboBoxSelectionChangedEventHandler comboBoxSelectionChangedEvent = this.ComboBoxSelectionChangedEvent;
            //    do
            //    {
            //        handler2 = comboBoxSelectionChangedEvent;
            //        ComboBoxSelectionChangedEventHandler handler3 = (ComboBoxSelectionChangedEventHandler)Delegate.Remove(handler2, value);
            //        comboBoxSelectionChangedEvent = Interlocked.CompareExchange<ComboBoxSelectionChangedEventHandler>(ref this.ComboBoxSelectionChangedEvent, handler3, handler2);
            //    }
            //    while (comboBoxSelectionChangedEvent != handler2);
            //}
        }

        public event RadioButtonCheckedEventHandler RadioButtonCheckedEvent
        {
            add { }
            remove { }
            //add
            //{
            //    RadioButtonCheckedEventHandler handler2;
            //    RadioButtonCheckedEventHandler radioButtonCheckedEvent = this.RadioButtonCheckedEvent;
            //    do
            //    {
            //        handler2 = radioButtonCheckedEvent;
            //        RadioButtonCheckedEventHandler handler3 = (RadioButtonCheckedEventHandler)Delegate.Combine(handler2, value);
            //        radioButtonCheckedEvent = Interlocked.CompareExchange<RadioButtonCheckedEventHandler>(ref this.RadioButtonCheckedEvent, handler3, handler2);
            //    }
            //    while (radioButtonCheckedEvent != handler2);
            //}
            //remove
            //{
            //    RadioButtonCheckedEventHandler handler2;
            //    RadioButtonCheckedEventHandler radioButtonCheckedEvent = this.RadioButtonCheckedEvent;
            //    do
            //    {
            //        handler2 = radioButtonCheckedEvent;
            //        RadioButtonCheckedEventHandler handler3 = (RadioButtonCheckedEventHandler)Delegate.Remove(handler2, value);
            //        radioButtonCheckedEvent = Interlocked.CompareExchange<RadioButtonCheckedEventHandler>(ref this.RadioButtonCheckedEvent, handler3, handler2);
            //    }
            //    while (radioButtonCheckedEvent != handler2);
            //}
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        public event RadioButtonUnCheckedEventHandler RadioButtonUnCheckedEvent
        {
            add { }
            remove { }
            //add
            //{
            //    RadioButtonUnCheckedEventHandler handler2;
            //    RadioButtonUnCheckedEventHandler radioButtonUnCheckedEvent = this.RadioButtonUnCheckedEvent;
            //    do
            //    {
            //        handler2 = radioButtonUnCheckedEvent;
            //        RadioButtonUnCheckedEventHandler handler3 = (RadioButtonUnCheckedEventHandler)Delegate.Combine(handler2, value);
            //        radioButtonUnCheckedEvent = Interlocked.CompareExchange<RadioButtonUnCheckedEventHandler>(ref this.RadioButtonUnCheckedEvent, handler3, handler2);
            //    }
            //    while (radioButtonUnCheckedEvent != handler2);
            //}
            //remove
            //{
            //    RadioButtonUnCheckedEventHandler handler2;
            //    RadioButtonUnCheckedEventHandler radioButtonUnCheckedEvent = this.RadioButtonUnCheckedEvent;
            //    do
            //    {
            //        handler2 = radioButtonUnCheckedEvent;
            //        RadioButtonUnCheckedEventHandler handler3 = (RadioButtonUnCheckedEventHandler)Delegate.Remove(handler2, value);
            //        radioButtonUnCheckedEvent = Interlocked.CompareExchange<RadioButtonUnCheckedEventHandler>(ref this.RadioButtonUnCheckedEvent, handler3, handler2);
            //    }
            //    while (radioButtonUnCheckedEvent != handler2);
            //}
        }

        public event TextBoxLostFocusEventHandler TextBoxLostFocusEvent
        {
            add { }
            remove { }
            //add
            //{
            //    TextBoxLostFocusEventHandler handler2;
            //    TextBoxLostFocusEventHandler textBoxLostFocusEvent = this.TextBoxLostFocusEvent;
            //    do
            //    {
            //        handler2 = textBoxLostFocusEvent;
            //        TextBoxLostFocusEventHandler handler3 = (TextBoxLostFocusEventHandler)Delegate.Combine(handler2, value);
            //        textBoxLostFocusEvent = Interlocked.CompareExchange<TextBoxLostFocusEventHandler>(ref this.TextBoxLostFocusEvent, handler3, handler2);
            //    }
            //    while (textBoxLostFocusEvent != handler2);
            //}
            //remove
            //{
            //    TextBoxLostFocusEventHandler handler2;
            //    TextBoxLostFocusEventHandler textBoxLostFocusEvent = this.TextBoxLostFocusEvent;
            //    do
            //    {
            //        handler2 = textBoxLostFocusEvent;
            //        TextBoxLostFocusEventHandler handler3 = (TextBoxLostFocusEventHandler)Delegate.Remove(handler2, value);
            //        textBoxLostFocusEvent = Interlocked.CompareExchange<TextBoxLostFocusEventHandler>(ref this.TextBoxLostFocusEvent, handler3, handler2);
            //    }
            //    while (textBoxLostFocusEvent != handler2);
            //}
        }

        public event TextBoxValueChangedEventHandler TextBoxValueChangedEvent
        {
            add { }
            remove { }
            //add
            //{
            //    TextBoxValueChangedEventHandler handler2;
            //    TextBoxValueChangedEventHandler textBoxValueChangedEvent = this.TextBoxValueChangedEvent;
            //    do
            //    {
            //        handler2 = textBoxValueChangedEvent;
            //        TextBoxValueChangedEventHandler handler3 = (TextBoxValueChangedEventHandler)Delegate.Combine(handler2, value);
            //        textBoxValueChangedEvent = Interlocked.CompareExchange<TextBoxValueChangedEventHandler>(ref this.TextBoxValueChangedEvent, handler3, handler2);
            //    }
            //    while (textBoxValueChangedEvent != handler2);
            //}
            //remove
            //{
            //    TextBoxValueChangedEventHandler handler2;
            //    TextBoxValueChangedEventHandler textBoxValueChangedEvent = this.TextBoxValueChangedEvent;
            //    do
            //    {
            //        handler2 = textBoxValueChangedEvent;
            //        TextBoxValueChangedEventHandler handler3 = (TextBoxValueChangedEventHandler)Delegate.Remove(handler2, value);
            //        textBoxValueChangedEvent = Interlocked.CompareExchange<TextBoxValueChangedEventHandler>(ref this.TextBoxValueChangedEvent, handler3, handler2);
            //    }
            //    while (textBoxValueChangedEvent != handler2);
            //}
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        public event MouseLeftButtonUpHandler tvcImageMouseLeftButtonUpEvent
        {
            add { }
            remove { }
            //add
            //{
            //    MouseLeftButtonUpHandler handler2;
            //    MouseLeftButtonUpHandler tvcImageMouseLeftButtonUpEvent = this.tvcImageMouseLeftButtonUpEvent;
            //    do
            //    {
            //        handler2 = tvcImageMouseLeftButtonUpEvent;
            //        MouseLeftButtonUpHandler handler3 = (MouseLeftButtonUpHandler)Delegate.Combine(handler2, value);
            //        tvcImageMouseLeftButtonUpEvent = Interlocked.CompareExchange<MouseLeftButtonUpHandler>(ref this.tvcImageMouseLeftButtonUpEvent, handler3, handler2);
            //    }
            //    while (tvcImageMouseLeftButtonUpEvent != handler2);
            //}
            //remove
            //{
            //    MouseLeftButtonUpHandler handler2;
            //    MouseLeftButtonUpHandler tvcImageMouseLeftButtonUpEvent = this.tvcImageMouseLeftButtonUpEvent;
            //    do
            //    {
            //        handler2 = tvcImageMouseLeftButtonUpEvent;
            //        MouseLeftButtonUpHandler handler3 = (MouseLeftButtonUpHandler)Delegate.Remove(handler2, value);
            //        tvcImageMouseLeftButtonUpEvent = Interlocked.CompareExchange<MouseLeftButtonUpHandler>(ref this.tvcImageMouseLeftButtonUpEvent, handler3, handler2);
            //    }
            //    while (tvcImageMouseLeftButtonUpEvent != handler2);
            //}
        }

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline"), SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework 仅使用目标框架中的 API")]
        static VicTreeView()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Victop.Wpf.Controls.VicTreeView), new FrameworkPropertyMetadata(typeof(Victop.Wpf.Controls.VicTreeView)));
            FrameworkElementFactory root = new FrameworkElementFactory(typeof(VirtualizingStackPanel));
            root.SetValue(VirtualizingStackPanel.VirtualizationModeProperty, VirtualizationMode.Recycling);
            ItemsPanelTemplate defaultValue = new ItemsPanelTemplate(root);
            defaultValue.Seal();
            ItemsControl.ItemsPanelProperty.OverrideMetadata(typeof(Victop.Wpf.Controls.VicTreeView), new FrameworkPropertyMetadata(defaultValue));
        }

        public VicTreeView()
        {
            base.CommandBindings.Add(new CommandBinding((ICommand)VicTreeViewCommands.ClearFilter, new ExecutedRoutedEventHandler(this.ClearFilter), new CanExecuteRoutedEventHandler(this.CanClearFilter)));
            base.CommandBindings.Add(new CommandBinding((ICommand)VicTreeViewCommands.DownSearch, new ExecutedRoutedEventHandler(this.DownSearch), new CanExecuteRoutedEventHandler(this.CanDownSearch)));
            base.CommandBindings.Add(new CommandBinding((ICommand)VicTreeViewCommands.UpSearch, new ExecutedRoutedEventHandler(this.UpSearch), new CanExecuteRoutedEventHandler(this.CanUpSearch)));
            base.SetValue(Panel.ZIndexProperty, -100);
            base.MouseRightButtonUp += new MouseButtonEventHandler(this.Tree_MouseRightButtonUp);
        }

        private int AddBrush(System.Windows.Media.Brush brush)
        {
            this._brushList.Add(brush);
            return (this._brushList.Count - 1);
        }

        [EnvironmentPermission(SecurityAction.LinkDemand, Unrestricted=true)]
        public void AddImage(Bitmap image)
        {
            if (image != null)
            {
                this._imageSource.Add(Imaging.CreateBitmapSourceFromHBitmap(image.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()));
            }
        }

        public void AddImage(Uri uri)
        {
            BitmapImage item = new BitmapImage();
            item.BeginInit();
            item.UriSource = uri;
            item.EndInit();
            this._imageSource.Add(item);
        }

        private void AddMenuItem()
        {
            Separator separator2 = new Separator {
                Name = "ahMenuItemSeperator"
            };
            Separator newItem = separator2;
            base.ContextMenu.Items.Add(newItem);
            MenuItem item = new MenuItem();
            if (!string.IsNullOrEmpty(this.ItemChildExpandHeader))
            {
                MenuItem item2 = new MenuItem {
                    Name = "ahItemChildExpandAll",
                    Header = this.ItemChildExpandHeader
                };
                item = item2;
                item.Click += new RoutedEventHandler(this.SelectMenuItemX_Child);
                base.ContextMenu.Items.Add(item);
            }
            if (!string.IsNullOrEmpty(this.ItemChildUnExpandHeader))
            {
                MenuItem item3 = new MenuItem {
                    Name = "ahItemChildUnExpand",
                    Header = this.ItemChildUnExpandHeader
                };
                item = item3;
                item.Click += new RoutedEventHandler(this.SelectMenuItemX_Child);
                base.ContextMenu.Items.Add(item);
            }
            if (!string.IsNullOrEmpty(this.ItemChildSelectHeader))
            {
                MenuItem item4 = new MenuItem {
                    Name = "ahMenuItemSelectChildAll",
                    Header = this.ItemChildSelectHeader
                };
                item = item4;
                item.Click += new RoutedEventHandler(this.SelectMenuItem_Child);
                base.ContextMenu.Items.Add(item);
            }
            if (!string.IsNullOrEmpty(this.ItemChildUnSelectHeader))
            {
                MenuItem item5 = new MenuItem {
                    Name = "ahMenuItemUnSelectChildAll",
                    Header = this.ItemChildUnSelectHeader
                };
                item = item5;
                item.Click += new RoutedEventHandler(this.SelectMenuItem_Child);
                base.ContextMenu.Items.Add(item);
            }
            if (!string.IsNullOrEmpty(this.ItemChildCancelSelectHeader))
            {
                MenuItem item6 = new MenuItem {
                    Name = "ahMenuItemCancelselectChildAll",
                    Header = this.ItemChildCancelSelectHeader
                };
                item = item6;
                item.Click += new RoutedEventHandler(this.SelectMenuItem_Child);
                base.ContextMenu.Items.Add(item);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public bool AddRow(DataTable dtNew)
        {
            if ((dtNew == null) || (dtNew.Rows.Count == 0))
            {
                return false;
            }
            DataView itemsSource = base.ItemsSource as DataView;
            if (itemsSource == null)
            {
                return false;
            }
            DataTable table = itemsSource.Table;
            if (!dtNew.Columns.Contains(this.AhTempColName_Selected))
            {
                DataColumn column = new DataColumn(this.AhTempColName_Selected, typeof(short));
                try
                {
                    column.DefaultValue = 0;
                    column.AllowDBNull = true;
                    dtNew.Columns.Add(column);
                }
                catch
                {
                    if (column != null)
                    {
                        column.Dispose();
                    }
                    return false;
                }
            }
            if (!dtNew.Columns.Contains(this.AhTempColName_Expanded))
            {
                DataColumn column2 = new DataColumn(this.AhTempColName_Expanded, typeof(short));
                try
                {
                    column2.DefaultValue = 0;
                    column2.AllowDBNull = true;
                    dtNew.Columns.Add(column2);
                }
                catch
                {
                    if (column2 != null)
                    {
                        column2.Dispose();
                    }
                    return false;
                }
            }
            if (!dtNew.Columns.Contains(this.AhTempColName_Foreground))
            {
                DataColumn column3 = new DataColumn(this.AhTempColName_Foreground, typeof(short));
                try
                {
                    column3.DefaultValue = 0;
                    column3.AllowDBNull = false;
                    dtNew.Columns.Add(column3);
                }
                catch
                {
                    if (column3 != null)
                    {
                        column3.Dispose();
                    }
                    return false;
                }
            }
            dtNew.Rows[0][this.AhTempColName_Expanded] = 1;
            dtNew.Rows[0][this.AhTempColName_Selected] = 1;
            table.Merge(dtNew, true, MissingSchemaAction.Ignore);
            return true;
        }

        private void CancelFilterRowsBachGroundColor()
        {
            if (this.FilterItemsOldColor.Count != 0)
            {
                Dictionary<DataRow, System.Windows.Media.Color>.Enumerator enumerator = this.FilterItemsOldColor.GetEnumerator();
                while (enumerator.MoveNext())
                {
                }
                this.FilterItemsOldColor.Clear();
            }
        }

        private void CanClearFilter(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(this.Filter);
        }

        private void CanDownSearch(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this._FilterListViewRowsDown != null;
        }

        private void CanUpSearch(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this._FilterListViewRowsUp != null;
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型"), SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores 标识符不应包含下划线")]
        protected void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (this.CheckBoxCheckedEvent != null)
                //{
                //    CheckBox box = sender as CheckBox;
                //    if ((box != null) && (box.DataContext != null))
                //    {
                //        DataRowView dataContext = box.DataContext as DataRowView;
                //        if (dataContext != null)
                //        {
                //            DataRow row = dataContext.Row;
                //            TreeListViewEventArgs args = new TreeListViewEventArgs {
                //                CurDataRow = row
                //            };
                //            this.CheckBoxCheckedEvent(sender, args);
                //        }
                //    }
                //}
            }
            catch (Exception exception)
            {
                throw new Exception("CheckBox Checked事件异常。", exception);
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型"), SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确"), SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores 标识符不应包含下划线")]
        protected void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (this.CheckBoxUnCheckedEvent != null)
                //{
                //    CheckBox box = sender as CheckBox;
                //    if ((box != null) && (box.DataContext != null))
                //    {
                //        DataRowView dataContext = box.DataContext as DataRowView;
                //        if (dataContext != null)
                //        {
                //            DataRow row = dataContext.Row;
                //            TreeListViewEventArgs args = new TreeListViewEventArgs {
                //                CurDataRow = row
                //            };
                //            this.CheckBoxUnCheckedEvent(sender, args);
                //        }
                //    }
                //}
            }
            catch (Exception exception)
            {
                throw new Exception("CheckBox UnChecked事件异常。", exception);
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型"), SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores 标识符不应包含下划线")]
        protected void CheckBoxThreeState_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //CheckBox box = sender as CheckBox;
                //if ((box != null) && (box.DataContext != null))
                //{
                //    DataRowView dataContext = box.DataContext as DataRowView;
                //    if (dataContext != null)
                //    {
                //        DataRow row = dataContext.Row;
                //        if (this.CheckBoxThreeStateCheckedEvent != null)
                //        {
                //            TreeListViewEventArgs args = new TreeListViewEventArgs {
                //                CurDataRow = row
                //            };
                //            this.CheckBoxThreeStateCheckedEvent(sender, args);
                //        }
                //    }
                //}
            }
            catch (Exception exception)
            {
                throw new Exception("CheckBoxThreeState Checked事件异常。", exception);
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型"), SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores 标识符不应包含下划线")]
        protected void CheckBoxThreeState_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //CheckBox cb = sender as CheckBox;
                //if ((cb != null) && (cb.DataContext != null))
                //{
                //    DataRowView dataContext = cb.DataContext as DataRowView;
                //    if (dataContext != null)
                //    {
                //        DataRow row = dataContext.Row;
                //        string bdField = Convert.ToString(cb.Tag, CultureInfo.CurrentCulture);
                //        if (!cb.IsChecked.HasValue)
                //        {
                //            cb.IsChecked = false;
                //        }
                //        this.CheckBoxThreeStateLinkFatherChild(cb, bdField);
                //        if (this.CheckBoxThreeStateClickEvent != null)
                //        {
                //            TreeListViewEventArgs args = new TreeListViewEventArgs {
                //                CurDataRow = row
                //            };
                //            this.CheckBoxThreeStateClickEvent(sender, args);
                //        }
                //    }
                //}
            }
            catch (Exception exception)
            {
                throw new Exception("CheckBoxThreeState_Click事件异常。", exception);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确"), SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores 标识符不应包含下划线"), SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型")]
        protected void CheckBoxThreeState_UnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                //CheckBox box = sender as CheckBox;
                //if ((box != null) && (box.DataContext != null))
                //{
                //    DataRowView dataContext = box.DataContext as DataRowView;
                //    if (dataContext != null)
                //    {
                //        DataRow row = dataContext.Row;
                //        if (this.CheckBoxThreeStateUnCheckedEvent != null)
                //        {
                //            TreeListViewEventArgs args = new TreeListViewEventArgs {
                //                CurDataRow = row
                //            };
                //            this.CheckBoxThreeStateUnCheckedEvent(sender, args);
                //        }
                //    }
                //}
            }
            catch (Exception exception)
            {
                throw new Exception("CheckBoxThreeState UnChecked事件异常。", exception);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void CheckBoxThreeStateCheckedLinkFather(DataRow dr, string bdField)
        {
            Func<DataRow, bool> predicate = null;
            if (dr != null)
            {
                try
                {
                    DataRow fatherRow = this.GetFatherRow(dr);
                    if (fatherRow != null)
                    {
                        DataRow[] childRows = this.GetChildRows(fatherRow);
                        if (childRows != null)
                        {
                            if (predicate == null)
                            {
                                predicate = item => !object.Equals(item[bdField].GetType(), typeof(DBNull)) && !object.Equals(Convert.ToInt32(item[bdField], CultureInfo.CurrentCulture), 0);
                            }
                            if (childRows.All<DataRow>(predicate))
                            {
                                if (object.Equals(fatherRow[bdField].GetType(), typeof(DBNull)))
                                {
                                    fatherRow[bdField] = 1;
                                }
                                else if (!object.Equals(Convert.ToInt32(fatherRow[bdField], CultureInfo.CurrentCulture), 1))
                                {
                                    fatherRow[bdField] = 1;
                                }
                            }
                            else
                            {
                                fatherRow[bdField] = DBNull.Value;
                            }
                            this.CheckBoxThreeStateCheckedLinkFather(fatherRow, bdField);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void CheckBoxThreeStateLinkChildren(DataRow dr, string bdField, bool isChecked)
        {
            if (dr != null)
            {
                try
                {
                    DataRow[] childRows = this.GetChildRows(dr);
                    if (childRows != null)
                    {
                        foreach (DataRow row in childRows)
                        {
                            int num2 = isChecked ? 1 : 0;
                            int num = object.Equals(row[bdField].GetType(), typeof(DBNull)) ? ((num2 == 1) ? 0 : 1) : Convert.ToInt32(row[bdField], CultureInfo.CurrentCulture);
                            if (num != num2)
                            {
                                row[bdField] = num2;
                                this.CheckBoxThreeStateLinkChildren(row, bdField, num2 != 0);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void CheckBoxThreeStateLinkFatherChild(CheckBox cb, string bdField)
        {
            DataRowView dataContext = cb.DataContext as DataRowView;
            if (dataContext != null)
            {
                DataRow dr = dataContext.Row;
                bool isChecked = !cb.IsChecked.HasValue ? false : cb.IsChecked.Value;
                this.CheckBoxThreeStateLinkChildren(dr, bdField, isChecked);
                if (isChecked)
                {
                    this.CheckBoxThreeStateCheckedLinkFather(dr, bdField);
                }
                else
                {
                    this.CheckBoxThreeStateUnCheckedLinkFather(dr, bdField);
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void CheckBoxThreeStateUnCheckedLinkFather(DataRow dr, string bdField)
        {
            Func<DataRow, bool> predicate = null;
            if (dr != null)
            {
                try
                {
                    DataRow fatherRow = this.GetFatherRow(dr);
                    DataRow[] childRows = this.GetChildRows(fatherRow);
                    if (childRows != null)
                    {
                        if (predicate == null)
                        {
                            predicate = item => !object.Equals(item[bdField].GetType(), typeof(DBNull)) && !object.Equals(Convert.ToInt32(item[bdField], CultureInfo.CurrentCulture), 1);
                        }
                        if (childRows.All<DataRow>(predicate))
                        {
                            if (fatherRow[bdField].GetType() == typeof(DBNull))
                            {
                                fatherRow[bdField] = 0;
                            }
                            else if (Convert.ToInt32(fatherRow[bdField], CultureInfo.CurrentCulture) != 0)
                            {
                                fatherRow[bdField] = 0;
                            }
                        }
                        else
                        {
                            fatherRow[bdField] = DBNull.Value;
                        }
                        this.CheckBoxThreeStateUnCheckedLinkFather(fatherRow, bdField);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void ClearFilter(object sender, ExecutedRoutedEventArgs e)
        {
            this.Filter = string.Empty;
        }

        private void ClearFilterItem()
        {
            this.CancelFilterRowsBachGroundColor();
            this.ClearTreeViewFilterItem();
        }

        private void ClearMenuItem()
        {
            if (base.ContextMenu != null)
            {
                List<string> list2 = new List<string> { "ahMenuItemSeperator", "ahMenuItemSelectChildAll", "ahMenuItemUnSelectChildAll", "ahMenuItemCancelselectChildAll", "ahItemChildExpandAll", "ahItemChildUnExpand" };
                List<string> list = list2;
                for (int i = base.ContextMenu.Items.Count - 1; i >= 0; i--)
                {
                    object removeItem = base.ContextMenu.Items[i];
                    if ((removeItem.GetType() == typeof(Separator)) && list.Contains((removeItem as Separator).Name))
                    {
                        base.ContextMenu.Items.Remove(removeItem);
                    }
                    if ((removeItem.GetType() == typeof(MenuItem)) && list.Contains((removeItem as MenuItem).Name))
                    {
                        base.ContextMenu.Items.Remove(removeItem);
                    }
                }
            }
        }

        protected virtual void ClearTreeViewFilterItem()
        {
            this._FilterListViewRowsDown = null;
            this._FilterListViewRowsUp = null;
            this._FilterListViewRowIndex = 0;
            this._FilterListViewRowCurrentIndex = 0;
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型")]
        protected void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e != null)
            {
                //try
                //{
                //    if (this.ComboBoxSelectionChangedEvent != null)
                //    {
                //        TreeListViewEventArgs args = new TreeListViewEventArgs {
                //            CurDataRowView = e.AddedItems[0] as DataRowView,
                //            CurDataRow = (e.AddedItems[0] as DataRowView).Row
                //        };
                //        this.ComboBoxSelectionChangedEvent(sender, args);
                //    }
                //}
                //catch (Exception exception)
                //{
                //    throw new Exception("TextBoxLostFocusEventHandler事件异常。", exception);
                //}
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static")]
        public void CopyRow(DataTable dt, DataRow rowOld)
        {
            if (dt != null)
            {
                DataRow row = dt.NewRow();
                if ((rowOld != null) && (rowOld.Table.Columns.Count != 0))
                {
                    foreach (DataColumn column in rowOld.Table.Columns)
                    {
                        row[column.ColumnName] = rowOld[column.ColumnName];
                    }
                    dt.Rows.Add(row);
                }
            }
        }

        public ContextMenu CreateMenu()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem item4 = new MenuItem {
                Header = "全选"
            };
            MenuItem newItem = item4;
            newItem.Click += delegate (object ss, RoutedEventArgs ee) {
                if (base.ItemsSource != null)
                {
                    foreach (DataRow row in (base.ItemsSource as DataView).Table.Rows)
                    {
                        row[this.CheckId] = 1;
                    }
                }
            };
            menu.Items.Add(newItem);
            MenuItem item5 = new MenuItem {
                Header = "反选"
            };
            MenuItem item2 = item5;
            item2.Click += delegate (object ss, RoutedEventArgs ee) {
                if (base.ItemsSource != null)
                {
                    foreach (DataRow row in (base.ItemsSource as DataView).Table.Rows)
                    {
                        row[this.CheckId] = (row[this.CheckId].ToString().Equals("1") || row[this.CheckId].ToString().ToLower(CultureInfo.CurrentCulture).Equals("true")) ? 0 : 1;
                    }
                }
            };
            menu.Items.Add(item2);
            MenuItem item6 = new MenuItem {
                Header = "清除"
            };
            MenuItem item3 = item6;
            item3.Click += delegate (object ss, RoutedEventArgs ee) {
                if (base.ItemsSource != null)
                {
                    foreach (DataRow row in (base.ItemsSource as DataView).Table.Rows)
                    {
                        row[this.CheckId] = 0;
                    }
                }
            };
            menu.Items.Add(item3);
            return menu;
        }

        public bool DelRow(DataRow dr)
        {
            if (dr == null)
            {
                return false;
            }
            DataRow parentRow = dr.GetParentRow(this.RelParentChild);
            if (parentRow != null)
            {
                parentRow[this.AhTempColName_Selected] = 1;
            }
            dr.Delete();
            return true;
        }

        public bool DelRow(DataView dvOld, bool isFocus)
        {
            DataTable table = (base.ItemsSource as DataView).Table;
            bool flag = false;
            if (dvOld == null)
            {
                return flag;
            }
            foreach (DataRowView view in dvOld)
            {
                table.Rows.Remove(view.Row);
                if (!flag && isFocus)
                {
                    DataRow parentRow = view.Row.GetParentRow(this.RelParentChild);
                    if (parentRow != null)
                    {
                        parentRow[this.AhTempColName_Selected] = 1;
                    }
                    flag = true;
                }
            }
            return true;
        }

        private void DownSearch(object sender, ExecutedRoutedEventArgs e)
        {
            this.UpDownSearch(true);
        }

        private DataRow ExpandAndSelectDataRowView(DataRow[] rows, DataRelation dataRealation, bool isDown)
        {
            DataTable dt = (base.ItemsSource as DataView).Table;
            DataRow row = null;
            DataRow row2 = null;
            Func<DataRow, bool> predicate = null;
            Func<DataRow, bool> func2 = null;
            foreach (DataRow item in rows)
            {
                if (!isDown)
                {
                }
                if (predicate == null)
                {
                    predicate = it => it[this.IDField].ToString().Equals(item[this.IDField].ToString());
                }
                row2 = (func2 != null) ? this._FilterListViewRowsDown.AsEnumerable().FirstOrDefault<DataRow>(predicate) : this._FilterListViewRowsUp.AsEnumerable().FirstOrDefault<DataRow>((func2 = it => it[this.IDField].ToString().Equals(item[this.IDField].ToString())));
                if (row2 != null)
                {
                    row = row2;
                }
                if (dataRealation != null)
                {
                    if ((row2 == null) || !isDown)
                    {
                        DataRow row3 = this.GetRow(dt, this.IDField, item[this.IDField].ToString());
                        if (row3 != null)
                        {
                            row2 = this.ExpandAndSelectDataRowView(row3.GetChildRows(dt.ChildRelations[0]), dataRealation, isDown);
                        }
                    }
                    if (row2 != null)
                    {
                        row = row2;
                    }
                }
                if (isDown && (row != null))
                {
                    return row;
                }
            }
            return row;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void ExpandedToRoot(DataRow dr)
        {
            if (dr != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(this.FIDField) && !string.IsNullOrEmpty(dr[this.FIDField].ToString()))
                    {
                        string filterExpression = object.Equals(dr[this.IDField].GetType(), typeof(string)) ? string.Format(CultureInfo.CurrentCulture, "{0}='{1}'", new object[] { this.IDField, dr[this.FIDField] }) : string.Format(CultureInfo.CurrentCulture, "{0}={1}", new object[] { this.IDField, dr[this.FIDField] });
                        DataRow[] rowArray = (base.ItemsSource as DataView).Table.Select(filterExpression);
                        if (rowArray.Length != 0)
                        {
                            rowArray[0]["ahtempcolIsExpanded"] = 1;
                            this.ExpandedToRoot(rowArray[0]);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters 不要将文本作为本地化参数传递")]
        protected void FilterTextBoxChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box != null)
            {
                string stringToFilter = box.Text.Trim();
                if (!string.IsNullOrEmpty(stringToFilter.Trim()))
                {
                    switch (this.FilterType)
                    {
                        case FilterType.Num:
                            if (!SQLFilter.NumFilter(stringToFilter))
                            {
                                System.Windows.MessageBox.Show("请输入数字。", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                if (e != null)
                                {
                                    IEnumerator<TextChange> enumerator = e.Changes.GetEnumerator();
                                    if (enumerator.MoveNext())
                                    {
                                        box.Text = stringToFilter.Remove(enumerator.Current.Offset, enumerator.Current.AddedLength);
                                        box.CaretIndex = enumerator.Current.Offset;
                                    }
                                    box.Focus();
                                    return;
                                }
                            }
                            return;

                        case FilterType.PlusInt:
                            if (!SQLFilter.PlusIntFilter(stringToFilter))
                            {
                                System.Windows.MessageBox.Show("请输入正整数。", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                if (e != null)
                                {
                                    IEnumerator<TextChange> enumerator2 = e.Changes.GetEnumerator();
                                    if (enumerator2.MoveNext())
                                    {
                                        box.Text = stringToFilter.Remove(enumerator2.Current.Offset, enumerator2.Current.AddedLength);
                                        box.CaretIndex = enumerator2.Current.Offset;
                                    }
                                    box.CaretIndex = box.Text.Length;
                                    return;
                                }
                            }
                            return;

                        case FilterType.Sql:
                            if (!SQLFilter.SqlFilter(stringToFilter))
                            {
                                System.Windows.MessageBox.Show("请勿使用敏感字符，详情请参见用户手册。", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                SQLFilter.ReplaceSqlFilter(ref stringToFilter);
                                box.Text = stringToFilter;
                                box.Focus();
                                box.CaretIndex = box.Text.Length;
                            }
                            return;
                    }
                }
            }
        }

        private void FormatDataTable(DataTable table)
        {
            if (!string.IsNullOrEmpty(this.FIDField))
            {
                foreach (DataRow row in table.Rows)
                {
                    if (!row[this.FIDField].Equals(DBNull.Value))
                    {
                        DataRow row2 = this.SearchBootRow(table, row);
                        if (row2 != null)
                        {
                            row2[this.FIDField] = DBNull.Value;
                        }
                    }
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public DataRow[] GetCheckedRows()
        {
            try
            {
                DataView itemsSource = base.ItemsSource as DataView;
                return itemsSource.Table.Select(string.Format(CultureInfo.CurrentCulture, "{0}=1", new object[] { this.CheckId }));
            }
            catch
            {
                return null;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public string GetCheckValues(string columnName)
        {
            try
            {
                DataView itemsSource = base.ItemsSource as DataView;
                if (itemsSource.Table.Columns.IndexOf(columnName) < 0)
                {
                    return "";
                }
                DataRow[] checkedRows = this.GetCheckedRows();
                string str = string.Empty;
                if (checkedRows.Length > 0)
                {
                    foreach (DataRow row in checkedRows)
                    {
                        str = string.Format(CultureInfo.CurrentCulture, "{0}{1},", new object[] { str, row[columnName] });
                    }
                    return str.Trim(new char[] { ',' });
                }
            }
            catch (Exception)
            {
                return "";
            }
            return "";
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public DataView GetChildItems(DataRowView drv)
        {
            if (drv == null)
            {
                return null;
            }
            DataView view = null;
            try
            {
                view = new DataView(drv.DataView.Table) {
                    RowFilter = string.Format(CultureInfo.CurrentCulture, "{0}='{1}'", new object[] { this.FIDField, drv[this.IDField] })
                };
            }
            catch (Exception)
            {
                if (view != null)
                {
                    view.Dispose();
                }
                return null;
            }
            return view;
        }

        public DataRow[] GetChildRows(DataRow row)
        {
            if (row == null)
            {
                return null;
            }
            string filterExpression = object.Equals(row[this.IDField].GetType(), typeof(string)) ? string.Format(CultureInfo.CurrentCulture, "{0}='{1}'", new object[] { this.FIDField, row[this.IDField] }) : string.Format(CultureInfo.CurrentCulture, "{0}={1}", new object[] { this.FIDField, row[this.IDField] });
            return (base.ItemsSource as DataView).Table.Select(filterExpression);
        }

        public DataRow GetFatherRow(DataRow row)
        {
            if (row == null)
            {
                return null;
            }
            if (row[this.FIDField] is DBNull)
            {
                return null;
            }
            string filterExpression = (row[this.FIDField] is string) ? string.Format(CultureInfo.CurrentCulture, "{0}='{1}'", new object[] { this.IDField, row[this.FIDField] }) : string.Format(CultureInfo.CurrentCulture, "{0}={1}", new object[] { this.IDField, row[this.FIDField] });
            return ((DataView) base.ItemsSource).Table.Select(filterExpression)[0];
        }

        public DataRow GetFirstParentDataRow(DataRowView drvInput)
        {
            DataRow[] parentDataRows = this.GetParentDataRows(drvInput);
            if (parentDataRows.Length > 0)
            {
                return parentDataRows[0];
            }
            return null;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public DataRow GetFirstParentDataRowBySelectRow()
        {
            DataRowView selectedItem = base.SelectedItem as DataRowView;
            if (selectedItem == null)
            {
                return null;
            }
            return this.GetFirstParentDataRow(selectedItem);
        }

        public DataRow[] GetParentDataRows(DataRowView drvInput)
        {
            if (drvInput == null)
            {
                return null;
            }
            return drvInput.DataView.Table.Select(string.Format(CultureInfo.CurrentCulture, "{0}='{1}'", new object[] { this.FIDField, drvInput[this.IDField] }));
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static")]
        private DataRow GetRow(DataTable dt, string fieldName, string fieldValue)
        {
            string filterExpression = "";
            filterExpression = string.Format(CultureInfo.CurrentCulture, (dt.Columns[fieldName].DataType == typeof(string)) ? "{0}='{1}'" : "{0}={1}", new object[] { fieldName, fieldValue });
            DataRow[] rowArray = dt.Select(filterExpression);
            if (rowArray.Length <= 0)
            {
                return null;
            }
            return rowArray[0];
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters 检查未使用的参数"), SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static")]
        private TreeViewItem GetSelectingItem(DataRowView boot, string fieldName, string fieldValue)
        {
            return null;
        }

        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily 避免进行不必要的强制转换")]
        private static T GetTemplatedAncestor<T>(FrameworkElement element) where T: FrameworkElement
        {
            if (element is T)
            {
                return (element as T);
            }
            FrameworkElement templatedParent = element.TemplatedParent as FrameworkElement;
            if (templatedParent != null)
            {
                return GetTemplatedAncestor<T>(templatedParent);
            }
            return default(T);
        }

        private bool HasMenuItemChild()
        {
            if (base.ContextMenu == null)
            {
                return false;
            }
            return base.ContextMenu.Items.Cast<object>().Any<object>(item => ((item.GetType() == typeof(MenuItem)) && (item as MenuItem).Name.Equals("ahMenuItemSelectChildAll")));
        }

        [SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework 仅使用目标框架中的 API")]
        private void InitCheckBox(CtrlField item)
        {
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(CheckBox), string.Format(CultureInfo.CurrentCulture, "myCtrlCheckBox{0}", new object[] { this._controlCount++ }));
            if (string.IsNullOrEmpty(this.CheckId))
            {
                this.CheckId = item.FieldName;
            }
            Binding binding = new Binding(item.FieldName) {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                StringFormat = item.StringFormat
            };
            factory.SetBinding(ToggleButton.IsCheckedProperty, binding);
            factory.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(this.CheckBox_Checked));
            factory.AddHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(this.CheckBox_UnChecked));
            this._FrameworkElementControls.Add(factory);
        }

        [SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework 仅使用目标框架中的 API")]
        private void InitCheckBoxThreeState(CtrlField item)
        {
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(CheckBox), string.Format(CultureInfo.CurrentCulture, "myCtrlCheckBoxThreeState{0}", new object[] { this._controlCount++ }));
            if (string.IsNullOrEmpty(this.CheckId))
            {
                this.CheckId = item.FieldName;
            }
            Binding binding = new Binding(item.FieldName) {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                StringFormat = item.StringFormat
            };
            factory.SetBinding(ToggleButton.IsCheckedProperty, binding);
            factory.SetValue(ToggleButton.IsThreeStateProperty, true);
            factory.SetValue(FrameworkElement.TagProperty, item.FieldName);
            factory.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(this.CheckBoxThreeState_Checked));
            factory.AddHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(this.CheckBoxThreeState_UnChecked));
            factory.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.CheckBoxThreeState_Click));
            this._FrameworkElementControls.Add(factory);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void InitExpandedField(DataTable newItemsSource)
        {
            DataColumn column = null;
            DataColumn column2 = null;
            try
            {
                column = new DataColumn(this.AhTempColName_Expanded, typeof(short)) {
                    DefaultValue = 1,
                    AllowDBNull = true
                };
                column2 = column;
                newItemsSource.Columns.Add(column2);
            }
            catch (Exception)
            {
                if (column != null)
                {
                    column.Dispose();
                }
            }
        }

        [SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework 仅使用目标框架中的 API")]
        private void InitImage(CtrlField item)
        {
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(System.Windows.Controls.Image), string.Format(CultureInfo.CurrentCulture, "myCtrlImage{0}", new object[] { this._controlCount++ }));
            Binding binding = new Binding(item.FieldName) {
                Mode = BindingMode.TwoWay,
                Converter = new ImageIndexToImageSource(),
                ConverterParameter = this._imageSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                StringFormat = item.StringFormat
            };
            factory.SetBinding(System.Windows.Controls.Image.SourceProperty, binding);
            factory.SetValue(FrameworkElement.HeightProperty, 16.0);
            factory.SetValue(FrameworkElement.WidthProperty, 16.0);
            this._FrameworkElementControls.Add(factory);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void InitPinYinField(DataTable newItemsSource)
        {
            if (this.IsPinYinSearch)
            {
                string name = this.PinYinField + this.PinYineFieldExt;
                if (!newItemsSource.Columns.Contains(name))
                {
                    DataColumn column = null;
                    DataColumn column2 = null;
                    try
                    {
                        column = new DataColumn(name, typeof(string)) {
                            DefaultValue = string.Empty,
                            AllowDBNull = true
                        };
                        column2 = column;
                        newItemsSource.Columns.Add(column2);
                        foreach (DataRow row in newItemsSource.Rows)
                        {
                            if (this.PinYinSearchType)
                            {
                            }
                            //row[name] = (CS$<>9__CachedAnonymousMethodDelegate29 != null) ? row[this.PinYinField].ToString().GetPinyins().Aggregate<string, string>(string.Empty, (current, st) => (current + st)) : row[this.PinYinField].ToString().GetPinyinsFirst().Aggregate<string, string>(string.Empty, CS$<>9__CachedAnonymousMethodDelegate29);
                        }
                    }
                    catch (Exception)
                    {
                        if (column != null)
                        {
                            column.Dispose();
                        }
                    }
                }
            }
        }

        [SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework 仅使用目标框架中的 API")]
        private void InitRadioButton(CtrlField item)
        {
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(RadioButton), string.Format(CultureInfo.CurrentCulture, "myCtrlRadioButton{0}", new object[] { this._controlCount++ }));
            if (string.IsNullOrEmpty(this.CheckId))
            {
                this.CheckId = item.FieldName;
            }
            Binding binding = new Binding(item.FieldName) {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                StringFormat = item.StringFormat
            };
            factory.SetBinding(ToggleButton.IsCheckedProperty, binding);
            factory.AddHandler(ToggleButton.CheckedEvent, new RoutedEventHandler(this.RadioButton_Checked));
            factory.AddHandler(ToggleButton.UncheckedEvent, new RoutedEventHandler(this.RadioButton_UnChecked));
            factory.SetValue(RadioButton.GroupNameProperty, item.FieldName);
            this._FrameworkElementControls.Add(factory);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private void InitSelectedField(DataTable newItemsSource, string newRowSelectItemSql)
        {
            DataColumn column = null;
            DataColumn column2 = null;
            try
            {
                column = new DataColumn(this.AhTempColName_Selected, typeof(short)) {
                    DefaultValue = string.IsNullOrEmpty(newRowSelectItemSql) ? 1 : 0,
                    AllowDBNull = true
                };
                column2 = column;
                newItemsSource.Columns.Add(column2);
            }
            catch (Exception)
            {
                if (column != null)
                {
                    column.Dispose();
                }
            }
        }

        [SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework 仅使用目标框架中的 API")]
        private void InitTextBlock(CtrlField item)
        {
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBlock), string.Format(CultureInfo.CurrentCulture, "myCtrlTextBlock{0}", new object[] { this._controlCount++ }));
            Binding binding = new Binding(item.FieldName) {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                StringFormat = item.StringFormat
            };
            factory.SetBinding(TextBlock.TextProperty, binding);
            this._FrameworkElementControls.Add(factory);
        }

        [SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework 仅使用目标框架中的 API")]
        private void InitTextBox(CtrlField item)
        {
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBox), string.Format(CultureInfo.CurrentCulture, "myCtrlTexBox{0}", new object[] { this._controlCount++ }));
            Binding binding = new Binding(item.FieldName) {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                StringFormat = item.StringFormat
            };
            factory.SetBinding(TextBox.TextProperty, binding);
            factory.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(this.TextBoxValueChanged));
            factory.AddHandler(UIElement.LostFocusEvent, new RoutedEventHandler(this.TextBoxLostFocus));
            this._FrameworkElementControls.Add(factory);
        }

        [SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework 仅使用目标框架中的 API")]
        private void InitTextBoxRightAlign(CtrlField item)
        {
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBox), string.Format(CultureInfo.CurrentCulture, "myCtrlTexBox{0}", new object[] { this._controlCount++ }));
            Binding binding = new Binding(item.FieldName) {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                StringFormat = item.StringFormat
            };
            factory.SetBinding(TextBox.TextProperty, binding);
            factory.SetValue(TextBox.TextAlignmentProperty, TextAlignment.Right);
            factory.AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(this.TextBoxValueChanged));
            factory.AddHandler(UIElement.LostFocusEvent, new RoutedEventHandler(this.TextBoxLostFocus));
            this._FrameworkElementControls.Add(factory);
        }

        private static void MakeDeleteFlag(DataTable newItemsSource, string[] primaryKeyFields, List<object> oldRowKeyValue, DataTable oldItemsSource)
        {
            Func<string, object> selector = null;
            for (int i = 0; i < oldItemsSource.Rows.Count; i++)
            {
                if (oldItemsSource.Rows[i].RowState == DataRowState.Deleted)
                {
                    oldItemsSource.Rows[i].AcceptChanges();
                    i--;
                }
                else
                {
                    oldRowKeyValue.Clear();
                    if (selector == null)
                    {
                        selector = str => oldItemsSource.Rows[i][str];
                    }
                    oldRowKeyValue.AddRange(primaryKeyFields.Select<string, object>(selector));
                    DataRow row = newItemsSource.Rows.Find(oldRowKeyValue.ToArray());
                    if (row != null)
                    {
                        foreach (DataColumn column in row.Table.Columns)
                        {
                            oldItemsSource.Rows[i][column.ColumnName] = row[column.ColumnName];
                        }
                    }
                    else
                    {
                        oldItemsSource.Rows[i].Delete();
                        oldItemsSource.Rows[i].AcceptChanges();
                        i--;
                    }
                }
            }
        }

        public override void OnApplyTemplate()
        {
            if (!object.Equals(base.GetType(), typeof(Victop.Wpf.Controls.VicTreeView)))
            {
                base.OnApplyTemplate();
            }
            if (base.GetTemplateChild("_txtFilter") != null)
            {
                (base.GetTemplateChild("_txtFilter") as TextBox).TextChanged += new TextChangedEventHandler(this.FilterTextBoxChanged);
                (base.GetTemplateChild("_txtFilter") as TextBox).MouseDoubleClick += new MouseButtonEventHandler(this.WatermarkTextBoxMouseDoubleClick);
            }
        }

        private static void OnFilterChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(o))
            {
                Victop.Wpf.Controls.VicTreeView view = o as Victop.Wpf.Controls.VicTreeView;
                if (view != null)
                {
                    view.OnFilterTreeViewChanged(o, (string) e.OldValue, (string) e.NewValue);
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型")]
        protected virtual void OnFilterTreeViewChanged(DependencyObject dependency, string oldValue, string newValue)
        {
            try
            {
                if (string.IsNullOrEmpty(newValue))
                {
                    this.ClearTreeViewFilterItem();
                }
                else if (!string.Equals(oldValue, newValue))
                {
                    if (base.ItemsSource != null)
                    {
                        string filterExpression = this.FilterExpression;
                        if (this.IsPinYinSearch)
                        {
                            if (string.IsNullOrEmpty(filterExpression))
                            {
                                filterExpression = string.Format(CultureInfo.CurrentCulture, "{0}{1} Like '%{{0}}%'", new object[] { this.PinYinField, this.PinYineFieldExt });
                            }
                            else
                            {
                                filterExpression = filterExpression + string.Format(CultureInfo.CurrentCulture, " {2} {0}{1} Like '%{{0}}%'", new object[] { this.PinYinField, this.PinYineFieldExt, this.PinYinSearchRelation });
                            }
                        }
                        if (string.IsNullOrEmpty(filterExpression))
                        {
                            throw new Exception("过滤条件FilterExpression为空！");
                        }
                        DataView itemsSource = (DataView) base.ItemsSource;
                        this._FilterListViewRowIndex = 0;
                        this._FilterListViewRowsDown = itemsSource.Table.Select(string.Format(CultureInfo.CurrentCulture, filterExpression, new object[] { newValue })).CopyToDataTable<DataRow>();
                        this._FilterListViewRowsUp = this._FilterListViewRowsDown.Clone();
                        this.SetFilterRowsBachGroundColor();
                        this.currentRow = null;
                        if ((this.SetFilterFirstRowSelected && (this._FilterListViewRowsDown != null)) && (this._FilterListViewRowsDown.Rows.Count > 0))
                        {
                            this.UpDownSearch(true);
                            (base.GetTemplateChild("_txtFilter") as TextBox).Focus();
                        }
                    }
                    else
                    {
                        this.ClearTreeViewFilterItem();
                    }
                }
            }
            catch
            {
            }
        }

        protected virtual void OnIsDownSearchChanged(bool oldValue, bool newValue)
        {
        }

        private static void OnIsDownSearchChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            this.ClearFilterItem();
            DataView drv = newValue as DataView;
            if (drv != null)
            {
                Style style = new Style(typeof(TreeViewItem));
                if (object.Equals(base.GetType(), typeof(Victop.Wpf.Controls.VicTreeView)))
                {
                    ResourceDictionary item = Application.LoadComponent(new Uri("Victop.Wpf.VicTreeView;component/Themes/TreeView.xaml", UriKind.Relative)) as ResourceDictionary;
                    if (!base.Resources.MergedDictionaries.Contains(item))
                    {
                        base.Resources.MergedDictionaries.Add(item);
                    }
                    Style style2 = base.FindResource("TreeStyle") as Style;
                    if (style2 != null)
                    {
                        style.BasedOn = style2;
                    }
                }
                if (!drv.Table.Columns.Contains("ahtempcolIsExpanded"))
                {
                    DataColumn column = null;
                    try
                    {
                        column = new DataColumn("ahtempcolIsExpanded", typeof(short))
                        {
                            DefaultValue = 0,
                            AllowDBNull = true
                        };
                        drv.Table.Columns.Add(column);
                        Binding binding = new Binding("ahtempcolIsExpanded")
                        {
                            Mode = BindingMode.TwoWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };
                        style.Setters.Add(new Setter(TreeViewItem.IsExpandedProperty, binding));
                    }
                    catch (Exception)
                    {
                        if (column != null)
                        {
                            column.Dispose();
                        }
                    }
                }
                else
                {
                    Binding binding2 = new Binding("ahtempcolIsExpanded")
                    {
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
                    style.Setters.Add(new Setter(TreeViewItem.IsExpandedProperty, binding2));
                }
                if (!drv.Table.Columns.Contains("ahtempcolIsSelected"))
                {
                    DataColumn column2 = null;
                    try
                    {
                        column2 = new DataColumn("ahtempcolIsSelected", typeof(short))
                        {
                            DefaultValue = 0,
                            AllowDBNull = true
                        };
                        drv.Table.Columns.Add(column2);
                        Binding binding3 = new Binding("ahtempcolIsSelected")
                        {
                            Mode = BindingMode.TwoWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };
                        style.Setters.Add(new Setter(TreeViewItem.IsSelectedProperty, binding3));
                        if (column2 != null)
                        {
                            column2.Dispose();
                        }
                    }
                    catch (Exception)
                    {
                        if (column2 != null)
                        {
                            column2.Dispose();
                        }
                    }
                }
                if (!drv.Table.Columns.Contains("ahtempcolForeground"))
                {
                    DataColumn column3 = null;
                    try
                    {
                        column3 = new DataColumn("ahtempcolForeground", typeof(short))
                        {
                            DefaultValue = 0,
                            AllowDBNull = true
                        };
                        drv.Table.Columns.Add(column3);
                        Binding binding4 = new Binding("ahtempcolForeground")
                        {
                            Mode = BindingMode.TwoWay,
                            Converter = new BrushIndexToBrush(),
                            ConverterParameter = this._brushList,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                        };
                        style.Setters.Add(new Setter(Control.ForegroundProperty, binding4));
                    }
                    catch (Exception)
                    {
                        if (column3 != null)
                        {
                            column3.Dispose();
                        }
                    }
                }
                if (style.Setters.Count > 0)
                {
                    base.ItemContainerStyle = style;
                }
            }
            if (drv != null)
            {
                drv.RowFilter = "";
                if (base.ItemTemplate == null)
                {
                    base.ItemTemplate = this.HirDataTemplate;
                }
                if ((this.IDField != null) && (this.FIDField != null))
                {
                    this.SetFidNull(drv);
                    if (drv.Table.DataSet == null)
                    {
                        DataSet set = null;
                        DataSet set2 = null;
                        using (set = new DataSet { Locale = CultureInfo.InvariantCulture })
                        {
                            set2 = set;
                            set2.Tables.Add(drv.Table);
                        }
                    }
                    drv.Table.DataSet.Relations.Clear();
                    if (!drv.Table.DataSet.Relations.Contains(this.RelParentChild))
                    {
                        DataColumn colID = drv.Table.Columns[this.IDField];
                        DataColumn colFID = drv.Table.Columns[this.FIDField];
                        if (colID == null || colFID == null) return;
                        drv.Table.DataSet.Relations.Add(this.RelParentChild, colID, colFID);
                    }
                    drv.RowFilter = string.Format(CultureInfo.CurrentCulture, "{0} is null", new object[] { this.FIDField });
                }
            }
            base.OnItemsSourceChanged(oldValue, newValue);
            if (this._isSelectFirstRow && (base.Items.Count > 0))
            {
                this.SetItemSelected(base.Items[0] as DataRowView);
            }
            this.SearchFilterItem();
        }

        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);
            if (e != null)
            {
                TreeViewItem templatedAncestor = GetTemplatedAncestor<TreeViewItem>(e.OriginalSource as FrameworkElement);
                if (templatedAncestor != null)
                {
                    templatedAncestor.IsSelected = true;
                }
            }
        }

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            base.OnSelectedItemChanged(e);
            this.BringSelectItemIntoView(base.SelectedItem);
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型"), SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores 标识符不应包含下划线")]
        protected void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (this.RadioButtonCheckedEvent != null)
                //{
                //    RadioButton button = sender as RadioButton;
                //    if ((button != null) && (button.DataContext != null))
                //    {
                //        DataRowView dataContext = button.DataContext as DataRowView;
                //        if (dataContext != null)
                //        {
                //            DataRow row = dataContext.Row;
                //            TreeListViewEventArgs args = new TreeListViewEventArgs {
                //                CurDataRow = row
                //            };
                //            this.RadioButtonCheckedEvent(sender, args);
                //        }
                //    }
                //}
            }
            catch (Exception exception)
            {
                throw new Exception("RadioButton Checked事件异常。", exception);
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型"), SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确"), SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores 标识符不应包含下划线")]
        protected void RadioButton_UnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (this.RadioButtonUnCheckedEvent != null)
                //{
                //    RadioButton button = sender as RadioButton;
                //    if ((button != null) && (button.DataContext != null))
                //    {
                //        DataRowView dataContext = button.DataContext as DataRowView;
                //        if (dataContext != null)
                //        {
                //            DataRow row = dataContext.Row;
                //            TreeListViewEventArgs args = new TreeListViewEventArgs {
                //                CurDataRow = row
                //            };
                //            this.RadioButtonUnCheckedEvent(sender, args);
                //        }
                //    }
                //}
            }
            catch (Exception exception)
            {
                throw new Exception("RadioButton UnChecked事件异常。", exception);
            }
        }

        public bool RefreshItemsSource(DataTable newItemsSource, string[] primaryKeyFields)
        {
            return this.RefreshItemsSource(newItemsSource, primaryKeyFields, false, null);
        }

        public bool RefreshItemsSource(DataTable newItemsSource, string[] primaryKeyFields, bool isSetNewRowSelected)
        {
            return this.RefreshItemsSource(newItemsSource, primaryKeyFields, isSetNewRowSelected, null);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public bool RefreshItemsSource(DataTable newItemsSource, string[] primaryKeyFields, bool isSetNewRowSelected, string newRowSelectItemSql)
        {
            this.ClearFilterItem();
            if (((newItemsSource == null) || (primaryKeyFields == null)) || (primaryKeyFields.Length == 0))
            {
                return false;
            }
            this.InitPinYinField(newItemsSource);
            this.FormatDataTable(newItemsSource);
            if (base.ItemsSource == null)
            {
                base.ItemsSource = newItemsSource.DefaultView;
                this.SearchFilterItem();
                return true;
            }
            DataTable oldItemsSource = (base.ItemsSource as DataView).Table;
            using (DataTable table2 = oldItemsSource.GetChanges(DataRowState.Deleted))
            {
                if (table2 != null)
                {
                    table2.AcceptChanges();
                }
            }
            List<DataColumn> list = new List<DataColumn>();
            List<DataColumn> list2 = new List<DataColumn>();
            List<object> oldRowKeyValue = new List<object>();
            List<object> list4 = new List<object>();
            try
            {
                foreach (string str in primaryKeyFields)
                {
                    list.Add(oldItemsSource.Columns[str]);
                    list2.Add(newItemsSource.Columns[str]);
                }
                oldItemsSource.PrimaryKey = list.ToArray();
                newItemsSource.PrimaryKey = list2.ToArray();
                MakeDeleteFlag(newItemsSource, primaryKeyFields, oldRowKeyValue, oldItemsSource);
                if (isSetNewRowSelected && !string.IsNullOrEmpty(this.Filter))
                {
                    isSetNewRowSelected = false;
                }
                if (isSetNewRowSelected)
                {
                    if (!newItemsSource.Columns.Contains(this.AhTempColName_Selected))
                    {
                        this.InitSelectedField(newItemsSource, newRowSelectItemSql);
                    }
                    if (!newItemsSource.Columns.Contains(this.AhTempColName_Expanded))
                    {
                        this.InitExpandedField(newItemsSource);
                    }
                }
                if (!string.IsNullOrEmpty(this.FIDField))
                {
                    string filter = string.Format(CultureInfo.CurrentCulture, "{0} is null", new object[] { this.FIDField });
                    this.TreeListAddRow(newItemsSource, filter, primaryKeyFields);
                    if (isSetNewRowSelected)
                    {
                        DataRow[] rowArray = oldItemsSource.Select(string.IsNullOrEmpty(newRowSelectItemSql) ? string.Format(CultureInfo.CurrentCulture, "{0}=1", new object[] { this.AhTempColName_Selected }) : newRowSelectItemSql);
                        if (rowArray.Length > 0)
                        {
                            foreach (DataRow row in rowArray)
                            {
                                row[this.AhTempColName_Selected] = 1;
                                this.ExpandedToRoot(row);
                            }
                        }
                    }
                }
                else
                {
                    IEnumerator enumerator = newItemsSource.Rows.GetEnumerator();
                        Func<string, object> selector = null;
                        DataRow row;
                        while (enumerator.MoveNext())
                        {
                            row = (DataRow) enumerator.Current;
                            list4.Clear();
                            if (selector == null)
                            {
                                selector = str => row[str];
                            }
                            list4.AddRange(primaryKeyFields.Select<string, object>(selector));
                            if (oldItemsSource.Rows.Find(list4.ToArray()) == null)
                            {
                                oldItemsSource.ImportRow(row);
                            }
                        }
                    if (isSetNewRowSelected)
                    {
                        DataRow[] rowArray2 = oldItemsSource.Select(string.IsNullOrEmpty(newRowSelectItemSql) ? string.Format(CultureInfo.CurrentCulture, "{0} = 1", new object[] { this.AhTempColName_Selected }) : newRowSelectItemSql);
                        if (rowArray2.Length > 0)
                        {
                            foreach (DataRow row3 in rowArray2)
                            {
                                row3[this.AhTempColName_Selected] = 1;
                            }
                        }
                    }
                }
                this.SearchFilterItem();
            }
            catch
            {
                base.ItemsSource = newItemsSource.DefaultView;
                this.SearchFilterItem();
            }
            return true;
        }

        private DataRow SearchBootRow(DataTable table, DataRow row)
        {
            DataRow[] rowArray = table.Select(string.Format(CultureInfo.CurrentCulture, "{0} = '{1}'", new object[] { this.IDField, row[this.FIDField] }));
            int index = 0;
            while (index < rowArray.Length)
            {
                DataRow row2 = rowArray[index];
                if (row2[this.FIDField].Equals(DBNull.Value))
                {
                    return null;
                }
                return this.SearchBootRow(table, row2);
            }
            return row;
        }

        private void SearchFilterItem()
        {
            string filter = this.Filter;
            if (!string.IsNullOrEmpty(filter))
            {
                this.OnFilterTreeViewChanged(this, string.Empty, filter);
            }
        }

        private void SelectMenuItem_Child(object sender, RoutedEventArgs e)
        {
            if (base.SelectedItem != null)
            {
                DataRowView selectedItem = base.SelectedItem as DataRowView;
                short status = 1;
                MenuItem source = (MenuItem) e.Source;
                string name = source.Name;
                if (name != null)
                {
                    if (!(name == "ahMenuItemUnSelectChildAll"))
                    {
                        if (name == "ahMenuItemCancelselectChildAll")
                        {
                            status = 3;
                        }
                    }
                    else
                    {
                        status = 2;
                    }
                }
                this.SetDataRowViewSelect(selectedItem.Row, this.IDField, this.FIDField, this.CheckId, status);
            }
        }

        private void SelectMenuItemX_Child(object sender, RoutedEventArgs e)
        {
            if (base.SelectedItem != null)
            {
                DataRowView selectedItem = base.SelectedItem as DataRowView;
                MenuItem source = (MenuItem) e.Source;
                bool expand = source.Name.Equals("ahItemChildExpandAll");
                selectedItem.Row[this.AhTempColName_Expanded] = expand ? 1 : 0;
                this.SetDataRowViewExpand(selectedItem.Row, this.IDField, this.FIDField, expand);
            }
        }

        private void SetDataRowViewExpand(DataRow dataRow, string keyField, string fidField, bool expand)
        {
            string filterExpression = string.Format("{0} = '{1}'", fidField, dataRow[keyField]);
            DataRow[] rowArray = dataRow.Table.Select(filterExpression);
            if (rowArray.Length != 0)
            {
                foreach (DataRow row in rowArray)
                {
                    row[this.AhTempColName_Expanded] = expand ? 1 : 0;
                    this.SetDataRowViewExpand(row, keyField, fidField, expand);
                }
            }
        }

        private void SetDataRowViewSelect(DataRow dataRow, string keyField, string fidField, string checkId, short status)
        {
            string filterExpression = string.Format("{0} = '{1}'", fidField, dataRow[keyField]);
            DataRow[] rowArray = dataRow.Table.Select(filterExpression);
            if (rowArray.Length != 0)
            {
                foreach (DataRow row in rowArray)
                {
                    switch (status)
                    {
                        case 1:
                            row[checkId] = 1;
                            break;

                        case 2:
                            row[checkId] = (row[checkId].ToString().Equals("1") || row[checkId].ToString().ToLower(CultureInfo.CurrentCulture).Equals("true")) ? 0 : 1;
                            break;

                        default:
                            row[checkId] = 0;
                            break;
                    }
                    if (this.IsExpandItemChild)
                    {
                        row[this.AhTempColName_Expanded] = 1;
                    }
                    this.SetDataRowViewSelect(row, keyField, fidField, checkId, status);
                }
            }
        }

        private void SetFidNull(DataView drv)
        {
            if (this.FIDValue != null)
            {
                foreach (DataRow row in drv.Table.Rows)
                {
                    if (this.FIDValue.Equals(row[this.FIDField]))
                    {
                        row[this.FIDField] = DBNull.Value;
                    }
                }
            }
            else
            {
                foreach (DataRow row2 in drv.Table.Rows)
                {
                    object obj2 = row2[this.FIDField];
                    bool flag = false;
                    foreach (DataRow row3 in drv.Table.Rows)
                    {
                        object obj3 = row3[this.IDField];
                        if (obj3.Equals(obj2))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag && (row2[this.FIDField] != DBNull.Value))
                    {
                        row2[this.FIDField] = DBNull.Value;
                    }
                }
            }
        }

        private void SetFilterRowsBachGroundColor()
        {
            if (this.SetFilterRowColor)
            {
                this.CancelFilterRowsBachGroundColor();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemChecked(string field, string where)
        {
            if (base.ItemsSource != null)
            {
                try
                {
                    DataView itemsSource = base.ItemsSource as DataView;
                    foreach (DataRow row in itemsSource.Table.Select(where))
                    {
                        row[field] = 1;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public void SetItemExpanded(DataRow drv)
        {
            if (drv != null)
            {
                drv["ahtempcolIsExpanded"] = 1;
                this.ExpandedToRoot(drv);
                base.UpdateLayout();
            }
        }

        public void SetItemExpanded(DataRowView drv)
        {
            if (drv != null)
            {
                drv.Row["ahtempcolIsExpanded"] = 1;
                this.ExpandedToRoot(drv.Row);
                base.UpdateLayout();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemExpanded(string where)
        {
            if (base.ItemsSource != null)
            {
                try
                {
                    DataView itemsSource = base.ItemsSource as DataView;
                    foreach (DataRow row in itemsSource.Table.Select(where))
                    {
                        row["ahtempcolIsExpanded"] = 1;
                        this.ExpandedToRoot(row);
                    }
                    base.UpdateLayout();
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemExpanded(string fieldName, string fieldValue)
        {
            if (base.ItemsSource != null)
            {
                try
                {
                    DataView itemsSource = base.ItemsSource as DataView;
                    string filterExpression = string.Format(CultureInfo.CurrentCulture, (itemsSource.Table.Columns[fieldName].DataType == typeof(string)) ? "{0}='{1}'" : "{0}={1}", new object[] { fieldName, fieldValue });
                    foreach (DataRow row in itemsSource.Table.Select(filterExpression))
                    {
                        row["ahtempcolIsExpanded"] = 1;
                        this.ExpandedToRoot(row);
                    }
                    base.UpdateLayout();
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemForeground(DataRowView drv, System.Windows.Media.Brush brush)
        {
            try
            {
                if ((drv != null) && (brush != null))
                {
                    int num = this.AddBrush(brush);
                    drv.Row["ahtempcolForeground"] = num;
                }
            }
            catch (Exception)
            {
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemForeground(string where, System.Windows.Media.Brush brush)
        {
            try
            {
                int num = this.AddBrush(brush);
                DataView itemsSource = base.ItemsSource as DataView;
                foreach (DataRow row in itemsSource.Table.Select(where))
                {
                    row["ahtempcolForeground"] = num;
                }
            }
            catch (Exception)
            {
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemForeground(string fieldName, string fieldValue, System.Windows.Media.Brush brush)
        {
            try
            {
                int num = this.AddBrush(brush);
                DataView itemsSource = base.ItemsSource as DataView;
                string filterExpression = object.Equals(itemsSource.Table.Columns[fieldName].DataType, typeof(string)) ? string.Format(CultureInfo.CurrentCulture, "{0}='{1}'", new object[] { fieldName, fieldValue }) : string.Format(CultureInfo.CurrentCulture, "{0}={1}", new object[] { fieldName, fieldValue });
                foreach (DataRow row in itemsSource.Table.Select(filterExpression))
                {
                    row["ahtempcolForeground"] = num;
                }
            }
            catch (Exception)
            {
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static")]
        public void SetItemSelected(DataRow drv)
        {
            if (drv != null)
            {
                try
                {
                    drv["ahtempcolIsSelected"] = 1;
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static")]
        public void SetItemSelected(DataRowView drv)
        {
            if (drv != null)
            {
                try
                {
                    drv.Row["ahtempcolIsSelected"] = 1;
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemSelected(string where)
        {
            if (base.ItemsSource != null)
            {
                try
                {
                    DataView itemsSource = base.ItemsSource as DataView;
                    itemsSource.Table.Select(where)[0]["ahtempcolIsSelected"] = 1;
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemSelected(DataRow drv, bool isExpandedToRoot)
        {
            if (drv != null)
            {
                try
                {
                    drv["ahtempcolIsSelected"] = 1;
                    if (isExpandedToRoot)
                    {
                        this.ExpandedToRoot(drv);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemSelected(DataRowView drv, bool isExpandedToRoot)
        {
            if (drv != null)
            {
                try
                {
                    drv.Row["ahtempcolIsSelected"] = 1;
                    if (isExpandedToRoot)
                    {
                        this.ExpandedToRoot(drv.Row);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemSelected(string where, bool isExpandedToRoot)
        {
            if (base.ItemsSource != null)
            {
                try
                {
                    DataView itemsSource = base.ItemsSource as DataView;
                    if (itemsSource != null)
                    {
                        DataRow[] rowArray = itemsSource.Table.Select(where);
                        rowArray[0]["ahtempcolIsSelected"] = 1;
                        if (isExpandedToRoot)
                        {
                            this.ExpandedToRoot(rowArray[0]);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemSelected(string fieldName, string fieldValue)
        {
            if (base.ItemsSource != null)
            {
                try
                {
                    DataView itemsSource = base.ItemsSource as DataView;
                    string filterExpression = string.Format(CultureInfo.CurrentCulture, (itemsSource.Table.Columns[fieldName].DataType == typeof(string)) ? "{0}='{1}'" : "{0}={1}", new object[] { fieldName, fieldValue });
                    itemsSource.Table.Select(filterExpression)[0]["ahtempcolIsSelected"] = 1;
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemSelected(string fieldName, string fieldValue, bool isExpandedToRoot)
        {
            if (base.ItemsSource != null)
            {
                try
                {
                    DataView itemsSource = base.ItemsSource as DataView;
                    string filterExpression = string.Format(CultureInfo.CurrentCulture, (itemsSource.Table.Columns[fieldName].DataType == typeof(string)) ? "{0}='{1}'" : "{0}={1}", new object[] { fieldName, fieldValue });
                    DataRow[] rowArray = itemsSource.Table.Select(filterExpression);
                    rowArray[0]["ahtempcolIsSelected"] = 1;
                    if (isExpandedToRoot)
                    {
                        this.ExpandedToRoot(rowArray[0]);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确"), SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemUnChecked(string field, string where)
        {
            if (base.ItemsSource != null)
            {
                try
                {
                    DataView itemsSource = base.ItemsSource as DataView;
                    foreach (DataRow row in itemsSource.Table.Select(where))
                    {
                        row[field] = 0;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确"), SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static"), SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public void SetItemUnSelected(DataRow drv)
        {
            if (drv != null)
            {
                try
                {
                    drv["ahtempcolIsSelected"] = 0;
                }
                catch (Exception)
                {
                }
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确"), SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static")]
        public void SetItemUnSelected(DataRowView drv)
        {
            if (drv != null)
            {
                try
                {
                    drv.Row["ahtempcolIsSelected"] = 0;
                }
                catch (Exception)
                {
                }
            }
        }

        public void SetTreeViewControl(params CtrlField[] controls)
        {
            this._FrameworkElementControls.Clear();
            if (controls != null)
            {
                foreach (CtrlField field in controls)
                {
                    switch (field.CtrlType)
                    {
                        case TreeViewControl.tvcTextBlock:
                            this.InitTextBlock(field);
                            break;

                        case TreeViewControl.tvcTextBox:
                            this.InitTextBox(field);
                            break;

                        case TreeViewControl.tvcTextBoxRightAlign:
                            this.InitTextBoxRightAlign(field);
                            break;

                        case TreeViewControl.tvcCheckBox:
                            this.InitCheckBox(field);
                            break;

                        case TreeViewControl.tvcCheckBoxThreeState:
                            this.InitCheckBoxThreeState(field);
                            break;

                        case TreeViewControl.tvcImage:
                            this.InitImage(field);
                            break;

                        case TreeViewControl.tvcRadioButton:
                            this.InitRadioButton(field);
                            break;
                    }
                }
                if (controls.Any<CtrlField>(delegate (CtrlField it) {
                    if (it.CtrlType != TreeViewControl.tvcCheckBox)
                    {
                        return it.CtrlType == TreeViewControl.tvcCheckBoxThreeState;
                    }
                    return true;
                }))
                {
                    base.ContextMenu = this.MenuSelect;
                }
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型")]
        protected void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ((this.TextBoxLostFocusEvent != null) && ((sender as TextBox).DataContext is DataRowView))
                //{
                //    DataRowView dataContext = (sender as TextBox).DataContext as DataRowView;
                //    TreeListViewEventArgs args = new TreeListViewEventArgs {
                //        CurDataRowView = dataContext,
                //        CurDataRow = dataContext.Row
                //    };
                //    this.TextBoxLostFocusEvent(sender, args);
                //}
            }
            catch (Exception exception)
            {
                throw new Exception("TextBoxLostFocusEventHandler事件异常。", exception);
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型")]
        protected void TextBoxValueChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //if (this.TextBoxValueChangedEvent != null)
                //{
                //    TextBox box = sender as TextBox;
                //    if (box != null)
                //    {
                //        DataRowView dataContext = box.DataContext as DataRowView;
                //        TreeListViewEventArgs args = new TreeListViewEventArgs {
                //            CurDataRowView = dataContext,
                //            CurDataRow = dataContext.Row
                //        };
                //        this.TextBoxValueChangedEvent(sender, args);
                //    }
                //}
            }
            catch (Exception exception)
            {
                throw new Exception("TextBoxValueChangedEventHandler事件异常。", exception);
            }
        }

        private void Tree_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((base.ContextMenu != null) && this.IsShowItemChildAction)
            {
                if (base.SelectedItem != null)
                {
                    if (!this.HasMenuItemChild())
                    {
                        this.AddMenuItem();
                    }
                }
                else
                {
                    this.ClearMenuItem();
                }
            }
        }

        private void TreeListAddRow(DataTable table, string filter, string[] primaryKeyFields)
        {
            List<object> list = new List<object>();
            DataRow[] rowArray = table.Select(filter);
            if (rowArray != null)
            {
                Func<string, object> selector = null;
                foreach (DataRow row in rowArray)
                {
                    list.Clear();
                    if (selector == null)
                    {
                        selector = str => row[str];
                    }
                    list.AddRange(primaryKeyFields.Select<string, object>(selector));
                    if ((base.ItemsSource as DataView).Table.Rows.Find(list.ToArray()) == null)
                    {
                        (base.ItemsSource as DataView).Table.ImportRow(row);
                    }
                    this.TreeListAddRow(table, string.Format(CultureInfo.CurrentCulture, "{0} = '{1}'", new object[] { this.FIDField, row[this.IDField] }), primaryKeyFields);
                }
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores 标识符不应包含下划线"), SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型"), SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        protected void tvcImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                try
                {
                    //if (this.tvcImageMouseLeftButtonUpEvent != null)
                    //{
                    //    System.Windows.Controls.Image image = sender as System.Windows.Controls.Image;
                    //    if ((image != null) && (image.DataContext != null))
                    //    {
                    //        DataRowView dataContext = image.DataContext as DataRowView;
                    //        if (dataContext != null)
                    //        {
                    //            DataRow row = dataContext.Row;
                    //            TreeListViewEventArgs args = new TreeListViewEventArgs {
                    //                CurDataRow = row
                    //            };
                    //            this.tvcImageMouseLeftButtonUpEvent(sender, args);
                    //        }
                    //    }
                    //}
                }
                catch (Exception exception)
                {
                    throw new Exception("Image MouseLeftButtonUp事件异常。", exception);
                }
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static")]
        public bool UpdateRow(DataRow drOld, DataRow drNew)
        {
            return this.UpdateRow(drOld, drNew, true);
        }

        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public bool UpdateRow(DataView dvOld, DataTable dtNew)
        {
            if ((dvOld == null) || (dtNew == null))
            {
                return false;
            }
            if (string.IsNullOrEmpty(this.IDField))
            {
                throw new ArgumentNullException("AHTreeView 的 IDField 属性不能为空。");
            }
            bool isSelected = true;
            string format = (dtNew.Columns[this.IDField].DataType == Type.GetType("System.String")) ? "{0}='{1}'" : "{0}={1}";
            foreach (DataRowView view in dvOld)
            {
                DataRow[] rowArray = dtNew.Select(string.Format(CultureInfo.CurrentCulture, format, new object[] { this.IDField, view[this.IDField] }));
                if (rowArray.Length > 0)
                {
                    this.UpdateRow(view.Row, rowArray[0], isSelected);
                    isSelected = false;
                }
            }
            return true;
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static")]
        public bool UpdateRow(DataRow drOld, DataRow drNew, bool isSelected)
        {
            if ((drOld == null) || (drNew == null))
            {
                return false;
            }
            foreach (DataColumn column in drNew.Table.Columns)
            {
                drOld[column.ColumnName] = drNew[column.ColumnName];
            }
            if (isSelected)
            {
                drOld["ahtempcolIsSelected"] = 1;
            }
            return true;
        }

        private void UpDownSearch(bool isDown)
        {
            if (this.currentRow != null)
            {
                if (isDown)
                {
                    this._FilterListViewRowsUp.Rows.Add(this._FilterListViewRowsUp.NewRow().ItemArray = this.currentRow.ItemArray);
                }
                else
                {
                    this._FilterListViewRowsDown.Rows.Add(this._FilterListViewRowsDown.NewRow().ItemArray = this.currentRow.ItemArray);
                }
            }
            if (isDown && (this._FilterListViewRowsDown.Rows.Count < 1))
            {
                this._FilterListViewRowsDown = this._FilterListViewRowsUp.Copy();
                this._FilterListViewRowsUp.Clear();
            }
            else if (!isDown && (this._FilterListViewRowsUp.Rows.Count < 1))
            {
                this._FilterListViewRowsUp = this._FilterListViewRowsDown.Copy();
                this._FilterListViewRowsDown.Clear();
            }
            DataRelation dataRealation = null;
            if ((base.ItemsSource as DataView).Table.ChildRelations.Count > 0)
            {
                dataRealation = (base.ItemsSource as DataView).Table.ChildRelations[0];
            }
            DataRow row = this.ExpandAndSelectDataRowView((base.ItemsSource as DataView).ToTable().Select("1=1"), dataRealation, isDown);
            if (this.currentRow == null)
            {
                this.currentRow = this._FilterListViewRowsDown.NewRow();
            }
            if (row != null)
            {
                this.currentRow.ItemArray = row.ItemArray;
                if (row != null)
                {
                    this.SetItemSelected(this.GetRow((base.ItemsSource as DataView).Table, this.IDField, row[this.IDField].ToString()));
                    this.SetItemExpanded(this.GetRow((base.ItemsSource as DataView).Table, this.IDField, row[this.IDField].ToString()));
                    if (isDown)
                    {
                        this._FilterListViewRowsDown.Rows.Remove(row);
                    }
                    else
                    {
                        this._FilterListViewRowsUp.Rows.Remove(row);
                    }
                }
            }
        }

        private void UpSearch(object sender, ExecutedRoutedEventArgs e)
        {
            this.UpDownSearch(false);
        }

        public void WatermarkTextBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Filter = string.Empty;
        }

        [Category("数据"), SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static"), Description("控制展开的内置DataColumn"), SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores 标识符不应包含下划线"), SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        public string AhTempColName_Expanded
        {
            get
            {
                return "ahtempcolIsExpanded";
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static"), SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确"), Category("数据"), Description("控制颜色的字段"), SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores 标识符不应包含下划线")]
        public string AhTempColName_Foreground
        {
            get
            {
                return "ahtempcolForeground";
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确"), SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores 标识符不应包含下划线"), Description("控制选中的内置DataColumn"), Category("数据"), SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static")]
        public string AhTempColName_Selected
        {
            get
            {
                return "ahtempcolIsSelected";
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<System.Windows.Media.Brush> BrushList
        {
            get
            {
                return this._brushList;
            }
            set
            {
                this._brushList = value;
            }
        }

        [Description("checkboxID"), Category("数据")]
        public string CheckId { get; set; }

        public int ControlCount
        {
            get
            {
                return this._controlCount;
            }
            internal set
            {
                this._controlCount = value;
            }
        }

        [Category("数据"), Description("默认模板显示绑定的字段(仅用于treeview)")]
        public string DisplayField { get; set; }

        [Description("是否显示过滤检索框"), Category("过滤")]
        public bool DisplayFilter
        {
            get
            {
                return (bool) base.GetValue(DisplayFilterProperty);
            }
            set
            {
                base.SetValue(DisplayFilterProperty, value);
            }
        }

        [Description("FID字段名"), SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确"), Category("数据")]
        public string FIDField { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确"), Description("FID值（fid为此值的记录为根节点，默认为null；如果设置此值，则控件内部会修改数据集，将fid为该值的设为null）"), Category("数据")]
        public object FIDValue { get; set; }

        [Description("过滤关键字"), Category("过滤")]
        public string Filter
        {
            get
            {
                return (string) base.GetValue(FilterProperty);
            }
            set
            {
                base.SetValue(FilterProperty, value);
            }
        }

        [Description("过滤条件背景颜色，默认为#bfdbff"), Category("过滤")]
        public System.Windows.Media.Color FilterBackground
        {
            get
            {
                return (System.Windows.Media.Color) base.GetValue(FilterBackgroundProperty);
            }
            set
            {
                base.SetValue(FilterBackgroundProperty, value);
            }
        }

        [Description("过滤表达式，比如：ParentItem Like '%{0}%'"), Category("过滤")]
        public string FilterExpression
        {
            get
            {
                return (string) base.GetValue(FilterExpressionProperty);
            }
            set
            {
                base.SetValue(FilterExpressionProperty, value);
            }
        }

        [Category("过滤"), Description("多层级过滤关键字分隔符，默认为#")]
        public string FilterLevelSeperate
        {
            get
            {
                return (string) base.GetValue(FilterLevelSeperateProperty);
            }
            set
            {
                base.SetValue(FilterLevelSeperateProperty, value);
            }
        }

        [Category("过滤"), Description("过滤行的背景颜色，默认为#dadfe7")]
        public System.Windows.Media.Color FilterRowColor
        {
            get
            {
                return (System.Windows.Media.Color) base.GetValue(FilterRowColorProperty);
            }
            set
            {
                base.SetValue(FilterRowColorProperty, value);
            }
        }

        [Description("是否过滤敏感字符"), Category("过滤")]
        public FilterType FilterType
        {
            get
            {
                return (FilterType)base.GetValue(FilterTypeProperty);
            }
            set
            {
                base.SetValue(FilterTypeProperty, value);
            }
        }

        [Description("输入提示"), Category("过滤")]
        public string FilterWatermark
        {
            get
            {
                return (string) base.GetValue(FilterWatermarkProperty);
            }
            set
            {
                base.SetValue(FilterWatermarkProperty, value);
            }
        }

        private string ForegroundField { get; set; }

        [Category("数据"), SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), Description("得到勾选行的ID集合")]
        public string GetCheckIds
        {
            get
            {
                System.Func<string, DataRow, string> func = null;
                try
                {
                    DataRow[] checkedRows = this.GetCheckedRows();
                    string seed = string.Empty;
                    if (checkedRows.Length > 0)
                    {
                        if (func == null)
                        {
                            func = (current, dr) => string.Format(CultureInfo.CurrentCulture, "{0}{1},", new object[] { current, dr[this.IDField] });
                        }
                        return checkedRows.Aggregate<DataRow, string>(seed, func).Trim(new char[] { ',' });
                    }
                }
                catch
                {
                    return "";
                }
                return "";
            }
        }

        private DataTemplate HirDataTemplate
        {
            get
            {
                HierarchicalDataTemplate template = new HierarchicalDataTemplate();
                Binding binding = new Binding {
                    Path = new PropertyPath(this.RelParentChild, new object[0])
                };
                template.ItemsSource = binding;
                DataTemplate template2 = new DataTemplate();
                if (this._FrameworkElementControls.Count > 0)
                {
                    FrameworkElementFactory factory = new FrameworkElementFactory(typeof(StackPanel), "myStackPanel");
                    factory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
                    foreach (FrameworkElementFactory factory2 in this._FrameworkElementControls)
                    {
                        factory.AppendChild(factory2);
                    }
                    template2.VisualTree = factory;
                }
                else
                {
                    FrameworkElementFactory factory3 = new FrameworkElementFactory(typeof(TextBlock), "myCtrlTextBlock");
                    Binding binding2 = new Binding(this.DisplayField) {
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
                    factory3.SetBinding(TextBlock.TextProperty, binding2);
                    template2.VisualTree = factory3;
                }
                template.VisualTree = template2.VisualTree;
                if ((this.IDField != null) && (this.FIDField != null))
                {
                    return template;
                }
                return template2;
            }
        }

        [Category("数据"), Description("ID字段名"), SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        public string IDField { get; set; }

        [Category("图片设置"), Description("图片默认高度设置"), SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型")]
        public double ImageHeight
        {
            get
            {
                return this._imageHeight;
            }
            set
            {
                if (value <= 0.0)
                {
                    throw new Exception("图片高度要大于零");
                }
                if (this._imageHeight != value)
                {
                    this._imageHeight = value;
                }
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public List<System.Windows.Media.ImageSource> ImageSource
        {
            get
            {
                return this._imageSource;
            }
            set
            {
                this._imageSource = value;
            }
        }

        [Description("图片默认宽度设置"), SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes 不要引发保留的异常类型"), Category("图片设置")]
        public double ImageWidth
        {
            get
            {
                return this._imageWidth;
            }
            set
            {
                if (value <= 0.0)
                {
                    throw new Exception("图片宽度要大于零");
                }
                if (this._imageWidth != value)
                {
                    this._imageWidth = value;
                }
            }
        }

        [Description("默认选择哪个检索按钮"), Category("过滤")]
        public bool IsDownSearch
        {
            get
            {
                return (bool) base.GetValue(IsDownSearchProperty);
            }
            set
            {
                base.SetValue(IsDownSearchProperty, value);
            }
        }

        [Category("节点子项"), Description("设置子节点状态时，展开当前节点；默认为false不展开")]
        public bool IsExpandCurrentItem
        {
            get
            {
                return (bool) base.GetValue(IsExpandCurrentItemProperty);
            }
            set
            {
                base.SetValue(IsExpandCurrentItemProperty, value);
            }
        }

        [Description("设置子节点状态时，展开子节点；默认为false不展开"), Category("节点子项")]
        public bool IsExpandItemChild
        {
            get
            {
                return (bool) base.GetValue(IsExpandItemChildProperty);
            }
            set
            {
                base.SetValue(IsExpandItemChildProperty, value);
            }
        }

        [Category("过滤"), Description("是否启用了拼音检索")]
        public bool IsPinYinSearch
        {
            get
            {
                return !string.IsNullOrEmpty(this.PinYinField);
            }
        }

        [Description("是否默认选中第一行"), Category("数据")]
        public bool IsSelectFirstRow
        {
            get
            {
                return this._isSelectFirstRow;
            }
            set
            {
                this._isSelectFirstRow = value;
            }
        }

        [Description("是否显示对子节点操作的菜单项；默认为true显示。只有当CheckId不为空时，该属性有意义。"), Category("节点子项")]
        public bool IsShowItemChildAction
        {
            get
            {
                return (bool) base.GetValue(IsShowItemChildActionProperty);
            }
            set
            {
                base.SetValue(IsShowItemChildActionProperty, value);
            }
        }

        [Category("节点子项"), Description("取消 子菜单标题；如果为空，则不显示。默认值：取消子节点")]
        public string ItemChildCancelSelectHeader
        {
            get
            {
                return (string) base.GetValue(ItemChildCancelSelectHeaderProperty);
            }
            set
            {
                base.SetValue(ItemChildCancelSelectHeaderProperty, value);
            }
        }

        [Description("展开子节点 右键菜单标题；如果为空，则不显示。默认值：展开子节点"), Category("节点子项")]
        public string ItemChildExpandHeader
        {
            get
            {
                return (string) base.GetValue(ItemChildExpandHeaderProperty);
            }
            set
            {
                base.SetValue(ItemChildExpandHeaderProperty, value);
            }
        }

        [Category("节点子项"), Description("全选 子菜单标题；如果为空，则不显示。默认值：全选子节点")]
        public string ItemChildSelectHeader
        {
            get
            {
                return (string) base.GetValue(ItemChildSelectHeaderProperty);
            }
            set
            {
                base.SetValue(ItemChildSelectHeaderProperty, value);
            }
        }

        [Category("节点子项"), Description("折叠子节点 右键菜单标题；如果为空，则不显示。默认值：折叠子节点")]
        public string ItemChildUnExpandHeader
        {
            get
            {
                return (string) base.GetValue(ItemChildUnExpandHeaderProperty);
            }
            set
            {
                base.SetValue(ItemChildUnExpandHeaderProperty, value);
            }
        }

        [Description("反选 子菜单标题；如果为空，则不显示。默认值：反选子节点"), Category("节点子项")]
        public string ItemChildUnSelectHeader
        {
            get
            {
                return (string) base.GetValue(ItemChildUnSelectHeaderProperty);
            }
            set
            {
                base.SetValue(ItemChildUnSelectHeaderProperty, value);
            }
        }

        public ContextMenu MenuSelect
        {
            get
            {
                if ((this._MenuSelect == null) && !string.IsNullOrEmpty(this.CheckId))
                {
                    this._MenuSelect = this.CreateMenu();
                }
                return this._MenuSelect;
            }
            set
            {
                this._MenuSelect = value;
            }
        }

        [Category("过滤"), Description("拼音检索扩展字段"), SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static")]
        public string PinYineFieldExt
        {
            get
            {
                return "PinYinExt";
            }
        }

        [Description("要进行拼音检索字段，比如：Name"), Category("过滤")]
        public string PinYinField
        {
            get
            {
                return (string) base.GetValue(PinYinFieldProperty);
            }
            set
            {
                base.SetValue(PinYinFieldProperty, value);
            }
        }

        [Category("过滤"), Description("拼音检索与其他关系：or 或者 and")]
        public string PinYinSearchRelation
        {
            get
            {
                return (string) base.GetValue(PinYinSearchRelationProperty);
            }
            set
            {
                base.SetValue(PinYinSearchRelationProperty, value);
            }
        }

        [Description("拼音检索类型：true按汉字拼音首字母检索，false按所有拼音检索"), Category("过滤")]
        public bool PinYinSearchType
        {
            get
            {
                return (bool) base.GetValue(PinYinSearchTypeProperty);
            }
            set
            {
                base.SetValue(PinYinSearchTypeProperty, value);
            }
        }

        [Description("树结构的父子关系名称"), SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic 将成员标记为 static"), Category("数据")]
        public string RelParentChild
        {
            get
            {
                return "RelParentChild";
            }
        }

        [Category("数据"), Description("当前选中item的DataRow"), SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public DataRow SelectedRow
        {
            get
            {
                try
                {
                    if (base.SelectedItem != null)
                    {
                        return (base.SelectedItem as DataRowView).Row;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                return null;
            }
        }

        [Category("数据"), Description("当前选中item的DataRowView"), SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public DataRowView SelectedRowView
        {
            get
            {
                try
                {
                    if (base.SelectedItem != null)
                    {
                        return (base.SelectedItem as DataRowView);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
                return null;
            }
        }

        [Description("设置选择行-第一行被选择"), Category("过滤")]
        public bool SetFilterFirstRowSelected
        {
            get
            {
                return (bool) base.GetValue(SetFilterFirstRowSelectedProperty);
            }
            set
            {
                base.SetValue(SetFilterFirstRowSelectedProperty, value);
            }
        }

        [Category("过滤"), Description("设置过滤行背景颜色，默认为false")]
        public bool SetFilterRowColor
        {
            get
            {
                return (bool) base.GetValue(SetFilterRowColorProperty);
            }
            set
            {
                base.SetValue(SetFilterRowColorProperty, value);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances 使用泛型事件处理程序实例"), SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible 嵌套类型不应是可见的")]
        public delegate void CheckBoxCheckedEventHandler(object sender, TreeListViewEventArgs e);

        [SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances 使用泛型事件处理程序实例"), SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible 嵌套类型不应是可见的")]
        public delegate void CheckBoxThreeStateCheckedEventHandler(object sender, TreeListViewEventArgs e);

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible 嵌套类型不应是可见的"), SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances 使用泛型事件处理程序实例")]
        public delegate void CheckBoxThreeStateClickEventHandler(object sender, TreeListViewEventArgs e);

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible 嵌套类型不应是可见的"), SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances 使用泛型事件处理程序实例"), SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确")]
        public delegate void CheckBoxThreeStateUnCheckedEventHandler(object sender, TreeListViewEventArgs e);

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible 嵌套类型不应是可见的"), SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确"), SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances 使用泛型事件处理程序实例")]
        public delegate void CheckBoxUnCheckedEventHandler(object sender, TreeListViewEventArgs e);

        [SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances 使用泛型事件处理程序实例"), SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible 嵌套类型不应是可见的")]
        public delegate void ComboBoxSelectionChangedEventHandler(object sender, TreeListViewEventArgs e);

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible 嵌套类型不应是可见的"), SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances 使用泛型事件处理程序实例")]
        public delegate void MouseLeftButtonUpHandler(object sender, TreeListViewEventArgs e);

        [SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances 使用泛型事件处理程序实例"), SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible 嵌套类型不应是可见的")]
        public delegate void RadioButtonCheckedEventHandler(object sender, TreeListViewEventArgs e);

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly 标识符的大小写应当正确"), SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible 嵌套类型不应是可见的"), SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances 使用泛型事件处理程序实例")]
        public delegate void RadioButtonUnCheckedEventHandler(object sender, TreeListViewEventArgs e);

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible 嵌套类型不应是可见的"), SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances 使用泛型事件处理程序实例")]
        public delegate void TextBoxLostFocusEventHandler(object sender, TreeListViewEventArgs e);

        [SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances 使用泛型事件处理程序实例"), SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible 嵌套类型不应是可见的")]
        public delegate void TextBoxValueChangedEventHandler(object sender, TreeListViewEventArgs e);
        private DataTable dtOld;
        [Description("获取数据源(去掉了AhTempColName_Foreground,AhTempColName_Selected,AhTempColName_Expanded三列)")]
        public DataTable GetItemSource()
        {
            DataTable dt = ((DataView)this.ItemsSource).Table;
            dtOld = dt.Copy();
            if (dt.Columns.Contains(this.AhTempColName_Foreground))
            {
                dt.Columns.Remove(this.AhTempColName_Foreground);
            }
            if (dt.Columns.Contains(this.AhTempColName_Selected))
            {
                dt.Columns.Remove(this.AhTempColName_Selected);
            }
            if (dt.Columns.Contains(this.AhTempColName_Expanded))
            {
                dt.Columns.Remove(this.AhTempColName_Expanded);
            }
            return dt;
        }

        [Description("恢复数据源(含有AhTempColName_Foreground,AhTempColName_Selected,AhTempColName_Expanded三列)")]
        public DataTable RecoverItemSource()
        {
            DataTable dt = ((DataView)this.ItemsSource).Table;
            Style style = new Style(typeof(TreeViewItem));
            if (!dt.Columns.Contains("ahtempcolIsExpanded"))
            {
                DataColumn column = null;
                try
                {
                    column = new DataColumn("ahtempcolIsExpanded", typeof(short))
                    {
                        DefaultValue = 0,
                        AllowDBNull = true
                    };
                    dt.Columns.Add(column);
                    Binding binding = new Binding("ahtempcolIsExpanded")
                    {
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
                    style.Setters.Add(new Setter(TreeViewItem.IsExpandedProperty, binding));
                }
                catch (Exception)
                {
                    if (column != null)
                    {
                        column.Dispose();
                    }
                }
            }
            else
            {
                Binding binding2 = new Binding("ahtempcolIsExpanded")
                {
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                style.Setters.Add(new Setter(TreeViewItem.IsExpandedProperty, binding2));
            }
            if (!dt.Columns.Contains("ahtempcolIsSelected"))
            {
                DataColumn column2 = null;
                try
                {
                    column2 = new DataColumn("ahtempcolIsSelected", typeof(short))
                    {
                        DefaultValue = 0,
                        AllowDBNull = true
                    };
                    dt.Columns.Add(column2);
                    Binding binding3 = new Binding("ahtempcolIsSelected")
                    {
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
                    style.Setters.Add(new Setter(TreeViewItem.IsSelectedProperty, binding3));
                    if (column2 != null)
                    {
                        column2.Dispose();
                    }
                }
                catch (Exception)
                {
                    if (column2 != null)
                    {
                        column2.Dispose();
                    }
                }
            }
            if (!dt.Columns.Contains("ahtempcolForeground"))
            {
                DataColumn column3 = null;
                try
                {
                    column3 = new DataColumn("ahtempcolForeground", typeof(short))
                    {
                        DefaultValue = 0,
                        AllowDBNull = true
                    };
                    dt.Columns.Add(column3);
                    Binding binding4 = new Binding("ahtempcolForeground")
                    {
                        Mode = BindingMode.TwoWay,
                        Converter = new BrushIndexToBrush(),
                        ConverterParameter = this._brushList,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
                    style.Setters.Add(new Setter(Control.ForegroundProperty, binding4));
                }
                catch (Exception)
                {
                    if (column3 != null)
                    {
                        column3.Dispose();
                    }
                }
            }
            if (dt.Rows.Count == dtOld.Rows.Count)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i][AhTempColName_Foreground] = dtOld.Rows[i][AhTempColName_Foreground];
                    dt.Rows[i][AhTempColName_Selected] = dtOld.Rows[i][AhTempColName_Selected];
                    dt.Rows[i][AhTempColName_Expanded] = dtOld.Rows[i][AhTempColName_Expanded];
                }
            }
            return dt;
        }

        #region 拖动节点
        [Category("数据"), Description("节点是否允许拖拽(仅用于treeview)")]
        public bool IsItemsAllowDrag { get; set; }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (IsItemsAllowDrag)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DataRowView selectedItem = (DataRowView)this.SelectedItem;
                    targetDrv = null;
                    if ((selectedItem != null))
                    {
                        TreeViewItem container = FindTreeViewItem(this, selectedItem);
                        if (container != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(container, selectedItem, DragDropEffects.Move);
                            if ((finalDropEffect == DragDropEffects.Move) && (targetDrv != null))
                            {
                                selectedItem[this.FIDField] = targetDrv[this.IDField];
                            }
                        }
                    }
                }
            }
        }
        /// <summary>拖动的目标节点 </summary>
        private DataRowView targetDrv;

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            e.Effects = DragDropEffects.None;
            e.Handled = true;
            TreeViewItem container = GetNearestContainer(e.OriginalSource as UIElement);
            if (container != null)
            {
                DataRowView source = (DataRowView)e.Data.GetData(typeof(DataRowView));
                DataRowView target = (DataRowView)container.Header;
                if ((source != null) && (target != null))
                {
                    if (!string.Equals(source[this.IDField].ToString(), target[this.IDField].ToString()))
                    {
                        targetDrv = target;
                        e.Effects = DragDropEffects.Move;
                    }
                }
            }
        }
        /// <summary>
        /// 获取选中节点
        /// </summary>
        /// <param name="item">表示一个可用于呈现项的集合的控件</param>
        /// <param name="data">选中行数据</param>
        /// <returns>TreeViewItem</returns>
        private TreeViewItem FindTreeViewItem(ItemsControl item, DataRowView data)
        {
            TreeViewItem findItem = null;
            bool itemIsExpand = false;
            if (item is TreeViewItem)
            {
                TreeViewItem tviCurrent = item as TreeViewItem;
                itemIsExpand = tviCurrent.IsExpanded;
                if (!tviCurrent.IsExpanded)
                {
                    //如果这个TreeViewItem未展开过，则不能通过ItemContainerGenerator来获得TreeViewItem
                    tviCurrent.SetValue(TreeViewItem.IsExpandedProperty, true);
                    //必须使用UpdaeLayour才能获取到TreeViewItem
                    tviCurrent.UpdateLayout();
                }
            }
            for (int i = 0; i < item.Items.Count; i++)
            {
                TreeViewItem tvItem = (TreeViewItem)item.ItemContainerGenerator.ContainerFromIndex(i);
                if (tvItem == null)
                    continue;
                object itemData = item.Items[i];
                DataRow dr = ((DataRowView)itemData).Row;
                if (dr[this.IDField].ToString() == data[this.IDField].ToString())
                {
                    findItem = tvItem;
                    break;
                }
                else if (tvItem.Items.Count > 0)
                {
                    findItem = FindTreeViewItem(tvItem, data);
                    if (findItem != null)
                        break;
                }
            }
            if (findItem == null)
            {
                TreeViewItem tviCurrent = item as TreeViewItem;
                tviCurrent.SetValue(TreeViewItem.IsExpandedProperty, itemIsExpand);
                tviCurrent.UpdateLayout();
            }
            return findItem;
        }

        /// <summary>
        /// 获取最近的节点
        /// </summary>
        /// <param name="element"></param>
        /// <returns>TreeViewItem</returns>
        private TreeViewItem GetNearestContainer(UIElement element)
        {
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }
        #endregion
    }
}
