namespace Victop.Wpf.VicDateTimeEditors
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    [TemplatePart(Name="Decrease", Type=typeof(RepeatButton)), TemplateVisualState(Name="HideButtons", GroupName="ButtonsVisibilityStates"), LicenseProvider, TemplateVisualState(Name="Focused", GroupName="FocusStates"), TemplateVisualState(Name="InvalidUnfocused", GroupName="ValidationStatesStates"), TemplateVisualState(Name="InvalidFocused", GroupName="ValidationStatesStates"), StyleTypedProperty(Property="ValidationDecoratorStyle", StyleTargetType=typeof(VicValidationDecorator)), TemplateVisualState(Name="Disabled", GroupName="CommonStates"), TemplateVisualState(Name="MouseOver", GroupName="CommonStates"), TemplateVisualState(Name="Valid", GroupName="ValidationStatesStates"), TemplatePart(Name="Increase", Type=typeof(RepeatButton)), TemplatePart(Name="TextBox", Type=typeof(TextBoxBase)), TemplateVisualState(Name="Unfocused", GroupName="FocusStates"), TemplateVisualState(Name="ShowButtons", GroupName="ButtonsVisibilityStates"), TemplateVisualState(Name="Normal", GroupName="CommonStates")]
    public class VicTimeEditor : Control
    {
        internal RepeatButton _elementDecrease;
        internal RepeatButton _elementIncrease;
        internal TextBoxBase _elementTextBox;
        internal bool _isLoaded;
        private string _mask;
        private TextBoxBase _maskedBox;
        private TimeSpan _maximum;
        private TimeSpan _minimum;
        private bool _needResetWidth;
        private bool _throwIsMouseOverChanged;
        public static readonly DependencyProperty AllowNullProperty = DependencyProperty.Register("AllowNull", typeof(bool), typeof(VicTimeEditor), new PropertyMetadata(false, new PropertyChangedCallback(VicTimeEditor.OnAllowNullPropertyChanged)));
        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register("ButtonBackground", typeof(Brush), typeof(VicTimeEditor), null);
        public static readonly DependencyProperty ButtonForegroundProperty = DependencyProperty.Register("ButtonForeground", typeof(Brush), typeof(VicTimeEditor), null);
        public static readonly DependencyProperty CaretBrushProperty = DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(VicTimeEditor), null);
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(System.Windows.CornerRadius), typeof(VicTimeEditor), null);
        public static readonly DependencyProperty CustomFormatProperty = DependencyProperty.Register("CustomFormat", typeof(string), typeof(VicTimeEditor), new PropertyMetadata(new PropertyChangedCallback(VicTimeEditor.OnCustomFormatPropertyChanged)));
        public static readonly DependencyProperty CycleChangesOnBoundariesProperty = DependencyProperty.Register("CycleChangesOnBoundaries", typeof(bool), typeof(VicTimeEditor), new PropertyMetadata(true));
        internal const string DecreaseElementName = "Decrease";
        public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(int), typeof(VicTimeEditor), new PropertyMetadata(300));
        public static readonly DependencyProperty DisabledCuesVisibilityProperty = DependencyProperty.Register("DisabledCuesVisibility", typeof(Visibility), typeof(VicTimeEditor), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty FocusBrushProperty = DependencyProperty.Register("FocusBrush", typeof(Brush), typeof(VicTimeEditor), null);
        public static readonly DependencyProperty FocusCuesVisibilityProperty = DependencyProperty.Register("FocusCuesVisibility", typeof(Visibility), typeof(VicTimeEditor), new PropertyMetadata(Visibility.Visible));
        internal static readonly DependencyProperty ForceMouseOverProperty = DependencyProperty.Register("ForceMouseOver", typeof(bool), typeof(VicTimeEditor), new PropertyMetadata(new PropertyChangedCallback(VicTimeEditor.OnForceMouseOverPropertyChanged)));
        public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(VicTimeEditorFormat), typeof(VicTimeEditor), new PropertyMetadata(VicTimeEditorFormat.LongTime, new PropertyChangedCallback(VicTimeEditor.OnFormatPropertyChanged)));
        public static readonly DependencyProperty HandleUpDownKeysProperty = DependencyProperty.Register("HandleUpDownKeys", typeof(bool), typeof(VicTimeEditor), new PropertyMetadata(true));
        internal const string IncreaseElementName = "Increase";
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(TimeSpan), typeof(VicTimeEditor), new PropertyMetadata(TimeSpan.FromMinutes(1.0)));
        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(int), typeof(VicTimeEditor), new PropertyMetadata(50));
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(VicTimeEditor), new PropertyMetadata(new PropertyChangedCallback(VicTimeEditor.OnIsReadOnlyPropertyChanged)));
        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register("Mask", typeof(string), typeof(VicTimeEditor), new PropertyMetadata(string.Empty, new PropertyChangedCallback(VicTimeEditor.OnMaskPropertyChanged)));
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(TimeSpan), typeof(VicTimeEditor), new PropertyMetadata(TimeSpan.FromDays(1.0), new PropertyChangedCallback(VicTimeEditor.OnMaximumPropertyChanged)));
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(TimeSpan), typeof(VicTimeEditor), new PropertyMetadata(TimeSpan.Zero, new PropertyChangedCallback(VicTimeEditor.OnMinimumPropertyChanged)));
        public static readonly DependencyProperty MouseOverBrushProperty = DependencyProperty.Register("MouseOverBrush", typeof(Brush), typeof(VicTimeEditor), null);
        public static readonly DependencyProperty PressedBrushProperty = DependencyProperty.Register("PressedBrush", typeof(Brush), typeof(VicTimeEditor), null);
        public static readonly DependencyProperty PromptProperty = DependencyProperty.Register("Prompt", typeof(string), typeof(VicTimeEditor), new PropertyMetadata("_", new PropertyChangedCallback(VicTimeEditor.OnPromptPropertyChanged)));
        public static readonly DependencyProperty SelectionBackgroundProperty = DependencyProperty.Register("SelectionBackground", typeof(Brush), typeof(VicTimeEditor), null);
        public static readonly DependencyProperty SelectionForegroundProperty = DependencyProperty.Register("SelectionForeground", typeof(Brush), typeof(VicTimeEditor), null);
        public static readonly DependencyProperty ShowButtonsProperty = DependencyProperty.Register("ShowButtons", typeof(bool), typeof(VicTimeEditor), new PropertyMetadata(true, new PropertyChangedCallback(VicTimeEditor.OnShowButtonsPropertyChanged)));
        internal const string TextBoxElementName = "TextBox";
        public static readonly DependencyProperty ValidationDecoratorStyleProperty = DependencyProperty.Register("ValidationDecoratorStyle", typeof(Style), typeof(VicTimeEditor), null);
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(TimeSpan?), typeof(VicTimeEditor), new PropertyMetadata(new TimeSpan?(TimeSpan.Zero), new PropertyChangedCallback(VicTimeEditor.OnValuePropertyChanged)));
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark", typeof(object), typeof(VicTimeEditor), new PropertyMetadata(new PropertyChangedCallback(VicTimeEditor.OnWatermarkPropertyChanged)));

        public event EventHandler<PropertyChangedEventArgs<bool>> IsMouseOverChanged;

        public event EventHandler<TextValidationErrorEventArgs> TextValidationError;

        public event EventHandler<NullablePropertyChangedEventArgs<TimeSpan>> ValueChanged;

        static VicTimeEditor()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VicTimeEditor), new FrameworkPropertyMetadata(typeof(VicTimeEditor)));
            UIElement.FocusableProperty.OverrideMetadata(typeof(VicTimeEditor), new FrameworkPropertyMetadata(false));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(VicTimeEditor), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
        }

        public VicTimeEditor()
        {
            RoutedEventHandler handler3 = null;
            DependencyPropertyChangedEventHandler handler4 = null;
            RoutedEventHandler handler = null;
            DependencyPropertyChangedEventHandler handler2 = null;
            this._minimum = TimeSpan.Zero;
            this._maximum = TimeSpan.FromDays(1.0);
            this._mask = string.Empty;
            this._throwIsMouseOverChanged = true;
            base.DefaultStyleKey = typeof(VicTimeEditor);
            if (handler == null)
            {
                if (handler3 == null)
                {
                    handler3 = delegate (object param0, RoutedEventArgs param1) {
                        this.ChangeVisualStateButtonsVisibility(false);
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

        protected void ChangeVisualStateButtonsVisibility(bool useTransitions)
        {
            if (!this.ShowButtons)
            {
                VisualStateHelper.GoToState(this, "HideButtons", useTransitions);
            }
            if (this.ShowButtons)
            {
                VisualStateHelper.GoToState(this, "ShowButtons", useTransitions);
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

        private void ElementDecrease_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateValue();
            this.SelectDefaultText();
            if (!this.Value.HasValue)
            {
                this.Value = new TimeSpan?(TimeSpan.Zero);
            }
            else
            {
                this.Value = new TimeSpan?(this.GetIncrementedValue(this.Increment.Negate()));
            }
            this.SelectDefaultText();
        }

        private void ElementIncrease_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateValue();
            this.SelectDefaultText();
            if (!this.Value.HasValue)
            {
                this.Value = new TimeSpan?(TimeSpan.Zero);
            }
            else
            {
                this.Value = new TimeSpan?(this.GetIncrementedValue(this.Increment));
            }
            this.SelectDefaultText();
        }

        private void ElementText_KeyDown(object sender, KeyEventArgs e)
        {
            string keyString = null;
            if (this.OnKeyDown(e.Key, e.VicGetPlatformKeyCode(), keyString, KeyboardUtil.Ctrl))
            {
                e.Handled = true;
            }
            else if (double.IsNaN(this._elementTextBox.Width))
            {
                this._elementTextBox.Width = this._elementTextBox.ActualWidth;
                this._needResetWidth = true;
            }
        }

        private void ElementTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((!this.IsReadOnly && !string.IsNullOrEmpty(this._elementTextBox.Text)) && (base.IsFocused || this.Value.HasValue))
            {
                VisualStateHelper.GoToState(this._elementTextBox, "Unwatermarked", false);
            }
        }

        internal void FinalizeEditing()
        {
            this.UpdateValue();
            this.UpdateText();
        }

        public static DateTimeFormatInfo GetCurrentDateTimeFormat()
        {
            return GetGregorianCulture(CultureInfo.CurrentCulture).DateTimeFormat;
        }

        public static CultureInfo GetGregorianCulture(CultureInfo culture)
        {
            if (culture.Calendar is GregorianCalendar)
            {
                return culture;
            }
            GregorianCalendar calendar = null;
            foreach (System.Globalization.Calendar calendar2 in culture.OptionalCalendars)
            {
                if (calendar2 is GregorianCalendar)
                {
                    calendar = calendar2 as GregorianCalendar;
                    if (calendar.CalendarType == GregorianCalendarTypes.Localized)
                    {
                        break;
                    }
                }
            }
            CultureInfo info = (calendar == null) ? new CultureInfo(CultureInfo.InvariantCulture.Name) : new CultureInfo(CultureInfo.CurrentCulture.Name);
            info.DateTimeFormat.Calendar = calendar ?? new GregorianCalendar();
            return info;
        }

        private TimeSpan GetIncrementedValue(TimeSpan increment)
        {
            TimeSpan ts = this.Value.HasValue ? this.Value.Value : TimeSpan.Zero;
            if (increment != TimeSpan.Zero)
            {
                try
                {
                    ts = ts.Add(increment);
                    if (this.CycleChangesOnBoundaries)
                    {
                        if (increment < TimeSpan.Zero)
                        {
                            if (ts < this._minimum)
                            {
                                ts = this._maximum.Subtract(this._minimum.Subtract(ts));
                            }
                            return ts;
                        }
                        if (ts == this._maximum)
                        {
                            return this._minimum;
                        }
                        if (ts > this._maximum)
                        {
                            ts = this._minimum.Add(ts.Subtract(this._maximum));
                        }
                        return ts;
                    }
                    if (ts < this._minimum)
                    {
                        return this._minimum;
                    }
                    if (ts > this._maximum)
                    {
                        ts = this._maximum;
                    }
                }
                catch (OverflowException)
                {
                    if (increment < TimeSpan.Zero)
                    {
                        return this._minimum;
                    }
                    if (this.CycleChangesOnBoundaries)
                    {
                        return this._minimum;
                    }
                    ts = this._maximum;
                }
            }
            return ts;
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
                if (this._elementDecrease != null)
                {
                    this._elementDecrease.IsEnabled = !this.IsReadOnly;
                }
                if (this._elementIncrease != null)
                {
                    this._elementIncrease.IsEnabled = !this.IsReadOnly;
                }
                this._elementTextBox.IsReadOnly = this.IsReadOnly;
            }
        }

        private void InitializeDecreasePart()
        {
            this._elementDecrease.Click += new RoutedEventHandler(this.ElementDecrease_Click);
            this._elementDecrease.Focusable = false;
        }

        private void InitializeIncreasePart()
        {
            this._elementIncrease.Click += new RoutedEventHandler(this.ElementIncrease_Click);
            this._elementIncrease.Focusable = false;
        }

        private void InitializeTextBoxPart()
        {
            this._maskedBox = this._elementTextBox as TextBox;
            //if (this._elementTextBox._elementWatermark != null)
            //{
            //    this._elementTextBox._elementWatermark.Focusable = false;
            //}
            this._elementTextBox.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(this.ElementText_KeyDown), true);
            this._elementTextBox.TextChanged += new TextChangedEventHandler(this.ElementTextBox_TextChanged);
            this._elementTextBox.LostFocus += (s, a) => this.FinalizeEditing();
        }

        private bool IsValidValue(TimeSpan span)
        {
            return ((span >= this._minimum) && (span <= this._maximum));
        }

        private TimeSpan newTimeSpan(int days, int hours, int minutes, int seconds)
        {
            if (days < 0)
            {
                if (hours > 0)
                {
                    hours = -hours;
                }
                if (minutes > 0)
                {
                    minutes = -minutes;
                }
                if (seconds > 0)
                {
                    seconds = -seconds;
                }
            }
            return new TimeSpan(days, hours, minutes, seconds);
        }

        private void OnAfterApplyTemplate()
        {
            this.HandleIsReadOnlyProperty();
            this.OnFormatChanged(this.Format);
            this.SelectDefaultText();
        }

        private void OnAllowNullChanged(bool oldValue)
        {
            if (!(this.AllowNull || this.Value.HasValue))
            {
                this.OnValueChanged((TimeSpan?) null);
            }
        }

        private static void OnAllowNullPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicTimeEditor editor = d as VicTimeEditor;
            bool oldValue = (bool) e.OldValue;
            editor.OnAllowNullChanged(oldValue);
        }

        public override void OnApplyTemplate()
        {
            string errors = string.Empty;
            base.OnApplyTemplate();
            this._isLoaded = true;
            this._elementTextBox = this.GetTemplateChild<TextBoxBase>("TextBox", true, ref errors);
            if (this._elementTextBox != null)
            {
                this.InitializeTextBoxPart();
            }
            this._elementIncrease = this.GetTemplateChild<RepeatButton>("Increase", false, ref errors);
            if (this._elementIncrease != null)
            {
                this.InitializeIncreasePart();
            }
            this._elementDecrease = this.GetTemplateChild<RepeatButton>("Decrease", false, ref errors);
            if (this._elementDecrease != null)
            {
                this.InitializeDecreasePart();
            }
            if (!string.IsNullOrEmpty(errors))
            {
                this._isLoaded = false;
                if (!DesignerProperties.GetIsInDesignMode(this))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Template cannot be applied to VicTimeEditor.\nDetails: {0}", new object[] { errors }));
                }
            }
            else
            {
                this.ChangeVisualStateButtonsVisibility(false);
                this.ChangeVisualStateCommon(false);
                this.ChangeVisualStateFocus(false);
                this.OnAfterApplyTemplate();
            }
        }

        private void OnCustomFormatChanged(string oldValue)
        {
            base.Dispatcher.BeginInvoke(() => this.OnCustomFormatChangedImpl(oldValue), new object[0]);
        }

        private void OnCustomFormatChangedImpl(string oldValue)
        {
            if (!string.IsNullOrEmpty(this.CustomFormat))
            {
                this.Format = VicTimeEditorFormat.Custom;
            }
            this.OnFormatChanged(this.Format);
        }

        private static void OnCustomFormatPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicTimeEditor editor = d as VicTimeEditor;
            string oldValue = (string) e.OldValue;
            editor.OnCustomFormatChanged(oldValue);
        }

        private static void OnForceMouseOverPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VicTimeEditor).ChangeVisualStateCommon(true);
        }

        private void OnFormatChanged(VicTimeEditorFormat oldValue)
        {
            this.OnMinimumChanged(this.Minimum);
            this.OnMaximumChanged(this.Maximum);
            this.OnMaskChanged(this.Mask);
            this.UpdateText();
        }

        private static void OnFormatPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicTimeEditor editor = d as VicTimeEditor;
            VicTimeEditorFormat oldValue = (VicTimeEditorFormat) e.OldValue;
            editor.OnFormatChanged(oldValue);
        }

        private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VicTimeEditor).ChangeVisualStateFocus(true);
        }

        private static void OnIsMouseOverPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicTimeEditor sender = d as VicTimeEditor;
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
            VicTimeEditor editor = d as VicTimeEditor;
            bool oldValue = (bool) e.OldValue;
            editor.OnIsReadOnlyChanged(oldValue);
        }

        internal bool OnKeyDown(Key key, int platformKeyCode, string keyString, bool ctrlPressed)
        {
            if (this.IsReadOnly)
            {
                return true;
            }
            if (key == Key.Return)
            {
                this.UpdateValue();
                this.SelectDefaultText();
                return true;
            }
            if (!(this.HandleUpDownKeys || (((key != Key.Up) && (key != Key.Down)) && ((key != Key.Prior) && (key != Key.Next)))))
            {
                return false;
            }
            TimeSpan zero = TimeSpan.Zero;
            switch (key)
            {
                case Key.Prior:
                    if (this.Increment.TotalDays >= 1.0)
                    {
                        zero = new TimeSpan(this.Increment.Days * 10, 0, 0, 0);
                    }
                    else if (this.Increment.TotalHours < 1.0)
                    {
                        if (this.Increment.TotalMinutes >= 1.0)
                        {
                            zero = new TimeSpan(1, 0, 0);
                        }
                        else
                        {
                            zero = new TimeSpan(0, 1, 0);
                        }
                    }
                    else
                    {
                        zero = new TimeSpan(1, 0, 0, 0);
                    }
                    goto Label_0225;

                case Key.Next:
                    if (this.Increment.TotalDays >= 1.0)
                    {
                        zero = new TimeSpan(this.Increment.Days * 10, 0, 0, 0);
                        break;
                    }
                    if (this.Increment.TotalHours < 1.0)
                    {
                        if (this.Increment.TotalMinutes >= 1.0)
                        {
                            zero = new TimeSpan(1, 0, 0);
                        }
                        else
                        {
                            zero = new TimeSpan(0, 1, 0);
                        }
                        break;
                    }
                    zero = new TimeSpan(1, 0, 0, 0);
                    break;

                case Key.Up:
                    zero = this.Increment;
                    goto Label_0225;

                case Key.Down:
                    zero = this.Increment.Negate();
                    goto Label_0225;

                default:
                    goto Label_0225;
            }
            zero = zero.Negate();
        Label_0225:
            if (zero != TimeSpan.Zero)
            {
                this.Value = new TimeSpan?(this.GetIncrementedValue(zero));
                this.SelectDefaultText();
                return true;
            }
            char c = (keyString != null) ? keyString[0] : '\0';
            if ((char.IsLetterOrDigit(c) || (c == '-')) || char.IsWhiteSpace(c))
            {
                VisualStateHelper.GoToState(this._elementTextBox, "Unwatermarked", false);
            }
            return false;
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
                this.UpdateText();
            }
        }

        private static void OnMaskPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicTimeEditor editor = d as VicTimeEditor;
            string oldValue = (string) e.OldValue;
            editor.OnMaskChanged(oldValue);
        }

        private void OnMaximumChanged(TimeSpan oldValue)
        {
            if ((this.Format == VicTimeEditorFormat.LongTime) || (this.Format == VicTimeEditorFormat.ShortTime))
            {
                if (this.Maximum < TimeSpan.Zero)
                {
                    this.Maximum = TimeSpan.Zero;
                }
                else if (this.Maximum > TimeSpan.FromDays(1.0))
                {
                    this.Maximum = TimeSpan.FromDays(1.0);
                }
            }
            if (this.Minimum > this.Maximum)
            {
                this.Maximum = this.Minimum;
            }
            this._maximum = this.Maximum;
            if (((this.Format == VicTimeEditorFormat.LongTime) || (this.Format == VicTimeEditorFormat.ShortTime)) && (this._maximum > TimeSpan.FromDays(1.0)))
            {
                this._maximum = TimeSpan.FromDays(1.0);
            }
            this.OnValueChanged(this.Value);
        }

        private static void OnMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicTimeEditor editor = d as VicTimeEditor;
            TimeSpan oldValue = (TimeSpan) e.OldValue;
            editor.OnMaximumChanged(oldValue);
        }

        private void OnMinimumChanged(TimeSpan oldValue)
        {
            if ((this.Format == VicTimeEditorFormat.LongTime) || (this.Format == VicTimeEditorFormat.ShortTime))
            {
                if (this.Minimum < TimeSpan.Zero)
                {
                    this.Minimum = TimeSpan.Zero;
                }
                else if (this.Minimum >= TimeSpan.FromDays(1.0))
                {
                    this.Minimum = TimeSpan.FromDays(1.0).Subtract(TimeSpan.FromMilliseconds(1.0));
                }
            }
            if (this.Minimum > this.Maximum)
            {
                this.Maximum = this.Minimum;
            }
            this._minimum = this.Minimum;
            this.OnValueChanged(this.Value);
        }

        private static void OnMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicTimeEditor editor = d as VicTimeEditor;
            TimeSpan oldValue = (TimeSpan) e.OldValue;
            editor.OnMinimumChanged(oldValue);
        }

        protected override void OnMouseWheel(System.Windows.Input.MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (!(!base.IsKeyboardFocusWithin || this.IsReadOnly))
            {
                this.Value = new TimeSpan?(this.GetIncrementedValue((e.Delta > 0) ? this.Increment : this.Increment.Negate()));
                this.SelectDefaultText();
                e.Handled = true;
            }
        }

        private void OnPromptChanged(string oldValue)
        {
            this.OnMaskChanged(this.Mask);
        }

        private static void OnPromptPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicTimeEditor editor = d as VicTimeEditor;
            string oldValue = (string) e.OldValue;
            editor.OnPromptChanged(oldValue);
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

        private static void OnShowButtonsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VicTimeEditor).ChangeVisualStateButtonsVisibility(true);
        }

        protected virtual void OnTextValidationError(TextValidationErrorEventArgs e)
        {
            EventHandler<TextValidationErrorEventArgs> textValidationError = this.TextValidationError;
            if (textValidationError != null)
            {
                textValidationError(this, e);
            }
        }

        protected virtual void OnValueChanged(NullablePropertyChangedEventArgs<TimeSpan> e)
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, e);
            }
        }

        private void OnValueChanged(TimeSpan? oldValue)
        {
            TimeSpan? nullable4;
            TimeSpan? nullable5;
            TimeSpan? nullable = this.Value;
            if (!(nullable.HasValue || this.AllowNull))
            {
                nullable = new TimeSpan?(TimeSpan.Zero);
            }
            if (nullable.HasValue)
            {
                TimeSpan? nullable2 = nullable;
                TimeSpan span = this._minimum;
                if (nullable2.HasValue ? (nullable2.GetValueOrDefault() < span) : false)
                {
                    nullable = new TimeSpan?(this.CycleChangesOnBoundaries ? this._maximum : this._minimum);
                }
                TimeSpan? nullable3 = nullable;
                TimeSpan span2 = this._maximum;
                if (nullable3.HasValue ? (nullable3.GetValueOrDefault() > span2) : false)
                {
                    nullable = new TimeSpan?(this.CycleChangesOnBoundaries ? this._minimum : this._maximum);
                }
            }
            if (nullable.HasValue && (((nullable4 = nullable).HasValue != (nullable5 = this.Value).HasValue) || (nullable4.HasValue && (nullable4.GetValueOrDefault() != nullable5.GetValueOrDefault()))))
            {
                base.SetValue(ValueProperty, nullable);
            }
            else
            {
                this.UpdateText();
                if (oldValue != this.Value)
                {
                    NullablePropertyChangedEventArgs<TimeSpan> e = new NullablePropertyChangedEventArgs<TimeSpan> {
                        OldValue = oldValue,
                        NewValue = this.Value
                    };
                    this.OnValueChanged(e);
                }
            }
        }

        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicTimeEditor editor = d as VicTimeEditor;
            TimeSpan? oldValue = (TimeSpan?) e.OldValue;
            editor.OnValueChanged(oldValue);
        }

        private void OnWatermarkChanged(object oldValue)
        {
            this.UpdateText();
        }

        private static void OnWatermarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicTimeEditor editor = d as VicTimeEditor;
            object oldValue = e.OldValue;
            editor.OnWatermarkChanged(oldValue);
        }

        internal void SelectDefaultText()
        {
            this._elementTextBox.SelectionStart = 0;
            this._elementTextBox.SelectionLength = this._elementTextBox.Text.Length;
        }

        private void SetCustomDefaultValues()
        {
            base.SnapsToDevicePixels = true;
            base.IsTabStop = false;
            this.UpdateText();
        }

        private void UpdateText()
        {
            string str;
            if (!this._isLoaded)
            {
                return;
            }
            DateTimeFormatInfo currentDateTimeFormat = GetCurrentDateTimeFormat();
            VicTimeEditorFormat longTime = this.Format;
            if ((longTime == VicTimeEditorFormat.Custom) && string.IsNullOrEmpty(this.CustomFormat))
            {
                longTime = VicTimeEditorFormat.LongTime;
            }
            if (this.Value.HasValue)
            {
                VisualStateHelper.GoToState(this._elementTextBox, "Unwatermarked", false);
                TimeSpan span = this.Value.Value;
                TimeSpan span2 = new TimeSpan(0, span.Hours, span.Minutes, span.Seconds, span.Milliseconds);
                DateTime time = DateTime.Today.Add(span2.Duration());
                str = string.Empty;
                switch (longTime)
                {
                    case VicTimeEditorFormat.ShortTime:
                        str = string.Format(CultureInfo.CurrentCulture, time.ToString(currentDateTimeFormat.ShortTimePattern, currentDateTimeFormat), new object[0]);
                        break;

                    case VicTimeEditorFormat.LongTime:
                        str = string.Format(CultureInfo.CurrentCulture, time.ToString(currentDateTimeFormat.LongTimePattern, currentDateTimeFormat), new object[0]);
                        break;

                    case VicTimeEditorFormat.TimeSpan:
                        str = this.Value.Value.ToString();
                        break;

                    case VicTimeEditorFormat.Custom:
                        try
                        {
                            str = time.ToString(this.CustomFormat, CultureInfo.CurrentCulture);
                        }
                        catch (FormatException)
                        {
                        }
                        break;
                }
            }
            else
            {
                this._elementTextBox.Text = string.Empty;
                if (!((this.Watermark == null) || string.IsNullOrEmpty(this.Watermark.ToString())))
                {
                    this._elementTextBox.Watermark = this.Watermark;
                }
                else
                {
                    string shortTimePattern = string.Empty;
                    switch (longTime)
                    {
                        case VicTimeEditorFormat.ShortTime:
                            shortTimePattern = currentDateTimeFormat.ShortTimePattern;
                            break;

                        case VicTimeEditorFormat.LongTime:
                            shortTimePattern = currentDateTimeFormat.LongTimePattern;
                            break;

                        case VicTimeEditorFormat.TimeSpan:
                            shortTimePattern = "[-][d.]hh:mm:ss[.fffffff]";
                            break;

                        case VicTimeEditorFormat.Custom:
                            shortTimePattern = this.CustomFormat;
                            break;
                    }
                    this._elementTextBox.Watermark = shortTimePattern.ToString();
                }
                VisualStateHelper.GoToState(this._elementTextBox, "Watermarked", false);
                goto Label_028D;
            }
            if (((this._maskedBox != null) && (this.Mask.Length > str.Length)) && (this.Mask.StartsWith("0") || this.Mask.StartsWith("9")))
            {
                str = "0" + str;
            }
            this._elementTextBox.Text = str;
        Label_028D:
            if (this._needResetWidth)
            {
                this._elementTextBox.ClearValue(FrameworkElement.WidthProperty);
                this._needResetWidth = false;
            }
        }

        private void UpdateValue()
        {
            string text = this._elementTextBox.Text;
            if (this.AllowNull && (string.IsNullOrEmpty(text) || ((base.IsKeyboardFocusWithin && (this._maskedBox != null)) && !this._maskedBox.IsValid)))
            {
                this.Value = null;
                return;
            }
            VicTimeEditorFormat longTime = this.Format;
            if ((longTime == VicTimeEditorFormat.Custom) && string.IsNullOrEmpty(this.CustomFormat))
            {
                longTime = VicTimeEditorFormat.LongTime;
            }
            switch (longTime)
            {
                case VicTimeEditorFormat.ShortTime:
                case VicTimeEditorFormat.LongTime:
                    try
                    {
                        DateTime time = DateTime.Parse(text, GetCurrentDateTimeFormat(), DateTimeStyles.None);
                        this.Value = new TimeSpan?(time.TimeOfDay);
                        return;
                    }
                    catch (FormatException exception)
                    {
                        TextValidationErrorEventArgs e = new TextValidationErrorEventArgs(exception, text);
                        this.OnTextValidationError(e);
                        if (e.ThrowException)
                        {
                            throw e.Exception;
                        }
                        goto Label_0247;
                    }
                    break;

                case VicTimeEditorFormat.TimeSpan:
                    break;

                case VicTimeEditorFormat.Custom:
                    try
                    {
                        DateTime time2;
                        if (!DateTime.TryParseExact(text, this.CustomFormat, GetCurrentDateTimeFormat(), DateTimeStyles.None, out time2))
                        {
                            time2 = DateTime.Parse(text, GetCurrentDateTimeFormat(), DateTimeStyles.None);
                        }
                        this.Value = new TimeSpan?(time2.TimeOfDay);
                        return;
                    }
                    catch (FormatException exception4)
                    {
                        TextValidationErrorEventArgs args5 = new TextValidationErrorEventArgs(exception4, text);
                        this.OnTextValidationError(args5);
                        if (args5.ThrowException)
                        {
                            throw args5.Exception;
                        }
                    }
                    goto Label_0247;

                default:
                    goto Label_0247;
            }
            try
            {
                TimeSpan span = TimeSpan.Parse(text);
                if (this.IsValidValue(span))
                {
                    this.Value = new TimeSpan?(span);
                    return;
                }
                TextValidationErrorEventArgs args2 = new TextValidationErrorEventArgs(new ArgumentOutOfRangeException("text", "TimeSpan value is not valid."), text);
                this.OnTextValidationError(args2);
                if (args2.ThrowException)
                {
                    throw args2.Exception;
                }
            }
            catch (OverflowException exception2)
            {
                TextValidationErrorEventArgs args3 = new TextValidationErrorEventArgs(exception2, text);
                this.OnTextValidationError(args3);
                if (args3.ThrowException)
                {
                    throw args3.Exception;
                }
            }
            catch (FormatException exception3)
            {
                TextValidationErrorEventArgs args4 = new TextValidationErrorEventArgs(exception3, text);
                this.OnTextValidationError(args4);
                if (args4.ThrowException)
                {
                    throw args4.Exception;
                }
            }
        Label_0247:
            this.UpdateText();
            this.SelectDefaultText();
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

        public bool CycleChangesOnBoundaries
        {
            get
            {
                return (bool) base.GetValue(CycleChangesOnBoundariesProperty);
            }
            set
            {
                base.SetValue(CycleChangesOnBoundariesProperty, value);
            }
        }

        public int Delay
        {
            get
            {
                return (int) base.GetValue(DelayProperty);
            }
            set
            {
                base.SetValue(DelayProperty, value);
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

        public VicTimeEditorFormat Format
        {
            get
            {
                return (VicTimeEditorFormat) base.GetValue(FormatProperty);
            }
            set
            {
                base.SetValue(FormatProperty, value);
            }
        }

        public bool HandleUpDownKeys
        {
            get
            {
                return (bool) base.GetValue(HandleUpDownKeysProperty);
            }
            set
            {
                base.SetValue(HandleUpDownKeysProperty, value);
            }
        }

        public TimeSpan Increment
        {
            get
            {
                return (TimeSpan) base.GetValue(IncrementProperty);
            }
            set
            {
                base.SetValue(IncrementProperty, value);
            }
        }

        public int Interval
        {
            get
            {
                return (int) base.GetValue(IntervalProperty);
            }
            set
            {
                base.SetValue(IntervalProperty, value);
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

        public TimeSpan Maximum
        {
            get
            {
                return (TimeSpan) base.GetValue(MaximumProperty);
            }
            set
            {
                base.SetValue(MaximumProperty, value);
            }
        }

        public TimeSpan Minimum
        {
            get
            {
                return (TimeSpan) base.GetValue(MinimumProperty);
            }
            set
            {
                base.SetValue(MinimumProperty, value);
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

        public bool ShowButtons
        {
            get
            {
                return (bool) base.GetValue(ShowButtonsProperty);
            }
            set
            {
                base.SetValue(ShowButtonsProperty, value);
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

        public TimeSpan? Value
        {
            get
            {
                return (TimeSpan?) base.GetValue(ValueProperty);
            }
            set
            {
                base.SetValue(ValueProperty, value);
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

