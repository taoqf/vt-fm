using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Victop.Server.Controls.Models;

namespace ThemeManagerPlugin.Models
{
    public class ThemeModel : PropertyModelBase
    {
        /// <summary>
        /// 版本号
        /// </summary>
        private int themeVerion = 0;
        public int ThemeVerion
        {
            get { return themeVerion; }
            set
            {
                if (themeVerion != value)
                {
                    themeVerion = value;
                    RaisePropertyChanged(()=> ThemeVerion);
                }
            }
        }
        private int skinOrder = 0;

        /// <summary>
        /// 皮肤序号
        /// </summary>
        public int SkinOrder
        {
            get { return skinOrder; }
            set { skinOrder = value; }
        }

        /// <summary>
        /// 状态改变
        /// </summary>
        private bool stateType;
        public bool StateType
        {
            get { return stateType; }
            set
            {
                if (stateType != value)
                {
                    stateType = value;
                    RaisePropertyChanged(()=> StateType);
                }
            }
        }

        /// <summary>
        /// 状态改变
        /// </summary>
        private bool stateChange;
        public bool StateChange
        {
            get { return stateChange; }
            set
            {
                if (stateChange != value)
                {
                    stateChange = value;
                    //RaisePropertyChanged("StateChange");
                }
            }
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

        /// <summary>
        /// 皮肤图标
        /// </summary>
        public string SkinFace
        {
            get;
            set;
        }
        public string SkinDllName
        {
            get;
            set;
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
