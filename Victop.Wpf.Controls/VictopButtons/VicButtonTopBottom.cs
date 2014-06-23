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
    ///     <MyNamespace:VicButtonTopBottom/>
    ///
    /// </summary>
    public class VicButtonTopBottom : Button
    {
        public static DependencyProperty TopBackgroundProperty = DependencyProperty.Register("TopBackground", typeof(Brush), typeof(VicButtonTopBottom), new PropertyMetadata(OnTopBackgroundPropertyChanged));

        [DefaultValue("")]
        [Description("获取或设置上方区域背景图片")]
        public Brush TopBackground
        {
            get { return (Brush)GetValue(TopBackgroundProperty); }

            set { SetValue(TopBackgroundProperty, value); }
        }

        public static DependencyProperty BottomContentProperty = DependencyProperty.Register("BottomContent", typeof(string), typeof(VicButtonTopBottom), new PropertyMetadata(OnBottomContentPropertyChanged));

        [DefaultValue("")]
        [Description("获取或设置下方区域显示文字")]
        public string BottomContent
        {
            get { return (string)GetValue(BottomContentProperty); }

            set { SetValue(BottomContentProperty, value); }
        }

        private Grid topGrid;
        private Label bottomLabel;
        static VicButtonTopBottom()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VicButtonTopBottom), new FrameworkPropertyMetadata(typeof(VicButtonTopBottom)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template != null)
            {
                topGrid = Template.FindName("top", this) as Grid;
                bottomLabel = Template.FindName("bottom", this) as Label;
            }
        }

        private static void OnTopBackgroundPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            VicButtonTopBottom btn = sender as VicButtonTopBottom;
            if (btn != null && btn.topGrid != null)
            {
                btn.topGrid.Background = btn.TopBackground;
            }
        }

        private static void OnBottomContentPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            VicButtonTopBottom btn = sender as VicButtonTopBottom;
            if (btn != null && btn.bottomLabel != null)
            {
                btn.bottomLabel.Content = btn.BottomContent;
            }
        }  
    }
}
