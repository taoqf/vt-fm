using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Victop.Server.Controls.Models;

namespace SystemTestingPlugin.ViewModels
{
    /// <summary>
    /// 模板控件Demo的ViewModel
    /// </summary>
    public class UCTempateControlDemoViewModel : ModelBase
    {
        private string blockName="mainView";

        public string BlockName
        {
            get { return blockName; }
            set
            {
                blockName = value;
                RaisePropertyChanged("BlockName");
            }
        }

        public ICommand mainViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    string temp = "a";
                });
            }
        }
    }
}
