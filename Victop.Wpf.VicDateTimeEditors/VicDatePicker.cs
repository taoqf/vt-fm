namespace Victop.Wpf.VicDateTimeEditors
{
    using Vic.WPF;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    [TemplateVisualState(Name="Focused", GroupName="FocusStates"), TemplateVisualState(Name="Disabled", GroupName="CommonStates"), TemplateVisualState(Name="Normal", GroupName="CommonStates"), TemplateVisualState(Name="InvalidUnfocused", GroupName="ValidationStatesStates"), TemplateVisualState(Name="Unfocused", GroupName="FocusStates"), TemplateVisualState(Name="Valid", GroupName="ValidationStatesStates"), StyleTypedProperty(Property="ValidationDecoratorStyle", StyleTargetType=typeof(VicValidationDecorator)), TemplateVisualState(Name="InvalidFocused", GroupName="ValidationStatesStates"), LicenseProvider, TemplatePart(Name="Button", Type=typeof(Button)), StyleTypedProperty(Property="CalendarStyle", StyleTargetType=typeof(System.Windows.Controls.Calendar)), TemplatePart(Name="TextBox", Type=typeof(TextBoxBase)), TemplatePart(Name="Root", Type=typeof(FrameworkElement)), TemplatePart(Name="Popup", Type=typeof(Popup)), TemplateVisualState(Name="MouseOver", GroupName="CommonStates")]
    public class VicDatePicker : Control
    {
        private System.Windows.Controls.Calendar _calendar;
        private string _defaultText;
        internal Button _elementButton;
        internal Popup _elementPopup;
        internal FrameworkElement _elementRoot;
        internal TextBoxBase _elementTextBox;
        internal bool _isLoaded;
        private TextBoxBase _maskedBox;
        private DateTime? _onOpenSelectedDate;
        private bool _settingSelectedDate;
        private bool _throwIsMouseOverChanged;
        private bool _typing;
        public static readonly DependencyProperty AllowNullProperty = DependencyProperty.Register("AllowNull", typeof(bool), typeof(VicDatePicker), new PropertyMetadata(false, new PropertyChangedCallback(VicDatePicker.OnAllowNullPropertyChanged)));
        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register("ButtonBackground", typeof(Brush), typeof(VicDatePicker), null);
        internal const string ButtonElementName = "Button";
        public static readonly DependencyProperty ButtonForegroundProperty = DependencyProperty.Register("ButtonForeground", typeof(Brush), typeof(VicDatePicker), null);
        public static readonly DependencyProperty CalendarStyleProperty = DependencyProperty.Register("CalendarStyle", typeof(Style), typeof(VicDatePicker), new PropertyMetadata(new PropertyChangedCallback(VicDatePicker.OnCalendarStylePropertyChanged)));
        public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(VicDatePicker), null);
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(System.Windows.CornerRadius), typeof(VicDatePicker), null);
        public static readonly DependencyProperty CustomFormatProperty = DependencyProperty.Register("CustomFormat", typeof(string), typeof(VicDatePicker), new PropertyMetadata(new PropertyChangedCallback(VicDatePicker.OnCustomFormatPropertyChanged)));
        public static readonly DependencyProperty DisabledCuesVisibilityProperty = DependencyProperty.Register("DisabledCuesVisibility", typeof(Visibility), typeof(VicDatePicker), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty DisplayDateEndProperty = DependencyProperty.Register("DisplayDateEnd", typeof(DateTime?), typeof(VicDatePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(VicDatePicker.OnDisplayDateEndChanged)));
        public static readonly DependencyProperty DisplayDateProperty = DependencyProperty.Register("DisplayDate", typeof(DateTime), typeof(VicDatePicker), new FrameworkPropertyMetadata(DateTime.Today, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(VicDatePicker.OnDisplayDateChanged)));
        public static readonly DependencyProperty DisplayDateStartProperty = DependencyProperty.Register("DisplayDateStart", typeof(DateTime?), typeof(VicDatePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(VicDatePicker.OnDisplayDateStartChanged)));
        public static readonly DependencyProperty FirstDayOfWeekProperty = DependencyProperty.Register("FirstDayOfWeek", typeof(DayOfWeek), typeof(VicDatePicker), new PropertyMetadata(new PropertyChangedCallback(VicDatePicker.OnFirstDayOfWeekPropertyChanged)));
        public static readonly DependencyProperty FocusBrushProperty = DependencyProperty.Register("FocusBrush", typeof(Brush), typeof(VicDatePicker), null);
        public static readonly DependencyProperty FocusCuesVisibilityProperty = DependencyProperty.Register("FocusCuesVisibility", typeof(Visibility), typeof(VicDatePicker), new PropertyMetadata(Visibility.Visible));
        internal static readonly DependencyProperty ForceMouseOverProperty = DependencyProperty.Register("ForceMouseOver", typeof(bool), typeof(VicDatePicker), new PropertyMetadata(new PropertyChangedCallback(VicDatePicker.OnForceMouseOverPropertyChanged)));
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(VicDatePicker), new PropertyMetadata(false, new PropertyChangedCallback(VicDatePicker.OnIsDropDownOpenPropertyChanged)));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(VicDatePicker), new PropertyMetadata(new PropertyChangedCallback(VicDatePicker.OnIsReadOnlyPropertyChanged)));
        public static readonly DependencyProperty IsTodayHighlightedProperty = DependencyProperty.Register("IsTodayHighlighted", typeof(bool), typeof(VicDatePicker), new PropertyMetadata(true, new PropertyChangedCallback(VicDatePicker.OnIsTodayHighlightedPropertyChanged)));
        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register("Mask", typeof(string), typeof(VicDatePicker), new PropertyMetadata(string.Empty, new PropertyChangedCallback(VicDatePicker.OnMaskPropertyChanged)));
        public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.Register("MouseOverBrush", typeof(Brush), typeof(VicDatePicker), null);
        internal const string PopupElementName = "Popup";
        public static readonly DependencyProperty PressedBrushProperty = DependencyProperty.Register("PressedBrush", typeof(Brush), typeof(VicDatePicker), null);
        public static readonly DependencyProperty PromptProperty = DependencyProperty.Register("Prompt", typeof(string), typeof(VicDatePicker), new PropertyMetadata("_", new PropertyChangedCallback(VicDatePicker.OnPromptPropertyChanged)));
        internal const string RootElementName = "Root";
        public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged", RoutingStrategy.Direct, typeof(SelectionChangedEventHandler), typeof(VicDatePicker));
        public static readonly DependencyProperty SelectedDateFormatProperty = DependencyProperty.Register("SelectedDateFormat", typeof(VicDatePickerFormat), typeof(VicDatePicker), new PropertyMetadata(VicDatePickerFormat.Short, new PropertyChangedCallback(VicDatePicker.OnSelectedDateFormatPropertyChanged)));
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(VicDatePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(VicDatePicker.OnSelectedDateChanged)));
        public static readonly DependencyProperty SelectionBackgroundProperty = DependencyProperty.Register("SelectionBackground", typeof(Brush), typeof(VicDatePicker), null);
        public static readonly DependencyProperty SelectionForegroundProperty = DependencyProperty.Register("SelectionForeground", typeof(Brush), typeof(VicDatePicker), null);
        internal const string TextBoxElementName = "TextBox";
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(VicDatePicker), new PropertyMetadata(string.Empty, new PropertyChangedCallback(VicDatePicker.OnTextPropertyChanged)));
        public static readonly DependencyProperty ValidationDecoratorStyleProperty = DependencyProperty.Register("ValidationDecoratorStyle", typeof(Style), typeof(VicDatePicker), null);
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(VicDatePicker), new PropertyMetadata(new PropertyChangedCallback(VicDatePicker.OnWatermarkPropertyChanged)));

        public event RoutedEventHandler CalendarClosed;

        public event RoutedEventHandler CalendarOpened;

        public event EventHandler<DatePickerDateValidationErrorEventArgs> DateValidationError;

        public event EventHandler<PropertyChangedEventArgs<bool>> IsMouseOverChanged;

        public event SelectionChangedEventHandler SelectedDateChanged
        {
            add
            {
                base.AddHandler(SelectedDateChangedEvent, value);
            }
            remove
            {
                base.RemoveHandler(SelectedDateChangedEvent, value);
            }
        }

        static VicDatePicker()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VicDatePicker), new FrameworkPropertyMetadata(typeof(VicDatePicker)));
            UIElement.FocusableProperty.OverrideMetadata(typeof(VicDatePicker), new FrameworkPropertyMetadata(false));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(VicDatePicker), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
        }

        public VicDatePicker()
        {
            RoutedEventHandler handler3 = null;
            DependencyPropertyChangedEventHandler handler4 = null;
            RoutedEventHandler handler = null;
            DependencyPropertyChangedEventHandler handler2 = null;
            this._defaultText = string.Empty;
            this._throwIsMouseOverChanged = true;
            base.DefaultStyleKey = typeof(VicDatePicker);
            if (handler == null)
            {
                if (handler3 == null)
                {
                    handler3 = delegate (object param0, RoutedEventArgs param1) {
                        this.ChangeVisualStateCommon(false);
                        this.ChangeVisualStateFocus(false);
                    };
                }
                handler = handler3;
            }
            base.Loaded += handler;
            if (handler2 == null)
            {
                if (handler4 == null)
                {
                    handler4 = (s, a) => this.ChangeVisualStateCommon(true);
                }
                handler2 = handler4;
            }
            base.IsEnabledChanged += handler2;
            this.SetCustomDefaultValues();
        }

        private void Calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            if (e.AddedDate != this.DisplayDate)
            {
                base.SetValue(DisplayDateProperty, e.AddedDate.Value);
            }
        }

        private void Calendar_KeyDown(object sender, KeyEventArgs e)
        {
            System.Windows.Controls.Calendar calendar = sender as System.Windows.Controls.Calendar;
            if ((((e.Key == Key.Return) || (e.Key == Key.Space)) || (e.Key == Key.Escape)) && (calendar.DisplayMode == CalendarMode.Month))
            {
                this.ClosePopUp();
                if (e.Key == Key.Escape)
                {
                    this.SelectedDate = this._onOpenSelectedDate;
                }
            }
        }

        private void Calendar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Calendar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled)
            {
                CalendarDayButton originalSource = e.OriginalSource as CalendarDayButton;
                if (originalSource == null)
                {
                    originalSource = VTreeHelper.GetParentOfType(e.OriginalSource as DependencyObject, typeof(CalendarDayButton)) as CalendarDayButton;
                }
                if (originalSource != null)
                {
                    this.ClosePopUp();
                }
            }
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate;
            if (e.AddedItems.Count > 0)
            {
                selectedDate = this.SelectedDate;
            }
            if (selectedDate.HasValue && (DateTime.Compare((DateTime) e.AddedItems[0], (selectedDate = this.SelectedDate).Value) != 0))
            {
                this.SelectedDate = (DateTime?) e.AddedItems[0];
            }
            else if (e.AddedItems.Count == 0)
            {
                this.SelectedDate = null;
            }
            else if (!(this.SelectedDate.HasValue || (e.AddedItems.Count <= 0)))
            {
                this.SelectedDate = (DateTime?) e.AddedItems[0];
            }
        }

        protected void ChangeVisualStateCommon(bool useTransitions)
        {
            if (!base.IsEnabled)
            {
                VisualStateHelper.GoToState(this, "Disabled", useTransitions);
            }
            if (base.IsEnabled)
            {
                VisualStateHelper.GoToState(this, "MouseOver", useTransitions);
            }
            if (!((!base.IsEnabled || base.IsMouseOver) || this.ForceMouseOver))
            {
                VisualStateHelper.GoToState(this, "Normal", useTransitions);
            }
        }

        protected void ChangeVisualStateFocus(bool useTransitions)
        {
            if (!base.IsFocused)
            {
                VisualStateHelper.GoToState(this, "Unfocused", useTransitions);
            }
            if (base.IsFocused)
            {
                VisualStateHelper.GoToState(this, "Focused", useTransitions);
            }
        }

        private void ClosePopUp()
        {
            if (this.IsDropDownOpen)
            {
                this.IsDropDownOpen = false;
            }
        }

        private string DateTimeToString(DateTime d)
        {
            DateTimeFormatInfo currentDateTimeFormat = VicTimeEditor.GetCurrentDateTimeFormat();
            switch (this.SelectedDateFormat)
            {
                case VicDatePickerFormat.Long:
                    return string.Format(CultureInfo.CurrentCulture, d.ToString(currentDateTimeFormat.LongDatePattern, currentDateTimeFormat), new object[0]);

                case VicDatePickerFormat.Short:
                    return string.Format(CultureInfo.CurrentCulture, d.ToString(currentDateTimeFormat.ShortDatePattern, currentDateTimeFormat), new object[0]);

                case VicDatePickerFormat.Custom:
                    try
                    {
                        return d.ToString(this.CustomFormat, CultureInfo.CurrentCulture);
                    }
                    catch (FormatException)
                    {
                    }
                    break;
            }
            return null;
        }

        private void ElementText_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Handled)
            {
                Key key = e.Key;
                if (key != Key.Return)
                {
                    if (key != Key.Down)
                    {
                        if ((key == Key.System) && ((e.SystemKey == Key.Down) && ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)))
                        {
                            this.TogglePopUp();
                            e.Handled = true;
                        }
                    }
                    else if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
                    {
                        this.TogglePopUp();
                        e.Handled = true;
                    }
                }
                else
                {
                    this.SetSelectedDate();
                    e.Handled = true;
                }
            }
        }

        private void ElementText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this._elementTextBox != null)
            {
                if (!(this.IsReadOnly || string.IsNullOrEmpty(this._elementTextBox.Text)))
                {
                    VisualStateHelper.GoToState(this._elementTextBox, "Unwatermarked", false);
                }
                try
                {
                    this._typing = true;
                    base.SetValue(TextProperty, this._elementTextBox.Text);
                }
                finally
                {
                    this._typing = false;
                }
            }
        }

        public void FinalizeEditing()
        {
            if (this._elementTextBox != null)
            {
                this.Text = this._elementTextBox.Text;
                if (this.AllowNull && string.IsNullOrEmpty(this.Text))
                {
                    this.SelectedDate = null;
                }
                this.SetSelectedDate();
            }
        }

        private T GetTemplateChild<T>(string childName, bool required, ref string errors) where T: class
        {
            DependencyObject templateChild = base.GetTemplateChild(childName);
            ApplyTemplateHelper.VerifyTemplateChild(typeof(T), "template part", childName, templateChild, required, ref errors);
            return (templateChild as T);
        }

        private static Storyboard GetTemplateChildResource(FrameworkElement root, string resourceName, bool required, ref string errors)
        {
            return GetTemplateChildResource<Storyboard>(root, resourceName, required, ref errors);
        }

        private static T GetTemplateChildResource<T>(FrameworkElement root, string resourceName, bool required, ref string errors) where T: class
        {
            object child = root.Resources[resourceName];
            ApplyTemplateHelper.VerifyTemplateChild(typeof(T), "resource", resourceName, child, required, ref errors);
            return (child as T);
        }

        private void HandleIsReadOnlyProperty()
        {
            if (this._isLoaded)
            {
                if (this._elementButton != null)
                {
                    this._elementButton.IsEnabled = !this.IsReadOnly;
                }
                this._elementTextBox.IsReadOnly = this.IsReadOnly;
            }
        }

        private void InitializeButtonPart()
        {
            RoutedEventHandler handler = null;
            RoutedEventHandler handler2 = null;
            if (this._elementButton != null)
            {
                if (handler == null)
                {
                    if (handler2 == null)
                    {
                        handler2 = (s, a) => this.TogglePopUp();
                    }
                    handler = handler2;
                }
                this._elementButton.Click += handler;
                this._elementButton.IsTabStop = false;
                if (this._elementButton.Content == null)
                {
                    this._elementButton.Content = "Show Calendar";
                }
            }
        }

        private void InitializeCalendar()
        {
            this._calendar = new System.Windows.Controls.Calendar();
            this._calendar.DisplayDateChanged += new EventHandler<CalendarDateChangedEventArgs>(this.Calendar_DisplayDateChanged);
            this._calendar.SelectedDatesChanged += new EventHandler<SelectionChangedEventArgs>(this.Calendar_SelectedDatesChanged);
            this._calendar.MouseLeftButtonDown += new MouseButtonEventHandler(this.Calendar_MouseLeftButtonDown);
            this._calendar.AddHandler(UIElement.MouseLeftButtonUpEvent, new MouseButtonEventHandler(this.Calendar_MouseLeftButtonUp), true);
            this._calendar.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.Calendar_KeyDown), true);
            this._calendar.SelectionMode = CalendarSelectionMode.SingleDate;
            this._calendar.HorizontalAlignment = HorizontalAlignment.Left;
            this._calendar.VerticalAlignment = VerticalAlignment.Top;
            this._calendar.SnapsToDevicePixels = true;
            this._calendar.Margin = new Thickness(0.0, -3.0, 0.0, -3.0);
            this._calendar.IsTabStop = true;
            this.BlackoutDates = this._calendar.BlackoutDates;
        }

        private void InitializePopupPart()
        {
            EventHandler handler = null;
            EventHandler handler2 = null;
            if (this._elementPopup != null)
            {
                this._elementPopup.StaysOpen = false;
                this._elementPopup.Child = this._calendar;
                if (handler == null)
                {
                    if (handler2 == null)
                    {
                        handler2 = (EventHandler) ((s, a) => this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down)));
                    }
                    handler = handler2;
                }
                this._elementPopup.Closed += handler;
                this.OnIsDropDownOpenChanged(false);
            }
        }

        private void InitializeTextBoxPart()
        {
            RoutedEventHandler handler = null;
            RoutedEventHandler handler2 = null;
            RoutedEventHandler handler3 = null;
            RoutedEventHandler handler4 = null;
            this._maskedBox = this._elementTextBox as VicMaskedTextBox;
            if (this._elementTextBox != null)
            {
                if (this._elementTextBox._elementWatermark != null)
                {
                    this._elementTextBox._elementWatermark.Focusable = false;
                }
                this._elementTextBox.KeyDown += new KeyEventHandler(this.ElementText_KeyDown);
                this._elementTextBox.TextChanged += new TextChangedEventHandler(this.ElementText_TextChanged);
                if (!this.SelectedDate.HasValue)
                {
                    if (!string.IsNullOrEmpty(this._defaultText))
                    {
                        this._elementTextBox.Text = this._defaultText;
                        this.SetSelectedDate();
                    }
                }
                else
                {
                    this._elementTextBox.Text = this.DateTimeToString(this.SelectedDate.Value);
                }
                if (handler == null)
                {
                    if (handler3 == null)
                    {
                        handler3 = delegate (object param0, RoutedEventArgs param1) {
                            this.ClosePopUp();
                            base.Dispatcher.BeginInvoke(delegate {
                                if (!string.IsNullOrEmpty(this._elementTextBox.Text))
                                {
                                    this._elementTextBox.SelectionStart = 0;
                                    this._elementTextBox.Select(0, this._elementTextBox.Text.Length);
                                }
                            }, new object[0]);
                        };
                    }
                    handler = handler3;
                }
                this._elementTextBox.GotFocus += handler;
                if (handler2 == null)
                {
                    if (handler4 == null)
                    {
                        handler4 = (s, a) => this.FinalizeEditing();
                    }
                    handler2 = handler4;
                }
                this._elementTextBox.LostFocus += handler2;
            }
        }

        private bool IsEmpty()
        {
            if ((this._maskedBox == null) || string.IsNullOrEmpty(this._maskedBox.Mask))
            {
                return string.IsNullOrEmpty(this._elementTextBox.Text);
            }
            string str = this._maskedBox.Value;
            if (!string.IsNullOrEmpty(str))
            {
                foreach (char ch in str)
                {
                    if (ch != this.Prompt[0])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsValidDateSelection(object value)
        {
            return ((value == null) || ((!this.BlackoutDates.Contains((DateTime) value) && (DateTime.Compare((DateTime) value, this.DisplayDateStart.GetValueOrDefault(DateTime.MinValue)) >= 0)) && (DateTime.Compare((DateTime) value, this.DisplayDateEnd.GetValueOrDefault(DateTime.MaxValue)) <= 0)));
        }

        private static bool IsValidSelectedDateFormat(VicDatePickerFormat value)
        {
            if ((value != VicDatePickerFormat.Long) && (value != VicDatePickerFormat.Short))
            {
                return (value == VicDatePickerFormat.Custom);
            }
            return true;
        }

        private void OnAfterApplyTemplate()
        {
            this.HandleIsReadOnlyProperty();
            if (!this.SelectedDate.HasValue)
            {
                this.SetWaterMarkText();
            }
            this.OnMaskChanged(this.Mask);
            this.OnCustomFormatChangedImpl(this.CustomFormat);
            this.SetSelectedDate();
        }

        private void OnAllowNullChanged(bool oldValue)
        {
            if (!(this.AllowNull || this.SelectedDate.HasValue))
            {
                this.SetSelectedDate();
            }
        }

        private static void OnAllowNullPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker picker = d as VicDatePicker;
            bool oldValue = (bool) e.OldValue;
            picker.OnAllowNullChanged(oldValue);
        }

        public override void OnApplyTemplate()
        {
            this.OnBeforeApplyTemplate();
            string errors = string.Empty;
            base.OnApplyTemplate();
            this._isLoaded = true;
            this._elementTextBox = this.GetTemplateChild<TextBoxBase>("TextBox", true, ref errors);
            if (this._elementTextBox != null)
            {
                this.InitializeTextBoxPart();
            }
            this._elementRoot = this.GetTemplateChild<FrameworkElement>("Root", false, ref errors);
            this._elementButton = this.GetTemplateChild<Button>("Button", false, ref errors);
            if (this._elementButton != null)
            {
                this.InitializeButtonPart();
            }
            this._elementPopup = this.GetTemplateChild<Popup>("Popup", false, ref errors);
            if (this._elementPopup != null)
            {
                this.InitializePopupPart();
            }
            if (!string.IsNullOrEmpty(errors))
            {
                this._isLoaded = false;
                if (!DesignerProperties.GetIsInDesignMode(this))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Template cannot be applied to VicDatePicker.\nDetails: {0}", new object[] { errors }));
                }
            }
            else
            {
                this.ChangeVisualStateCommon(false);
                this.ChangeVisualStateFocus(false);
                this.OnAfterApplyTemplate();
            }
        }

        private void OnBeforeApplyTemplate()
        {
            if (this._elementPopup != null)
            {
                this._elementPopup.Child = null;
                AdornerDecorator parent = VisualTreeHelper.GetParent(this._calendar) as AdornerDecorator;
                if (parent != null)
                {
                    parent.Child = null;
                }
            }
            if (this._elementTextBox != null)
            {
                this._elementTextBox.KeyDown -= new KeyEventHandler(this.ElementText_KeyDown);
                this._elementTextBox.TextChanged -= new TextChangedEventHandler(this.ElementText_TextChanged);
            }
        }

        private void OnCalendarClosed(RoutedEventArgs e)
        {
            RoutedEventHandler calendarClosed = this.CalendarClosed;
            if (calendarClosed != null)
            {
                calendarClosed(this, e);
            }
        }

        private void OnCalendarOpened(RoutedEventArgs e)
        {
            RoutedEventHandler calendarOpened = this.CalendarOpened;
            if (calendarOpened != null)
            {
                calendarOpened(this, e);
            }
        }

        private void OnCalendarStyleChanged(Style oldValue)
        {
            if (((this.CalendarStyle != null) && (this._calendar != null)) && (this._calendar.Style == oldValue))
            {
                this._calendar.Style = this.CalendarStyle;
            }
        }

        private static void OnCalendarStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker picker = d as VicDatePicker;
            Style oldValue = (Style) e.OldValue;
            picker.OnCalendarStyleChanged(oldValue);
        }

        private void OnCustomFormatChanged(string oldValue)
        {
            base.Dispatcher.BeginInvoke(() => this.OnCustomFormatChangedImpl(oldValue), new object[0]);
        }

        private void OnCustomFormatChangedImpl(string oldValue)
        {
            if (!string.IsNullOrEmpty(this.CustomFormat))
            {
                this.SelectedDateFormat = VicDatePickerFormat.Custom;
            }
            if (this._elementTextBox != null)
            {
                if (!(this.SelectedDate.HasValue && !string.IsNullOrEmpty(this._elementTextBox.Text)))
                {
                    this.SetWaterMarkText();
                }
                else
                {
                    DateTime? selectedDate = this.SelectedDate;
                    DateTime? nullable2 = selectedDate.HasValue ? new DateTime?(selectedDate.GetValueOrDefault()) : this.ParseText(this._elementTextBox.Text);
                    if (nullable2.HasValue)
                    {
                        string str = this.DateTimeToString(nullable2.Value);
                        this.Text = str;
                    }
                }
            }
        }

        private static void OnCustomFormatPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker picker = d as VicDatePicker;
            string oldValue = (string) e.OldValue;
            picker.OnCustomFormatChanged(oldValue);
        }

        protected virtual void OnDateValidationError(DatePickerDateValidationErrorEventArgs e)
        {
            EventHandler<DatePickerDateValidationErrorEventArgs> dateValidationError = this.DateValidationError;
            if (dateValidationError != null)
            {
                dateValidationError(this, e);
            }
        }

        private void OnDisplayDateChanged(DependencyPropertyChangedEventArgs e)
        {
            DateTime displayDate = this._calendar.DisplayDate;
            DateTime newValue = (DateTime) e.NewValue;
            if ((((displayDate.Year - newValue.Year) * 12) + (displayDate.Month - newValue.Month)) != 0)
            {
                this._calendar.DisplayDate = this.DisplayDate;
                if (DateTime.Compare(this._calendar.DisplayDate, this.DisplayDate) != 0)
                {
                    this.DisplayDate = this._calendar.DisplayDate;
                }
            }
        }

        private static void OnDisplayDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((VicDatePicker) d).OnDisplayDateChanged(e);
        }

        private void OnDisplayDateEndChanged(DependencyPropertyChangedEventArgs e)
        {
            this._calendar.DisplayDateEnd = this.DisplayDateEnd;
            if ((this._calendar.DisplayDateEnd.HasValue && this.DisplayDateEnd.HasValue) && (DateTime.Compare(this._calendar.DisplayDateEnd.Value, this.DisplayDateEnd.Value) != 0))
            {
                this.DisplayDateEnd = this._calendar.DisplayDateEnd;
            }
        }

        private static void OnDisplayDateEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((VicDatePicker) d).OnDisplayDateEndChanged(e);
        }

        private void OnDisplayDateStartChanged(DependencyPropertyChangedEventArgs e)
        {
            this._calendar.DisplayDateStart = this.DisplayDateStart;
            if ((this._calendar.DisplayDateStart.HasValue && this.DisplayDateStart.HasValue) && (DateTime.Compare(this._calendar.DisplayDateStart.Value, this.DisplayDateStart.Value) != 0))
            {
                this.DisplayDateStart = this._calendar.DisplayDateStart;
            }
        }

        private static void OnDisplayDateStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((VicDatePicker) d).OnDisplayDateStartChanged(e);
        }

        private void OnFirstDayOfWeekChanged(DayOfWeek oldValue)
        {
            if (this._calendar != null)
            {
                this._calendar.FirstDayOfWeek = this.FirstDayOfWeek;
            }
        }

        private static void OnFirstDayOfWeekPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker picker = d as VicDatePicker;
            DayOfWeek oldValue = (DayOfWeek) e.OldValue;
            picker.OnFirstDayOfWeekChanged(oldValue);
        }

        private static void OnForceMouseOverPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VicDatePicker).ChangeVisualStateCommon(true);
        }

        private void OnIsDropDownOpenChanged(bool oldValue)
        {
            if (((this._elementPopup != null) && (this._elementPopup.Child != null)) && (this.IsDropDownOpen != oldValue))
            {
                if (this._calendar.DisplayMode != CalendarMode.Month)
                {
                    this._calendar.DisplayMode = CalendarMode.Month;
                }
                if (this.IsDropDownOpen)
                {
                    if (!this.IsReadOnly)
                    {
                        this._onOpenSelectedDate = this.SelectedDate;
                        this._elementPopup.IsOpen = true;
                        this._calendar.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                        this.OnCalendarOpened(new RoutedEventArgs());
                    }
                }
                else
                {
                    this._elementPopup.IsOpen = false;
                    this.OnCalendarClosed(new RoutedEventArgs());
                    this._elementTextBox.Focus();
                }
            }
        }

        private static void OnIsDropDownOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker picker = d as VicDatePicker;
            bool oldValue = (bool) e.OldValue;
            picker.OnIsDropDownOpenChanged(oldValue);
        }

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VicDatePicker).ChangeVisualStateFocus(true);
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!(!this.IsDropDownOpen || base.IsKeyboardFocusWithin))
            {
                this.ClosePopUp();
            }
            base.OnIsKeyboardFocusWithinChanged(e);
        }

        private static void OnIsMouseOverPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker sender = d as VicDatePicker;
            if ((sender.IsMouseOverChanged != null) && sender._throwIsMouseOverChanged)
            {
                PropertyChangedEventArgs<bool> args = new PropertyChangedEventArgs<bool> {
                    OldValue = (bool) e.OldValue,
                    NewValue = (bool) e.NewValue
                };
                sender.IsMouseOverChanged(sender, args);
            }
            sender.ChangeVisualStateCommon(true);
        }

        private void OnIsReadOnlyChanged(bool oldValue)
        {
            this.HandleIsReadOnlyProperty();
        }

        private static void OnIsReadOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker picker = d as VicDatePicker;
            bool oldValue = (bool) e.OldValue;
            picker.OnIsReadOnlyChanged(oldValue);
        }

        private void OnIsTodayHighlightedChanged(bool oldValue)
        {
            this._calendar.IsTodayHighlighted = this.IsTodayHighlighted;
        }

        private static void OnIsTodayHighlightedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker picker = d as VicDatePicker;
            bool oldValue = (bool) e.OldValue;
            picker.OnIsTodayHighlightedChanged(oldValue);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (!(!this.IsDropDownOpen || base.IsKeyboardFocusWithin))
            {
                this.ClosePopUp();
            }
            this.SetSelectedDate();
            base.OnLostFocus(e);
        }

        private void OnMaskChanged(string oldValue)
        {
            if (this._maskedBox != null)
            {
                this._maskedBox.Mask = this.Mask;
                if (!string.IsNullOrEmpty(this.Mask))
                {
                    this._maskedBox.PromptChar = this.Prompt[0];
                }
                if (this.SelectedDate.HasValue)
                {
                    this._maskedBox.Text = this.DateTimeToString(this.SelectedDate.Value);
                }
            }
        }

        private static void OnMaskPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker picker = d as VicDatePicker;
            string oldValue = (string) e.OldValue;
            picker.OnMaskChanged(oldValue);
        }

        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (base.IsKeyboardFocusWithin && ((!e.Handled && !this.IsReadOnly) && this.SelectedDate.HasValue))
            {
                DateTime time = this.SelectedDate.Value;
                DateTime? nullable = null;
                System.Globalization.Calendar calendar = new GregorianCalendar();
                try
                {
                    nullable = new DateTime?(calendar.AddDays(time, (e.Delta > 0) ? -1 : 1));
                }
                catch (ArgumentException)
                {
                }
                if (nullable.HasValue && this.IsValidDateSelection(nullable.Value))
                {
                    this.SelectedDate = nullable;
                    e.Handled = true;
                }
            }
        }

        private void OnPromptChanged(string oldValue)
        {
            this.OnMaskChanged(this.Mask);
        }

        private static void OnPromptPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker picker = d as VicDatePicker;
            string oldValue = (string) e.OldValue;
            picker.OnPromptChanged(oldValue);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == UIElement.IsMouseOverProperty)
            {
                OnIsMouseOverPropertyChanged(this, e);
            }
            else if (e.Property == UIElement.IsFocusedProperty)
            {
                OnIsFocusedPropertyChanged(this, e);
            }
        }

        protected virtual void OnSelectedDateChanged(SelectionChangedEventArgs e)
        {
            base.RaiseEvent(e);
        }

        private void OnSelectedDateChanged(DependencyPropertyChangedEventArgs e)
        {
            Collection<DateTime> addedItems = new Collection<DateTime>();
            Collection<DateTime> removedItems = new Collection<DateTime>();
            DateTime? newValue = (DateTime?) e.NewValue;
            DateTime? oldValue = (DateTime?) e.OldValue;
            if (newValue.HasValue)
            {
                addedItems.Add(newValue.Value);
            }
            if (oldValue.HasValue)
            {
                removedItems.Add(oldValue.Value);
            }
            if (newValue != this._calendar.SelectedDate)
            {
                this._calendar.SelectedDate = newValue;
            }
            if (!(this.SelectedDate.HasValue || this.AllowNull))
            {
                this.SelectedDate = new DateTime?(DateTime.Today);
            }
            if (this.SelectedDate.HasValue)
            {
                DateTime day = this.SelectedDate.Value;
                base.Dispatcher.BeginInvoke(delegate {
                    this._settingSelectedDate = true;
                    this.Text = this.DateTimeToString(day);
                    this._settingSelectedDate = false;
                    this.OnSelectedDateChanged(new SelectionChangedEventArgs(SelectedDateChangedEvent, removedItems, addedItems));
                }, new object[0]);
                if (!(((day.Month == this.DisplayDate.Month) && (day.Year == this.DisplayDate.Year)) || this.IsDropDownOpen))
                {
                    this.DisplayDate = day;
                }
            }
            else
            {
                this._settingSelectedDate = true;
                this.SetWaterMarkText();
                this._settingSelectedDate = false;
                this.OnSelectedDateChanged(new SelectionChangedEventArgs(SelectedDateChangedEvent, removedItems, addedItems));
            }
        }

        private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((VicDatePicker) d).OnSelectedDateChanged(e);
        }

        private void OnSelectedDateFormatChanged(VicDatePickerFormat oldValue)
        {
            if (!IsValidSelectedDateFormat(this.SelectedDateFormat))
            {
                throw new ArgumentOutOfRangeException("SelectedDateFormat", "VicDatePickerFormat value is not valid.");
            }
            if (this._elementTextBox != null)
            {
                if (string.IsNullOrEmpty(this._elementTextBox.Text))
                {
                    this.SetWaterMarkText();
                }
                else
                {
                    DateTime? nullable = this.ParseText(this._elementTextBox.Text);
                    if (nullable.HasValue)
                    {
                        string str = this.DateTimeToString(nullable.Value);
                        this.Text = str;
                    }
                }
            }
        }

        private static void OnSelectedDateFormatPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker picker = d as VicDatePicker;
            VicDatePickerFormat oldValue = (VicDatePickerFormat) e.OldValue;
            picker.OnSelectedDateFormatChanged(oldValue);
        }

        private void OnTextChanged(string oldValue)
        {
            if (!this._typing)
            {
                if (this.Text == null)
                {
                    if (!this._settingSelectedDate)
                    {
                        base.SetValue(SelectedDateProperty, null);
                    }
                }
                else
                {
                    if (this._elementTextBox != null)
                    {
                        this._elementTextBox.Text = this.Text;
                    }
                    else
                    {
                        this._defaultText = this.Text;
                    }
                    if (!this._settingSelectedDate)
                    {
                        this.SetSelectedDate();
                    }
                }
            }
            else
            {
                this.SetWaterMarkText();
            }
        }

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker picker = d as VicDatePicker;
            string oldValue = (string) e.OldValue;
            picker.OnTextChanged(oldValue);
        }

        private void OnWatermarkChanged(object oldValue)
        {
            if (this._elementTextBox != null)
            {
                this.SetTextBoxValue(this._elementTextBox.Text);
            }
        }

        private static void OnWatermarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePicker picker = d as VicDatePicker;
            object oldValue = e.OldValue;
            picker.OnWatermarkChanged(oldValue);
        }

        private DateTime? ParseText(string text)
        {
            DateTime minValue = DateTime.MinValue;
            try
            {
                bool flag = false;
                if (!((this.SelectedDateFormat != VicDatePickerFormat.Custom) || string.IsNullOrEmpty(this.CustomFormat)))
                {
                    flag = DateTime.TryParseExact(text, this.CustomFormat, VicTimeEditor.GetCurrentDateTimeFormat(), DateTimeStyles.None, out minValue);
                }
                if (!flag)
                {
                    minValue = DateTime.Parse(text, VicTimeEditor.GetCurrentDateTimeFormat());
                }
                if (this.IsValidDateSelection(minValue))
                {
                    return new DateTime?(minValue);
                }
                DatePickerDateValidationErrorEventArgs e = new DatePickerDateValidationErrorEventArgs(new ArgumentOutOfRangeException("text", "SelectedDate value is not valid."), text);
                this.OnDateValidationError(e);
                if (e.ThrowException)
                {
                    throw e.Exception;
                }
            }
            catch (FormatException exception)
            {
                DatePickerDateValidationErrorEventArgs args2 = new DatePickerDateValidationErrorEventArgs(exception, text);
                this.OnDateValidationError(args2);
                if (args2.ThrowException)
                {
                    throw args2.Exception;
                }
            }
            return null;
        }

        private void SetCustomDefaultValues()
        {
            base.SnapsToDevicePixels = true;
            base.IsTabStop = false;
            this.InitializeCalendar();
            this.FirstDayOfWeek = VicTimeEditor.GetCurrentDateTimeFormat().FirstDayOfWeek;
            this.DisplayDate = DateTime.Today;
        }

        private void SetSelectedDate()
        {
            if (this._elementTextBox != null)
            {
                if (!this.IsEmpty())
                {
                    string text = this._elementTextBox.Text;
                    if (!this.SelectedDate.HasValue || (this.DateTimeToString(this.SelectedDate.Value) != text))
                    {
                        DateTime? nullable = this.SetTextBoxValue(text);
                        if (!this.SelectedDate.Equals(nullable))
                        {
                            this.SelectedDate = nullable;
                        }
                    }
                }
                else if (!this.AllowNull)
                {
                    if (!this.SelectedDate.HasValue)
                    {
                        this.SelectedDate = new DateTime?(DateTime.Today);
                    }
                    this.SetTextBoxValue(this.DateTimeToString(this.SelectedDate.Value));
                }
                else
                {
                    DateTime? selectedDate = this.SelectedDate;
                    if (selectedDate.HasValue)
                    {
                        this.SelectedDate = null;
                    }
                }
            }
            else
            {
                DateTime? nullable2 = this.SetTextBoxValue(this._defaultText);
                if (!this.SelectedDate.Equals(nullable2))
                {
                    this.SelectedDate = nullable2;
                }
            }
        }

        private DateTime? SetTextBoxValue(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                base.SetValue(TextProperty, s);
                if (!this.SelectedDate.HasValue)
                {
                    this.SetWaterMarkText();
                }
                return this.SelectedDate;
            }
            DateTime? nullable = this.ParseText(s);
            if (nullable.HasValue)
            {
                base.SetValue(TextProperty, s);
                return nullable;
            }
            DateTime? selectedDate = this.SelectedDate;
            if (selectedDate.HasValue)
            {
                string str = this.DateTimeToString(this.SelectedDate.Value);
                base.SetValue(TextProperty, str);
                return this.SelectedDate;
            }
            this.SetWaterMarkText();
            return null;
        }

        private void SetWaterMarkText()
        {
            if (this._elementTextBox != null)
            {
                this._defaultText = this.Text = string.Empty;
                if ((this.Watermark == null) || string.IsNullOrEmpty(this.Watermark.ToString()))
                {
                    switch (this.SelectedDateFormat)
                    {
                        case VicDatePickerFormat.Long:
                            this._elementTextBox.Watermark = string.Format(CultureInfo.CurrentCulture, "<{0}>", new object[] { VicTimeEditor.GetCurrentDateTimeFormat().LongDatePattern.ToString() });
                            break;

                        case VicDatePickerFormat.Short:
                            this._elementTextBox.Watermark = string.Format(CultureInfo.CurrentCulture, "<{0}>", new object[] { VicTimeEditor.GetCurrentDateTimeFormat().ShortDatePattern.ToString() });
                            break;

                        case VicDatePickerFormat.Custom:
                            this._elementTextBox.Watermark = string.Format(CultureInfo.CurrentCulture, "<{0}>", new object[] { this.CustomFormat });
                            break;
                    }
                }
                else
                {
                    this._elementTextBox.Watermark = this.Watermark;
                }
                if (!this._typing)
                {
                    VisualStateHelper.GoToState(this._elementTextBox, "Watermarked", false);
                }
            }
        }

        private void TogglePopUp()
        {
            if (this.IsDropDownOpen)
            {
                this.ClosePopUp();
            }
            else if (!this.IsReadOnly)
            {
                this.SetSelectedDate();
                this.IsDropDownOpen = true;
            }
        }

        public override string ToString()
        {
            if (this.SelectedDate.HasValue)
            {
                return this.SelectedDate.Value.ToString(VicTimeEditor.GetCurrentDateTimeFormat());
            }
            return string.Empty;
        }

        public bool AllowNull
        {
            get
            {
                return (bool) base.GetValue(AllowNullProperty);
            }
            set
            {
                base.SetValue(AllowNullProperty, value);
            }
        }

        public CalendarBlackoutDatesCollection BlackoutDates { get; private set; }

        public Brush ButtonBackground
        {
            get
            {
                return (Brush) base.GetValue(ButtonBackgroundProperty);
            }
            set
            {
                base.SetValue(ButtonBackgroundProperty, value);
            }
        }

        public Brush ButtonForeground
        {
            get
            {
                return (Brush) base.GetValue(ButtonForegroundProperty);
            }
            set
            {
                base.SetValue(ButtonForegroundProperty, value);
            }
        }

        public Style CalendarStyle
        {
            get
            {
                return (Style) base.GetValue(CalendarStyleProperty);
            }
            set
            {
                base.SetValue(CalendarStyleProperty, value);
            }
        }

        public Brush CaretBrush
        {
            get
            {
                return (Brush) base.GetValue(CaretBrushProperty);
            }
            set
            {
                base.SetValue(CaretBrushProperty, value);
            }
        }

        public System.Windows.CornerRadius CornerRadius
        {
            get
            {
                return (System.Windows.CornerRadius) base.GetValue(CornerRadiusProperty);
            }
            set
            {
                base.SetValue(CornerRadiusProperty, value);
            }
        }

        public string CustomFormat
        {
            get
            {
                return (string) base.GetValue(CustomFormatProperty);
            }
            set
            {
                base.SetValue(CustomFormatProperty, value);
            }
        }

        public Visibility DisabledCuesVisibility
        {
            get
            {
                return (Visibility) base.GetValue(DisabledCuesVisibilityProperty);
            }
            set
            {
                base.SetValue(DisabledCuesVisibilityProperty, value);
            }
        }

        [TypeConverter(typeof(DateTimeTypeConverter))]
        public DateTime DisplayDate
        {
            get
            {
                return (DateTime) base.GetValue(DisplayDateProperty);
            }
            set
            {
                base.SetValue(DisplayDateProperty, value);
            }
        }

        [TypeConverter(typeof(DateTimeTypeConverter))]
        public DateTime? DisplayDateEnd
        {
            get
            {
                return (DateTime?) base.GetValue(DisplayDateEndProperty);
            }
            set
            {
                base.SetValue(DisplayDateEndProperty, value);
            }
        }

        [TypeConverter(typeof(DateTimeTypeConverter))]
        public DateTime? DisplayDateStart
        {
            get
            {
                return (DateTime?) base.GetValue(DisplayDateStartProperty);
            }
            set
            {
                base.SetValue(DisplayDateStartProperty, value);
            }
        }

        public DayOfWeek FirstDayOfWeek
        {
            get
            {
                return (DayOfWeek) base.GetValue(FirstDayOfWeekProperty);
            }
            set
            {
                base.SetValue(FirstDayOfWeekProperty, value);
            }
        }

        public Brush FocusBrush
        {
            get
            {
                return (Brush) base.GetValue(FocusBrushProperty);
            }
            set
            {
                base.SetValue(FocusBrushProperty, value);
            }
        }

        public Visibility FocusCuesVisibility
        {
            get
            {
                return (Visibility) base.GetValue(FocusCuesVisibilityProperty);
            }
            set
            {
                base.SetValue(FocusCuesVisibilityProperty, value);
            }
        }

        internal bool ForceMouseOver
        {
            get
            {
                return (bool) base.GetValue(ForceMouseOverProperty);
            }
            set
            {
                base.SetValue(ForceMouseOverProperty, value);
            }
        }

        public bool IsDropDownOpen
        {
            get
            {
                return (bool) base.GetValue(IsDropDownOpenProperty);
            }
            set
            {
                base.SetValue(IsDropDownOpenProperty, value);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return (bool) base.GetValue(IsReadOnlyProperty);
            }
            set
            {
                base.SetValue(IsReadOnlyProperty, value);
            }
        }

        public bool IsTodayHighlighted
        {
            get
            {
                return (bool) base.GetValue(IsTodayHighlightedProperty);
            }
            set
            {
                base.SetValue(IsTodayHighlightedProperty, value);
            }
        }

        public string Mask
        {
            get
            {
                return (string) base.GetValue(MaskProperty);
            }
            set
            {
                base.SetValue(MaskProperty, value);
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return (Brush) base.GetValue(MouseOverBrushProperty);
            }
            set
            {
                base.SetValue(MouseOverBrushProperty, value);
            }
        }

        public Brush PressedBrush
        {
            get
            {
                return (Brush) base.GetValue(PressedBrushProperty);
            }
            set
            {
                base.SetValue(PressedBrushProperty, value);
            }
        }

        public string Prompt
        {
            get
            {
                return (string) base.GetValue(PromptProperty);
            }
            set
            {
                base.SetValue(PromptProperty, value);
            }
        }

        [TypeConverter(typeof(DateTimeTypeConverter))]
        public DateTime? SelectedDate
        {
            get
            {
                return (DateTime?) base.GetValue(SelectedDateProperty);
            }
            set
            {
                base.SetValue(SelectedDateProperty, value);
            }
        }

        public VicDatePickerFormat SelectedDateFormat
        {
            get
            {
                return (VicDatePickerFormat) base.GetValue(SelectedDateFormatProperty);
            }
            set
            {
                base.SetValue(SelectedDateFormatProperty, value);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
        public Brush SelectionBackground
        {
            get
            {
                return (Brush) base.GetValue(SelectionBackgroundProperty);
            }
            set
            {
                base.SetValue(SelectionBackgroundProperty, value);
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public Brush SelectionForeground
        {
            get
            {
                return (Brush) base.GetValue(SelectionForegroundProperty);
            }
            set
            {
                base.SetValue(SelectionForegroundProperty, value);
            }
        }

        public string Text
        {
            get
            {
                return (string) base.GetValue(TextProperty);
            }
            set
            {
                base.SetValue(TextProperty, value);
            }
        }

        public Style ValidationDecoratorStyle
        {
            get
            {
                return (Style) base.GetValue(ValidationDecoratorStyleProperty);
            }
            set
            {
                base.SetValue(ValidationDecoratorStyleProperty, value);
            }
        }

        public object Watermark
        {
            get
            {
                return base.GetValue(WatermarkProperty);
            }
            set
            {
                base.SetValue(WatermarkProperty, value);
            }
        }
    }
}

