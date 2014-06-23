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
    ///     xmlns:MyNamespace="clr-namespace:Victop.Wpf.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Victop.Wpf.Controls;assembly=Victop.Wpf.Controls"
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
    ///     <MyNamespace:VicButtonLeftRight/>
    ///
    /// </summary>
    public class VicButtonLeftRight : Button
    {
        public static DependencyProperty LeftBackgroundProperty = DependencyProperty.Register("LeftBackground", typeof(Brush), typeof(VicButtonLeftRight), new PropertyMetadata(OnLeftBackgroundPropertyChanged));

        [DefaultValue("")]
        [Description("获取或设置左方区域背景图片")]
        public Brush LeftBackground
        {
            get { return (Brush)GetValue(LeftBackgroundProperty); }

            set { SetValue(LeftBackgroundProperty, value); }
        }

        public static DependencyProperty RightContentProperty = DependencyProperty.Register("RightContent", typeof(string), typeof(VicButtonLeftRight), new PropertyMetadata(OnRightContentPropertyChanged));

        [DefaultValue("")]
        [Description("获取或设置右方区域显示文字")]
        public string RightContent
        {
            get { return (string)GetValue(RightContentProperty); }

            set { SetValue(RightContentProperty, value); }
        }

        private Grid leftGrid;
        private Label rightLabel;

        static VicButtonLeftRight()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VicButtonLeftRight), new FrameworkPropertyMetadata(typeof(VicButtonLeftRight)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template != null)
            {
                leftGrid = Template.FindName("left", this) as Grid;
                rightLabel = Template.FindName("right", this) as Label;
            }
        }

        private static void OnLeftBackgroundPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            VicButtonLeftRight btn = sender as VicButtonLeftRight;
            if (btn != null && btn.leftGrid != null)
            {
                btn.leftGrid.Background = btn.LeftBackground;
            }
        }

        private static void OnRightContentPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            VicButtonLeftRight btn = sender as VicButtonLeftRight;
            if (btn != null && btn.rightLabel != null)
            {
                btn.rightLabel.Content = btn.RightContent;
            }
        }  
    }
}
