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
namespace MachinePlatformPlugin.ViewModels
{
    public class MachinePlatformViewModel : ModelBase
    {
        #region 字段

        private UCMachinePlatform ucMachineMainView;
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
                    ucMachineMainView = (UCMachinePlatform)x;
                    cabinetInfoModel.SystemId = UCMachinePlatform.ParamDict["systemid"].ToString();
                    cabinetInfoModel.ConfigSystemId = UCMachinePlatform.ParamDict["configsytemid"].ToString();
                    cabinetInfoModel.SpaceId = UCMachinePlatform.ParamDict["spaceid"].ToString();
                    cabinetInfoModel.CabinetFitData = JsonHelper.ToObject<List<Dictionary<string, object>>>(JsonHelper.ToJson(UCMachinePlatform.ParamDict["fitdata"]));
                    cabinetInfoModel.CabinetCADName = UCMachinePlatform.ParamDict["cadname"].ToString();
                    cabinetInfoModel.CabinetCode = UCMachinePlatform.ParamDict["menuno"].ToString();

                    CompntSingleDataGridWithCheckBox datagridMaster = (CompntSingleDataGridWithCheckBox)ucMachineMainView.FindName("datagridMaster");
                    InitCabinetData();
                    datagridMaster.ParamsModel.SystemId = cabinetInfoModel.SystemId;
                    string masterSetting = cabinetInfoModel.CabinetFitData.Find(it => it["key"].ToString().Equals("datagridMaster"))["value"].ToString();
                    datagridMaster.ParamsModel.SettingModel = JsonHelper.ToObject<CompntSettingModel>(FileHelper.ReadFitData(masterSetting));
                    
                    string JsonStr = datagridMaster.DoRender();
                    datagridMaster.Search();
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
                return new RelayCommand(() => {
                    OperationWindow opWindow = new OperationWindow(cabinetInfoModel);
                    opWindow.ShowDialog();
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
            GetLoginUserInfo();
            GetCabinetInfo();
            GetCabinetTaskState();
            MainViewAble = true;
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
            tableConCabinetDic.Add("cabinet_no", "30030");
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
        #endregion
    }
}
