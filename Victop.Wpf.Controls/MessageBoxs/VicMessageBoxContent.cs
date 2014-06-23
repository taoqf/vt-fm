using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace Victop.Wpf.Controls
{
    public class VicMessageBoxContent : Control
    {
        internal Panel _elementButtons;
        internal Image _elementIcon;
        private bool _throwIsMouseOverChanged;
        public static readonly DependencyProperty ButtonProperty = DependencyProperty.Register("Button", typeof(VicMessageBoxButton), typeof(VicMessageBoxContent), new PropertyMetadata(new PropertyChangedCallback(VicMessageBoxContent.OnButtonPropertyChanged)));
        internal const string ButtonsElementName = "Buttons";
        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(VicMessageBoxContent), null);
        public static readonly DependencyProperty CancelTextProperty = DependencyProperty.Register("CancelText", typeof(string), typeof(VicMessageBoxContent), null);
        internal static readonly DependencyProperty ForceMouseOverProperty = DependencyProperty.Register("ForceMouseOver", typeof(bool), typeof(VicMessageBoxContent), new PropertyMetadata(new PropertyChangedCallback(VicMessageBoxContent.OnForceMouseOverPropertyChanged)));
        internal const string IconElementName = "Icon";
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(VicMessageBoxIcon), typeof(VicMessageBoxContent), new PropertyMetadata(new PropertyChangedCallback(VicMessageBoxContent.OnIconPropertyChanged)));
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(VicMessageBoxContent), null);
        public static readonly DependencyProperty NoTextProperty = DependencyProperty.Register("NoText", typeof(string), typeof(VicMessageBoxContent), null);
        public static readonly DependencyProperty OKTextProperty = DependencyProperty.Register("OKText", typeof(string), typeof(VicMessageBoxContent), null);
        public static readonly DependencyProperty YesTextProperty = DependencyProperty.Register("YesText", typeof(string), typeof(VicMessageBoxContent), null);

        public event Action<MessageBoxResult> ButtonClick;

        public event EventHandler<PropertyChangedEventArgs<bool>> IsMouseOverChanged;

        public VicMessageBoxContent()
        {
            RoutedEventHandler handler3 = null;
            DependencyPropertyChangedEventHandler handler4 = null;
            RoutedEventHandler handler = null;
            DependencyPropertyChangedEventHandler handler2 = null;
            this._throwIsMouseOverChanged = true;
            base.DefaultStyleKey = typeof(VicMessageBoxContent);
            if (handler == null)
            {
                if (handler3 == null)
                {
                    handler3 = delegate (object param0, RoutedEventArgs param1) {
                        this.ChangeVisualStateIcon(false);
                        this.ChangeVisualStateCommon(false);
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

        private void AddButton(string text, MessageBoxResult result)
        {
            System.Windows.Controls.Button element = new System.Windows.Controls.Button {
                Content = text
            };
            this._elementButtons.Children.Add(element);
            if (this.ButtonStyle != null)
            {
                element.Style = this.ButtonStyle;
            }
            element.Click += delegate (object s, RoutedEventArgs e) {
                if (this.ButtonClick != null)
                {
                    this.ButtonClick(result);
                }
            };
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
        }

        protected void ChangeVisualStateIcon(bool useTransitions)
        {
            if (this.Icon == VicMessageBoxIcon.Information)
            {
                VisualStateHelper.GoToState(this, "Information", useTransitions);
            }
            if (this.Icon == VicMessageBoxIcon.None)
            {
                VisualStateHelper.GoToState(this, "Normal", useTransitions);
            }
            if (this.Icon == VicMessageBoxIcon.Question)
            {
                VisualStateHelper.GoToState(this, "Question", useTransitions);
            }
            if (this.Icon == VicMessageBoxIcon.Warning)
            {
                VisualStateHelper.GoToState(this, "Warning", useTransitions);
            }
            if (this.Icon == VicMessageBoxIcon.Error)
            {
                VisualStateHelper.GoToState(this, "Error", useTransitions);
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

        private void InitializeButtonsPart()
        {
            this.OKText = this.OKText ?? "确定";
            this.CancelText = this.CancelText ?? "取消";
            this.YesText = this.YesText ?? "是";
            this.NoText = this.NoText ?? "否";
            this.OnButtonChanged(VicMessageBoxButton.OK);
        }

        private void OnAfterApplyTemplate()
        {
            this.OnIconChanged(VicMessageBoxIcon.None);
        }

        public override void OnApplyTemplate()
        {
            string errors = string.Empty;
            base.OnApplyTemplate();
            this._elementButtons = this.GetTemplateChild<Panel>("Buttons", true, ref errors);
            if (this._elementButtons != null)
            {
                this.InitializeButtonsPart();
            }
            this._elementIcon = this.GetTemplateChild<Image>("Icon", true, ref errors);
            if (!string.IsNullOrEmpty(errors))
            {
                if (!DesignerProperties.GetIsInDesignMode(this))
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Template cannot be applied to VicMessageBoxContent.\nDetails: {0}", new object[] { errors }));
                }
            }
            else
            {
                this.ChangeVisualStateIcon(false);
                this.ChangeVisualStateCommon(false);
                this.OnAfterApplyTemplate();
            }
        }

        private void OnButtonChanged(VicMessageBoxButton oldValue)
        {
            if (this._elementButtons != null)
            {
                this._elementButtons.Children.Clear();
                switch (this.Button)
                {
                    case VicMessageBoxButton.OK:
                        this.AddButton(this.OKText, MessageBoxResult.OK);
                        break;

                    case VicMessageBoxButton.OKCancel:
                        this.AddButton(this.OKText, MessageBoxResult.OK);
                        this.AddButton(this.CancelText, MessageBoxResult.Cancel);
                        break;

                    case VicMessageBoxButton.YesNoCancel:
                        this.AddButton(this.YesText, MessageBoxResult.Yes);
                        this.AddButton(this.NoText, MessageBoxResult.No);
                        this.AddButton(this.CancelText, MessageBoxResult.Cancel);
                        break;

                    case VicMessageBoxButton.YesNo:
                        this.AddButton(this.YesText, MessageBoxResult.Yes);
                        this.AddButton(this.NoText, MessageBoxResult.No);
                        break;
                }
            }
        }

        private static void OnButtonPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicMessageBoxContent content = d as VicMessageBoxContent;
            VicMessageBoxButton oldValue = (VicMessageBoxButton)e.OldValue;
            content.OnButtonChanged(oldValue);
        }

        private static void OnForceMouseOverPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as VicMessageBoxContent).ChangeVisualStateCommon(true);
        }

        private void OnIconChanged(VicMessageBoxIcon oldValue)
        {
            if ((this._elementIcon != null) && (this.Icon != VicMessageBoxIcon.None))
            {
                Uri uriResource = new Uri("/Victop.Wpf.Controls;component/Resources/" + this.Icon + ".png", UriKind.Relative);
                try
                {
                    StreamResourceInfo resourceStream = Application.GetResourceStream(uriResource);
                    if (resourceStream != null)
                    {
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = resourceStream.Stream;
                        image.EndInit();
                        this._elementIcon.Source = image;
                    }
                }
                catch
                {
                }
            }
        }

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicMessageBoxContent content = d as VicMessageBoxContent;
            VicMessageBoxIcon oldValue = (VicMessageBoxIcon)e.OldValue;
            content.OnIconChanged(oldValue);
            content.ChangeVisualStateIcon(true);
        }

        private static void OnIsMouseOverPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicMessageBoxContent sender = d as VicMessageBoxContent;
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

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == UIElement.IsMouseOverProperty)
            {
                OnIsMouseOverPropertyChanged(this, e);
            }
        }

        private void SetCustomDefaultValues()
        {
        }

        public VicMessageBoxButton Button
        {
            get
            {
                return (VicMessageBoxButton) base.GetValue(ButtonProperty);
            }
            set
            {
                base.SetValue(ButtonProperty, value);
            }
        }

        public Style ButtonStyle
        {
            get
            {
                return (Style) base.GetValue(ButtonStyleProperty);
            }
            set
            {
                base.SetValue(ButtonStyleProperty, value);
            }
        }

        public string CancelText
        {
            get
            {
                return (string) base.GetValue(CancelTextProperty);
            }
            set
            {
                base.SetValue(CancelTextProperty, value);
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

        public VicMessageBoxIcon Icon
        {
            get
            {
                return (VicMessageBoxIcon) base.GetValue(IconProperty);
            }
            set
            {
                base.SetValue(IconProperty, value);
            }
        }

        public string Message
        {
            get
            {
                return (string) base.GetValue(MessageProperty);
            }
            set
            {
                base.SetValue(MessageProperty, value);
            }
        }

        public string NoText
        {
            get
            {
                return (string) base.GetValue(NoTextProperty);
            }
            set
            {
                base.SetValue(NoTextProperty, value);
            }
        }

        public string OKText
        {
            get
            {
                return (string) base.GetValue(OKTextProperty);
            }
            set
            {
                base.SetValue(OKTextProperty, value);
            }
        }

        public string YesText
        {
            get
            {
                return (string) base.GetValue(YesTextProperty);
            }
            set
            {
                base.SetValue(YesTextProperty, value);
            }
        }
    }
}
