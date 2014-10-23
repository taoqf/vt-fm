using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThemeManagerPlugin.Models
{
    public class ThemeModel : IComparable<ThemeModel>
    {
        private int skinOrder = 0;

        /// <summary>
        /// 皮肤序号
        /// </summary>
        public int SkinOrder
        {
            get { return skinOrder; }
            set { skinOrder = value; }
        }

        private string skinPath;

        /// <summary>
        /// 皮肤路径
        /// </summary>
        public string SkinPath
        {
            get
            {
                return skinPath;
            }
            set
            {
                skinPath = value;
            }
        }

        private string themeName;

        /// <summary>
        /// 样式文件名称
        /// </summary>
        public string ThemeName
        {
            get
            {
                return themeName;
            }
            set
            {
                themeName = value;
            }
        }

        private string skinName;

        /// <summary>
        /// 皮肤名称
        /// </summary>
        public string SkinName
        {
            get
            {
                return skinName;
            }
            set
            {
                skinName = value;
            }
        }

        public int CompareTo(ThemeModel other)
        {
            if (other == null)
                return 1;
            int value = this.SkinOrder - other.SkinOrder;
            return value;
        }
    }
}
