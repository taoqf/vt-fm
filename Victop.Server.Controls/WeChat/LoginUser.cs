using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Server.Controls.WeChat
{
    public class LoginUser
    {

        private string uid;

        public string Uid
        {
            get { return uid; }
            set { uid = value; }
        }
        private string pwd;

        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }
        public LoginUser()
        {
            Uid = null;
            Pwd = null;

        }

    }
}
