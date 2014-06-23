namespace Victop.Wpf.VicDateTimeEditors
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    [TemplateVisualState(Name="InvalidFocused", GroupName="ValidationStatesStates"), TemplatePart(Name="DatePicker", Type=typeof(VicDatePicker)), TemplatePart(Name="Root", Type=typeof(FrameworkElement)), TemplateVisualState(Name="MouseOver", GroupName="CommonStates"), TemplateVisualState(Name="Disabled", GroupName="CommonStates"), TemplateVisualState(Name="Normal", GroupName="CommonStates"), TemplateVisualState(Name="Focused", GroupName="FocusStates"), TemplateVisualState(Name="Unfocused", GroupName="FocusStates"), TemplateVisualState(Name="Valid", GroupName="ValidationStatesStates"), TemplateVisualState(Name="InvalidUnfocused", GroupName="ValidationStatesStates"), TemplatePart(Name="TimeEditor", Type=typeof(VicTimeEditor)), StyleTypedProperty(Property="ValidationDecoratorStyle", StyleTargetType=typeof(VicValidationDecorator)), LicenseProvider]
    public class VicDateTimePicker : Control
    {
        internal VicDatePicker _elementDatePicker;
        internal FrameworkElement _elementRoot;
        internal VicTimeEditor _elementTimeEditor;
        internal bool _isLoaded;
        private double _lastUsedSeparatorWidth;
        private bool _throwIsMouseOverChanged;
        private bool _updatingControls;
        public static readonly DependencyProperty AllowNullProperty = DependencyProperty.Register("AllowNull", typeof(bool), typeof(VicDateTimePicker), new PropertyMetadata(false, new PropertyChangedCallback(VicDateTimePicker.OnAllowNullPropertyChanged)));
        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register("ButtonBackground", typeof(Brush), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty ButtonForegroundProperty = DependencyProperty.Register("ButtonForeground", typeof(Brush), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(VicDateTimePicker), new PropertyMetadata(null, new PropertyChangedCallback(VicDateTimePicker.OnCaretBrushPropertyChanged)));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(System.Windows.CornerRadius), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty CustomDateFormatProperty = DependencyProperty.Register("CustomDateFormat", typeof(string), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty CustomTimeFormatProperty = DependencyProperty.Register("CustomTimeFormat", typeof(string), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty DateFormatProperty = DependencyProperty.Register("DateFormat", typeof(VicDatePickerFormat), typeof(VicDateTimePicker), new PropertyMetadata(VicDatePickerFormat.Short));
        public static readonly DependencyProperty DateMaskProperty = DependencyProperty.Register("DateMask", typeof(string), typeof(VicDateTimePicker), new PropertyMetadata(string.Empty));
        internal const string DatePickerElementName = "DatePicker";
        public static readonly DependencyProperty DateTimeProperty = DependencyProperty.Register("DateTime", typeof(System.DateTime?), typeof(VicDateTimePicker), new PropertyMetadata(null, new PropertyChangedCallback(VicDateTimePicker.OnDateTimePropertyChanged)));
        public static readonly DependencyProperty DateWatermarkProperty = DependencyProperty.Register("DateWatermark", typeof(object), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty DisabledCuesVisibilityProperty = DependencyProperty.Register("DisabledCuesVisibility", typeof(Visibility), typeof(VicDateTimePicker), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty EditModeProperty = DependencyProperty.Register("EditMode", typeof(VicDateTimePickerEditMode), typeof(VicDateTimePicker), new PropertyMetadata(VicDateTimePickerEditMode.DateTime, new PropertyChangedCallback(VicDateTimePicker.OnEditModePropertyChanged)));
        public static readonly DependencyProperty FirstDayOfWeekProperty = DependencyProperty.Register("FirstDayOfWeek", typeof(DayOfWeek), typeof(VicDateTimePicker), new PropertyMetadata(new PropertyChangedCallback(VicDateTimePicker.OnFirstDayOfWeekPropertyChanged)));
        public static readonly DependencyProperty FocusBrushProperty = DependencyProperty.Register("FocusBrush", typeof(Brush), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty FocusCuesVisibilityProperty = DependencyProperty.Register("FocusCuesVisibility", typeof(Visibility), typeof(VicDateTimePicker), new PropertyMetadata(Visibility.Visible));
        internal static readonly DependencyProperty ForceMouseOverProperty = DependencyProperty.Register("ForceMouseOver", typeof(bool), typeof(VicDateTimePicker), new PropertyMetadata(new PropertyChangedCallback(VicDateTimePicker.OnForceMouseOverPropertyChanged)));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty MaxDateProperty = DependencyProperty.Register("MaxDate", typeof(System.DateTime), typeof(VicDateTimePicker), new PropertyMetadata(System.DateTime.MaxValue, new PropertyChangedCallback(VicDateTimePicker.OnMaxDatePropertyChanged)));
        public static readonly DependencyProperty MinDatePickerWidthProperty = DependencyProperty.Register("MinDatePickerWidth", typeof(double), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty MinDateProperty = DependencyProperty.Register("MinDate", typeof(System.DateTime), typeof(VicDateTimePicker), new PropertyMetadata(System.DateTime.MinValue, new PropertyChangedCallback(VicDateTimePicker.OnMinDatePropertyChanged)));
        public static readonly DependencyProperty MinTimeEditorWidthProperty = DependencyProperty.Register("MinTimeEditorWidth", typeof(double), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.Register("MouseOverBrush", typeof(Brush), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty PressedBrushProperty = DependencyProperty.Register("PressedBrush", typeof(Brush), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty PromptProperty = DependencyProperty.Register("Prompt", typeof(string), typeof(VicDateTimePicker), new PropertyMetadata("_"));
        internal const string RootElementName = "Root";
        public static readonly DependencyProperty SelectionBackgroundProperty = DependencyProperty.Register("SelectionBackground", typeof(Brush), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty SelectionForegroundProperty = DependencyProperty.Register("SelectionForeground", typeof(Brush), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty SeparatorWidthProperty = DependencyProperty.Register("SeparatorWidth", typeof(double), typeof(VicDateTimePicker), new PropertyMetadata(3.0, new PropertyChangedCallback(VicDateTimePicker.OnSeparatorWidthPropertyChanged)));
        internal const string TimeEditorElementName = "TimeEditor";
        public static readonly DependencyProperty TimeFormatProperty = DependencyProperty.Register("TimeFormat", typeof(VicTimeEditorFormat), typeof(VicDateTimePicker), new FrameworkPropertyMetadata(VicTimeEditorFormat.LongTime, new PropertyChangedCallback(VicDateTimePicker.OnTimeFormatPropertyChanged), new CoerceValueCallback(VicDateTimePicker.CoerceTimeFormat)));
        public static readonly DependencyProperty TimeIncrementProperty = DependencyProperty.Register("TimeIncrement", typeof(TimeSpan), typeof(VicDateTimePicker), new PropertyMetadata(TimeSpan.FromMinutes(5.0)));
        public static readonly DependencyProperty TimeMaskProperty = DependencyProperty.Register("TimeMask", typeof(string), typeof(VicDateTimePicker), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty TimeWatermarkProperty = DependencyProperty.Register("TimeWatermark", typeof(object), typeof(VicDateTimePicker), null);
        public static readonly DependencyProperty ValidationDecoratorStyleProperty = DependencyProperty.Register("ValidationDecoratorStyle", typeof(Style), typeof(VicDateTimePicker), null);

        public event EventHandler<NullablePropertyChangedEventArgs<System.DateTime>> DateTimeChanged;

        public event EventHandler<DatePickerDateValidationErrorEventArgs> DateValidationError;

        public event EventHandler<PropertyChangedEventArgs<bool>> IsMouseOverChanged;

        static VicDateTimePicker()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VicDateTimePicker), new FrameworkPropertyMetadata(typeof(VicDateTimePicker)));
            UIElement.FocusableProperty.OverrideMetadata(typeof(VicDateTimePicker), new FrameworkPropertyMetadata(false));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(VicDateTimePicker), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
        }

        public VicDateTimePicker()
        {
            RoutedEventHandler handler3 = null;
            DependencyPropertyChangedEventHandler handler4 = null;
            RoutedEventHandler handler = null;
            DependencyPropertyChangedEventHandler handler2 = null;
            this._throwIsMouseOverChanged = true;
            base.DefaultStyleKey = typeof(VicDateTimePicker);
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

        private static Brush CloneBrush(Brush source)
        {
            Brush brush = null;
            if (source != null)
            {
                if (source is SolidColorBrush)
                {
                    SolidColorBrush brush2 = source as SolidColorBrush;
                    return new SolidColorBrush(brush2.Color);
                }
                if (!(source is GradientBrush))
                {
                    return brush;
                }
                GradientBrush brush3 = source as GradientBrush;
                if (source is LinearGradientBrush)
                {
                    LinearGradientBrush brush4 = source as LinearGradientBrush;
                    LinearGradientBrush brush5 = new LinearGradientBrush {
                        StartPoint = brush4.StartPoint,
                        EndPoint = brush4.EndPoint
                    };
                    brush = brush5;
                }
                else
                {
                    RadialGradientBrush brush7 = source as RadialGradientBrush;
                    RadialGradientBrush brush8 = new RadialGradientBrush {
                        Center = brush7.Center,
                        GradientOrigin = brush7.GradientOrigin,
                        RadiusX = brush7.RadiusX,
                        RadiusY = brush7.RadiusY
                    };
                    brush = brush8;
                }
                GradientBrush brush10 = brush as GradientBrush;
                brush10.ColorInterpolationMode = brush3.ColorInterpolationMode;
                brush10.MappingMode = brush3.MappingMode;
                brush10.RelativeTransform = brush3.RelativeTransform;
                brush10.SpreadMethod = brush3.SpreadMethod;
                brush10.Transform = brush3.Transform;
                foreach (GradientStop stop in brush3.GradientStops)
                {
                    GradientStop stop2 = new GradientStop {
                        Offset = stop.Offset,
                        Color = stop.Color
                    };
                    brush10.GradientStops.Add(stop2);
                }
            }
            return brush;
        }

        private static object CoerceTimeFormat(DependencyObject d, object baseValue)
        {
            if (((baseValue != null) && (baseValue != DependencyProperty.UnsetValue)) && (((VicTimeEditorFormat) baseValue) != VicTimeEditorFormat.TimeSpan))
            {
                return baseValue;
            }
            VicDateTimePicker picker = d as VicDateTimePicker;
            if (picker != null)
            {
                return ((picker.TimeFormat != VicTimeEditorFormat.TimeSpan) ? ((object) picker.TimeFormat) : ((object) 1));
            }
            return VicTimeEditorFormat.LongTime;
        }

        public void FinalizeEditing()
        {
            if ((this._elementTimeEditor != null) && (this._elementTimeEditor.Visibility == Visibility.Visible))
            {
                this._elementTimeEditor.FinalizeEditing();
            }
            if ((this._elementDatePicker != null) && (this._elementDatePicker.Visibility == Visibility.Visible))
            {
                this._elementDatePicker.FinalizeEditing();
                if (!this._elementDatePicker.SelectedDate.HasValue)
                {
                    this.UpdateValue(true);
                }
                else
                {
                    this.UpdateValue(false);
                }
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

        private void InitializeDatePickerPart()
        {
            this.BlackoutDates = this._elementDatePicker.BlackoutDates;
            this._elementDatePicker.FirstDayOfWeek = this.FirstDayOfWeek;
            this._elementDatePicker.SelectedDateChanged += delegate (object param0, SelectionChangedEventArgs param1) {
                if (!this._elementDatePicker.SelectedDate.HasValue)
                {
                    this.UpdateValue(true);
                }
                else
                {
                    this.UpdateValue(false);
                }
            };
            this._elementDatePicker.DateValidationError += delegate (object sender, DatePickerDateValidationErrorEventArgs e) {
                if (this.DateValidationError != null)
                {
                    this.DateValidationError(sender, e);
                }
            };
        }

        private void InitializeTimeEditorPart()
        {
            this._elementTimeEditor.CycleChangesOnBoundaries = true;
            this._elementTimeEditor.ValueChanged += (s, a) => this.UpdateValue(false);
            this._elementTimeEditor.TextValidationError += delegate (object sender, TextValidationErrorEventArgs e) {
                if (this.DateValidationError != null)
                {
                    DatePickerDateValidationErrorEventArgs args = new DatePickerDateValidationErrorEventArgs(e.Exception, e.Text);
                    this.DateValidationError(sender, args);
                    e.ThrowException = args.ThrowException;
                }
            };
        }

        private void OnAfterApplyTemplate()
        {
            this.OnMaxDateChanged(this.MaxDate);
            this.OnMinDateChanged(this.MinDate);
            this.OnEditModeChanged(VicDateTimePickerEditMode.DateTime);
            this.OnTimeFormatChanged(VicTimeEditorFormat.LongTime);
            this.OnCaretBrushChanged();
        }

        private void OnAllowNullChanged(bool oldValue)
        {
            if (!(this.AllowNull || this.DateTime.HasValue))
            {
                this.OnDateTimeChanged((System.DateTime?) null);
            }
        }

        private static void OnAllowNullPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDateTimePicker picker = d as VicDateTimePicker;
            bool oldValue = (bool) e.OldValue;
            picker.OnAllowNullChanged(oldValue);
        }

        public override void OnApplyTemplate()
        {
            string errors = string.Empty;
            base.OnApplyTemplate();
            this._isLoaded = true;
            this._elementDatePicker = this.GetTemplateChild<VicDatePicker>("DatePicker", true, ref errors);
            if (this._elementDatePicker != null)
            {
                this.InitializeDatePickerPart();
            }
            this._elementTimeEditor = this.GetTemplateChild<VicTimeEditor>("TimeEditor", true, ref errors);
            if (this._elementTimeEditor != null)
            {
                this.InitializeTimeEditorPart();
            }
            this._elementRoot = this.GetTemplateChild<FrameworkElement>("Root", false, ref errors);
            if (!string.IsNullOrEmpty(errors))
            {
                this._isLoaded = false;
                if (!DesignerProperties.GetIsInDesignMode(this))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Template cannot be applied to VicDateTimePicker.\nDetails: {0}", new object[] { errors }));
                }
            }
            else
            {
                this.ChangeVisualStateCommon(false);
                this.ChangeVisualStateFocus(false);
                this.OnAfterApplyTemplate();
            }
        }

        private void OnCaretBrushChanged()
        {
            if (this._elementTimeEditor != null)
            {
                this._elementTimeEditor.CaretBrush = CloneBrush(this.CaretBrush);
            }
            if (this._elementDatePicker != null)
            {
                this._elementDatePicker.CaretBrush = CloneBrush(this.CaretBrush);
            }
        }

        private static void OnCaretBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VicDateTimePicker).OnCaretBrushChanged();
        }

        protected virtual void OnDateTimeChanged(NullablePropertyChangedEventArgs<System.DateTime> e)
        {
            if (this.DateTimeChanged != null)
            {
                this.DateTimeChanged(this, e);
            }
        }

        private void OnDateTimeChanged(System.DateTime? oldValue)
        {
            System.DateTime? nullable4;
            System.DateTime? nullable5;
            System.DateTime? dateTime = this.DateTime;
            if (!(dateTime.HasValue || this.AllowNull))
            {
                dateTime = new System.DateTime?(System.DateTime.Today);
            }
            else
            {
                System.DateTime? nullable2 = dateTime;
                System.DateTime minDate = this.MinDate;
                if (nullable2.HasValue ? (nullable2.GetValueOrDefault() < minDate) : false)
                {
                    dateTime = new System.DateTime?(this.MinDate);
                }
                System.DateTime? nullable3 = dateTime;
                System.DateTime maxDate = this.MaxDate;
                if (nullable3.HasValue ? (nullable3.GetValueOrDefault() > maxDate) : false)
                {
                    dateTime = new System.DateTime?(this.MaxDate);
                }
            }
            if (dateTime.HasValue && (((nullable4 = dateTime).HasValue != (nullable5 = this.DateTime).HasValue) || (nullable4.HasValue && (nullable4.GetValueOrDefault() != nullable5.GetValueOrDefault()))))
            {
                base.SetValue(DateTimeProperty, dateTime);
            }
            else
            {
                this.UpdateControls();
                if (oldValue != this.DateTime)
                {
                    NullablePropertyChangedEventArgs<System.DateTime> e = new NullablePropertyChangedEventArgs<System.DateTime> {
                        OldValue = oldValue,
                        NewValue = this.DateTime
                    };
                    this.OnDateTimeChanged(e);
                }
            }
        }

        private static void OnDateTimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDateTimePicker picker = d as VicDateTimePicker;
            System.DateTime? oldValue = (System.DateTime?) e.OldValue;
            picker.OnDateTimeChanged(oldValue);
        }

        private void OnEditModeChanged(VicDateTimePickerEditMode oldValue)
        {
            if (this._isLoaded)
            {
                Thickness margin = this._elementTimeEditor.Margin;
                if (this._lastUsedSeparatorWidth > 0.0)
                {
                    margin = new Thickness(margin.Left - this._lastUsedSeparatorWidth, margin.Top, margin.Right, margin.Bottom);
                    this._lastUsedSeparatorWidth = 0.0;
                }
                switch (this.EditMode)
                {
                    case VicDateTimePickerEditMode.Date:
                        this._elementDatePicker.Visibility = Visibility.Visible;
                        this._elementTimeEditor.Visibility = Visibility.Collapsed;
                        this._elementTimeEditor.Margin = margin;
                        break;

                    case VicDateTimePickerEditMode.Time:
                        this._elementDatePicker.Visibility = Visibility.Collapsed;
                        this._elementTimeEditor.Visibility = Visibility.Visible;
                        this._elementTimeEditor.Margin = margin;
                        break;

                    case VicDateTimePickerEditMode.DateTime:
                        this._elementDatePicker.Visibility = Visibility.Visible;
                        this._elementTimeEditor.Visibility = Visibility.Visible;
                        this._elementTimeEditor.Margin = new Thickness(margin.Left + this.SeparatorWidth, margin.Top, margin.Right, margin.Bottom);
                        this._lastUsedSeparatorWidth = this.SeparatorWidth;
                        break;
                }
                this.OnTimeFormatChanged(this.TimeFormat);
            }
        }

        private static void OnEditModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDateTimePicker picker = d as VicDateTimePicker;
            VicDateTimePickerEditMode oldValue = (VicDateTimePickerEditMode) e.OldValue;
            picker.OnEditModeChanged(oldValue);
        }

        private void OnFirstDayOfWeekChanged(DayOfWeek oldValue)
        {
            if (this._elementDatePicker != null)
            {
                this._elementDatePicker.FirstDayOfWeek = this.FirstDayOfWeek;
            }
        }

        private static void OnFirstDayOfWeekPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDateTimePicker picker = d as VicDateTimePicker;
            DayOfWeek oldValue = (DayOfWeek) e.OldValue;
            picker.OnFirstDayOfWeekChanged(oldValue);
        }

        private static void OnForceMouseOverPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VicDateTimePicker).ChangeVisualStateCommon(true);
        }

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VicDateTimePicker).ChangeVisualStateFocus(true);
        }

        private static void OnIsMouseOverPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDateTimePicker sender = d as VicDateTimePicker;
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

        private void OnMaxDateChanged(System.DateTime oldValue)
        {
            if (this.MinDate > this.MaxDate)
            {
                this.MaxDate = this.MinDate;
            }
            if (this._elementDatePicker != null)
            {
                this._elementDatePicker.DisplayDateEnd = new System.DateTime?(this.MaxDate);
            }
            this.OnDateTimeChanged(this.DateTime);
        }

        private static void OnMaxDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDateTimePicker picker = d as VicDateTimePicker;
            System.DateTime oldValue = (System.DateTime) e.OldValue;
            picker.OnMaxDateChanged(oldValue);
        }

        private void OnMinDateChanged(System.DateTime oldValue)
        {
            if (this.MinDate > this.MaxDate)
            {
                this.MaxDate = this.MinDate;
            }
            if (this._elementDatePicker != null)
            {
                this._elementDatePicker.DisplayDateStart = new System.DateTime?(this.MinDate);
            }
            this.OnDateTimeChanged(this.DateTime);
        }

        private static void OnMinDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDateTimePicker picker = d as VicDateTimePicker;
            System.DateTime oldValue = (System.DateTime) e.OldValue;
            picker.OnMinDateChanged(oldValue);
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

        private void OnSeparatorWidthChanged(double oldValue)
        {
            if (!(this._lastUsedSeparatorWidth == 0.0))
            {
                Thickness margin = this._elementTimeEditor.Margin;
                margin = new Thickness((margin.Left - this._lastUsedSeparatorWidth) + this.SeparatorWidth, margin.Top, margin.Right, margin.Bottom);
                this._lastUsedSeparatorWidth = this.SeparatorWidth;
                this._elementTimeEditor.Margin = margin;
            }
        }

        private static void OnSeparatorWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDateTimePicker picker = d as VicDateTimePicker;
            double oldValue = (double) e.OldValue;
            picker.OnSeparatorWidthChanged(oldValue);
        }

        private void OnTimeFormatChanged(VicTimeEditorFormat oldValue)
        {
            if (this.TimeFormat == VicTimeEditorFormat.TimeSpan)
            {
                this.TimeFormat = (oldValue != VicTimeEditorFormat.TimeSpan) ? oldValue : VicTimeEditorFormat.LongTime;
            }
        }

        private static void OnTimeFormatPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDateTimePicker picker = d as VicDateTimePicker;
            VicTimeEditorFormat oldValue = (VicTimeEditorFormat) e.OldValue;
            picker.OnTimeFormatChanged(oldValue);
        }

        private void SetCustomDefaultValues()
        {
            base.SnapsToDevicePixels = true;
            this.FirstDayOfWeek = VicTimeEditor.GetCurrentDateTimeFormat().FirstDayOfWeek;
            base.IsTabStop = false;
        }

        private void UpdateControls()
        {
            if (this._isLoaded)
            {
                this._updatingControls = true;
                System.DateTime? dateTime = this.DateTime;
                if (dateTime.HasValue)
                {
                    this._elementDatePicker.SelectedDate = new System.DateTime?(this.DateTime.Value.Date);
                    this._elementTimeEditor.Value = new TimeSpan?(this.DateTime.Value.TimeOfDay);
                }
                else
                {
                    this._elementDatePicker.SelectedDate = null;
                    this._elementTimeEditor.Value = null;
                }
                this._updatingControls = false;
            }
        }

        private void UpdateValue(bool forceNull)
        {
            if (!this._updatingControls)
            {
                System.DateTime? selectedDate = new System.DateTime?(System.DateTime.Today);
                if (this.AllowNull)
                {
                    selectedDate = this._elementDatePicker.SelectedDate;
                    if (this._elementTimeEditor.Value.HasValue && !forceNull)
                    {
                        if (selectedDate.HasValue)
                        {
                            if (selectedDate.Value.TimeOfDay == TimeSpan.Zero)
                            {
                                selectedDate = new System.DateTime?(selectedDate.Value.Add(this._elementTimeEditor.Value.Value));
                            }
                        }
                        else
                        {
                            selectedDate = new System.DateTime?(System.DateTime.Today.Add(this._elementTimeEditor.Value.Value));
                        }
                    }
                }
                else if (forceNull)
                {
                    selectedDate = this.DateTime;
                }
                else
                {
                    if (this._elementDatePicker.SelectedDate.HasValue)
                    {
                        selectedDate = new System.DateTime?(this._elementDatePicker.SelectedDate.Value);
                    }
                    if (this._elementTimeEditor.Value.HasValue && (selectedDate.Value.TimeOfDay == TimeSpan.Zero))
                    {
                        selectedDate = new System.DateTime?(selectedDate.Value.Add(this._elementTimeEditor.Value.Value));
                    }
                }
                if (selectedDate != this.DateTime)
                {
                    this.DateTime = selectedDate;
                }
                this.UpdateControls();
            }
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

        public string CustomDateFormat
        {
            get
            {
                return (string) base.GetValue(CustomDateFormatProperty);
            }
            set
            {
                base.SetValue(CustomDateFormatProperty, value);
            }
        }

        public string CustomTimeFormat
        {
            get
            {
                return (string) base.GetValue(CustomTimeFormatProperty);
            }
            set
            {
                base.SetValue(CustomTimeFormatProperty, value);
            }
        }

        public VicDatePickerFormat DateFormat
        {
            get
            {
                return (VicDatePickerFormat) base.GetValue(DateFormatProperty);
            }
            set
            {
                base.SetValue(DateFormatProperty, value);
            }
        }

        public string DateMask
        {
            get
            {
                return (string) base.GetValue(DateMaskProperty);
            }
            set
            {
                base.SetValue(DateMaskProperty, value);
            }
        }

        public System.DateTime? DateTime
        {
            get
            {
                return (System.DateTime?) base.GetValue(DateTimeProperty);
            }
            set
            {
                base.SetValue(DateTimeProperty, value);
            }
        }

        public object DateWatermark
        {
            get
            {
                return base.GetValue(DateWatermarkProperty);
            }
            set
            {
                base.SetValue(DateWatermarkProperty, value);
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

        public VicDateTimePickerEditMode EditMode
        {
            get
            {
                return (VicDateTimePickerEditMode) base.GetValue(EditModeProperty);
            }
            set
            {
                base.SetValue(EditModeProperty, value);
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

        public System.DateTime MaxDate
        {
            get
            {
                return (System.DateTime) base.GetValue(MaxDateProperty);
            }
            set
            {
                base.SetValue(MaxDateProperty, value);
            }
        }

        public System.DateTime MinDate
        {
            get
            {
                return (System.DateTime) base.GetValue(MinDateProperty);
            }
            set
            {
                base.SetValue(MinDateProperty, value);
            }
        }

        public double MinDatePickerWidth
        {
            get
            {
                return (double) base.GetValue(MinDatePickerWidthProperty);
            }
            set
            {
                base.SetValue(MinDatePickerWidthProperty, value);
            }
        }

        public double MinTimeEditorWidth
        {
            get
            {
                return (double) base.GetValue(MinTimeEditorWidthProperty);
            }
            set
            {
                base.SetValue(MinTimeEditorWidthProperty, value);
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

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
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

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

        public double SeparatorWidth
        {
            get
            {
                return (double) base.GetValue(SeparatorWidthProperty);
            }
            set
            {
                base.SetValue(SeparatorWidthProperty, value);
            }
        }

        public VicTimeEditorFormat TimeFormat
        {
            get
            {
                return (VicTimeEditorFormat) base.GetValue(TimeFormatProperty);
            }
            set
            {
                base.SetValue(TimeFormatProperty, value);
            }
        }

        public TimeSpan TimeIncrement
        {
            get
            {
                return (TimeSpan) base.GetValue(TimeIncrementProperty);
            }
            set
            {
                base.SetValue(TimeIncrementProperty, value);
            }
        }

        public string TimeMask
        {
            get
            {
                return (string) base.GetValue(TimeMaskProperty);
            }
            set
            {
                base.SetValue(TimeMaskProperty, value);
            }
        }

        public object TimeWatermark
        {
            get
            {
                return base.GetValue(TimeWatermarkProperty);
            }
            set
            {
                base.SetValue(TimeWatermarkProperty, value);
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
    }
}

