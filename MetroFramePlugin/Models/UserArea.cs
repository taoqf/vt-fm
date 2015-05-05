using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace MetroFramePlugin.Models
{
    public class UserArea : PropertyModelBase
    {
        /// <summary>
        ///用户Code
        /// </summary>
        private string userCode=string.Empty;
        public string UserCode
        {
            get { return userCode; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) == false)
                {
                    userCode = value;
                    RaisePropertyChanged("UserCode");
                }
            }
        }
        /// <summary>
        /// 区域菜单
        /// </summary>
        private ObservableCollection<AreaMenu> userAreaMenu;
        public ObservableCollection<AreaMenu> UserAreaMenu
        {
            get
            {
                if (userAreaMenu == null)
                    userAreaMenu = new ObservableCollection<AreaMenu>();
                return userAreaMenu;
            }
            set
            {
                if (userAreaMenu != value)
                {
                    userAreaMenu = value;
                    RaisePropertyChanged("UserAreaMenu");
                }
            }
        }
    }
}
