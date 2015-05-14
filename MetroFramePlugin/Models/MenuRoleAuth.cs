using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace MetroFramePlugin.Models
{
    /// <summary>
    /// 菜单角色授权
    /// </summary>
    public class MenuRoleAuth:PropertyModelBase
    {
        private long authCode;

        public long AuthCode
        {
            get { return authCode; }
            set { authCode = value; }
        }
        private string role_No;

        public string Role_No
        {
            get { return role_No; }
            set { role_No = value; }
        }
    }
}
