using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PortalFramePlugin.Models;

namespace PortalFramePlugin
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PortalFramePlugin"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PortalFramePlugin;assembly=PortalFramePlugin"
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
    ///     <MyNamespace:VicRadioButtonSecondLevelMenu/>
    ///
    /// </summary>
    public class VicRadioButtonSecondLevelMenu : RadioButton
    {

        public static readonly DependencyProperty ImgNameProperty = DependencyProperty.Register("ImgName", typeof(string), typeof(VicRadioButtonSecondLevelMenu), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ChildMenuSourceProperty = DependencyProperty.Register("ChildMenuSource", typeof(IEnumerable), typeof(VicRadioButtonSecondLevelMenu), new FrameworkPropertyMetadata(OnChildMenuSourcePropertyChanged));
        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(VicRadioButtonSecondLevelMenu), new PropertyMetadata(default(string)));

        private ListBox listBox;
        #region 属性
        [Category("Appearance")]
        public string ImgName
        {
            get { return (string)GetValue(ImgNameProperty); }
            set { SetValue(ImgNameProperty, value); }
        }

        [Category("Appearance")]
        public IEnumerable ChildMenuSource
        {
            get { return (IEnumerable)GetValue(ChildMenuSourceProperty); }
            set { SetValue(ChildMenuSourceProperty, value); }
        }

        [Category("Appearance")]
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        #endregion

        static VicRadioButtonSecondLevelMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VicRadioButtonSecondLevelMenu), new FrameworkPropertyMetadata(typeof(VicRadioButtonSecondLevelMenu)));
        }

        #region 重写OnApplyTemplate
        /// <summary>
        /// 重写OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template != null)
            {
                listBox = Template.FindName("listBox", this) as ListBox;
            }
        }
        #endregion

        private static void OnChildMenuSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicRadioButtonSecondLevelMenu radioBtn = d as VicRadioButtonSecondLevelMenu;
            if (radioBtn != null && radioBtn.listBox!=null)
            {
                if (radioBtn.ChildMenuSource != null && ((ObservableCollection<MenuModel>)radioBtn.ChildMenuSource).Count > 0) return;
                radioBtn.listBox.Width = 0;
                radioBtn.listBox.Height = 0;
            }
        }
    }
}
