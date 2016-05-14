using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace SystemTestingPlugin.ActionMgrs
{
    public class ActionElement : PropertyModelBase
    {
        private string id;
        private string actionNo;
        private string targetNo;
        private string targetName;
        private string targetValue;
        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                if (id != value)
                {
                    id = value;
                    RaisePropertyChanged(() => Id);
                }
            }
        }

        public string ActionNo
        {
            get
            {
                return actionNo;
            }

            set
            {
                if (actionNo != value)
                {
                    actionNo = value;
                    RaisePropertyChanged(() => ActionNo);
                }
            }
        }

        public string TargetNo
        {
            get
            {
                return targetNo;
            }

            set
            {
                if (targetNo != value)
                {
                    targetNo = value;
                    RaisePropertyChanged(() => TargetNo);
                }
            }
        }

        public string TargetName
        {
            get
            {
                return targetName;
            }

            set
            {
                if (targetName != value)
                {
                    targetName = value;
                    RaisePropertyChanged(() => TargetName);
                }
            }
        }

        public string TargetValue
        {
            get
            {
                return targetValue;
            }

            set
            {
                if (targetValue != value)
                {
                    targetValue = value;
                    RaisePropertyChanged(() => TargetValue);
                }
            }
        }
    }
}
