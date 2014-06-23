using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Victop.Server.Controls.Models;

namespace VictopControlShowDemo.ViewModels
{
    public class TestMvvmViewModel : ModelBase
    {

        /// <summary> 取消按钮命令</summary>
        public ICommand btnAddClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    
                });
            }
        }

        /// <summary> 取消按钮命令</summary>
        public ICommand btnCancleClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                });
            }
        }

        /// <summary> 取消按钮命令</summary>
        public ICommand btnMouseOverCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                });
            }
        }
    }

}
