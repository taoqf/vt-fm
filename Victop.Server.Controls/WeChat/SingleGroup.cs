using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Server.Controls.WeChat
{
    public class SingleGroup//存储一个分组的信息的类
    {
        public string group_name;

        public List<Dictionary<string, object>> groupdata;

    }
}
