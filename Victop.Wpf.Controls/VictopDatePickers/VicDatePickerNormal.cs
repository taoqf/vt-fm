using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Victop.Wpf.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Victop.Wpf.Controls.VictopDatePickers"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Victop.Wpf.Controls.VictopDatePickers;assembly=Victop.Wpf.Controls.VictopDatePickers"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:VicDatePickerNormal/>
    ///
    /// </summary>
    public class VicDatePickerNormal : DatePicker
    {
        //
        // 摘要:
        //     获取或设置自定义日期/时间格式字符串。
        //
        // 返回结果:
        //     表示自定义日期/时间格式的字符串。默认值为 null。
        [DefaultValue("")]
        [Description("获取或设置自定义日期/时间格式字符串")]
        public string FormatText
        {
            get { return (string)GetValue(FormatTextProperty); }
            set { SetValue(FormatTextProperty, value); }
        }

        //
        // 摘要:
        //     获取或设置自定义日期/时间格式字符串。
        //
        // 返回结果:
        //     表示自定义日期/时间格式的字符串。默认值为 null。
        [DefaultValue("")]
        [Description("获取或设置自定义日期/时间格式字符串")]
        public string StringFormat
        {
            get { return (string)GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }
        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register("StringFormat", typeof(string), typeof(VicDatePickerNormal), new FrameworkPropertyMetadata(OnStringFormatPropertyChanged));

        public static readonly DependencyProperty FormatTextProperty = DependencyProperty.Register("FormatText", typeof(string), typeof(VicDatePickerNormal), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty HideButtonProperty = DependencyProperty.Register("HideButton", typeof(bool), typeof(VicDatePickerNormal), new PropertyMetadata(default(bool)));

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool HideButton
        {
            get { return (bool)GetValue(HideButtonProperty); }
            set { SetValue(HideButtonProperty, value); }
        }
        static VicDatePickerNormal()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VicDatePickerNormal), new FrameworkPropertyMetadata(typeof(VicDatePickerNormal)));
            SelectedDateProperty.OverrideMetadata(typeof(VicDatePickerNormal), new FrameworkPropertyMetadata(OnSelectedDatePropertyChanged));
        }


        public VicDatePickerNormal()
        {
           
            //this.SelectedDateChanged += VicDatePickerNormal_SelectedDateChanged;
        }

        //void VicDatePickerNormal_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    this.SelectedDate = DateTime.Parse(this.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
        //    this.Text = (DateTime.Parse(this.Text)).ToString("yyyy-MM-dd HH:mm:ss");
        //}
        private static void OnSelectedDatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicDatePickerNormal vicDatePicker = d as VicDatePickerNormal;
            if (vicDatePicker != null )
            {
                vicDatePicker.FormatText = vicDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private static void OnStringFormatPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //VicDatePickerNormal vicDatePicker = d as VicDatePickerNormal;
            //if (vicDatePicker != null)
            //{
            //    Thread.CurrentThread.CurrentCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            //    Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern = vicDatePicker.StringFormat;
            //}
        }
    }
}
