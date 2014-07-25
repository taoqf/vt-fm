using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace ThemeManagerPlugin.Models
{
  public  class ThemeModel:ModelBase
    {
        private string _themeName;
        public string ThemeName
        {
            get { return _themeName; }
            set
            {
                if (_themeName != value)
                {
                    _themeName = value;
                    RaisePropertyChanged("ThemeName");
                }
            }
        }
        private string _skinName;
        public string SkinName
        {
            get { return _skinName; }
            set {
                 if (_skinName != value)
                {
                    _skinName = value;
                    RaisePropertyChanged("SkinName");
                }
            }
        }
    }
}
