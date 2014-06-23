using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Victop.Wpf.Controls
{
    public class CtrlField
    {
        public CtrlField(TreeViewControl ctrlType, string fieldName)
        {
            this.CtrlType = ctrlType;
            this.FieldName = fieldName;
        }

        public CtrlField(TreeViewControl ctrlType, string fieldName, string value)
        {
            this.CtrlType = ctrlType;
            this.FieldName = fieldName;
            this.StringFormat = value;
        }

        public TreeViewControl CtrlType { get; set; }

        public string FieldName { get; set; }

        public string StringFormat { get; set; }
    }
}
