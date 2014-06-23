using System;
using System.Collections.Generic;
using System.IO;
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
using System.Configuration;
using System.Xml;
using Victop.Server.Controls;

namespace ThemeManagerPlugin.Views
{
    /// <summary>
    /// ThemesWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ThemesWindow : Window
    {
        string ThemePath;//当前主题路径
        public ThemesWindow()
        {
            InitializeComponent();

            GetThemePath();

            Screeninitialization();
        }

        private void GetThemePath()
        {
            //XmlDocument xml = new XmlDocument();
            //string appPath = AppDomain.CurrentDomain.BaseDirectory;
            //string strFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "VictopFrameWork.exe.config";
            //xml.Load(strFileName);   //XML地址
            ////找出名称为“add”的所有元素
            //XmlNodeList nodes = xml.GetElementsByTagName("add");
            //for (int i = 0; i < nodes.Count; i++)
            //{
            //    //获得将当前元素的key属性
            //    XmlAttribute att = nodes[i].Attributes["key"];
            //    //根据元素的第一个属性来判断当前的元素是不是目标元素
            //    if (att.Value == "skinurl")
            //    {
            //        //对目标元素中的第二个属性赋值
            //        att = nodes[i].Attributes["value"];
            //        ThemePath = att.Value;
            //        break;
            //    }
            //}
            ThemePath = ConfigurationManager.AppSettings.Get("skinurl");
        }
        /// <summary>
        /// 动态布局
        /// </summary>
        private void Screeninitialization()
        {
            //获取主题包中主题信息
            string[] dir = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "theme");

            string[] ThemePathpart = ThemePath.Split("/".ToCharArray());
            string Themename = ThemePathpart[ThemePathpart.Length - 2].ToString();

            tblockSkinNum.Text = ThemePathpart.Length.ToString();

            //循环添加控件
            for (int i = 0; i < dir.Length; i++)
            {
                StackPanel stpnl = new StackPanel();
                stpnl.Width = 203;
                stpnl.Height = 142;
                stpnl.Margin = new Thickness(12, 10, 0, 10);
                //当前循环中主题包路径
                string skinallpath = dir[i].ToString();
                //获取主题包名称
                string[] skinnamepart = skinallpath.Split("\\".ToCharArray());
                string skinname = skinnamepart[skinnamepart.Length - 1].ToString();

                //创建一个按钮
                RadioButton btnskin = new RadioButton();
                //把主题包名称记录
                btnskin.Tag = skinname;
                //定义按钮上下左右的间距
                //btnskin.Margin = new Thickness(10, 10, 0, 10);
                btnskin.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                //设置按钮的高度
                btnskin.Height = 122;
                //设置按钮的宽度
                btnskin.Width = 203;
                ////设置皮肤名字
                //btnskin.Content = "             皮肤" + skinname.Substring(4, skinname.Length - 4);
                //设置按钮的背景（主题缩略图并添加样式）
                ResourceDictionary resource = new ResourceDictionary();
                resource.Source = new Uri(skinallpath + "\\Resources.xaml", UriKind.RelativeOrAbsolute);
                btnskin.Style = resource["btnChSkinBackGroudStyle"] as Style;
                //控制按钮不同容器互斥
                btnskin.GroupName = "ChangeSkin";
                //ImageBrush im = new ImageBrush();
                //im.ImageSource = new BitmapImage(new Uri(skinallpath + "\\index.jpg", UriKind.RelativeOrAbsolute));
                //btnskin.Background = im;
                //程序当前选中样式使得该按钮选中
                if (skinname == Themename)
                {
                    btnskin.IsChecked = true;
                }
                else
                {
                    btnskin.IsChecked = false;
                }

                //添加单击事件
                btnskin.Checked += new RoutedEventHandler(btnskin_Checked);
                stpnl.Children.Add(btnskin);

                TextBlock tblockName = new TextBlock();
                tblockName.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                tblockName.Width = 203;
                tblockName.Height = 20;
                tblockName.Text = "         皮肤" + skinname.Substring(4, skinname.Length - 4);
                //tblockName.TextAlignment = TextAlignment.Center;
                stpnl.Children.Add(tblockName);




                //加到 WrapPanel 里面
                wp.Children.Add(stpnl);
            }

        }

        //按钮单击的事件
        private void btnskin_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton btnskin = (RadioButton)sender;

            string skinname = (string)btnskin.Tag;
            //FrameWorkThemeOperation themeOp = new FrameWorkThemeOperation();
            ////临时定义皮肤资源
            //string tempTheme = "theme/" + skinname + "/Resources.xaml";
            //themeOp.ChangeFrameWorkTheme(tempTheme);
        }

        #region  最小化 关闭操作
        /// <summary>最小化</summary>
        private void Mini_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        /// <summary>关闭</summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

 
        #endregion

        private void titleBar_MouseMove(object sender, MouseEventArgs e)
        {
            titleBar.AllowDrop = true;
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
