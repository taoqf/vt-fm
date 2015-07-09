﻿using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Victop.Server.Controls.Models;
using Victop.Frame.CmptRuntime;
using System.Data;

namespace SystemTestingPlugin.ViewModels
{
    /// <summary>
    /// 模板控件Demo的ViewModel
    /// </summary>
    public class UCTempateControlDemoViewModel : ModelBase
    {
        #region 字段
        /// <summary>
        /// 主窗口
        /// </summary>
        private TemplateControl mainViewControl;
        /// <summary>
        /// 分区1展示层Block
        /// </summary>
        private PresentationBlockModel preOneBlockModel;
        /// <summary>
        /// 分区1-1展示层Block
        /// </summary>
        private PresentationBlockModel preOneOfOneBlockModel;
        #endregion
        #region 属性
        /// <summary>
        /// 分区1展示层Block
        /// </summary>
        public PresentationBlockModel PreOneBlockModel
        {
            get
            {
                return preOneBlockModel;
            }
            set
            {
                if (preOneBlockModel != value)
                {
                    preOneBlockModel = value;
                    RaisePropertyChanged("PreOneBlockModel");
                }
            }
        }
        /// <summary>
        /// 分区1-1展示层Block
        /// </summary>
        public PresentationBlockModel PreOneOfOneBlockModel
        {
            get
            {
                return preOneOfOneBlockModel;
            }
            set
            {
                if (preOneOfOneBlockModel != value)
                {
                    preOneOfOneBlockModel = value;
                    RaisePropertyChanged("PreOneOfOneBlockModel");
                }
            }
        }
        #endregion
        int temp = 0;
        public ICommand mainViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    mainViewControl = (TemplateControl)x;
                    PreOneBlockModel = mainViewControl.DefinModel.CompntPresentation.PresentationBlocks.First(it => it.BlockName.Equals("p_1"));
                    PreOneBlockModel.GetData();
                    PreOneBlockModel.PreBlockSelectedRow = PreOneBlockModel.ViewBlockDataTable.Rows[temp];
                    PreOneBlockModel.ViewBlock.SetCurrentRow(PreOneBlockModel.PreBlockSelectedRow);
                    PreOneOfOneBlockModel = mainViewControl.DefinModel.CompntPresentation.PresentationBlocks.First(it => it.BlockName.Equals("p_1_1"));
                    PreOneOfOneBlockModel.GetData();
                });
            }
        }

        public ICommand btnPreiClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    temp -= 1;
                    PreOneBlockModel.PreBlockSelectedRow = PreOneBlockModel.ViewBlockDataTable.Rows[temp];
                    PreOneBlockModel.ViewBlock.SetCurrentRow(PreOneBlockModel.PreBlockSelectedRow);
                    PreOneOfOneBlockModel.GetData();
                }, () =>
                {
                    return temp > 0;
                });
            }
        }

        public ICommand btnNextClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    temp += 1;
                    PreOneBlockModel.PreBlockSelectedRow = PreOneBlockModel.ViewBlockDataTable.Rows[temp];
                    PreOneBlockModel.ViewBlock.SetCurrentRow(PreOneBlockModel.PreBlockSelectedRow);
                    PreOneOfOneBlockModel.GetData();
                }, () =>
                {
                    return PreOneBlockModel != null && temp < PreOneBlockModel.ViewBlockDataTable.Rows.Count - 1;
                });
            }
        }

        public ICommand dgridProductSelectionChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    DataRowView drv = x as DataRowView;
                    PreOneBlockModel.PreBlockSelectedRow = drv.Row;
                    PreOneBlockModel.ViewBlock.SetCurrentRow(drv.Row);
                    PreOneOfOneBlockModel.GetData();
                });
            }
        }
    }
}
