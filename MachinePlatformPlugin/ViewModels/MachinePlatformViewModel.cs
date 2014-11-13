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
                        cabinetInfoModel.CabinetMenuCode = string.IsNullOrEmpty(UCMachinePlatform.ParamDict["menucode"].ToString()) ? 0 : Convert.ToInt64(UCMachinePlatform.ParamDict["menucode"].ToString());
                        cabinetInfoModel.CabinetAuthorityCode = string.IsNullOrEmpty(UCMachinePlatform.ParamDict["authoritycode"].ToString()) ? 0 : Convert.ToInt64(UCMachinePlatform.ParamDict["authoritycode"].ToString());
                        InitCabinetData();

                    }
                });
            }
        }
        public ICommand MachinePlatformViewUnoadedCommand
        {
            get
            {
                return new RelayCommand<Object>((x) => {
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
                return new RelayCommand(() => {
                    //TODO:附加查询条件
                    datagridMaster.Search();
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
            //GetCabinetTaskState();
            MainViewAble = true;
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
            contentDic.Add("modelid", "victop_model_cabinet_0002");
            if (conClientList != null)
            {
                contentDic.Add("conditions", conClientList);
            }
            Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
            if (returnDic != null)
            {
                string viewId = returnDic["DataChannelId"].ToString();
                DataOperation dataOp = new DataOperation();
                DataTable cabinetInfoDt = dataOp.GetData(viewId, "[\"cabinet\"]").Tables["dataArray"];
                cabinetInfoModel.CabinetId = cabinetInfoDt.Rows[0]["_id"].ToString();
                cabinetInfoModel.CabinetBeginState = Convert.ToInt32(cabinetInfoDt.Rows[0]["begin_state"].ToString());
                cabinetInfoModel.CabinetEndState = Convert.ToInt32(cabinetInfoDt.Rows[0]["end_state"].ToString());
                cabinetInfoModel.CabinetName = cabinetInfoDt.Rows[0]["cabinet_name"].ToString();
            }
        }
        /// <summary>
        /// 获取机台状态
        /// </summary>
        private void GetCabinetTaskState()
        {
            string MessageType = "MongoDataChannelService.findBusiData";
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", cabinetInfoModel.SystemId);
            contentDic.Add("configsystemid", cabinetInfoModel.ConfigSystemId);
            contentDic.Add("modelid", "victop_model_cabinet_state_0001");
            List<Dictionary<string, object>> conList = new List<Dictionary<string, object>>();
            Dictionary<string, object> conDic = new Dictionary<string, object>();
            conDic.Add("name", "cabinet_state");
            conDic.Add("tablecondition", "[{\"cabinet_id\":\"" + cabinetInfoModel.CabinetId + "\"}]");
            conList.Add(conDic);
            contentDic.Add("conditions", conList);
            List<string> client_point_idList = new List<string>();
            Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
            if (returnDic != null)
            {
                DataOperation dataOp = new DataOperation();
                string viewId = returnDic["DataChannelId"].ToString();
                List<object> pathList = new List<object>();
                pathList.Add("cabinet_state");
                string dataPath = JsonHelper.ToJson(pathList);
                cabinetInfoModel.CabinetTaskStateDt = dataOp.GetData(viewId, dataPath).Tables["dataArray"];
                foreach (DataRow item in cabinetInfoModel.CabinetTaskStateDt.Rows)
                {
                    int stateNum = Convert.ToInt32(item["cabinet_state"].ToString());
                    switch (item["cabinet_state_name"].ToString())
                    {
                        case "未派工":
                            taskStateModel.UnDivideCode = stateNum;
                            break;
                        case "已派工":
                            taskStateModel.DivideCode = stateNum;
                            break;
                        case "已开工":
                            taskStateModel.StartWork = stateNum;
                            break;
                        case "挂起":
                            taskStateModel.SuspendWork = stateNum;
                            break;
                        case "已交工":
                            taskStateModel.EndWork = stateNum;
                            break;
                        case "已审核":
                            taskStateModel.EndReview = stateNum;
                            break;
                        default:
                            break;
                    }
                }
                DataRow dr = cabinetInfoModel.CabinetTaskStateDt.NewRow();
                dr["cabinet_state"] = 0;
                dr["cabinet_state_name"] = "所有";
                cabinetInfoModel.CabinetTaskStateDt.Rows.InsertAt(dr, 0);
                CabTaskStatusDt = cabinetInfoModel.CabinetTaskStateDt.Copy();
            }
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
        public T GetParentObject<T>(DependencyObject obj) where T : FrameworkElement
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
        #endregion
    }
}
