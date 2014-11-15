using System.Data;
using Victop.Server.Controls.Models;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MachinePlatformPlugin.Models;
using MachinePlatformPlugin.Views;
using System.Collections.Generic;
using Victop.Frame.SyncOperation;
using Victop.Frame.PublicLib.Helpers;
using System;
using Victop.Frame.DataChannel;
using Victop.Frame.Components;
using Victop.Server.Controls.Runtime;
using System.Windows;
using System.Windows.Media;
using Victop.Wpf.Controls;
using MachinePlatformPlugin.Enums;
using System.Windows.Forms;
namespace MachinePlatformPlugin.ViewModels
{
    public class MachinePlatformViewModel : ModelBase
    {
        #region 字段
        private DataOperation dataOp = new DataOperation();
        /// <summary>
        /// 主窗体
        /// </summary>
        private UCMachinePlatform ucMachineMainView;
        /// <summary>
        /// 主Tab区
        /// </summary>
        private CompntSingleDataGridWithCheckBox datagridMaster;
        /// <summary>
        /// 明细区
        /// </summary>
        private CompntSingleDataGridWithCheckBox datagridDetial;
        /// <summary>
        /// 备明细区
        /// </summary>
        private CompntSingleDataGridWithCheckBox datagridSubDetial;
        /// <summary>
        /// 机台信息实体
        /// </summary>
        private CabinetInfoModel cabinetInfoModel = new CabinetInfoModel();
        /// <summary>
        /// 机台状态实体
        /// </summary>
        private CabinetTaskStateModel taskStateModel = new CabinetTaskStateModel();
        /// <summary>
        /// 任务Tab实体
        /// </summary>
        private CabinetTaskTabModel taskTabModel;
        /// <summary>
        /// 机台按钮实体
        /// </summary>
        private CabinetButtonsModel btnsModel;
        /// <summary>
        /// 机台状态集合
        /// </summary>
        private DataTable cabTaskStatusDt;
        /// <summary>
        /// 传送带数据
        /// </summary>
        private DataTable conveyorData;
        /// <summary>
        /// 检索任务状态
        /// </summary>
        private long searchTaskState;
        /// <summary>
        /// 检索任务状态
        /// </summary>
        public long SearchTaskState
        {
            get
            {
                return searchTaskState;
            }
            set
            {
                if (searchTaskState != value)
                {
                    searchTaskState = value;
                    RaisePropertyChanged("SearchTaskState");
                }
            }
        }
        /// <summary>
        /// 主视图可用
        /// </summary>
        private bool mainViewAble;
        /// <summary>
        /// 第一次加载
        /// </summary>
        private bool isFristLoaded = true;
        #endregion

        #region 属性
        /// <summary>
        /// 机台信息
        /// </summary>
        public CabinetInfoModel CabinetInfoModel
        {
            get
            {
                return cabinetInfoModel;
            }
            set
            {
                cabinetInfoModel = value;
                RaisePropertyChanged("CabinetInfoModel");
            }
        }
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
        private DataTable outPutData;
        public DataTable OutPutData
        {
            get
            {
                return outPutData;
            }
            set
            {
                if (outPutData != value)
                {
                    outPutData = value;
                    RaisePropertyChanged("OutPutData");
                }
            }
        }
        /// <summary>
        /// 传送带数据源
        /// </summary>
        public DataTable ConveyorData
        {
            get
            {
                return conveyorData;
            }
            set
            {
                if (conveyorData != value)
                    conveyorData = value;
                RaisePropertyChanged("ConveyorData");
            }
        }

        /// <summary>
        /// 主视图可用
        /// </summary>
        public bool MainViewAble
        {
            get
            {
                return mainViewAble;
            }
            set
            {
                if (mainViewAble != value)
                {
                    mainViewAble = value;
                    RaisePropertyChanged("MainViewAble");
                }
            }
        }
        /// <summary>
        /// 任务状态实体
        /// </summary>
        public CabinetTaskTabModel TaskTabModel
        {
            get
            {
                if (taskTabModel == null)
                    taskTabModel = new CabinetTaskTabModel();
                return taskTabModel;
            }
            set
            {
                if (taskTabModel != value)
                {
                    taskTabModel = value;
                    RaisePropertyChanged("TaskTabModel");
                }
            }
        }
        /// <summary>
        /// 机台按钮实体
        /// </summary>
        public CabinetButtonsModel BtnsModel
        {
            get
            {
                if (btnsModel == null)
                    btnsModel = new CabinetButtonsModel();
                return btnsModel;
            }
            set
            {
                if (btnsModel != value)
                {
                    btnsModel = value;
                    RaisePropertyChanged("BtnsModel");
                }
            }
        }

        #endregion

        #region 命令
        /// <summary>
        /// 初始化带参数
        /// </summary>
        public ICommand MachinePlatformViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    if (isFristLoaded)
                    {
                        ucMachineMainView = (UCMachinePlatform)x;
                        cabinetInfoModel.SystemId = UCMachinePlatform.ParamDict["systemid"].ToString();
                        cabinetInfoModel.ConfigSystemId = UCMachinePlatform.ParamDict["configsytemid"].ToString();
                        cabinetInfoModel.SpaceId = UCMachinePlatform.ParamDict["spaceid"].ToString();
                        cabinetInfoModel.CabinetFitData = JsonHelper.ToObject<List<Dictionary<string, object>>>(JsonHelper.ToJson(UCMachinePlatform.ParamDict["fitdata"]));
                        cabinetInfoModel.CabinetCADName = UCMachinePlatform.ParamDict["cadname"].ToString();
                        cabinetInfoModel.CabinetCode = UCMachinePlatform.ParamDict["menuno"].ToString();
                        cabinetInfoModel.CabinetMenuCode = (UCMachinePlatform.ParamDict["menucode"] == null || string.IsNullOrEmpty(UCMachinePlatform.ParamDict["menucode"].ToString())) ? 65535 : Convert.ToInt64(UCMachinePlatform.ParamDict["menucode"].ToString());
                        cabinetInfoModel.CabinetAuthorityCode = (UCMachinePlatform.ParamDict["authoritycode"] == null || string.IsNullOrEmpty(UCMachinePlatform.ParamDict["authoritycode"].ToString())) ? 65535 : Convert.ToInt64(UCMachinePlatform.ParamDict["authoritycode"].ToString());
                        InitCabinetData();

                    }
                });
            }
        }
        /// <summary>
        /// 机台初始化
        /// </summary>
        public ICommand MachinePlatformViewUnoadedCommand
        {
            get
            {
                return new RelayCommand<Object>((x) =>
                {
                    string Uid = (string)x;
                    PluginOperation pluginOp = new PluginOperation();
                    pluginOp.StopPlugin(Uid);
                });
            }
        }
        /// <summary>
        /// 指南区下载
        /// </summary>
        public ICommand btnDownFileLeftClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    if (x != null)
                    {
                        DataRowView drView = (DataRowView)x;
                        DownLoadFileById(drView["file"].ToString(), drView["name"].ToString(), drView["file_type"].ToString());
                    }
                });
            }
        }
        /// <summary>
        /// 生产日志
        /// </summary>
        public ICommand btnProLogClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (datagridMaster.ParamsModel.CheckedRows.Length != 1)
                    {
                        VicMessageBoxNormal.Show("查看生产日志时，只能选择单一任务查看，请重新选择!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    ProductLog win = new ProductLog();
                    ProductLogViewModel.value = datagridMaster.ParamsModel.CheckedRows[0]["production_log"].ToString();
                    win.Owner = GetParentObject<Window>(ucMachineMainView);
                    win.ShowDialog();
                });
            }
        }
        /// <summary>
        /// 问题日志
        /// </summary>
        public ICommand btnIssueLogClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (datagridMaster.ParamsModel.CheckedRows.Length != 1)
                    {
                        VicMessageBoxNormal.Show("查看问题日志时，只能选择单一任务查看，请重新选择!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    IssueLog win = new IssueLog();
                    IssueLogViewModel.value = datagridMaster.ParamsModel.CheckedRows[0]["issue_log"].ToString();
                    win.Owner = GetParentObject<Window>(ucMachineMainView);
                    win.ShowDialog();
                });
            }
        }
        /// <summary>
        /// 设计
        /// </summary>
        public ICommand btnDeviseClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    OperationWindow opWindow = new OperationWindow(cabinetInfoModel);
                    Window parentWin = GetParentObject<Window>(ucMachineMainView);
                    opWindow.Owner = parentWin;
                    opWindow.ShowDialog();
                });
            }
        }
        /// <summary>
        /// 清除
        /// </summary>
        public ICommand btnClearClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                });
            }
        }
        /// <summary>
        /// 传出区下载
        /// </summary>
        public ICommand btnDownFileRightClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    if (x != null)
                    {
                        DataRowView drView = (DataRowView)x;
                        DownLoadFileById(drView["file_path"].ToString(), drView["file_name"].ToString(), drView["file_type"].ToString());
                    }
                });
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        public ICommand btnAddClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    datagridMaster.Add();
                    datagridMaster.ParamsModel.GridSelectedRow["wt_state"] = ((long)TaskStateEnum.未派工).ToString();
                });
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        public ICommand btnDeleteClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (datagridMaster.ParamsModel.CheckedRows.Length > 0)
                    {
                        if (MessageBoxResult.OK == VicMessageBoxNormal.Show("确定要删除当前选中的信息", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning))
                        {
                            for (int i = 0; i < datagridMaster.ParamsModel.CheckedRows.Length; i++)
                            {
                                datagridMaster.ParamsModel.GridSelectedRow = datagridMaster.ParamsModel.CheckedRows[i];
                                string strTemp = "\n";
                                strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " + "删除";
                                datagridMaster.ParamsModel.GridSelectedRow["production_log"] += strTemp;
                                datagridMaster.Delete();
                                datagridMaster.Save();
                            }

                        }

                    }
                });
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        public ICommand btnSaveClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string JsonStr = datagridMaster.Save();
                    string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                    string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败。
                    if (!string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值不等于1，则抛出操作失败的原因；反之，程序没有提示
                    {
                        VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
        }
        /// <summary>
        /// 派工
        /// </summary>
        public ICommand btnDivideClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (datagridMaster.ParamsModel.CheckedRows.Length <= 0)
                    {
                        VicMessageBoxNormal.Show("请选择需要派工的任务！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    SelectPersonnel peronWindow = new SelectPersonnel(this);
                    peronWindow.Owner = GetParentObject<Window>(ucMachineMainView);
                    peronWindow.ShowDialog();
                    if (cabinetInfoModel.CabinetSelectedStaff != null)
                    {
                        for (int i = 0; i < datagridMaster.ParamsModel.CheckedRows.Length; i++)
                        {
                            if (datagridMaster.ParamsModel.CheckedRows[i]["wt_state"].ToString() != ((long)TaskStateEnum.未派工).ToString())
                            {
                                if (MessageBoxResult.OK == VicMessageBoxNormal.Show("选定的任务中存在不符合派工条件的任务，是否停止派工", "提示", MessageBoxButton.OK, MessageBoxImage.Information))
                                {
                                    datagridMaster.Search();
                                    return;
                                }
                            }
                            datagridMaster.ParamsModel.CheckedRows[i]["work_staff_no"] = cabinetInfoModel.CabinetSelectedStaff["staff_no"];
                            datagridMaster.ParamsModel.CheckedRows[i]["wt_state"] = ((long)TaskStateEnum.已派工).ToString();
                            datagridMaster.ParamsModel.CheckedRows[i]["start_time"] = DBNull.Value;
                            datagridMaster.ParamsModel.CheckedRows[i]["finish_time"] = DBNull.Value;
                            datagridMaster.ParamsModel.CheckedRows[i]["last_wo_no"] = "";
                            //添加生产日志
                            string strTemp = "\n";
                            strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " + cabinetInfoModel.CabinetName + "     派工给:" + SelectPersonnelViewModel.staffName; ;

                            datagridMaster.ParamsModel.CheckedRows[i]["production_log"] += strTemp;
                        }
                        string JsonStr = datagridMaster.Save();
                        string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                        string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();
                        if (ResultMode != "1" && !string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败。
                        {
                            VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            VicMessageBoxNormal.Show("派工成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                });
            }
        }
        /// <summary>
        /// 开工
        /// </summary>
        public ICommand btnWorkClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (datagridMaster.ParamsModel.GridSelectedRow != null)
                    {
                        if (!datagridMaster.ParamsModel.GridSelectedRow["wt_state"].ToString().Equals(((long)TaskStateEnum.已派工).ToString()))
                        {
                            VicMessageBoxNormal.Show("请选择已派工的任务进行开工操作！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            string sysTime = GetSysServiceTime();
                            datagridMaster.ParamsModel.GridSelectedRow["start_time"] = sysTime;
                            datagridMaster.ParamsModel.GridSelectedRow["wt_state"] = ((long)TaskStateEnum.已开工).ToString();
                            string strTemp = "\n";
                            strTemp += cabinetInfoModel.UserName + ": " + sysTime + ": " + cabinetInfoModel.CabinetName + "  开工  ";
                            datagridMaster.ParamsModel.GridSelectedRow["production_log"] += strTemp;
                            string JsonStr = datagridMaster.Save();
                            string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                            string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();
                            if (ResultMode != "1" && !string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败。
                            {
                                VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                VicMessageBoxNormal.Show("开工成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                });
            }
        }
        /// <summary>
        /// 挂起
        /// </summary>
        public ICommand btnSuspendClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (datagridMaster.ParamsModel.GridSelectedRow != null)
                    {
                        if (!datagridMaster.ParamsModel.GridSelectedRow["wt_state"].ToString().Equals(((long)TaskStateEnum.已开工).ToString()))
                        {
                            VicMessageBoxNormal.Show("选择的任务不可进行挂起操作，请重新选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            datagridMaster.ParamsModel.GridSelectedRow["wt_state"] = ((long)TaskStateEnum.挂起).ToString();
                            string strTemp = "\n";
                            strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " + cabinetInfoModel.CabinetName + "  挂起  " + datagridMaster.ParamsModel.GridSelectedRow["product_no"].ToString();
                            datagridMaster.ParamsModel.GridSelectedRow["production_log"] += strTemp;
                            string JsonStr = datagridMaster.Save();
                            string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                            string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();
                            if (ResultMode != "1" && !string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败。
                            {
                                VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                VicMessageBoxNormal.Show("挂起成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        VicMessageBoxNormal.Show("请选择需要挂起的任务", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
        }
        /// <summary>
        /// 解挂
        /// </summary>
        public ICommand btnContinueClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (datagridMaster.ParamsModel.GridSelectedRow != null)
                    {
                        if (!datagridMaster.ParamsModel.GridSelectedRow["wt_state"].ToString().Equals(((long)TaskStateEnum.挂起).ToString()))
                        {
                            VicMessageBoxNormal.Show("选择的任务无法解挂，请重新选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            datagridMaster.ParamsModel.GridSelectedRow["wt_state"] = ((long)TaskStateEnum.已开工).ToString();
                            // 添加生产日志
                            string strTemp = "\n";
                            strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " + cabinetInfoModel.CabinetName + "  解挂  " + datagridMaster.ParamsModel.GridSelectedRow["product_no"].ToString();
                            datagridMaster.ParamsModel.GridSelectedRow["production_log"] += strTemp;
                            string JsonStr = datagridMaster.Save();
                            string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                            string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();
                            if (ResultMode != "1" && !string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败。
                            {
                                VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                VicMessageBoxNormal.Show("解挂成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        VicMessageBoxNormal.Show("请选择需要解挂的任务", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
        }
        /// <summary>
        /// 交工
        /// </summary>
        public ICommand btnEndWorkClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (datagridMaster.ParamsModel.GridSelectedRow != null)
                    {
                        if (datagridMaster.ParamsModel.GridSelectedRow["wt_state"].ToString() != ((long)TaskStateEnum.已开工).ToString())
                        {
                            VicMessageBoxNormal.Show("选择任务非已开工状态任务,不能交工，请重新选择!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(datagridMaster.ParamsModel.GridSelectedRow["file_name"].ToString()) || string.IsNullOrEmpty(datagridMaster.ParamsModel.GridSelectedRow["file_path"].ToString()))
                            {
                                if (MessageBoxResult.OK == VicMessageBoxNormal.Show("当前任务无产出内容，是否确认交工", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information))
                                {
                                    string sysTime = GetSysServiceTime();
                                    datagridMaster.ParamsModel.GridSelectedRow["wt_state"] = ((long)TaskStateEnum.已交工).ToString();
                                    datagridMaster.ParamsModel.GridSelectedRow["finish_time"] = sysTime;
                                    string strTemp = "\n";
                                    strTemp += cabinetInfoModel.UserName + ":" + sysTime + "  " + cabinetInfoModel.CabinetName + "  " + datagridMaster.ParamsModel.GridSelectedRow["product_no"].ToString() + "  " + "任务交工";
                                    datagridMaster.ParamsModel.GridSelectedRow["production_log"] += strTemp;
                                    string JsonStr = datagridMaster.Save();
                                    string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                                    string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();
                                    if (ResultMode != "1" && !string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败。
                                    {
                                        VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    else
                                    {
                                        VicMessageBoxNormal.Show("交工成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        VicMessageBoxNormal.Show("未选择需要交工的任务，请重新选择!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
        }
        /// <summary>
        /// 审核
        /// </summary>
        public ICommand btnExamineClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (datagridMaster.ParamsModel.GridSelectedRow != null)
                    {
                        if (datagridMaster.ParamsModel.GridSelectedRow["wt_state"].ToString() != ((long)TaskStateEnum.已交工).ToString())
                        {
                            VicMessageBoxNormal.Show("选择的任务无法驳回，请重新选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            datagridMaster.ParamsModel.GridSelectedRow["wt_state"] = taskStateModel.EndReview;
                            string strTemp = "\n";
                            strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " + cabinetInfoModel.CabinetName + "  审核";
                            datagridMaster.ParamsModel.GridSelectedRow["production_log"] += strTemp;
                            string JsonStr = datagridMaster.Save();
                            string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                            string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();
                            if (ResultMode != "1" && !string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败。
                            {
                                VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                VicMessageBoxNormal.Show("审核成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                        }
                    }
                    else
                    {
                        VicMessageBoxNormal.Show("请选择需要审核的任务", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
        }
        /// <summary>
        /// 驳回
        /// </summary>
        public ICommand btnRejectClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (datagridMaster.ParamsModel.GridSelectedRow != null)
                    {
                        if (datagridMaster.ParamsModel.GridSelectedRow["wt_state"].ToString() != ((long)TaskStateEnum.已交工).ToString())
                        {
                            VicMessageBoxNormal.Show("选择的任务无法驳回，请重新选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            datagridMaster.ParamsModel.GridSelectedRow["wt_state"] = ((long)TaskStateEnum.未派工).ToString();
                            datagridMaster.ParamsModel.GridSelectedRow["start_time"] = DBNull.Value;
                            datagridMaster.ParamsModel.GridSelectedRow["finish_time"] = DBNull.Value;
                            datagridMaster.ParamsModel.GridSelectedRow["work_staff_no"] = "";
                            string strTemp = "\n";
                            strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " + cabinetInfoModel.CabinetName + "  驳回";
                            datagridMaster.ParamsModel.GridSelectedRow["production_log"] += strTemp;
                            string JsonStr = datagridMaster.Save();
                            string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                            string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();
                            if (ResultMode != "1" && !string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败。
                            {
                                VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                VicMessageBoxNormal.Show("驳回成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        VicMessageBoxNormal.Show("请选择需要驳回的任务", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                });
            }
        }
        /// <summary>
        /// 问题反馈
        /// </summary>
        public ICommand btnFeedbackClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Feedback();
                });
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand btnSeachClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                    if (SearchTaskState > 0)
                    {
                        List<object> conlist = new List<object>();
                        Dictionary<string, object> tableConDic = new Dictionary<string, object>();
                        tableConDic.Add("wt_state", SearchTaskState.ToString());
                        conlist.Add(tableConDic);
                        datagridMaster.ParamsModel.ConditionList = conlist;
                        datagridMaster.Search();
                    }
                    else
                    {
                        datagridMaster.Search();
                    }
                });
            }
        }
        /// <summary>
        /// 选择人员确定
        /// </summary>
        public ICommand btnAffirmClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    SelectPersonnel peronWindow = (SelectPersonnel)x;
                    peronWindow.Close();
                });
            }
        }
        /// <summary>
        /// 选择人员取消
        /// </summary>
        public ICommand btnCancelClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    SelectPersonnel peronWindow = (SelectPersonnel)x;
                    peronWindow.Close();
                    cabinetInfoModel.CabinetSelectedStaff = null;
                });
            }
        }
        #endregion
        #region 方法
        /// <summary>
        /// 初始化机台数据
        /// </summary>
        private void InitCabinetData()
        {
            InitTaskTabStatus();
            GetLoginUserInfo();
            InitCabinetButtonVisibility();
            GetCabinetInfo();
            GetCabinetTaskState();
            MainViewAble = true;
        }
        /// <summary>
        /// 问题反馈
        /// </summary>
        private void Feedback()
        {
            if (datagridMaster.ParamsModel.CheckedRows.Length != 1)
            {
                VicMessageBoxNormal.Show("问题反馈时，只能选择单一任务反馈，请重新选择!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                string strTemp = "\n";
                strTemp += cabinetInfoModel.UserName + "说: " + GetSysServiceTime() + "\n";
                IsueFeedback win = new IsueFeedback();
                IsueFeedbackViewModel.valueInput = (strTemp + datagridMaster.ParamsModel.GridSelectedRow["issue_log"].ToString());
                win.Owner = GetParentObject<Window>(ucMachineMainView);
                win.ShowDialog();
                if (string.IsNullOrEmpty(IsueFeedbackViewModel.valueOutput))
                {
                    return;
                }
                strTemp = IsueFeedbackViewModel.valueInput;
                datagridMaster.ParamsModel.CheckedRows[0]["issue_log"] = strTemp;


                //添加生产日志
                string strTemp2 = "\n";
                strTemp += cabinetInfoModel.UserName + ":" + GetSysServiceTime() + "  " + cabinetInfoModel.CabinetName + "  问题反馈";
                datagridMaster.ParamsModel.GridSelectedRow["production_log"] += strTemp2;
                string JsonStr = datagridMaster.Save();
                string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();
                if (ResultMode != "1" && !string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败。
                {
                    VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    VicMessageBoxNormal.Show("问题反馈成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }
        /// <summary> 
        /// 获取服务器时间 
        /// </summary>         
        /// <returns></returns> 
        private string GetSysServiceTime()
        {
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> result = messageOp.SendMessage("MongoDataChannelService.fetchSystime", new Dictionary<string, object>());
            return JsonHelper.ReadJsonString(result["ReplyContent"].ToString(), "simpleDate");


        }
        /// <summary>
        /// 初始化机台任务Tab
        /// </summary>
        private void InitTaskTabStatus()
        {
            try
            {
                if (cabinetInfoModel.CabinetFitData != null && cabinetInfoModel.CabinetFitData.Count > 0)
                {
                    foreach (Dictionary<string, object> item in cabinetInfoModel.CabinetFitData)
                    {
                        if (!string.IsNullOrEmpty(item["value"].ToString()))
                        {
                            object gridObj = ucMachineMainView.FindName(item["key"].ToString());
                            if (gridObj != null)
                            {
                                CompntSingleDataGridWithCheckBox datagrid = (CompntSingleDataGridWithCheckBox)gridObj;
                                datagrid.ParamsModel.SystemId = cabinetInfoModel.SystemId;
                                datagrid.ParamsModel.SettingModel = JsonHelper.ToObject<CompntSettingModel>(FileHelper.ReadFitData(item["value"].ToString()));
                                switch (item["key"].ToString())
                                {
                                    case "datagridMaster":
                                        datagridMaster = datagrid;
                                        datagridMaster.ParamsModel.SelectedItemChanged += ParamsModel_SelectedItemChanged;
                                        TaskTabModel.MasterTabHeader = item["title"].ToString();
                                        TaskTabModel.MasterTabName = item["key"].ToString();
                                        TaskTabModel.MasterTabVisibility = Visibility.Visible;
                                        datagrid.DoRender();
                                        //datagrid.Search();
                                        break;
                                    case "datagridDetail":
                                        datagridDetial = datagrid;
                                        TaskTabModel.DetialTabHeader = item["title"].ToString();
                                        TaskTabModel.DetialTabName = item["key"].ToString();
                                        TaskTabModel.DetialTabVisibility = Visibility.Visible;
                                        datagrid.DoRender();
                                        break;
                                    case "datagridSubDetail":
                                        datagridSubDetial = datagrid;
                                        TaskTabModel.SubDetialTabHeader = item["title"].ToString();
                                        TaskTabModel.SubDetialTabName = item["key"].ToString();
                                        TaskTabModel.SubDetialTabVisibility = Visibility.Visible;
                                        datagrid.DoRender();
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    isFristLoaded = false;
                }
            }
            catch (Exception ex)
            {
                MainViewAble = false;
                LoggerHelper.ErrorFormat("初始化机台任务列表状态异常:{0}", ex.Message);
            }
        }

        void ParamsModel_SelectedItemChanged(object sender, DataRow dr)
        {
            cabinetInfoModel.CabinetSelectedDataRow = dr;
            DataTable OutPutDt = new DataTable();
            DataColumn clientNoDc = new DataColumn("client_no");
            clientNoDc.Caption = "客户编号";
            DataColumn productNoDc = new DataColumn("product_no");
            productNoDc.Caption = "客户";
            DataColumn fileNameDc = new DataColumn("file_name");
            fileNameDc.Caption = "产出文件";
            DataColumn fileTypeDc = new DataColumn("file_type");
            fileTypeDc.Caption = "文件类型";
            DataColumn filePathDc = new DataColumn("file_path");
            filePathDc.Caption = "文件标识";
            OutPutDt.Columns.Add(clientNoDc);
            OutPutDt.Columns.Add(productNoDc);
            OutPutDt.Columns.Add(fileNameDc);
            OutPutDt.Columns.Add(fileTypeDc);
            OutPutDt.Columns.Add(filePathDc);
            DataRow drow = OutPutDt.NewRow();
            drow["client_no"] = dr["client_no"];
            drow["product_no"] = dr["product_no"];
            drow["file_name"] = datagridMaster.ParamsModel.GridSelectedRow["file_name"];
            drow["file_type"] = datagridMaster.ParamsModel.GridSelectedRow["file_type"];
            drow["file_path"] = datagridMaster.ParamsModel.GridSelectedRow["file_path"];
            OutPutDt.Rows.Add(drow);
            OutPutData = OutPutDt.Copy();
            ConveyorMethod(dr);

        }
        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        private void GetLoginUserInfo()
        {
            try
            {
                MessageOperation messageOp = new MessageOperation();
                Dictionary<string, object> result = messageOp.SendMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
                cabinetInfoModel.UserName = JsonHelper.ReadJsonString(result["ReplyContent"].ToString(), "UserName");
                cabinetInfoModel.UserCode = JsonHelper.ReadJsonString(result["ReplyContent"].ToString(), "UserCode");
                cabinetInfoModel.UserId = JsonHelper.ReadJsonString(result["ReplyContent"].ToString(), "UserId");
            }
            catch (Exception ex)
            {
                MainViewAble = false;
                LoggerHelper.ErrorFormat("获取用户信息异常:{0}", ex.Message);
            }
        }
        /// <summary>
        /// 获取机台基础信息
        /// </summary>
        private void GetCabinetInfo()
        {
            try
            {
                MessageOperation messageOp = new MessageOperation();
                string MessageType = "MongoDataChannelService.findBusiData";
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                List<object> conCitionsList = new List<object>();
                Dictionary<string, object> tableConCabinetDic = new Dictionary<string, object>();
                tableConCabinetDic.Add("cabinet_no", cabinetInfoModel.CabinetCode);
                conCitionsList.Add(tableConCabinetDic);
                List<object> conClientList = null;
                if (conCitionsList != null && conCitionsList.Count > 0)
                {
                    conClientList = new List<object>();
                    Dictionary<string, object> conDic = new Dictionary<string, object>();
                    conDic.Add("name", "cabinet");
                    conDic.Add("tablecondition", conCitionsList);
                    conClientList.Add(conDic);
                }
                contentDic.Clear();
                contentDic.Add("systemid", cabinetInfoModel.SystemId);
                contentDic.Add("configsystemid", cabinetInfoModel.ConfigSystemId);
                contentDic.Add("modelid", "feidao_core_model_cabinet_0001");
                if (conClientList != null)
                {
                    contentDic.Add("conditions", conClientList);
                }
                Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                if (returnDic != null)
                {
                    List<object> pathList = new List<object>();
                    pathList.Add("cabinet");
                    string viewId = returnDic["DataChannelId"].ToString();
                    DataOperation dataOp = new DataOperation();
                    DataTable cabinetInfoDt = dataOp.GetData(viewId, JsonHelper.ToJson(pathList)).Tables["dataArray"];
                    cabinetInfoModel.CabinetId = cabinetInfoDt.Rows[0]["_id"].ToString();
                    cabinetInfoModel.CabinetBomNo = cabinetInfoDt.Rows[0]["wt_category_no"].ToString();
                    cabinetInfoModel.CabinetName = cabinetInfoDt.Rows[0]["cabinet_name"].ToString();
                    Dictionary<string, object> cabinetCurrentDic = new Dictionary<string, object>();
                    cabinetCurrentDic.Add("key", "_id");
                    cabinetCurrentDic.Add("value", cabinetInfoModel.CabinetId);
                    pathList.Add(cabinetCurrentDic);
                    pathList.Add("cabinet_staff");
                    CabinetInfoModel.CabinetStaffDt = dataOp.GetData(viewId, JsonHelper.ToJson(pathList)).Tables["dataArray"];

                    pathList.Clear();
                    pathList.Add("cabinet");
                    pathList.Add(cabinetCurrentDic);
                    pathList.Add("conveyor");
                    DataTable conveyorDt = dataOp.GetData(viewId, JsonHelper.ToJson(pathList)).Tables["dataArray"];
                    if (conveyorDt != null && conveyorDt.Rows.Count > 0)
                    {
                        List<object> conveyorList = new List<object>();
                        List<Dictionary<string, object>> paramsList = new List<Dictionary<string, object>>();
                        foreach (object item in pathList)
                        {
                            conveyorList.Add(item);
                        }
                        foreach (DataRow item in conveyorDt.Rows)
                        {
                            Dictionary<string, object> itemDic = new Dictionary<string, object>();
                            itemDic.Add("conveyor_id", item["_id"].ToString());
                            Dictionary<string, object> conveyorDic = new Dictionary<string, object>();
                            conveyorDic.Add("key", "_id");
                            conveyorDic.Add("value", item["_id"]);
                            conveyorList.Add(conveyorDic);
                            conveyorList.Add("current_cont");
                            DataTable CurrentContDt = dataOp.GetData(viewId, JsonHelper.ToJson(conveyorList)).Tables["dataArray"];
                            List<string> paramList = new List<string>();
                            if (CurrentContDt.Rows.Count > 0)
                            {
                                foreach (DataRow dritem in CurrentContDt.Rows)
                                {
                                    paramList.Add(dritem["current_where"].ToString());
                                }
                            }
                            itemDic.Add("params", paramList);
                            paramsList.Add(itemDic);
                        }
                        cabinetInfoModel.CabinetParamsList = paramsList;
                    }

                }
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("获取机台基础数据异常:{0}", ex.Message);
            }
        }
        /// <summary>
        /// 获取机台状态
        /// </summary>
        private void GetCabinetTaskState()
        {
            CabTaskStatusDt = new DataTable();
            DataColumn valueDc = new DataColumn("val", typeof(Int64));
            DataColumn txtDc = new DataColumn("txt", typeof(string));
            CabTaskStatusDt.Columns.Add(valueDc);
            CabTaskStatusDt.Columns.Add(txtDc);
            Array valueList = Enum.GetValues(typeof(TaskStateEnum));
            for (int i = 0; i < valueList.Length; i++)
            {
                DataRow dr = CabTaskStatusDt.NewRow();
                int stateVal = (int)valueList.GetValue(i);
                dr["val"] = stateVal;
                dr["txt"] = Enum.GetName(typeof(TaskStateEnum), stateVal);
                CabTaskStatusDt.Rows.Add(dr);
            }
            DataRow allDr = CabTaskStatusDt.NewRow();
            allDr["val"] = -1;
            allDr["txt"] = "所有";
            CabTaskStatusDt.Rows.InsertAt(allDr, 0);
            SearchTaskState = -1;
        }
        /// <summary>
        /// 初始化机台按钮可视状态
        /// </summary>
        private void InitCabinetButtonVisibility()
        {
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.SEARCH) > 0)
            {
                BtnsModel.SearchBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.ADD) > 0)
            {
                BtnsModel.AddBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.DELETE) > 0)
            {
                BtnsModel.DelBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.SAVE) > 0)
            {
                BtnsModel.SaveBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.DISPATCH) > 0)
            {
                BtnsModel.DivideBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.DISPATCH) > 0)
            {
                BtnsModel.DivideBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.WORK) > 0)
            {
                BtnsModel.WorkBtnVisibility = Visibility.Visible;
                BtnsModel.DeviseBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.SUSPEND) > 0)
            {
                BtnsModel.SuspendBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.CONTINUE) > 0)
            {
                BtnsModel.ContinueBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.ENDWORK) > 0)
            {
                BtnsModel.EndWorkBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.ERJECT) > 0)
            {
                BtnsModel.RejectBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.EXAMINE) > 0)
            {
                BtnsModel.ExamineBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.FEEDBACK) > 0)
            {
                BtnsModel.FeedbackBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.DOWNLOAD) > 0)
            {
                BtnsModel.InputDownBtnVisibility = Visibility.Visible;
                BtnsModel.OutPutDownBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.PROLOG) > 0)
            {
                BtnsModel.ProLogBtnVisibility = Visibility.Visible;
            }
            if ((cabinetInfoModel.CabinetMenuCode & (long)BtnCodeEnum.ISSUELOG) > 0)
            {
                BtnsModel.IssueLogBtnVisibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// 获取父级控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private T GetParentObject<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        /// <summary>
        /// 传送带方法
        /// </summary>
        /// <param name="pageGuid"></param>
        private void ConveyorMethod(DataRow dr)
        {
            #region 创建dataTable
            DataTable dtConveyor = new DataTable("dataArray");
            DataColumn sourceDc = new DataColumn("source", typeof(string));
            sourceDc.Caption = "来源";
            dtConveyor.Columns.Add(sourceDc);
            DataColumn nameDc = new DataColumn("name", typeof(string));
            nameDc.Caption = "名称";
            dtConveyor.Columns.Add(nameDc);
            DataColumn userageDc = new DataColumn("useage", typeof(string));
            userageDc.Caption = "用途";
            dtConveyor.Columns.Add(userageDc);
            DataColumn fileTypeDc = new DataColumn("file_type", typeof(string));
            fileTypeDc.Caption = "文件类型";
            dtConveyor.Columns.Add(fileTypeDc);
            DataColumn fileDc = new DataColumn("file", typeof(string));
            fileDc.Caption = "文件";
            dtConveyor.Columns.Add(fileDc);
            #endregion
            string MessageType = "MongoDataChannelService.conveyor";
            MessageOperation op = new MessageOperation();
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", cabinetInfoModel.SystemId);
            contentDic.Add("configsystemid", cabinetInfoModel.ConfigSystemId);
            contentDic.Add("cabinet_id", cabinetInfoModel.CabinetId);
            List<Dictionary<string, object>> ConveyorList = new List<Dictionary<string, object>>();
            if (cabinetInfoModel.CabinetParamsList != null && cabinetInfoModel.CabinetParamsList.Count > 0)
            {
                foreach (Dictionary<string, object> item in cabinetInfoModel.CabinetParamsList)
                {
                    Dictionary<string, object> paramDic = new Dictionary<string, object>();
                    paramDic.Add("conveyor_id", item["conveyor_id"]);
                    List<object> paramValList = new List<object>();
                    List<string> paramList = item["params"] as List<string>;
                    if (paramList != null && paramList.Count > 0)
                    {
                        foreach (string paramItem in paramList)
                        {
                            if (dr.Table.Columns.Contains(paramItem))
                            {
                                paramValList.Add(dr[paramItem]);
                            }
                        }
                    }
                    paramDic.Add("params", paramValList);
                    ConveyorList.Add(paramDic);
                }
            }
            contentDic.Add("conveyor", ConveyorList);
            Dictionary<string, object> resultDic = op.SendMessage(MessageType, contentDic, "JSON");
            if (resultDic != null && !resultDic["ReplyMode"].ToString().Equals("0"))
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dtConveyor);
                ds = dataOp.GetData(resultDic["DataChannelId"].ToString(), "[\"conveyor\"]", ds);
                ConveyorData = ds.Tables["dataArray"];
            }
        }
        /// <summary>
        /// 初始化传送带传入区参数
        /// </summary>
        private void GetConveyorParams()
        {
            #region 查询关联conveyorid
            string MessageType = "MongoDataChannelService.findBusiData";
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", cabinetInfoModel.SystemId);
            contentDic.Add("configsystemid", cabinetInfoModel.ConfigSystemId);
            contentDic.Add("modelid", "victop_model_conveyor_0002");
            List<object> conCitionsList = new List<object>();
            Dictionary<string, object> tableConClientDic = new Dictionary<string, object>();
            tableConClientDic.Add("cabinet_id", cabinetInfoModel.CabinetId);
            conCitionsList.Add(tableConClientDic);
            List<object> conditionsList = new List<object>();
            Dictionary<string, object> conditionsDic = new Dictionary<string, object>();
            conditionsDic.Add("name", "conveyor");
            conditionsDic.Add("tablecondition", conCitionsList);
            conditionsList.Add(conditionsDic);
            contentDic.Add("conditions", conditionsList);
            Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
            if (returnDic != null)
            {
                List<Dictionary<string, object>> paramsList = new List<Dictionary<string, object>>();
                string viewId = returnDic["DataChannelId"].ToString();
                List<object> pathList = new List<object>();
                pathList.Add("conveyor");
                DataTable dtConveryor = dataOp.GetData(viewId, JsonHelper.ToJson(pathList)).Tables["dataArray"];
                if (dtConveryor.Rows.Count > 0)
                {
                    foreach (DataRow item in dtConveryor.Rows)
                    {
                        Dictionary<string, object> itemDic = new Dictionary<string, object>();
                        itemDic.Add("conveyor_id", item["_id"].ToString());
                        pathList.Clear();
                        pathList.Add("conveyor");
                        Dictionary<string, object> currentDic = new Dictionary<string, object>();
                        currentDic.Add("key", "_id");
                        currentDic.Add("value", item["_id"].ToString());
                        pathList.Add(currentDic);
                        pathList.Add("current_cont");
                        DataTable dtCurrentCont = dataOp.GetData(viewId, JsonHelper.ToJson(pathList)).Tables["dataArray"];
                        List<string> paramList = new List<string>();
                        if (dtCurrentCont.Rows.Count > 0)
                        {
                            foreach (DataRow dritem in dtCurrentCont.Rows)
                            {
                                paramList.Add(dritem["current_where"].ToString());
                            }
                        }
                        itemDic.Add("params", paramList);
                        paramsList.Add(itemDic);
                    }
                }
                cabinetInfoModel.CabinetParamsList = paramsList;
            }
            #endregion
        }
        /// <summary>
        /// 根据文件标识下载文件
        /// </summary>
        /// <param name="fileId"></param>
        private void DownLoadFileById(string fileId,string fileName,string fileType)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = fileName;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            saveFileDialog.Filter = string.Format("{0}文件|*.{0}", fileType);
            saveFileDialog.OverwritePrompt = true;
            if (DialogResult.OK == saveFileDialog.ShowDialog())
            {
                MessageOperation messageOp = new MessageOperation();
                string messageType = "ServerCenterService.DownloadDocument";
                Dictionary<string, object> messageContent = new Dictionary<string, object>();
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("DownloadFileId", fileId);
                contentDic.Add("DownloadToPath", saveFileDialog.FileName);
                messageContent.Add("ServiceParams", JsonHelper.ToJson(contentDic));
                Dictionary<string, object> resultDic = messageOp.SendMessage(messageType, messageContent);
                if (resultDic != null)
                {
                    VicMessageBoxNormal.Show(resultDic["ReplyContent"].ToString(), "提示", MessageBoxButton.OK);
                }
            }
        }
        #endregion
    }
}
