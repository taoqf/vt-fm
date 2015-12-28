﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace ChangeRolePlugin.Models
{
   public class ChangeRoleModel:ModelBase
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        private string roleNo;
        /// <summary>
        /// 角色编号
        /// </summary>
        public string Role_No
        {
            get
            {
                return roleNo;
            }
            set
            {
                if (roleNo != value)
                {
                    roleNo = value;
                    RaisePropertyChanged(()=> Role_No);
                }
            }
        }
        /// <summary>
        /// 角色编号
        /// </summary>
        private string roleName;
        /// <summary>
        /// 角色编号
        /// </summary>
        public string Role_Name
        {
            get
            {
                return roleName;
            }
            set
            {
                if (roleName != value)
                {
                    roleName = value;
                    RaisePropertyChanged(()=> Role_Name);
                }
            }
        }
    }
}
