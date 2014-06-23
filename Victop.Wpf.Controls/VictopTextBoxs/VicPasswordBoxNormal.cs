using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
    ///     xmlns:MyNamespace="clr-namespace:Victop.Wpf.Controls.VictopTextBoxs"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Victop.Wpf.Controls.VictopTextBoxs;assembly=Victop.Wpf.Controls.VictopTextBoxs"
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
    ///     <MyNamespace:VicTextBoxWaterMarkPassword/>
    ///
    /// </summary>
    public class VicPasswordBoxNormal : TextBox
    {
        public static readonly DependencyProperty WateMarkProperty = DependencyProperty.Register("WateMark", typeof(string), typeof(VicPasswordBoxNormal), new UIPropertyMetadata(string.Empty));

        [DefaultValue("")]
        [Description("获取或设置水印")]
        public string WateMark
        {
            get { return (string)GetValue(WateMarkProperty); }

            set { SetValue(WateMarkProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(VicPasswordBoxNormal), new PropertyMetadata(OnPasswordPropertyChanged));

        [DefaultValue("")]
        [Description("获取或设置水印")]
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }

            set { SetValue(PasswordProperty, value); }
        }
        private TextBlock WatermarkContent;
       private PasswordBox PasswordContent;  
        static VicPasswordBoxNormal()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VicPasswordBoxNormal), new FrameworkPropertyMetadata(typeof(VicPasswordBoxNormal)));
        }

        public VicPasswordBoxNormal()  
        {
            DefaultStyleKey = typeof(VicPasswordBoxNormal);
            this.GotFocus += new RoutedEventHandler(VicPasswordBoxNormal_GotFocus);
            this.LostFocus += new RoutedEventHandler(VicPasswordBoxNormal_LostFocus);
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.WatermarkContent = this.GetTemplateChild("watermarkContent") as TextBlock;
            this.PasswordContent = this.GetTemplateChild("ContentElement") as PasswordBox;
            if (WatermarkContent != null && PasswordContent != null)
            {
                PasswordContent.LostKeyboardFocus += PasswordContent_LostKeyboardFocus;
                PasswordContent.PasswordChanged += PasswordContent_PasswordChanged;
            }
        }
        void PasswordContent_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.DetermineWatermarkContentVisibility();
        }

        void PasswordContent_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            PasswordBox passwdBx = sender as PasswordBox;
            if (passwdBx != null)
            {
                this.Password = passwdBx.Password;
            }
        }
           void VicPasswordBoxNormal_GotFocus(object sender, RoutedEventArgs e)
        {
            if (WatermarkContent != null && !string.IsNullOrEmpty(this.PasswordContent.Password))
            {
                this.WatermarkContent.Visibility = Visibility.Collapsed;
            }
            PasswordContent.Focus();  
        }
        void VicPasswordBoxNormal_LostFocus(object sender, RoutedEventArgs e)
        {
            if (WatermarkContent != null && string.IsNullOrEmpty(this.PasswordContent.Password))
            {
                this.WatermarkContent.Visibility = Visibility.Visible;
            }
        }
       
       private void DetermineWatermarkContentVisibility()  
       {  
         if (string.IsNullOrEmpty(this.PasswordContent.Password))  
         {  
             this.WatermarkContent.Visibility = Visibility.Visible;  
         }  
         else  
         {  
             this.WatermarkContent.Visibility = Visibility.Collapsed;  
         }  
       }  

       private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)  
       {
           VicPasswordBoxNormal watermarkTextBox = sender as VicPasswordBoxNormal;  
           if (watermarkTextBox != null && watermarkTextBox.PasswordContent != null)  
           {
               watermarkTextBox.PasswordContent.Password = watermarkTextBox.Password; 
           }  
       }  

    }
}

