using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MachinePlatformPlugin.Views;
using System.Windows.Controls;
using Victop.Wpf.Controls;
using Victop.Frame.Components;
using System.Windows;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Runtime;
using Victop.Frame.SyncOperation;
using Microsoft.Win32;
using System.Net;
using System.Windows.Forms;
using Victop.Frame.DataChannel;
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

        private System.Windows.Controls.UserControl PageFlowMachinePlatformView;
        /// <summary>
        /// 图纸/指南区DataGrid
        /// </summary>
        private VicDataGrid dgridMap;
        /// <summary>
        /// 文件模板区
        /// </summary>
        private VicDataGrid dgridFile;
        /// <summary>
        /// 传出
        /// </summary>
        private VicDataGrid dgridWorkdOutput;
        /// <summary>
        /// 工作区
        /// </summary>
        private CompntSingleDataGridWithCheckBox comdgridWork;
        /// <summary>
        /// 明细
        /// </summary>
        private CompntSingleDataGrid comdgridDetail;
        /// <summary>
        /// 图纸/指南区数据标签
        /// 默认显示图片/模板区
        /// </summary>
        private bool vIGFlag = true;
        /// <summary>
        /// 文件模板区数据标签
        ///默认文件模板区隐藏 
        /// </summary>
        private bool vFFlag = false;
        /// <summary>
        /// 图纸/指南区按钮标签
        /// 默认显示黑体加粗
        /// </summary>
        private bool iStyleFlag = true;
        /// <summary>
        /// 文件模板区数据标签
        ///默认显示正常 
        /// </summary>
        private bool fStyleFlag = false;
        /// <summary>
        /// 客户名称
        /// </summary>
        private string client;
        /// <summary>
        /// DataTable控件
        /// </summary>
        private VicTabControlNormal tabWork;
        /// <summary>
        /// 状态ComBox
        /// </summary>
        private VicComboBoxNormal comStatus;
        private int i;//选择的行
        /// <summary>
        /// 取数ChannelId
        /// </summary>
        private string viewId = string.Empty;
        /// <summary>
        /// 框架取数操作对象
        /// </summary>
        DataOperation dataOp = new DataOperation();
        /// <summary>
        /// 客户表
        /// </summary>
        private DataTable DtClientData;
        DataRow drow;
        /// <summary>
        /// cabinetStaffId
        /// </summary>
        private string cabinetStaffId;
        /// <summary>
        /// 控制取消按钮IsEnable状态
        /// </summary>
        private bool cannelFlag = false;
        /// <summary>
        /// 权限标签
        /// </summary>
        private bool authorityFlag = false;
        /// <summary>
        /// SystemId
        /// </summary>
        private string systemId = "906";
        /// <summary>
        /// 取数configsystemid
        /// </summary>
        private string configsystemid = "101";
        /// <summary>
        /// spaceId
        /// </summary>
        private string spaceId = "victop_core";
        /// <summary>
        /// 菜单码
        /// </summary>
        private string menuCode;
        /// <summary>
        /// 权限码
        /// </summary>
        private string authorityCode;
        /// <summary>
        ///存储机台数据表
        /// </summary>
        private DataTable DtCabinetData;
        /// <summary>
        ///存储用户数据表
        /// </summary>
        private DataTable DtUserData;
        /// <summary>
        /// 传送带表
        /// </summary>
        private DataTable dtConveryor;
        private DataTable dtConData;
        private string taskId;
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
        /// 客户名称
        /// </summary>
        public string Client
        {
            get
            {
                return client;
            }
            set
            {
                if (client != value)
                {
                    client = value;
                }
                RaisePropertyChanged("Client");
            }
        }
        /// <summary>
        /// 图纸/指南区数据标签
        /// </summary>
        public bool VFFlag
        {
            get
            {
                return vFFlag;
            }
            set
            {
                if (vFFlag != value)
                {
                    vFFlag = value;
                }
                RaisePropertyChanged("VFFlag");
            }
        }
        /// <summary>
        /// 文件模板区数据标签
        /// </summary>
        public bool VIGFlag
        {
            get
            {
                return vIGFlag;
            }
            set
            {
                if (vIGFlag != value)
                {
                    vIGFlag = value;
                }
                RaisePropertyChanged("VIGFlag");
            }
        }
        /// <summary>
        /// 图纸/指南区按钮标签
        /// </summary>
        public bool IStyleFlag
        {
            get
            {
                return iStyleFlag;
            }
            set
            {
                if (iStyleFlag != value)
                {
                    iStyleFlag = value;
                }
                RaisePropertyChanged("IStyleFlag");
            }
        }
        /// <summary>
        /// 文件模板区按钮标签
        /// </summary>
        public bool FStyleFlag
        {
            get
            {
                return fStyleFlag;
            }
            set
            {
                if (fStyleFlag != value)
                {
                    fStyleFlag = value;
                }
                RaisePropertyChanged("FStyleFlag");
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
                    systemId = UCMachinePlatform.ParamDict["systemid"].ToString();
                    PageFlowMachinePlatformView = (System.Windows.Controls.UserControl)x;

                    dgridMap = (VicDataGrid)PageFlowMachinePlatformView.FindName("dgridMap");//图纸指南区
                    dgridFile = (VicDataGrid)PageFlowMachinePlatformView.FindName("dgridFile");//文件模板区

                    dgridWorkdOutput = (VicDataGrid)PageFlowMachinePlatformView.FindName("dgridWorkdOutput");//产出区
                    comdgridWork = (CompntSingleDataGridWithCheckBox)PageFlowMachinePlatformView.FindName("comdgridWork");//工作区

                    tabWork = (VicTabControlNormal)PageFlowMachinePlatformView.FindName("tabWork");//页签控件对象
                    comStatus = (VicComboBoxNormal)PageFlowMachinePlatformView.FindName("comStatus");//状态控件对象
                    comdgridDetail = (CompntSingleDataGrid)PageFlowMachinePlatformView.FindName("comdgridDetail");

                    comdgridWork.ParamsModel.SelectedItemChanged += ParamsModel_SelectedItemChanged;//注册工作区datagrid选项改变事件
                    //获取机台初始化数据
                    InitCabinetData();
                    UCMachinePlatform.ParamDict["cabinet_id"] = cabinetInfoModel.CabinetId;
                    //cabinetInfoModel.CabinetUserId = GetCabinetStaffData();
                    //工作区列表
                    comdgridWork.ParamsModel.SystemId = systemId;
                    comdgridWork.ParamsModel.SettingModel = JsonHelper.ToObject<CompntSettingModel>(FileHelper.ReadFitData("CompntSingleDataGridWithCheckBox_MachinePlatformPlugin_Work_Scheme_Setting"));
                    comdgridWork.DoRender();
                    //产出区创建数据源
                    DtData = CreateDataTable();
                    drow = DtData.NewRow();
                    #region 明细
                    tabWork.SelectionChanged += tabWork_SelectionChanged;
                    #endregion  
                    //机台状态默认值
                    comStatus.SelectedValue = taskStateModel.UnDivideCode;
                    Search();
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
                    ImaGui();
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
                    FileTem();
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
                    if (dgridMap.SelectedItem == null)
                    {
                        VicMessageBoxNormal.Show("请选择传入文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    try
                    {
                        Down(((DataRowView)dgridMap.SelectedItem).Row["name"].ToString(), ((DataRowView)dgridMap.SelectedItem).Row["file_type"].ToString());
                    }
                    catch (Exception)
                    {
                        VicMessageBoxNormal.Show("文件名或文件类型未知！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
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
                    if (comdgridWork.ParamsModel.GridSelectedRow == null)
                    {
                        VicMessageBoxNormal.Show("请选择一行工作内容！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    if (comdgridWork.ParamsModel.CheckedRows.Length > 1)
                    {
                        VicMessageBoxNormal.Show("只能选择一行工作内容下载文件，请勿多选！！！", "下载文件", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    try
                    {
                        Down(comdgridWork.ParamsModel.GridSelectedRow["file_path"].ToString(), comdgridWork.ParamsModel.GridSelectedRow["file_type"].ToString());
                    }
                    catch (Exception)
                    {
                        VicMessageBoxNormal.Show("文件名或文件类型未知！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
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
                    PruLog();
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

                    QusLog();
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
                    Devise();
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
                    BtnClear();
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
                    comdgridWork.Add();
                   
                    //添加生产日志
                    string strTemp = "\n";
                    strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " + cabinetInfoModel.CabinetName+"  新增任务";
                    comdgridWork.ParamsModel.GridSelectedRow["production_log"] += strTemp;
                    comdgridWork.ParamsModel.GridSelectedRow["current_state"] = taskStateModel.UnDivideCode;
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
                    BtnDelete();
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
                    Search();
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
                    string JsonStr = comdgridWork.Save();
                    string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                    string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败。
                    if (!string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值不等于1，则抛出操作失败的原因；反之，程序没有提示
                    {
                        VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
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
                    BtnDivide();
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
                    BtnWorkClick();

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
                    BtnHangupClick();
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
                    BtnUnHangupClick();
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
                    BtnEndWorkClick();
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
                    BtnExamineClick();
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
                    BtnRejectClick();
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
                    btnFeedback();
                });
            }
        }
        #endregion

        #endregion

        #region 方法
        /// <summary>
        /// 传送带方法(客户id)
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
            contentDic.Add("systemid", systemId);
            contentDic.Add("configsystemid", configsystemid);
            contentDic.Add("spaceId", spaceId);
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
                            paramValList.Add(dr[paramItem]);
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
                DtConData = ds.Tables["dataArray"];
            }
        }

        private void SetColum()
        {
            dgridMap.Columns[0].Header = "来源";
            dgridMap.Columns[1].Header = "名称";
            dgridMap.Columns[2].Header = "用途";
            dgridMap.Columns[3].Header = "文件类型";
            dgridMap.Columns[4].Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// 创建表
        /// </summary>
        /// <returns></returns>
        public DataTable CreateDataTable()
        {
            DataTable table = new DataTable("table");
            DataColumn[] columns = new DataColumn[] { new DataColumn("客户编号", typeof(string)), new DataColumn("客户", typeof(string)), new DataColumn("产出文件", typeof(string)), new DataColumn("文件类型", typeof(string))};
            table.Columns.AddRange(columns);
            return table;
        }
        /// <summary>
        /// 选中行给产出区赋值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="dr"></param>
        private void ParamsModel_SelectedItemChanged(object sender, DataRow dr)
        {
            DtData = CreateDataTable();
            drow = DtData.NewRow();
            drow["客户编号"] = dr["client_id"];
            drow["客户"] = dr["company_name"];
            drow["产出文件"] = comdgridWork.ParamsModel.GridSelectedRow["file_name"];
            drow["文件类型"] = comdgridWork.ParamsModel.GridSelectedRow["file_type"];
            DtData.Rows.Add(drow);
            ConveyorMethod(dr);
        }
        /// <summary>
        /// 取明细数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabWork_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source.GetType() == typeof(VicTabControlNormal))
            {
                if (comdgridWork.ParamsModel.GridSelectedRow == null && tabWork.SelectedIndex != 0)
                {
                    VicMessageBoxNormal.Show("请选择需要查看明细的数据", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    tabWork.SelectedIndex = 0;
                    return;
                }
                //明细 
                comdgridDetail.CompntDataGridParams.SystemId = systemId;
                comdgridDetail.CompntDataGridParams.SettingModel = JsonHelper.ToObject<CompntSettingModel>(FileHelper.ReadFitData("CompntSingleDataGrid_PageFlowMachinePlatform_Detail_Scheme_Setting"));
                comdgridDetail.DoRender();
                //条件
                List<object> conlist = new List<object>();
                Dictionary<string, object> tableConDic = new Dictionary<string, object>();

                if (comdgridWork.ParamsModel.GridSelectedRow != null && !string.IsNullOrEmpty(comdgridWork.ParamsModel.GridSelectedRow["_id"].ToString()))
                {
                    //RegexHelper.Contains(comdgridWork.ParamsModel.GridSelectedRow["_id"].ToString())
                    tableConDic.Add("client_point_id", RegexHelper.Contains(comdgridWork.ParamsModel.GridSelectedRow["_id"].ToString()));
                }
                conlist.Add(tableConDic);
                comdgridDetail.CompntDataGridParams.ConditionList = conlist;
                string JsonStr = comdgridDetail.Search();
                string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败 
                if (ResultMode != "1" && string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值不等于1，则抛出操作失败的原因；反之，程序没有提示
                {
                    VicMessageBoxNormal.Show(ResultMess, "查询", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        #region 图纸/指南区
        /// <summary>
        /// 图纸/指南区方法
        /// </summary>
        public void ImaGui()
        {
            //字体显示
            IStyleFlag = true;
            FStyleFlag = false;

            //数据显示
            VFFlag = false;
            VIGFlag = true;
        }
        #endregion

        #region 文件模板区
        /// <summary>
        /// 文件模板区方法
        /// </summary>
        public void FileTem()
        {
            //字体显示
            FStyleFlag = true;
            IStyleFlag = false;

            //数据显示
            VIGFlag = false;
            VFFlag = true;
        }
        #endregion

        #region 下载
        /// <summary>
        /// 下载
        /// </summary>
        public void Down(string fileName, string fileType)
        {
            bool isok = false;
            string FilePath = "";//本地保存路径
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.FileName = fileName;
            sfd.Filter = string.Format("{0}文件|*.{0}", fileType);
            sfd.OverwritePrompt = true;
            if (DialogResult.OK == sfd.ShowDialog())
            {
                try
                {
                    FilePath = sfd.FileName.Substring(0, sfd.FileName.LastIndexOf(".")) + "." + fileType;
                }
                catch (Exception)
                {
                    FilePath = sfd.FileName + "." + fileType;
                }
                isok = FtpUpLoadOrDownLoadHelper.fileDownload(FilePath, @"http://192.168.40.191:8080/fsweb/getfile?id=" + fileName);
                if (isok == true)
                {
                    VicMessageBoxNormal.Show("下载成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    VicMessageBoxNormal.Show("文件未找到！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
        }
        #endregion

        #region 生产日志
        /// <summary>
        /// 生产日志方法
        /// </summary>
        public void PruLog()
        {
            if (comdgridWork.ParamsModel.CheckedRows.Length != 1)
            {
                VicMessageBoxNormal.Show("查看生产日志时，只能选择单一任务查看，请重新选择!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            ProductLog win = new ProductLog();
            ProductLogViewModel.value = comdgridWork.ParamsModel.CheckedRows[0]["production_log"].ToString();
            win.ShowDialog();

        }
        #endregion

        #region 问题日志
        /// <summary>
        /// 问题日志方法
        /// </summary>
        public void QusLog()
        {
            if (comdgridWork.ParamsModel.CheckedRows.Length != 1)
            {
                VicMessageBoxNormal.Show("查看问题日志时，只能选择单一任务查看，请重新选择!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            IssueLog win = new IssueLog();
            IssueLogViewModel.value = comdgridWork.ParamsModel.CheckedRows[0]["issue_log"].ToString();
            win.ShowDialog();
        }
        #endregion

        #region 问题反馈
        private void btnFeedback()
        {
            if (comdgridWork.ParamsModel.CheckedRows.Length != 1)
            {
                VicMessageBoxNormal.Show("问题反馈时，只能选择单一任务反馈，请重新选择!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                string strTemp = "\n";
                strTemp += cabinetInfoModel.UserName + "说: " + GetSysServiceTime() + "\n";
                IsueFeedback win = new IsueFeedback();
                IsueFeedbackViewModel.valueInput = (strTemp + comdgridWork.ParamsModel.GridSelectedRow["issue_log"].ToString());
                win.ShowDialog();
                if (string.IsNullOrEmpty(IsueFeedbackViewModel.valueOutput))
                {
                    return;
                }
                strTemp = IsueFeedbackViewModel.valueInput;
                comdgridWork.ParamsModel.CheckedRows[0]["issue_log"] = strTemp;


                //添加生产日志
                string strTemp2 = "\n";
                strTemp += cabinetInfoModel.UserName + ":" + GetSysServiceTime() + "  " + "页面流程机台  问题反馈";
                comdgridWork.ParamsModel.GridSelectedRow["production_log"] += strTemp2;
                string JsonStr = comdgridWork.Save();
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
        #endregion

        #region 清除
        /// <summary>
        /// 清除方法
        /// </summary>
        public void Delete()
        {

        }
        #endregion

        #region 查询

        /// <summary>
        /// 查询方法
        /// </summary>
        public void Search()
        {
            try
            {
                List<object> conlist = new List<object>();
                Dictionary<string, object> tableConDic = new Dictionary<string, object>();
                    int state = Convert.ToInt32(comStatus.SelectedValue);

                    if (state == 0)//所有
                    {
                        Dictionary<string, object> taskStateDic = new Dictionary<string, object>();
                        taskStateDic.Add("$gte", cabinetInfoModel.CabinetBeginState);
                        taskStateDic.Add("$lte", cabinetInfoModel.CabinetEndState);
                        tableConDic.Add("current_state", taskStateDic);
                        if (string.IsNullOrEmpty(Client))
                        {
                            //comdgridWork.DoRender();
                            //return;
                        }
                        else
                        {
                            //获取返回的客户id
                            List<object> clientIdList = GetClientData(Client);
                            Dictionary<string, object> clientIdDic = new Dictionary<string, object>();
                            clientIdDic.Add("$in", clientIdList);
                            //赋值查询条件
                            tableConDic.Add("client_id", clientIdDic);
                        }
                    }
                    else
                    {
                        tableConDic.Add("current_state", state);
                        if (!string.IsNullOrEmpty(Client))
                        {
                            //获取返回的客户id
                            List<object> clientIdList = GetClientData(Client);
                            Dictionary<string, object> clientIdDic = new Dictionary<string, object>();
                            clientIdDic.Add("$in", clientIdList);
                            //赋值查询条件
                            tableConDic.Add("client_id", clientIdDic);
                        }
                    }
                    
                //获取查询条件，进行查询
                conlist.Add(tableConDic);
                comdgridWork.ParamsModel.ConditionList = conlist;
                string JsonStr = comdgridWork.Search();
                string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败 
                if (ResultMode != "1" && string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值不等于1，则抛出操作失败的原因；反之，程序没有提示
                {
                    VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            catch
            { }
        }

        #endregion

        #region 派工
        /// <summary>
        /// 派工
        /// </summary>
        private void BtnDivide()
        {

            try
            {
                if (comdgridWork.ParamsModel.CheckedRows.Length > 0)
                {
                    for (i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                    {
                        if (Convert.ToInt32(comdgridWork.ParamsModel.CheckedRows[i]["current_state"].ToString()) != taskStateModel.UnDivideCode)
                        {
                            VicMessageBoxNormal.Show("选择的任务存在无法派工的任务，请重新选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                    SelectPersonnel win = new SelectPersonnel();
                    win.ShowDialog();
                    if ((!SelectPersonnelViewModel.chooseFlag) || (string.IsNullOrEmpty(SelectPersonnelViewModel.guid)))//取消或直接关闭窗口，则不进行派工
                    {
                        VicMessageBoxNormal.Show("未选中任何人员，故不能派工！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    for (i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                    {
                        comdgridWork.ParamsModel.CheckedRows[i]["work_staff_id"] = SelectPersonnelViewModel.guid;
                        comdgridWork.ParamsModel.CheckedRows[i]["work_staff_name"] = SelectPersonnelViewModel.staffName;
                        comdgridWork.ParamsModel.CheckedRows[i]["current_state"] = taskStateModel.DivideCode;
                        comdgridWork.ParamsModel.CheckedRows[i]["startwork_time"] = DBNull.Value;
                        comdgridWork.ParamsModel.CheckedRows[i]["finishwork_time"] = DBNull.Value;
                        comdgridWork.ParamsModel.CheckedRows[i]["last_cabinet_id"] = "";
                        comdgridWork.ParamsModel.CheckedRows[i]["last_staff_id"] = "";
                        comdgridWork.ParamsModel.CheckedRows[i]["last_staff_name"] = "";
                        comdgridWork.ParamsModel.CheckedRows[i]["last_cabinet_name"] = "";
                        //添加生产日志
                        string strTemp = "\n";
                        strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " +cabinetInfoModel.CabinetName+ "     派工给:" + SelectPersonnelViewModel.staffName; ;

                        comdgridWork.ParamsModel.CheckedRows[i]["production_log"] += strTemp;
                    }
                    string JsonStr = comdgridWork.Save();
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
                else
                {
                    VicMessageBoxNormal.Show("请选择需要派工的任务", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            { }
        }
        #endregion

        #region 开工
        /// <summary>
        /// 开工
        /// </summary>
        private void BtnWorkClick()
        {
            try
            {
                if (comdgridWork.ParamsModel.CheckedRows.Length > 0)
                {
                    for (i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                    {
                        if (Convert.ToInt32(comdgridWork.ParamsModel.CheckedRows[i]["current_state"].ToString()) != taskStateModel.DivideCode)
                        {
                            VicMessageBoxNormal.Show("选择的任务存在无法开工的任务，请重新选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                    }
                    for (i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                    {
                        comdgridWork.ParamsModel.CheckedRows[i]["current_state"] = taskStateModel.StartWork;
                        comdgridWork.ParamsModel.CheckedRows[i]["startwork_time"] = GetSysServiceTime();
                        // 添加生产日志
                        string strTemp = "\n";
                        strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " +cabinetInfoModel.CabinetName+ "  开工  ";

                        comdgridWork.ParamsModel.CheckedRows[i]["production_log"] += strTemp;
                    }
                    string JsonStr = comdgridWork.Save();
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
                else
                {
                    VicMessageBoxNormal.Show("请选择需要开工的任务", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            { }
        }
        #endregion

        #region 挂起
        /// <summary>
        /// 挂起
        /// </summary>
        private void BtnHangupClick()
        {
            if (comdgridWork.ParamsModel.CheckedRows.Length > 0)
            {
                for (i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                {
                    if (Convert.ToInt32(comdgridWork.ParamsModel.CheckedRows[i]["current_state"].ToString()) != taskStateModel.StartWork)
                    {
                        VicMessageBoxNormal.Show("选择的任务存在无法挂起的任务，请重新选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                for (i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                {
                    comdgridWork.ParamsModel.CheckedRows[i]["current_state"] =taskStateModel.SuspendWork;
                    comdgridWork.ParamsModel.CheckedRows[i]["startwork_time"] = GetSysServiceTime();
                    // 添加生产日志
                    string strTemp = "\n";
                    strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " + cabinetInfoModel.CabinetName+"  挂起  " + comdgridWork.ParamsModel.CheckedRows[i]["company_name"].ToString();

                    comdgridWork.ParamsModel.CheckedRows[i]["production_log"] += strTemp;
                    // 添加问题日志
                }
                string JsonStr = comdgridWork.Save();
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
            else
            {
                VicMessageBoxNormal.Show("请选择需要挂起的任务", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        #region 解挂
        /// <summary>
        /// 解挂
        /// </summary>
        private void BtnUnHangupClick()
        {
            if (comdgridWork.ParamsModel.CheckedRows.Length > 0)
            {
                for (i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                {
                    if (Convert.ToInt32(comdgridWork.ParamsModel.CheckedRows[i]["current_state"].ToString()) != taskStateModel.SuspendWork)
                    {
                        VicMessageBoxNormal.Show("选择的任务存在无法解挂的任务，请重新选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                for (i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                {
                    comdgridWork.ParamsModel.CheckedRows[i]["current_state"] = taskStateModel.StartWork;
                    // 添加生产日志
                    string strTemp = "\n";
                    strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " + cabinetInfoModel.CabinetName+"  解挂  " + comdgridWork.ParamsModel.CheckedRows[i]["company_name"].ToString();

                    comdgridWork.ParamsModel.CheckedRows[i]["production_log"] += strTemp;

                }
                string JsonStr = comdgridWork.Save();
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
            else
            {
                VicMessageBoxNormal.Show("请选择需要解挂的任务", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        #region 获取服务器时间
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
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        private void BtnDelete()
        {
            if (comdgridWork.ParamsModel.CheckedRows.Length > 0)
            {
                if (MessageBoxResult.OK == VicMessageBoxNormal.Show("确定要删除当前选中的信息", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning))
                {
                    for (int i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                    {
                        comdgridWork.ParamsModel.GridSelectedRow = comdgridWork.ParamsModel.CheckedRows[i];
                        string strTemp = "\n";
                        strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " + "删除" + comdgridWork.ParamsModel.GridSelectedRow["company_no"].ToString() + ":" + comdgridWork.ParamsModel.GridSelectedRow["company_name"].ToString();

                        comdgridWork.ParamsModel.GridSelectedRow["production_log"] += strTemp;
                        comdgridWork.Delete();
                        comdgridWork.Save();
                    }

                }

            }
        }
        #endregion

        #region 清除
        /// <summary>
        /// 清除
        /// </summary>
        private void BtnClear()
        {
            if (MessageBoxResult.OK == VicMessageBoxNormal.Show("确定要删除当前选中的信息", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning))
            {
                if (comdgridWork.ParamsModel.GridSelectedRow == null && comdgridWork.ParamsModel.CheckedRows.Length == 0)
                {
                    VicMessageBoxNormal.Show("请选择先工作内容后，然后清除", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else if (comdgridWork.ParamsModel.CheckedRows.Length > 1)
                {
                    VicMessageBoxNormal.Show("只能选择一行数据清除，请勿多选！！！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    if (comdgridWork.ParamsModel.CheckedRows.Length == 1)
                    {
                        if (string.IsNullOrEmpty(comdgridWork.ParamsModel.CheckedRows[0]["file_name"].ToString()) || string.IsNullOrEmpty(comdgridWork.ParamsModel.CheckedRows[0]["file_type"].ToString()))
                        {
                            VicMessageBoxNormal.Show("未对任务进行设计，故不能清除！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        comdgridWork.ParamsModel.CheckedRows[0]["file_name"] = "";
                        comdgridWork.ParamsModel.CheckedRows[0]["file_type"] = "";
                        drow["产出文件"] = "";
                        drow["文件类型"] = "";
                    }
                    string JsonStr = comdgridWork.Save();
                    string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                    string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败。
                    if (ResultMode != "1" && !string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值不等于1，则抛出操作失败的原因；反之，程序没有提示
                    {
                        VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
        #endregion

        #region 获取登录用户名
        /// <summary>
        /// 获取登录用户名
        /// </summary>
        /// <returns></returns>
        public void GetUserName()
        {
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> result = messageOp.SendMessage("ServerCenterService.GetUserInfo", new Dictionary<string, object>());
            cabinetInfoModel.UserName = JsonHelper.ReadJsonString(result["ReplyContent"].ToString(), "UserName");
            cabinetInfoModel.UserCode = JsonHelper.ReadJsonString(result["ReplyContent"].ToString(), "UserCode");
            cabinetInfoModel.UserId = JsonHelper.ReadJsonString(result["ReplyContent"].ToString(), "UserId");
        }
        #endregion

        #region 交工
        /// <summary>
        /// 交工
        /// </summary>
        private void BtnEndWorkClick()
        {
            if (comdgridWork.ParamsModel.CheckedRows.Length != 1)
            {

                VicMessageBoxNormal.Show("任务交工时，只能选择单一任务反馈，请重新选择!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Convert.ToInt32(comdgridWork.ParamsModel.GridSelectedRow["current_state"].ToString()) !=taskStateModel.StartWork)
            {
                VicMessageBoxNormal.Show("选择任务非已开工状态任务,不能交工，请重新选择!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrEmpty(comdgridWork.ParamsModel.GridSelectedRow["file_name"].ToString()) || string.IsNullOrEmpty(comdgridWork.ParamsModel.GridSelectedRow["file_type"].ToString()))
            {
                VicMessageBoxNormal.Show("未对选择任务进行设计，保存！故不能交工!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //保存交工数据
            comdgridWork.ParamsModel.GridSelectedRow["current_state"] = taskStateModel.EndWork;
            comdgridWork.ParamsModel.GridSelectedRow["finishwork_time"] = GetSysServiceTime();
            comdgridWork.ParamsModel.GridSelectedRow["last_cabinet_id"] = cabinetInfoModel.CabinetId;
            comdgridWork.ParamsModel.GridSelectedRow["last_staff_id"] = cabinetInfoModel.CabinetUserId;
            comdgridWork.ParamsModel.GridSelectedRow["last_staff_name"] = cabinetInfoModel.CabinetUserName;
            comdgridWork.ParamsModel.GridSelectedRow["last_cabinet_name"] =cabinetInfoModel.CabinetName;

            //添加生产日志
            string strTemp = "\n";
            strTemp += cabinetInfoModel.UserName + ":" + GetSysServiceTime() + "  " + cabinetInfoModel.CabinetName + "  " + comdgridWork.ParamsModel.GridSelectedRow["company_no"].ToString() + "  " + comdgridWork.ParamsModel.GridSelectedRow["company_name"].ToString() + "  " + "任务交工";
            comdgridWork.ParamsModel.GridSelectedRow["production_log"] += strTemp;

            string JsonStr = comdgridWork.Save();
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
        #endregion

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        private void BtnExamineClick()
        {
            if (comdgridWork.ParamsModel.CheckedRows.Length > 0)
            {
                for (i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                {
                    if (Convert.ToInt32(comdgridWork.ParamsModel.CheckedRows[i]["current_state"].ToString()) !=taskStateModel.EndWork)
                    {
                        VicMessageBoxNormal.Show("选择的任务存在无法审核的任务，请重新选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                for (i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                {
                    comdgridWork.ParamsModel.CheckedRows[i]["current_state"] = taskStateModel.EndReview;
                    string strTemp = "\n";
                    strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " +cabinetInfoModel.CabinetName+ "  审核";

                    comdgridWork.ParamsModel.CheckedRows[i]["production_log"] += strTemp;
                }
                string JsonStr = comdgridWork.Save();
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
            else
            {
                VicMessageBoxNormal.Show("请选择需要审核的任务", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        #region 设计

        private void Devise()
        {
            if (comdgridWork.ParamsModel.CheckedRows.Length != 1)
            {
                VicMessageBoxNormal.Show("任务设计时，只能选择单一任务设计，请重新选择!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Convert.ToInt32(comdgridWork.ParamsModel.GridSelectedRow["current_state"].ToString()) != taskStateModel.StartWork)
            {
                VicMessageBoxNormal.Show("选择的任务为非开工状态的任务，请重新选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Dictionary<string, object> dic = new Dictionary<string, object>();
            taskId = comdgridWork.ParamsModel.GridSelectedRow["_id"].ToString();//获取任务id
            dic.Add("id", taskId);
            dic.Add("systemid", systemId);
            PluginOperation po = new PluginOperation();
            PluginModel pm = po.StratPlugin("PageFlowPlugin", dic);
            Window win = pm.PluginInterface.StartWindow;
            try
            {
                win.ShowDialog();
            }
            catch (Exception)
            {
                VicMessageBoxNormal.Show("工具调用失败，请确认是否安装Visio2013(32位)！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (pm.PluginInterface.ParamDict != null && pm.PluginInterface.ParamDict.Count > 0)
            {
                if (pm.PluginInterface.ParamDict.ContainsKey("page_flow_name") && pm.PluginInterface.ParamDict.ContainsKey("page_flow_type"))
                {
                    string strTime = GetSysServiceTime();
                    drow["产出文件"] = comdgridWork.ParamsModel.GridSelectedRow["file_name"] + "_页面地图机台_" + strTime.Substring(0, strTime.LastIndexOf(":")) + cabinetInfoModel.UserName;
                   drow["文件类型"] = pm.PluginInterface.ParamDict["page_flow_type"];
                   comdgridWork.ParamsModel.GridSelectedRow["file_path"] = pm.PluginInterface.ParamDict["page_flow_name"];
                   comdgridWork.ParamsModel.GridSelectedRow["file_name"] = comdgridWork.ParamsModel.GridSelectedRow["file_name"] + "_页面地图机台_" + strTime.Substring(0,strTime.LastIndexOf(":"))+ cabinetInfoModel.UserName;
                    comdgridWork.ParamsModel.GridSelectedRow["file_type"] = pm.PluginInterface.ParamDict["page_flow_type"];
                    comdgridWork.Save();
                }
            }
        }
        #endregion

        #region 驳回
        /// <summary>
        /// 驳回
        /// </summary>
        private void BtnRejectClick()
        {
            if (comdgridWork.ParamsModel.CheckedRows.Length > 0)
            {
                for (i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                {
                    if (Convert.ToInt32(comdgridWork.ParamsModel.CheckedRows[i]["current_state"].ToString()) != taskStateModel.EndWork)
                    {
                        VicMessageBoxNormal.Show("选择的任务存在无法驳回的任务，请重新选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                for (i = 0; i < comdgridWork.ParamsModel.CheckedRows.Length; i++)
                {
                    comdgridWork.ParamsModel.CheckedRows[i]["current_state"] = taskStateModel.StartWork;
                    DateTime dt = new DateTime(1970, 1, 1);
                    comdgridWork.ParamsModel.CheckedRows[i]["finishwork_time"] = dt;//驳回后完工时间重置
                    comdgridWork.ParamsModel.GridSelectedRow["last_cabinet_id"] = "";
                    comdgridWork.ParamsModel.GridSelectedRow["last_staff_id"] = "";
                    comdgridWork.ParamsModel.GridSelectedRow["last_staff_name"] = "";
                    comdgridWork.ParamsModel.GridSelectedRow["last_cabinet_name"] = "";
                   
                    // 添加生产日志
                    string strTemp = "\n";
                    strTemp += cabinetInfoModel.UserName + ": " + GetSysServiceTime() + ": " +cabinetInfoModel.CabinetName+ "  驳回";

                    comdgridWork.ParamsModel.CheckedRows[i]["production_log"] += strTemp;
                }
                string JsonStr = comdgridWork.Save();
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
            else
            {
                VicMessageBoxNormal.Show("请选择需要驳回的任务", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        #region 状态
        private int convertState(string strState)
        {
            switch (strState)
            {
                case "未派工":
                    return 100;
                case "已派工":
                    return 110;
                case "已开工":
                    return 120;
                case "挂起":
                    return 130;
                case "已交工":
                    return 140;
                case "已审核":
                    return 150;
                default:
                    return 0;
            }
        }
        #endregion 

        #region 初始化机台数据
        /// <summary>
        /// 初始化机台数据
        /// </summary>
        private void InitCabinetData()
        {
            GetUserName();
            try
            {
                MessageOperation messageOp = new MessageOperation();
                string MessageType = string.Empty;
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                #region 获取登录人员信息
                //GetUserData();
                #endregion
                #region 获取机台信息
                MessageType = "MongoDataChannelService.findBusiData";
                List<object> conCitionsList = new List<object>();
                Dictionary<string, object> tableConCabinetDic = new Dictionary<string, object>();
                tableConCabinetDic.Add("cabinet_no", "20020");
                conCitionsList.Add(tableConCabinetDic);
                //设置表参数
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
                contentDic.Add("systemid", systemId);
                contentDic.Add("configsystemid", configsystemid);
                contentDic.Add("spaceId", spaceId);
                contentDic.Add("modelid", "victop_model_cabinet_0002");
                if (conClientList != null)
                {
                    contentDic.Add("conditions", conClientList);
                }
                Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                if (returnDic != null)
                {
                    viewId = returnDic["DataChannelId"].ToString();
                    DataTable cabinetInfoDt = dataOp.GetData(viewId, "[\"cabinet\"]").Tables["dataArray"];
                    cabinetInfoModel.CabinetId = cabinetInfoDt.Rows[0]["_id"].ToString();
                    cabinetInfoModel.CabinetBeginState = Convert.ToInt32(cabinetInfoDt.Rows[0]["begin_state"].ToString());
                    cabinetInfoModel.CabinetEndState = Convert.ToInt32(cabinetInfoDt.Rows[0]["end_state"].ToString());
                    cabinetInfoModel.CabinetCode = cabinetInfoDt.Rows[0]["cabinet_no"].ToString();
                    cabinetInfoModel.CabinetName = cabinetInfoDt.Rows[0]["cabinet_name"].ToString();
                }
                #endregion

                #region 获取机台任务状态
                GetCabinetTaskState();
                cabinetInfoModel.CabinetUserId = GetCabinetStaffData("staff_id", cabinetInfoModel.UserId, "cabinet_id", cabinetInfoModel.CabinetId);
                #endregion

                #region 初始化传送带参数
                GetConveyorParams();
                #endregion
            }
            catch (Exception ex)
            {
                LoggerHelper.Error(ex.Message);
            }
        }
        #endregion

        #region 获取用户id
        /// <summary>
        /// 获取用户id
        /// </summary>
        private void GetUserData()
        {
            //传入编号
            List<object> conCitionsList = new List<object>();
            Dictionary<string, object> tableConCabinetDic = new Dictionary<string, object>();
            tableConCabinetDic.Add("userCode", cabinetInfoModel.UserCode);
            conCitionsList.Add(tableConCabinetDic);
            //设置表参数
            List<object> conClientList = null;
            if (conCitionsList != null && conCitionsList.Count > 0)
            {
                conClientList = new List<object>();
                Dictionary<string, object> conDic = new Dictionary<string, object>();
                conDic.Add("name", "pub_user_connect");
                conDic.Add("tablecondition", conCitionsList);
                conClientList.Add(conDic);
            }
            string MessageType = "MongoDataChannelService.findBusiData";
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", systemId);
            contentDic.Add("configsystemid", configsystemid);
            contentDic.Add("spaceId", spaceId);
            contentDic.Add("modelid", "victop_core_pub_user_connect_0001");
            if (conClientList != null)
            {
                contentDic.Add("conditions", conClientList);
            }
            Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
            if (returnDic != null)
            {
                viewId = returnDic["DataChannelId"].ToString();
                DtUserData = dataOp.GetData(viewId, "[\"pub_user_connect\"]").Tables["dataArray"];
                cabinetInfoModel.UserId = DtUserData.Rows[0]["pk_val"].ToString();
            }
        }
        #endregion
        /// <summary>
        /// 获取机台状态
        /// </summary>
        private void GetCabinetTaskState()
        {
            string MessageType = "MongoDataChannelService.findBusiData";
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", systemId);
            contentDic.Add("configsystemid", configsystemid);
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
                viewId = returnDic["DataChannelId"].ToString();
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
        #region 获取客户数据
        /// <summary>
        /// 获取客户数据
        /// </summary>
        private List<object> GetClientData(string strName)
        {

            List<object> clientList = new List<object>();//存储客户id
            //获取客户名称
            List<object> conCitionsList = new List<object>();
            Dictionary<string, object> tableConClientDic = new Dictionary<string, object>();
            tableConClientDic.Add("company_name", RegexHelper.Contains(strName).ToString());
            conCitionsList.Add(tableConClientDic);
            //设置客户表参数
            List<object> conClientList = null;
            if (conCitionsList != null && conCitionsList.Count > 0)
            {
                conClientList = new List<object>();
                Dictionary<string, object> conDic = new Dictionary<string, object>();
                conDic.Add("name", "pub_client");
                conDic.Add("tablecondition", conCitionsList);
                conClientList.Add(conDic);
            }
            //取数
            string MessageType = "MongoDataChannelService.findBusiData";
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", systemId);
            contentDic.Add("configsystemid", configsystemid);
            contentDic.Add("spaceId", spaceId);
            contentDic.Add("modelid", "victop_core_pub_client_0001");
            if (conClientList != null)
            {
                contentDic.Add("conditions", conClientList);
            }
            Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
            if (returnDic != null)
            {
                try
                {
                    viewId = returnDic["DataChannelId"].ToString();
                    DtClientData = dataOp.GetData(viewId, "[\"pub_client\"]").Tables["dataArray"];
                    for (int i = 0; i < DtClientData.Rows.Count; i++)
                    {
                        //获取模糊查询id
                        clientList.Add(DtClientData.Rows[i]["_id"].ToString());
                    }
                    return clientList;
                }
                catch (Exception)
                { }
                return clientList;
            }
            return clientList;
        }
        #endregion

        #region 获取机台id
        /// <summary>
        /// 获取机台id
        /// </summary>
        private void GetCabinetData()
        {
            try
            {
                //传入编号
                List<object> conCitionsList = new List<object>();
                Dictionary<string, object> tableConCabinetDic = new Dictionary<string, object>();
                tableConCabinetDic.Add("cabinet_no", "20020");
                conCitionsList.Add(tableConCabinetDic);
                //设置表参数
                List<object> conClientList = null;
                if (conCitionsList != null && conCitionsList.Count > 0)
                {
                    conClientList = new List<object>();
                    Dictionary<string, object> conDic = new Dictionary<string, object>();
                    conDic.Add("name", "cabinet");
                    conDic.Add("tablecondition", conCitionsList);
                    conClientList.Add(conDic);
                }
                string MessageType = "MongoDataChannelService.findBusiData";
                MessageOperation messageOp = new MessageOperation();
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("systemid", systemId);
                contentDic.Add("configsystemid", configsystemid);
                contentDic.Add("spaceId", spaceId);
                contentDic.Add("modelid", "victop_model_cabinet_0002");
                if (conClientList != null)
                {
                    contentDic.Add("conditions", conClientList);
                }
                Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                if (returnDic != null)
                {
                    viewId = returnDic["DataChannelId"].ToString();
                    DtCabinetData = dataOp.GetData(viewId, "[\"cabinet\"]").Tables["dataArray"];
                    //cabinetId = DtCabinetData.Rows[0]["_id"].ToString();
                    //cabinetBeginState = Convert.ToInt64(DtCabinetData.Rows[0]["begin_state"].ToString());
                    cabinetInfoModel.CabinetId = DtCabinetData.Rows[0]["_id"].ToString();
                    cabinetInfoModel.CabinetBeginState = Convert.ToInt32(DtCabinetData.Rows[0]["begin_state"].ToString());
                    cabinetInfoModel.CabinetEndState = Convert.ToInt32(DtCabinetData.Rows[0]["end_state"].ToString());
                    cabinetInfoModel.CabinetCode = DtCabinetData.Rows[0]["cabinet_no"].ToString();
                    cabinetInfoModel.CabinetName = DtCabinetData.Rows[0]["cabinet_name"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 获取cabinetStaffid
        /// <summary>
        /// 获取cabinetStaffid
        /// </summary>
        private string GetCabinetStaffData(string tableStaffFiled, string inputStaffFiled, string tableCabinetFile, string inputCabintFiled)
        {
            try
            {
                //传入编号
                List<object> conCitionsList = new List<object>();
                Dictionary<string, object> tableConCabinetDic = new Dictionary<string, object>();
                tableConCabinetDic.Add(tableStaffFiled, inputStaffFiled);
                tableConCabinetDic.Add(tableCabinetFile, inputCabintFiled);
                conCitionsList.Add(tableConCabinetDic);
                //设置表参数
                List<object> conClientList = null;
                if (conCitionsList != null && conCitionsList.Count > 0)
                {
                    conClientList = new List<object>();
                    Dictionary<string, object> conDic = new Dictionary<string, object>();
                    conDic.Add("name", "cabinet_staff");
                    conDic.Add("tablecondition", conCitionsList);
                    conClientList.Add(conDic);
                }
                string MessageType = "MongoDataChannelService.findBusiData";
                MessageOperation messageOp = new MessageOperation();
                Dictionary<string, object> contentDic = new Dictionary<string, object>();
                contentDic.Add("systemid", systemId);
                contentDic.Add("configsystemid", configsystemid);
                contentDic.Add("spaceId", spaceId);
                contentDic.Add("modelid", "victop_model_cabinet_staff_0002");
                if (conClientList != null)
                {
                    contentDic.Add("conditions", conClientList);
                }
                Dictionary<string, object> returnDic = messageOp.SendMessage(MessageType, contentDic, "JSON");
                if (returnDic != null)
                {
                    viewId = returnDic["DataChannelId"].ToString();
                    DataTable DtCSData = dataOp.GetData(viewId, "[\"cabinet_staff\"]").Tables["dataArray"];
                    if (DtCSData.Rows.Count == 0)
                    {
                        VicMessageBoxNormal.Show("您对该机台没有作业权限，所做工作将不能交工！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return null;
                    }
                    cabinetStaffId = DtCSData.Rows[0]["_id"].ToString();
                    cabinetInfoModel.CabinetUserName = DtCSData.Rows[0]["cabinet_staff_name"].ToString();
                }
                return cabinetStaffId;
            }
            catch (Exception)
            {

                throw;
            }
            return cabinetStaffId;
        }
        #endregion

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
            contentDic.Add("systemid", systemId);
            contentDic.Add("configsystemid", configsystemid);
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
                viewId = returnDic["DataChannelId"].ToString();
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

        #region 获取所有按钮对象
        /// <summary>
        /// 获取所有按钮对象
        /// </summary>
        private void checkPower()
        {
            VicButtonNormal btnAdd = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnAdd");
            VicButtonNormal btnDeleteT = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnDelete");
            VicButtonNormal btnSave = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnSave");
            VicButtonNormal btnAllotWork = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnAllotWork");
            VicButtonNormal btnStart = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnStart");
            VicButtonNormal btnSuspend = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnSuspend");
            VicButtonNormal btnContinue = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnContinue");
            VicButtonNormal btnEnd = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnEnd");
            VicButtonNormal btnCheckup = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnCheckup");
            VicButtonNormal btnBack = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnBack");
            VicButtonNormal btnIssueBack = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnIssueBack");
            VicButtonNormal btnSearch = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnSearch");
            VicButtonNormal btnPruLog = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnPruLog");
            VicButtonNormal btnQusLog = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnQusLog");
            VicButtonNormal btnUpFile = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnUpFile");

            VicButtonNormal btnDelete = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnDelete");
            VicButtonNormal btnDown = (VicButtonNormal)PageFlowMachinePlatformView.FindName("btnDown");
            ////////////////////////////////////////////////////////
            if (string.Compare(menuCode, "32") < 0)//管理员
            {
                btnStart.Visibility = Visibility.Collapsed;
                btnSuspend.Visibility = Visibility.Collapsed;
                btnEnd.Visibility = Visibility.Collapsed;

                //初始状态为“未开工”
                comStatus.SelectedIndex = 2;
            }
            else//普通角色
            {
                ////////////////////
                btnAdd.Visibility = Visibility.Collapsed;
                btnDeleteT.Visibility = Visibility.Collapsed;
                btnSave.Visibility = Visibility.Collapsed;

                btnAllotWork.Visibility = Visibility.Collapsed;
                btnContinue.Visibility = Visibility.Collapsed;
                btnCheckup.Visibility = Visibility.Collapsed;

                btnBack.Visibility = Visibility.Collapsed;
                btnDeleteT.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;
                ////////////////////

                //初始状态为“已派工”
                comStatus.SelectedIndex = 3;
            }
        }
        #endregion
        #endregion
    }
}
