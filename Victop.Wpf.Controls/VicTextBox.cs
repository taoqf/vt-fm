using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Victop.Wpf.Controls
{
    public class VicTextBox : TextBox
    {

        #region 变量或者属性
        /// <summary>
        /// 文本框
        /// </summary>
        private TextBox mInputTextBox = null;
        /// <summary>
        /// 按钮
        /// </summary>
        private Button mOpenButton = null;
        public delegate void onBtnClick(object sender, RoutedEventArgs e);
        /// <summary>
        /// 按钮单击事件
        /// </summary>
        public event onBtnClick VicTextBoxClick;
        private double textBoxWidth = 80;   //文本框默认宽度设为100像素
        /// <summary>
        /// 文本框宽度
        /// </summary>
        public double TextBoxWidth
        {
            get { return textBoxWidth; }
            set { textBoxWidth = value; }
        }

        public string VicText
        {
            get { return (string)GetValue(VicTextProperty); }
            set { SetValue(VicTextProperty, value); }
        }
        public static readonly DependencyProperty VicTextProperty = DependencyProperty.Register("VicText", typeof(string), typeof(VicTextBox), new FrameworkPropertyMetadata(OnVicTextPropertyChanged));

        public static readonly DependencyProperty HideDataRefButtonProperty = DependencyProperty.Register("HideDataRefButton", typeof(bool), typeof(VicTextBox), new PropertyMetadata(default(bool)));

        [Category("Appearance")]
        [DefaultValue(false)]
        public bool HideDataRefButton
        {
            get { return (bool)GetValue(HideDataRefButtonProperty); }
            set { SetValue(HideDataRefButtonProperty, value); }
        }
        #endregion

        #region 静态函数
        /// <summary>
        /// 静态函数
        /// </summary>
        static VicTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VicTextBox), new FrameworkPropertyMetadata(typeof(VicTextBox)));
            //FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VicTextBox), new FrameworkPropertyMetadata(typeof(VicTextBox)));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(VicTextBox), new FrameworkPropertyMetadata(VerticalAlignment.Center));
            HorizontalContentAlignmentProperty.OverrideMetadata(typeof(VicTextBox), new FrameworkPropertyMetadata(HorizontalAlignment.Right));
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public VicTextBox()
        {
            Height = 23; //自定义控件整体高度默认为23像素
        }
        #endregion

        #region 重写OnApplyTemplate
        /// <summary>
        /// 重写OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template != null)
            {
                mInputTextBox = Template.FindName("PART_Input", this) as TextBox;
                mOpenButton = Template.FindName("PART_Open", this) as Button;
            }
            Attach();
        }
        #endregion

        #region Button单击事件
        /// <summary>
        /// Button单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mOpenButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VicTextBoxClick(this, e);
            }
            catch (Exception ex)
            {              
                
            }
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        private void Attach()
        {
            if (mOpenButton != null)
            {                
                mOpenButton.Click += mOpenButton_Click;
                mOpenButton.Height = Height;
            }
            if (mInputTextBox != null)
            {
                mInputTextBox.Text = VicText;              
                mInputTextBox.PreviewKeyDown += PreviewTextBoxKeyDown;
                mInputTextBox.LostKeyboardFocus += TextBoxLostKeyboardFocus;
            }
        }
        #endregion

        #region 文本框KeyDown事件
        /// <summary>
        /// 文本框KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviewTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    VicText = mInputTextBox.Text;
                    VicTextBoxClick(sender, e);
                }
            }
            catch (Exception ex)
            {               
                
            }
        }
        #endregion

        #region 文本框焦点失去事件
        /// <summary>
        /// 文本框焦点失去事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            VicText = mInputTextBox.Text;
        }
        #endregion

        #region VicText依赖属性值改变事件
        /// <summary>
        /// VicText依赖属性值改变事件
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnVicTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VicTextBox vicTextBox = d as VicTextBox;
            if (vicTextBox != null && vicTextBox.mInputTextBox != null)
            {
                vicTextBox.mInputTextBox.Text = vicTextBox.VicText;
            }
        }
        #endregion
    }
}
