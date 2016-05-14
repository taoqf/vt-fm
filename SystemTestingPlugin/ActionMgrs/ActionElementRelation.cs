using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace SystemTestingPlugin.ActionMgrs
{
    public class ActionElementRelation : PropertyModelBase
    {
        private string actionValue1;
        private string actionValue2;

        public string ActionValue1
        {
            get
            {
                return actionValue1;
            }

            set
            {
                if (actionValue1 != value)
                {
                    actionValue1 = value;
                    RaisePropertyChanged(() => ActionValue1);
                }
            }
        }

        public string ActionValue2
        {
            get
            {
                return actionValue2;
            }

            set
            {
                if (actionValue2 != value)
                {
                    actionValue2 = value;
                    RaisePropertyChanged(() => ActionValue2);
                }
            }
        }
    }
}
