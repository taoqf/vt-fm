using System.Data;
using Victop.Server.Controls.Models;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MachinePlatformPlugin.Models;
namespace MachinePlatformPlugin.ViewModels
{
    public class MachinePlatformViewModel : ModelBase
    {
        #region 字段
        /// <summary>
        /// 机台信息实体
        /// </summary>
        private CabinetInfoModel cabinetInfoModel = new CabinetInfoModel();
        /// <summary>
        /// 机台状态实体
        /// </summary>
        private CabinetTaskStateModel taskStateModel = new CabinetTaskStateModel();
        /// <summary>
        /// 机台状态集合
        /// </summary>
        private DataTable cabTaskStatusDt;
        /// <summary>
        /// 传送带数据
        /// </summary>
        private DataTable dtConData;
        #endregion

        #region 属性
        /// <summary>
        /// 机台状态集合
        /// </summary>
        public DataTable CabTaskStatusDt
        {
            get
            {
                return cabTaskStatusDt;
            }
            set
            {
                if (cabTaskStatusDt != value)
                {
                    cabTaskStatusDt = value;
                    RaisePropertyChanged("CabTaskStatusDt");
                }
            }
        }
        /// <summary>
        /// 产出区数据源
        /// </summary>
        private DataTable dtData;
        public DataTable DtData
        {
            get
            {
                return dtData;
            }
            set
            {
                if (dtData != value)
                    dtData = value;
                RaisePropertyChanged("DtData");
            }
        }
        /// <summary>
        /// 传送带数据源
        /// </summary>
        public DataTable DtConData
        {
            get
            {
                return dtConData;
            }
            set
            {
                if (dtConData != value)
                    dtConData = value;
                RaisePropertyChanged("DtConData");
            }
        }
        #endregion

        #region 命令
        /// <summary>
        /// 初始化带参数
        /// </summary>
        public ICommand PageFlowMachinePlatformViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    
                });
            }
        }

        #region 图纸指南区
        /// <summary>
        /// 图纸指南区
        /// </summary>
        public ICommand btnDrawingClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                   
                });
            }
        }
        #endregion

        #region 文件模板区
        /// <summary>
        /// 文件模板区
        /// </summary>
        public ICommand btnFileTemplateClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
             
                });
            }
        }
        #endregion

        #region 下载文件左区
        /// <summary>
        /// 下载文件左区
        /// </summary>
        public ICommand btnDownFileLeftClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                });
            }
        }
        #endregion

        #region 下载文件右区
        /// <summary>
        /// 下载文件右区
        /// </summary>
        public ICommand btnDownFileRightClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {

                });
            }
        }
        #endregion

        #region 查看
        /// <summary>
        /// 查看
        /// </summary>
        public ICommand btnOpenFileClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {

                });
            }
        }
        #endregion

        #region 生产日志
        /// <summary>
        /// 生产日志
        /// </summary>
        public ICommand btnProLogClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                });
            }
        }
        #endregion

        #region 问题日志
        /// <summary>
        /// 问题日志
        /// </summary>
        public ICommand btnIssueLogClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {

                });
            }
        }
        #endregion

        #region 设计
        /// <summary>
        /// 设计
        /// </summary>
        public ICommand btnDeviseClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                });
            }
        }

        #endregion

        #region 清除
        /// <summary>
        /// 清除
        /// </summary>
        public ICommand btnClearClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                });
            }
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        public ICommand btnAddClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {

                });
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        public ICommand btnDeleteClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                  
                });
            }
        }
        #endregion

        #region 查询
        public ICommand btnSeachClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                   
                });
            }
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        public ICommand btnSaveClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                   
                });
            }
        }
        #endregion

        #region 派工
        /// <summary>
        /// 派工
        /// </summary>
        public ICommand btnDivideClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                   
                });
            }
        }
        #endregion

        #region 开工
        /// <summary>
        /// 开工
        /// </summary>
        public ICommand btnWorkClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                   
                });
            }
        }


        #endregion

        #region 挂起
        /// <summary>
        /// 挂起
        /// </summary>
        public ICommand btnHangupClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                   
                });
            }
        }
        #endregion

        #region 解挂
        /// <summary>
        /// 解挂
        /// </summary>
        public ICommand btnUnHangupClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                   
                });
            }
        }
        #endregion

        #region 交工
        /// <summary>
        /// 交工
        /// </summary>
        public ICommand btnEndWorkClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    
                });
            }
        }
        #endregion

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        public ICommand btnExamineClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    
                });
            }
        }
        #endregion

        #region 驳回
        /// <summary>
        /// 驳回
        /// </summary>
        public ICommand btnRejectClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                   
                });
            }
        }
        #endregion

        #region 问题反馈
        /// <summary>
        /// 问题反馈按钮
        /// </summary>
        public ICommand btnFeedbackClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                   
                });
            }
        }
        #endregion

        #endregion

        #region 方法
       
        #endregion
    }
}
