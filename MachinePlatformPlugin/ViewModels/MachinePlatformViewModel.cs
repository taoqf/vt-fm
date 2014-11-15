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
        private DataTable dtConData;
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
                return new RelayCommand(() =>
                {

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
        /// 传入区下载
        /// </summary>
        public ICommand btnDownFileRightClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

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
                    SelectPersonnel peronWindow = new SelectPersonnel(this);
                    peronWindow.Owner = GetParentObject<Window>(ucMachineMainView);
                    peronWindow.ShowDialog();
                    if(cabinetInfoModel.CabinetSelectedStaff!=null)
                    {
                        for (int i = 0; i < datagridMaster.ParamsModel.CheckedRows.Length; i++)
                        {
                            datagridMaster.ParamsModel.CheckedRows[i]["work_staff_no"] = cabinetInfoModel.CabinetSelectedStaff["staff_no"];
                            datagridMaster.ParamsModel.CheckedRows[i]["wt_state"] = ((long)TaskStateEnum.未派工).ToString();
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
                return new RelayCommand<object>((x) => {
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
                return new RelayCommand<object>((x) => {
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
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.ErrorFormat("获取机台基础数据异常:{0}",ex.Message);
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

        #region  初始化传送带传入区参数
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
        #endregion
        #endregion
    }
}
