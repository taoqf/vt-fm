using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Victop.Themes.ClassicSkin
{
    public class Skin
    {
        /// <summary>
        /// 皮肤路径
        /// </summary>
        public int SkinOrder = 0;

        /// <summary>
        /// 皮肤名称
        /// </summary>
        public string SkinName = "默认皮肤";

        /// <summary>
        /// 皮肤xaml文件
        /// </summary>
        public string ThemeName = "/Victop.Themes.ClassicSkin;component/Styles.xaml";

        /// <summary>
        /// 皮肤图标
        /// </summary>
        public string SkinFace = "/Victop.Themes.ClassicSkin;component/Images/loginbg.png";
    }
}
