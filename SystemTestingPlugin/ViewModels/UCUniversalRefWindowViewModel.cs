using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Victop.Server.Controls.Models;
using GalaSoft.MvvmLight.Command;
using SystemTestingPlugin.Models;
using System.Collections.ObjectModel;
using System.Data;
using Victop.Frame.DataMessageManager;
using SystemTestingPlugin.Views;
using Victop.Frame.PublicLib.Helpers;
using System.Windows;
using System.Windows.Controls;
using Victop.Wpf.Controls;
using Victop.Frame.Units;

namespace SystemTestingPlugin.ViewModels
{
    public class UCUniversalRefWindowViewModel : ModelBase
    {
        #region 字段
        /// <summary>
        /// 引用数据信息
        /// </summary>
        private RefDataModel refDataInfo;

        private VicWindowNormal refWindow;

        UnitPager unitPage;
        /// <summary>
        /// 前导树显示状态
        /// </summary>
        private Visibility treeVisibility = Visibility.Collapsed;
        /// <summary>
        /// 标准引用信息
        /// </summary>
        private MongoModelInfoOfClientRefModel clientRefInfo;

        /// <summary>
        /// 前导树选定节点
        /// </summary>
        private RefForerunnerTreeModel foreTreeSelectedNode;

        /// <summary>
        /// 前导树信息
        /// </summary>
        private ObservableCollection<RefForerunnerTreeModel> forerunnerTreeList;
        /// <summary>
        /// 数据列表
        /// </summary>
        private DataTable gridDataTable;
        /// <summary>
        /// 数据列表选择模式
        /// </summary>
        private SelectionMode gridSelectionMode = SelectionMode.Single;

        /// <summary>
        /// 条件值
        /// </summary>
        private string conditionValue;

        /// <summary>
        /// 查询字段标题
        /// </summary>
        private string searchFieldCaption;

        #endregion
        #region 属性
        /// <summary>
        /// 前导树显示状态
        /// </summary>
        public Visibility TreeVisibility
        {
            get
            {
                return treeVisibility;
            }
            set
            {
                if (treeVisibility != value)
                {
                    treeVisibility = value;
                    RaisePropertyChanged("TreeVisibility");
                }
            }
        }
        /// <summary>
        /// 数据列表
        /// </summary>
        public DataTable GridDataTable
        {
            get
            {
                return gridDataTable;
            }
            set
            {
                if (gridDataTable != value)
                {
                    gridDataTable = value;
                    RaisePropertyChanged("GridDataTable");
                }
            }
        }
        /// <summary>
        /// 列表选择模式
        /// </summary>
        public SelectionMode GridSelectionMode
        {
            get
            {
                return gridSelectionMode;
            }
            set
            {
                if (gridSelectionMode != value)
                {
                    gridSelectionMode = value;
                    RaisePropertyChanged("GridSelectionMode");
                }
            }
        }
        /// <summary>
        /// 前导树选定节点
        /// </summary>
        public RefForerunnerTreeModel ForeTreeSelectedNode
        {
            get
            {
                return foreTreeSelectedNode;
            }
            set
            {
                if (foreTreeSelectedNode != value)
                {
                    foreTreeSelectedNode = value;
                    RaisePropertyChanged("ForeTreeSelectedNode");
                }
            }
        }
        /// <summary>
        /// 前导树信息
        /// </summary>
        public ObservableCollection<RefForerunnerTreeModel> ForerunnerTreeList
        {
            get
            {
                if (forerunnerTreeList == null)
                    forerunnerTreeList = new ObservableCollection<RefForerunnerTreeModel>();
                return forerunnerTreeList;
            }
            set
            {
                if (forerunnerTreeList != value)
                {
                    forerunnerTreeList = value;
                    RaisePropertyChanged("ForerunnerTreeList");
                }
            }
        }
        /// <summary>
        /// 查询字段标题
        /// </summary>
        public string SearchFieldCaption
        {
            get
            {
                return searchFieldCaption;
            }
            set
            {
                if (searchFieldCaption != value)
                {
                    searchFieldCaption = value;
                    RaisePropertyChanged("SearchFieldCaption");
                }
            }
        }

        /// <summary>
        /// 条件值
        /// </summary>
        public string ConditionValue
        {
            get
            {
                return conditionValue;
            }
            set
            {
                if (conditionValue != value)
                {
                    conditionValue = value;
                    RaisePropertyChanged("ConditionValue");
                }
            }
        }
        #endregion
        #region 命令
        /// <summary>
        /// 视图加载
        /// </summary>
        public ICommand mainViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    refDataInfo = UCUniversalRefWindow.RefDataInfo;
                    ConditionValue = refDataInfo.RefFieldValue;
                    DataMessageOperation dataOp = new DataMessageOperation();
                    DataSet tempDs = new DataSet();
                    string resultMessage = dataOp.GetRefData(refDataInfo.ViewId, refDataInfo.DataPath, refDataInfo.FieldName, refDataInfo.RowValue, out tempDs, null, refDataInfo.SystemId, refDataInfo.ConfigSystemId, false);
                    Dictionary<string, object> resultDic = JsonHelper.ToObject<Dictionary<string, object>>(resultMessage);
                    if (!resultDic["ReplyMode"].ToString().Equals("0"))
                    {
                        refDataInfo.RefDataSet = tempDs;
                        refDataInfo.RefContent = resultDic["ReplyContent"].ToString();
                        clientRefInfo = JsonHelper.ToObject<MongoModelInfoOfClientRefModel>(refDataInfo.RefContent);
                        List<Dictionary<string, object>> defaultCondition = OrganizeDefaultCondition(1);
                        resultMessage = dataOp.GetRefData(refDataInfo.ViewId, refDataInfo.DataPath, refDataInfo.FieldName, refDataInfo.RowValue, out tempDs, defaultCondition, refDataInfo.SystemId, refDataInfo.ConfigSystemId, false);
                        refDataInfo.RefDataSet = tempDs;
                        refDataInfo.RefContent = resultDic["ReplyContent"].ToString();
                        GridSelectionMode = clientRefInfo.ClientRefPopupSetting.SettingSingleRow == 0 ? SelectionMode.Single : SelectionMode.Multiple;
                        SearchFieldCaption = refDataInfo.RefFieldCaption;
                        if (clientRefInfo.ClientRefForeRunner != null && !string.IsNullOrEmpty(clientRefInfo.ClientRefForeRunner.RefModel))
                        {
                            string treeTableName = clientRefInfo.ClientRefForeRunner.ForeRunnerProperty[0].PropertyValue.Substring(0, clientRefInfo.ClientRefForeRunner.ForeRunnerProperty[0].PropertyValue.IndexOf("."));
                            DataSet ds = FindDataTable(refDataInfo.SystemId, refDataInfo.ConfigSystemId, clientRefInfo.ClientRefForeRunner.RefModel, treeTableName, null);
                            if (ds != null && ds.Tables.Contains("dataArray"))
                            {
                                DataRow[] drs = ds.Tables["dataArray"].Select(string.Format("{0} is null", clientRefInfo.ClientRefForeRunner.TreeParentId));
                                if (drs != null && drs.Count() > 0)
                                {
                                    foreach (DataRow item in drs)
                                    {
                                        RefForerunnerTreeModel treeModel = new RefForerunnerTreeModel();
                                        treeModel.TreeId = item[clientRefInfo.ClientRefForeRunner.TreeId].ToString();
                                        treeModel.TreeParentId = item[clientRefInfo.ClientRefForeRunner.TreeParentId].ToString();
                                        treeModel.TreeDisplay = item[clientRefInfo.ClientRefForeRunner.TreeDisplay].ToString();
                                        treeModel.TreeValue = item;
                                        CreateTreeData(ds.Tables["dataArray"], treeModel.TreeId, treeModel);
                                        ForerunnerTreeList.Add(treeModel);
                                    }
                                }
                            }
                            TreeVisibility = Visibility.Visible;
                        }
                        else
                        {
                            if (refDataInfo.RefDataSet != null && refDataInfo.RefDataSet.Tables.Contains("dataArray"))
                            {
                                GridDataTable = refDataInfo.RefDataSet.Tables["dataArray"];
                            }
                            TreeVisibility = Visibility.Collapsed;
                        }
                    }
                });
            }
        }

        private List<Dictionary<string, object>> OrganizeDefaultCondition(int currentPage)
        {
            List<Dictionary<string, object>> defaultCondition = new List<Dictionary<string, object>>();
            MongoModelInfoOfClientRefPropertyModel propertyModel = clientRefInfo.ClientRefProperty.FirstOrDefault(it => it.PropertyKey.Equals(clientRefInfo.ClientRefField));
            if (propertyModel != null)
            {
                string refTableName = propertyModel.PropertyValue.Substring(0,propertyModel.PropertyValue.IndexOf("."));
                string refFiledName = propertyModel.PropertyValue.Substring(propertyModel.PropertyValue.LastIndexOf(".") + 1);
                Dictionary<string, object> tableDic = new Dictionary<string, object>();
                tableDic.Add("name", refTableName);
                List<object> tableConditionList = new List<object>();
                Dictionary<string, object> conditionDic = new Dictionary<string, object>();
                conditionDic.Add(refFiledName, RegexHelper.Contains(ConditionValue));
                if (ForeTreeSelectedNode != null)
                {
                    foreach (MongoModelInfoOfClientRefPropertyModel item in clientRefInfo.ClientRefForeRunner.ForeRunnerProperty)
                    {
                        string filedStr = item.PropertyKey.Substring(item.PropertyKey.LastIndexOf(".") + 1);
                        string fileldValue = item.PropertyValue.Substring(item.PropertyValue.LastIndexOf(".") + 1);
                        conditionDic.Add(filedStr, ForeTreeSelectedNode.TreeValue[fileldValue]);
                    }
                }
                tableConditionList.Add(conditionDic);
                tableDic.Add("tablecondition", tableConditionList);
                Dictionary<string, object> pageDic = new Dictionary<string, object>();
                pageDic.Add("size", 20);
                pageDic.Add("index", currentPage);
                tableDic.Add("paging", pageDic);
                defaultCondition.Add(tableDic);
            }
            return defaultCondition;
        }
        /// <summary>
        /// 分页部件加载命令
        /// </summary>
        public ICommand unitPageLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    unitPage = (UnitPager)x;
                    if (refDataInfo.RefDataSet.Tables.Contains("summary") && refDataInfo.RefDataSet.Tables["summary"] != null && refDataInfo.RefDataSet.Tables["summary"].Rows.Count > 0)
                    {
                        unitPage.ParamsModel.TotalNum = int.Parse(refDataInfo.RefDataSet.Tables["summary"].Rows[0]["totalRow"].ToString());
                        unitPage.ParamsModel.TotalPage = int.Parse(refDataInfo.RefDataSet.Tables["summary"].Rows[0]["totalPage"].ToString());
                    }
                    if (refDataInfo.RefDataSet.Tables.Contains("pagingCurrent") && refDataInfo.RefDataSet.Tables["pagingCurrent"] != null && refDataInfo.RefDataSet.Tables["pagingCurrent"].Rows.Count > 0)
                    {
                        unitPage.ParamsModel.PageSize = refDataInfo.RefDataSet.Tables["pagingCurrent"].Columns.Contains("size") ? int.Parse(refDataInfo.RefDataSet.Tables["pagingCurrent"].Rows[0]["size"].ToString()) : 20;
                        unitPage.ParamsModel.CurrentPage = refDataInfo.RefDataSet.Tables["pagingCurrent"].Columns.Contains("index") ? int.Parse(refDataInfo.RefDataSet.Tables["pagingCurrent"].Rows[0]["index"].ToString()) : 1;
                    }
                    unitPage.ParamsModel.PagerButtonClick += ParamsModel_PagerButtonClick;
                });
            }
        }

        void ParamsModel_PagerButtonClick(int currentPage)
        {
            SearchData(currentPage);
        }
        /// <summary>
        /// 树节点选中事件
        /// </summary>
        public ICommand tViewTypeSelectedItemChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    ForeTreeSelectedNode = (RefForerunnerTreeModel)x;

                });
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        public ICommand btnSearchClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SearchData();
                });
            }
        }

        private void SearchData(int currentPage = 1)
        {
            List<Dictionary<string, object>> defaultCondition = OrganizeDefaultCondition(currentPage);
            DataMessageOperation dataOp = new DataMessageOperation();
            DataSet ds = new DataSet();
            string resultStr = dataOp.GetRefData(refDataInfo.ViewId, refDataInfo.DataPath, refDataInfo.FieldName, refDataInfo.RowValue, out ds, defaultCondition.Count > 0 ? defaultCondition : null, refDataInfo.SystemId, refDataInfo.ConfigSystemId, true);
            Dictionary<string, object> resultDic = JsonHelper.ToObject<Dictionary<string, object>>(resultStr);
            if (!resultDic["ReplyMode"].ToString().Equals("0"))
            {
                if (ds != null && ds.Tables.Contains("dataArray"))
                {
                    GridDataTable = ds.Tables["dataArray"];
                }
                if (ds.Tables.Contains("summary") && ds.Tables["summary"] != null && ds.Tables["summary"].Rows.Count > 0)
                {
                    unitPage.ParamsModel.TotalNum = int.Parse(ds.Tables["summary"].Rows[0]["totalRow"].ToString());
                    unitPage.ParamsModel.TotalPage = int.Parse(ds.Tables["summary"].Rows[0]["totalPage"].ToString());
                }
                if (ds.Tables.Contains("pagingCurrent") && ds.Tables["pagingCurrent"] != null && ds.Tables["pagingCurrent"].Rows.Count > 0)
                {
                    unitPage.ParamsModel.PageSize = ds.Tables["pagingCurrent"].Columns.Contains("size") ? int.Parse(ds.Tables["pagingCurrent"].Rows[0]["size"].ToString()) : 20;
                    unitPage.ParamsModel.CurrentPage = ds.Tables["pagingCurrent"].Columns.Contains("index") ? int.Parse(ds.Tables["pagingCurrent"].Rows[0]["index"].ToString()) : 1;
                }
            }
        }
        /// <summary>
        /// 确定按钮
        /// </summary>
        public ICommand btnConfirmClickCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    VicDataGrid dgrid = (VicDataGrid)x;
                    DataRowView dr = (DataRowView)dgrid.SelectedItem;
                    Dictionary<string, object> resultDic = new Dictionary<string, object>();
                    foreach (MongoModelInfoOfClientRefPropertyModel item in clientRefInfo.ClientRefProperty)
                    {
                        string keyField = item.PropertyKey.Substring(item.PropertyKey.LastIndexOf(".") + 1);
                        string valueField = item.PropertyValue.Substring(item.PropertyValue.LastIndexOf(".") + 1);
                        resultDic.Add(keyField, dr[valueField]);
                    }
                    if (refDataInfo.RefCallBack != null)
                    {
                        refDataInfo.RefCallBack(resultDic);
                    }
                }, (x) =>
                {
                    //if (x != null)
                    //{
                    //    VicDataGrid dgrid = (VicDataGrid)x;
                    //    if (GridSelectionMode == SelectionMode.Single && dgrid.SelectedItem != null)
                    //    {
                    //        return true;
                    //    }
                    //    if (GridSelectionMode == SelectionMode.Multiple && dgrid.GetCheckedDataRows() != null)
                    //    {
                    //        return true;
                    //    }
                    //}
                    return true;
                });
            }
        }

        #endregion
        #region 方法
        #region 查询数据
        private DataSet FindDataTable(string systemId, string configsystemId, string modelId, string tableName, Dictionary<string, object> conditionDic)
        {
            DataSet ds = new DataSet();
            DataMessageOperation messageOp = new DataMessageOperation();
            string messageType = "MongoDataChannelService.findBusiData";
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", systemId);
            contentDic.Add("configsystemid", configsystemId);
            contentDic.Add("modelid", modelId);
            List<object> conditionsList = new List<object>();
            Dictionary<string, object> conditionsDic = new Dictionary<string, object>();
            conditionsDic.Add("name", tableName);
            if (conditionDic != null)
            {
                List<object> tableCondition = new List<object>();
                tableCondition.Add(conditionDic);
                conditionsDic.Add("tablecondition", tableCondition);
            }
            conditionsList.Add(conditionsDic);
            contentDic.Add("conditions", conditionsList);
            Dictionary<string, object> returnDic = messageOp.SendSyncMessage(messageType, contentDic, "JSON");
            if (returnDic != null && returnDic["ReplyMode"].ToString() != "0")
            {
                ds = messageOp.GetData(returnDic["DataChannelId"].ToString(), "[\"" + tableName + "\"]");
            }
            return ds;
        }
        #endregion
        /// <summary>
        /// 创建树数据
        /// </summary>
        /// <param name="sourceDt">源数据</param>
        /// <param name="parentValue">父节点值</param>
        /// <param name="treeModel">树节点模型</param>
        private void CreateTreeData(DataTable sourceDt, string parentValue, RefForerunnerTreeModel treeModel)
        {
            string sqlStr = string.Format("{0}='{1}'", clientRefInfo.ClientRefForeRunner.TreeParentId, parentValue);
            DataRow[] drs = sourceDt.Select(sqlStr);
            if (drs != null && drs.Count() > 0)
            {
                foreach (DataRow item in drs)
                {
                    RefForerunnerTreeModel childTreeModel = new RefForerunnerTreeModel();
                    childTreeModel.TreeId = item[clientRefInfo.ClientRefForeRunner.TreeId].ToString();
                    childTreeModel.TreeParentId = item[clientRefInfo.ClientRefForeRunner.TreeParentId].ToString();
                    childTreeModel.TreeDisplay = item[clientRefInfo.ClientRefForeRunner.TreeDisplay].ToString();
                    childTreeModel.TreeValue = item;
                    CreateTreeData(sourceDt, childTreeModel.TreeId, treeModel);
                    treeModel.ForerunnerTreeList.Add(childTreeModel);
                }
            }

        }
        #endregion
    }
}
