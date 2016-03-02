using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Victop.Frame.DataMessageManager;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls;
using Victop.Wpf.Controls;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 单据模板类
    /// </summary>
    public class BillTemplateControl : TemplateControl
    {
        #region 字段
        /// <summary>
        /// 单号
        /// </summary>
        public string doccode = "newdoccode";
        /// <summary>
        /// 数据引用操作
        /// </summary>
        public DataMessageOperation dataOp = new DataMessageOperation();
        /// <summary>
        /// window窗口
        /// </summary>
        private VicWindowNormal window;
        /// <summary>
        /// 活动插件按钮
        /// </summary>
        public VicButtonNormal btnPluginList = new VicButtonNormal();
        /// <summary>
        /// 显示活动插件的窗体
        /// </summary>
        private VicWindowNormal win_PluginList;
        /// <summary>
        /// 活动插件列表
        /// </summary>
        private List<Dictionary<string, object>> pluginList;
        #endregion

        #region 属性
        private TCBillDefinition billDefinition;
        /// <summary>
        /// 凭证定义
        /// </summary>
        public TCBillDefinition BillDefinition
        {
            get
            {
                if (billDefinition == null)
                {
                    billDefinition = new TCBillDefinition();
                }
                return billDefinition;
            }
            set { billDefinition = value; }
        }
        private string objectId = string.Empty;
        /// <summary>
        /// 插件标识
        /// </summary>
        public string ObjectId
        {
            get { return objectId; }
            set { objectId = value; }
        }
        private string businessTypeNo = string.Empty;
        /// <summary>
        /// 业务类型编号
        /// </summary>
        public string BusinessTypeNo
        {
            get { return businessTypeNo; }
            set { businessTypeNo = value; }
        }
        private Dictionary<string, object> docStatusDic;
        /// <summary>
        /// 单号状态键值对
        /// </summary>
        public Dictionary<string, object> DocStatusDic
        {
            get
            {
                if (docStatusDic == null)
                {
                    docStatusDic = new Dictionary<string, object>();
                }
                return docStatusDic;
            }
            set { docStatusDic = value; }
        }
        private DataTable dtSummary;
        /// <summary>
        /// 汇总
        /// </summary>
        public DataTable DtSummary
        {
            get
            {
                if (dtSummary == null)
                {
                    dtSummary = new DataTable();
                }
                return dtSummary;
            }
            set
            {
                dtSummary = value;
            }
        }
        private DataTable dtVoucherMaster;
        /// <summary>
        /// 凭证定义主表
        /// </summary>
        public DataTable DtVoucherMaster
        {
            get
            {
                if (dtVoucherMaster == null)
                {
                    dtVoucherMaster = new DataTable();
                }
                return dtVoucherMaster;
            }
            set
            {
                dtVoucherMaster = value;
            }
        }
        private DataTable dtGridDataCopy;
        /// <summary>
        /// 数据复制
        /// </summary>
        public DataTable DtGridDataCopy
        {
            get
            {
                if (dtGridDataCopy == null)
                {
                    dtGridDataCopy = new DataTable();
                }
                return dtGridDataCopy;
            }
            set
            {
                dtGridDataCopy = value;
            }
        }
        private DataTable dtMaster;
        /// <summary>
        /// 业务数据主表
        /// </summary>
        public DataTable DtMaster
        {
            get
            {
                if (dtMaster == null)
                {
                    dtMaster = new DataTable();
                }
                return dtMaster;
            }
            set
            {
                dtMaster = value;
            }
        }
        private DataTable dtBusinessType;
        /// <summary>
        /// 业务类型
        /// </summary>
        public DataTable DtBusinessType
        {
            get
            {
                if (dtBusinessType == null)
                {
                    dtBusinessType = new DataTable();
                }
                return dtBusinessType;
            }
            set
            {
                dtBusinessType = value;
            }
        }
        private DataTable dtPrintTemplate;
        /// <summary>
        /// 打印模板
        /// </summary>
        public DataTable DtPrintTemplate
        {
            get
            {
                if (dtPrintTemplate == null)
                {
                    dtPrintTemplate = new DataTable();
                }
                return dtPrintTemplate;
            }
            set
            {
                dtPrintTemplate = value;
            }
        }
        private int editState = 0;
        /// <summary>
        /// 编辑状态
        /// </summary>
        public int EditState
        {
            get { return editState; }
            set { editState = value; }
        }
        private int position = 1;
        /// <summary>
        /// 位置表示
        /// </summary>
        public int Position
        {
            get { return position; }
            set { position = value; }
        }
        private DataTable dtFormula;
        /// <summary>
        /// 公式
        /// </summary>
        public DataTable DtFormula
        {
            get
            {
                if (dtFormula == null)
                {
                    dtFormula = new DataTable();
                }
                return dtFormula;
            }
            set
            {
                dtFormula = value;
            }
        }
        private DataTable dtQueryFields;
        /// <summary>
        /// 查询字段
        /// </summary>
        public DataTable DtQueryFields
        {
            get
            {
                if (dtQueryFields == null)
                {
                    dtQueryFields = new DataTable();
                }
                return dtQueryFields;
            }
            set
            {
                dtQueryFields = value;
            }
        }
        private DataTable dtDataCheck;
        /// <summary>
        /// 范围检查
        /// </summary>
        public DataTable DtDataCheck
        {
            get
            {
                if (dtDataCheck == null)
                {
                    dtDataCheck = new DataTable();
                }
                return dtDataCheck;
            }
            set
            {
                dtDataCheck = value;
            }
        }
        private DataTable dtDefaultValues;
        /// <summary>
        /// 默认值
        /// </summary>
        public DataTable DtDefaultValues
        {
            get
            {
                if (dtDefaultValues == null)
                {
                    dtDefaultValues = new DataTable();
                }
                return dtDefaultValues;
            }
            set
            {
                dtDefaultValues = value;
            }
        }
        private DataTable dtClearData;
        /// <summary>
        /// 清空数据
        /// </summary>
        public DataTable DtClearData
        {
            get
            {
                if (dtClearData == null)
                {
                    dtClearData = new DataTable();
                }
                return dtClearData;
            }
            set
            {
                dtClearData = value;
            }
        }
        private DataTable dtDomTree;
        /// <summary>
        /// dom树
        /// </summary>
        public DataTable DtDomTree
        {
            get
            {
                if (dtDomTree == null)
                {
                    dtDomTree = new DataTable();
                }
                return dtDomTree;
            }
            set
            {
                dtDomTree = value;
            }
        }
        private DataTable dtListField;
        /// <summary>
        /// 列表显示字段
        /// </summary>
        public DataTable DtListField
        {
            get
            {
                if (dtListField == null)
                {
                    dtListField = new DataTable();
                }
                return dtListField;
            }
            set
            {
                dtListField = value;
            }
        }
        private List<AutoObject> objectPool;
        /// <summary>
        /// 控件池
        /// </summary>
        public List<AutoObject> ObjectPool
        {
            get
            {
                if (objectPool == null)
                {
                    objectPool = new List<AutoObject>();
                }
                return objectPool;
            }
            set { objectPool = value; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 凭证初始化
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            try
            {
                BillDefinition.Init();//初始化凭证定义PVDJson
                BillDefinition.FormId = FormId;
                DtVoucherMaster = BillDefinition.GetData("masterPblock");//获取主表数据
                if (DtVoucherMaster.Rows.Count == 0)
                {
                    return false;
                }
                DocStatusDic.Clear();//单据状态展示数据
                DocStatusDic.Add("-1", "已提交确认");
                DocStatusDic.Add("-2", "已被冲销");
                int predocstatus = Convert.ToInt32(DtVoucherMaster.Rows[0]["predocstatus"].ToString());
                int docstatus = Convert.ToInt32(DtVoucherMaster.Rows[0]["docstatus"].ToString());
                int auditstatus = Convert.ToInt32(DtVoucherMaster.Rows[0]["auditstatus"].ToString());
                if (!DocStatusDic.ContainsKey(predocstatus.ToString()))
                {
                    DocStatusDic.Add(predocstatus.ToString(), "制单");
                }
                if (auditstatus > predocstatus && auditstatus < docstatus)
                {
                    if (!DocStatusDic.ContainsKey((auditstatus - 1).ToString()))
                    {
                        DocStatusDic.Add((auditstatus - 1).ToString(), "审核未通过");
                    }
                    if (!DocStatusDic.ContainsKey("-100"))
                    {
                        DocStatusDic.Add("-100", "已作废");
                    }
                    if (!DocStatusDic.ContainsKey(auditstatus.ToString()))
                    {
                        DocStatusDic.Add(auditstatus.ToString(), "审核通过");
                    }
                }
                if (!DocStatusDic.ContainsKey(docstatus.ToString()))
                {
                    DocStatusDic.Add(docstatus.ToString(), "已确认");
                }
                DtDomTree = BillDefinition.GetData("dom_treePblock");
                DtListField = BillDefinition.GetData("list_fieldsPblock");
                DtBusinessType = BillDefinition.GetData("business_typePblock");
                DtGridDataCopy = BillDefinition.GetData("griddatacopyPblock");
                DtSummary = BillDefinition.GetData("summaryPblock");
                foreach (DataRow dr in BillDefinition.GetData("gsdocstatePblock").Rows)
                {
                    if (!DocStatusDic.ContainsKey(dr["docstatus"].ToString()))
                    {
                        DocStatusDic.Add(dr["docstatus"].ToString(), dr["state"].ToString());
                    }
                }
                string jsonCompntDefine = DtVoucherMaster.Rows[0]["compnt_define"].ToString();//单据PVD图数据json
                if (string.IsNullOrEmpty(jsonCompntDefine))
                {
                    return false;
                }
                btnPluginList.MouseEnter += btnActivePlugin_MouseEnter;
                btnPluginList.SetResourceReference(StyleProperty, "btnActivePluginStyle");
                //初始化快捷键
                if (Position == 2)
                {
                    window = XamlTreeHelper.GetParentObject<VicWindowNormal>(this);
                    if (!string.IsNullOrEmpty(doccode) && !doccode.Equals("newdoccode"))
                    {
                        RegBusinessKey();
                    }
                }
                return base.InitVictopUserControl(jsonCompntDefine);
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("单据初始化异常：{0}", ex.Message);
                return false;
            }
        }

        #region 弹窗显示获取的插件信息
        /// <summary>
        /// 活动插件按钮鼠标经过事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnActivePlugin_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                //获取插件信息
                pluginList = dataOp.GetPluginInfo();
                //创建窗体
                if (win_PluginList == null)
                {
                    win_PluginList = new VicWindowNormal();
                    //状态栏不显示
                    win_PluginList.ShowInTaskbar = false;
                    //窗体大小自适应其内容
                    win_PluginList.MouseLeave += win_PluginList_MouseLeave;
                    win_PluginList.SizeToContent = SizeToContent.Manual;
                    win_PluginList.SizeToContent = SizeToContent.Width;
                    win_PluginList.SizeToContent = SizeToContent.Height;
                    win_PluginList.SizeToContent = SizeToContent.WidthAndHeight;
                    win_PluginList.WindowStyle = WindowStyle.None;
                    win_PluginList.ResizeMode = ResizeMode.NoResize;
                    win_PluginList.Visibility = Visibility.Visible;
                }
                if (pluginList.Count < 1)
                {
                    win_PluginList.Visibility = Visibility.Hidden;
                    return;
                }
                win_PluginList.Content = GetActivePluginInfo();
                this.win_PluginList.Visibility = Visibility.Visible;
                if (!win_PluginList.IsActive)
                {
                    System.Windows.Point point = btnPluginList.PointToScreen(new System.Windows.Point(-90, 0));
                    win_PluginList.Left = point.X;
                    win_PluginList.Top = point.Y + btnPluginList.Height + 5;
                    win_PluginList.Show();
                    win_PluginList.Activate();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("活动插件按钮鼠标经过事件异常：{0}", ex.Message);
            }
        }
        /// <summary>
        /// 弹窗显示获取的插件信息
        /// </summary>
        private VicStackPanelNormal GetActivePluginInfo()
        {
            VicStackPanelNormal PluginListContent = new VicStackPanelNormal();
            try
            {
                PluginListContent.Orientation = Orientation.Vertical;
                PluginListContent.Width = 120;
                foreach (Dictionary<string, object> PluginInfo in pluginList)
                {
                    VicButtonNormal btn = new VicButtonNormal();
                    btn.Width = 120;
                    IPlugin Plugin = PluginInfo["IPlugin"] as IPlugin;
                    if (Plugin.ShowType == 0)
                    {
                        if (Plugin.ParamDict.ContainsKey("Title"))
                        {
                            if (Plugin.ParamDict["Title"].ToString().Length > 8)
                            {
                                btn.Content = Plugin.ParamDict["Title"].ToString().Substring(0, 6) + "...";
                            }
                            else
                            {
                                btn.Content = Plugin.ParamDict["Title"];
                            }
                            btn.ToolTip = Plugin.ParamDict["Title"];
                        }
                        else
                        {
                            btn.Content = Plugin.PluginTitle;
                            btn.ToolTip = Plugin.PluginTitle;
                        }
                        btn.Tag = PluginInfo;
                        btn.Click += ActivatePlugin_Click;
                        PluginListContent.Children.Add(btn);
                    }
                    else
                    {
                        btn.Content = Plugin.PluginTitle;
                        btn.Tag = PluginInfo;
                        btn.Click += ActivatePlugin_Click;
                        PluginListContent.Children.Add(btn);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("弹窗显示获取的插件信息异常：{0}", ex.Message);
            }
            return PluginListContent;
        }
        /// <summary>
        /// 点击活动插件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActivatePlugin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VicButtonNormal btn = sender as VicButtonNormal;
                Dictionary<string, object> pluginInfo = (Dictionary<string, object>)btn.Tag;
                IPlugin Plugin = pluginInfo["IPlugin"] as IPlugin;
                string PluginUid = pluginInfo["ObjectId"].ToString();
                if (Plugin.ShowType == 0)//窗口
                {
                    WindowCollection WinCollection = Application.Current.Windows;
                    for (int i = 0; i < WinCollection.Count; i++)
                    {
                        if (WinCollection[i].Uid.Equals(PluginUid))
                        {
                            switch (WinCollection[i].ResizeMode)
                            {
                                case ResizeMode.NoResize:
                                case ResizeMode.CanMinimize:
                                    WinCollection[i].WindowState = WindowState.Normal;
                                    break;
                                case ResizeMode.CanResize:
                                case ResizeMode.CanResizeWithGrip:
                                    WinCollection[i].WindowState = WindowState.Maximized;
                                    break;
                            }
                            WinCollection[i].Activate();
                            break;
                        }
                    }
                }
                else
                {
                    XamlTreeHelper.GetParentObject<VicWindowNormal>(this).WindowState = WindowState.Minimized;
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("活动插件激活异常：{0}", ex.Message);
            }
        }
        private void win_PluginList_MouseLeave(object sender, MouseEventArgs e)
        {
            this.win_PluginList.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 设置活动插件标题
        /// </summary>
        /// <param name="windowTitle"></param>
        public void SetPluginTitle(string windowTitle)
        {
            try
            {
                List<Dictionary<string, object>> pluginList = new List<Dictionary<string, object>>();
                pluginList = dataOp.GetPluginInfo();
                foreach (Dictionary<string, object> plugin in pluginList)
                {
                    IPlugin Plugin = plugin["IPlugin"] as IPlugin;
                    if (Plugin.ParamDict.ContainsKey("doccode") && Plugin.ParamDict["doccode"].ToString() == doccode)
                    {
                        if (Plugin.ParamDict.ContainsKey("Title"))
                        {
                            Plugin.ParamDict["Title"] = windowTitle;
                        }
                        else
                        {
                            Plugin.ParamDict.Add("Title", windowTitle);
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("设置活动插件标题异常：{0}" + ex.Message);
            }
        }
        #endregion

        #region 配置方法
        /// <summary> 
        /// 动态按钮只读设置 
        /// </summary> 
        public void SetReadOnly()
        {
            AutoObject.isreadonly = true;
            if (AutoObject.isreadonly)
            {
                if (ObjectPool != null)
                {
                    foreach (AutoObject o in ObjectPool)
                    {
                        switch (o.objType)
                        {
                            case "VicTextBoxBelowBor":
                                ((VicTextBoxBelowBor)o.obj).IsReadOnly = true;
                                break;
                            case "VicTextBox":
                                ((VicTextBox)o.obj).IsReadOnly = true;
                                break;
                            case "VicComboBoxNormal":
                                ((VicComboBoxNormal)o.obj).IsEnabled = false;
                                break;
                            case "VicDatePickerNormal":
                                ((VicDatePickerNormal)o.obj).IsEnabled = false;
                                break;
                            case "VicCheckBoxNormal":
                                ((VicCheckBoxNormal)o.obj).IsEnabled = false;
                                break;
                            case "VicButtonNormal":
                                ((VicButtonNormal)o.obj).IsEnabled = false;
                                break;
                            case "VicDataGrid":
                                ((VicDataGrid)o.obj).IsReadOnly = true;
                                break;
                            case "VicNumericUpDown":
                                ((VicDataGrid)o.obj).IsReadOnly = true;
                                break;
                            default:
                                break;
                        }
                    }
                }

            }
        }
        /// <summary> 
        /// 动态按钮恢复只读前设置 
        /// </summary> 
        public void ReSetReadOnly()
        {
            AutoObject.isreadonly = false;
            if (!AutoObject.isreadonly)
            {
                if (ObjectPool != null)
                {
                    foreach (AutoObject o in ObjectPool)
                    {
                        switch (o.objType)
                        {
                            case "VicTextBoxBelowBor":
                                ((VicTextBoxBelowBor)o.obj).IsReadOnly = o.readonlyByfit;
                                break;
                            case "VicTextBox":
                                ((VicTextBox)o.obj).IsReadOnly = o.readonlyByfit;
                                break;
                            case "VicComboBoxNormal":
                                ((VicComboBoxNormal)o.obj).IsEnabled = o.readonlyByfit;
                                break;
                            case "VicDatePickerNormal":
                                ((VicDatePickerNormal)o.obj).IsEnabled = o.readonlyByfit;
                                break;
                            case "VicCheckBoxNormal":
                                ((VicCheckBoxNormal)o.obj).IsEnabled = o.readonlyByfit;
                                break;
                            case "VicButtonNormal":
                                ((VicButtonNormal)o.obj).IsEnabled = o.readonlyByfit;
                                break;
                            case "VicDataGrid":
                                ((VicDataGrid)o.obj).IsReadOnly = o.readonlyByfit;
                                break;
                            case "VicNumericUpDown":
                                ((VicNumericUpDown)o.obj).IsReadOnly = o.readonlyByfit;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        ///  注册动态创建的控件(向控件池中添加控件对象)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <param name="readOnly"></param>
        public void RegControl(object obj, string type, bool readOnly)
        {
            AutoObject item = new AutoObject();
            item.obj = obj;
            item.objType = type;
            item.readonlyByfit = readOnly;
            if (ObjectPool.FirstOrDefault(it => it.obj.Equals(obj)) == null)
            {
                ObjectPool.Add(item);
            }
        }
        /// <summary>
        /// 设置详情界面所有控件状态
        /// </summary>
        public void SetControlStatus()
        {
            try
            {
                if (DtMaster.Rows.Count > 0 && DtVoucherMaster.Rows.Count > 0)
                {
                    if (DtMaster.Rows[0]["docstatus"].ToString() == DtVoucherMaster.Rows[0]["predocstatus"].ToString())
                    {
                        ReSetReadOnly();
                    }
                    else
                    {
                        SetReadOnly();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("设置设置详情界面所有控件状态异常{0}" + ex.Message);
            }
        }
        #endregion

        /// <summary> 
        /// 注册businessKey 
        /// </summary> 
        private void RegBusinessKey()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ObjectId", window.Uid);
            dic.Add("BusinessKey", doccode);
            Dictionary<string, object> result = dataOp.SendSyncMessage("PluginService.SetPluginInfo", dic);
        }
        #endregion
    }
    /// <summary>
    /// 凭证定义定义辅助类
    /// </summary>
    public class TCBillDefinition : TemplateControl
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            base.InitVictopUserControl(Properties.Resources.TCBillDefinition);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pBlockName">展示层区块名称</param>
        /// <returns>展示层区块数据表</returns>
        public DataTable GetData(string pBlockName)
        {
            DataTable dtData = new DataTable();
            PresentationBlockModel preBlockModel = new PresentationBlockModel();
            if (string.IsNullOrEmpty(base.FormId))
            {
                return dtData;
            }
            preBlockModel = base.GetPresentationBlockModel(pBlockName);
            if (preBlockModel != null)
            {
                if (string.IsNullOrEmpty(preBlockModel.ViewBlock.ViewModel.ViewId))
                {
                    ViewsBlockConditionModel ConditionModel = new ViewsBlockConditionModel();
                    Dictionary<string, object> searchDic = new Dictionary<string, object>();
                    searchDic.Add("formid", base.FormId);
                    ConditionModel.TableCondition = searchDic;
                    preBlockModel.SetSearchCondition(ConditionModel);
                    preBlockModel.SearchData();
                }
                preBlockModel.GetData();
                dtData = preBlockModel.ViewBlockDataTable;
            }
            return dtData;
        }
        /// <summary>
        /// 设置当前行
        /// </summary>
        /// <param name="pBlockName">展示层区块名称</param>
        /// <param name="dr">当前行</param>
        public virtual void SetCurrentRow(string pBlockName, DataRow dr)
        {
            PresentationBlockModel PreBlockModel = new PresentationBlockModel();
            PreBlockModel = base.GetPresentationBlockModel(pBlockName);
            if (PreBlockModel != null)
            {
                PreBlockModel.PreBlockSelectedRow = dr;
                PreBlockModel.SetCurrentRow(dr);
            }
        }
    }
    /// <summary>
    /// 控件操作对象
    /// </summary>
    public class AutoObject
    {
        /// <summary>
        /// 控件
        /// </summary>
        public object obj;
        /// <summary>
        /// 控件类型
        /// </summary>
        public string objType;
        /// <summary>
        /// 装配是否只读
        /// </summary>
        public bool readonlyByfit;
        /// <summary>
        /// 是否只读
        /// </summary>
        public static bool isreadonly = false;
    }
}
