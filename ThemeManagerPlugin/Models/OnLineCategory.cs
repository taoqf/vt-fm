using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace ThemeManagerPlugin.Models
{
    public class OnLineCategory : PropertyModelBase
    {
        /// <summary>
        /// 分类编号
        /// </summary>
        private string category_no;
        public string Category_No
        {
            get { return category_no; }
            set
            {
                if (category_no != value)
                {
                    category_no = value;
                    RaisePropertyChanged(()=> Category_No);
                }
            }
        }
        /// <summary>
        /// 分类名称
        /// </summary>
        private string category_name;
        public string Category_Name
        {
            get { return category_name; }
            set
            {
                if (category_name != value)
                {
                    category_name = value;
                    RaisePropertyChanged(()=> Category_Name);
                }
            }
        }
    }
}
