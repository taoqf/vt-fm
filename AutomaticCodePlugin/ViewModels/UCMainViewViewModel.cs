using AutomaticCodePlugin.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victop.Frame.CmptRuntime;
using Victop.Server.Controls.Models;
using AutomaticCodePlugin.Views;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace AutomaticCodePlugin.ViewModels
{
    public class UCMainViewViewModel : ModelBase
    {
        #region 私有字段
        MainStateMachine mainFsm;
        UCMainView mainView;
        PresentationBlockModel mainPBlock;
        private string test;
        #endregion
        #region 公共属性

        public UCMainView MainView
        {
            get
            {
                return mainView;
            }
            set
            {
                mainView = value;
            }
        }

        public PresentationBlockModel MainPBlock
        {
            get
            {
                return mainPBlock;
            }
            set
            {
                mainPBlock = value;
                RaisePropertyChanged("MainPBlock");
            }
        }
        public string Test
        {
            get
            {
                return test;
            }
            set
            {
                test = value;
                RaisePropertyChanged("Test");
            }
        }
        #endregion
        #region Command
        public ICommand mainViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    mainView = x as UCMainView;
                    mainFsm = new MainStateMachine(this);
                    mainFsm.MainLoad();
                });
            }
        }

        public ICommand btnSearchClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    mainFsm.Search();
                });
            }
        }
        public ICommand btnAddClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    mainFsm.AddRow();
                });
            }
        }

        public ICommand dgridSelectionChangedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    mainFsm.SelectRow();
                });
            }
        }
        #endregion
    }
}
