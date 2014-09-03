using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;
using DataCruisePlugin.Models;
using System.Collections.ObjectModel;
using System.IO;
using Victop.Frame.PublicLib.Helpers;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Victop.Wpf.Controls;
using System.Windows;
using Victop.Frame.DataChannel;
using Victop.Frame.SyncOperation;
using System.Data;
using System.Windows.Data;
using System.Windows.Controls;

namespace DataCruisePlugin.ViewModels
{
    public class UCCruiseWindowViewModelNew : ModelBase
    {
        #region 独立变量
        /// <summary>
        /// 路径实体集合
        /// </summary>
        private List<EntityDefinitionModel> dataPathEntityList;
        /// <summary>
        /// 所有实体集合
        /// </summary>
        private ObservableCollection<EntityDefinitionModel> allEntityModels;
        /// <summary>
        /// 主Tab中当前选择内容
        /// </summary>
        private DataRowView masterSelectedModel;
        /// <summary>
        /// 编辑区选择内容
        /// </summary>
        private DataRowView currentSeletecModel;
        /// <summary>
        /// 编辑标志
        /// </summary>
        private bool editFlag = true;
        #endregion
        #region 实体定义
        /// <summary>
        /// 主TabHeader
        /// </summary>
        private string masterHeader;
        /// <summary>
        /// 主TabHeader
        /// </summary>
        public string MasterHeader
        {
            get
            {
                return masterHeader;
            }
            set
            {
                masterHeader = value;
                RaisePropertyChanged("MasterHeader");
            }
        }
        /// <summary>
        /// 主Tab展示内容
        /// </summary>
        private object masterContent;
        /// <summary>
        /// 主Tab展示内容
        /// </summary>
        public object MasterContent
        {
            get
            {
                return masterContent;
            }
            set
            {
                masterContent = value;
                RaisePropertyChanged("MasterContent");
            }
        }
        /// <summary>
        /// 主Tab实体
        /// </summary>
        private EntityDefinitionModel masterEntity;
        /// <summary>
        /// 主Tab实体
        /// </summary>
        public EntityDefinitionModel MasterEntity
        {
            get
            {
                return masterEntity;
            }
            set
            {
                masterEntity = value;
                RaisePropertyChanged("MasterEntity");
            }
        }
        /// <summary>
        /// 当前TabHeader
        /// </summary>
        private string currentHeader;
        /// <summary>
        /// 当前TabHeader
        /// </summary>
        public string CurrentHeader
        {
            get
            {
                return currentHeader;
            }
            set
            {
                currentHeader = value;
                RaisePropertyChanged("CurrentHeader");
            }
        }
        /// <summary>
        /// 当前展示内容
        /// </summary>
        private object currentContent;
        /// <summary>
        /// 当前展示内容
        /// </summary>
        public object CurrentContent
        {
            get
            {
                return currentContent;
            }
            set
            {
                currentContent = value;
                RaisePropertyChanged("CurrentContent");
            }
        }
        /// <summary>
        /// 当前编辑实体
        /// </summary>
        private EntityDefinitionModel currentEntity;
        /// <summary>
        /// 当前编辑实体
        /// </summary>
        public EntityDefinitionModel CurrentEntity
        {
            get { return currentEntity; }
            set
            {
                currentEntity = value;
                RaisePropertyChanged("CurrentEntity");
            }
        }
        /// <summary>
        /// 入口展示内容
        /// </summary>
        private object enterContent;
        /// <summary>
        /// 入口展示内容
        /// </summary>
        public object EnterContent
        {
            get
            {
                return enterContent;
            }
            set
            {
                enterContent = value;
                RaisePropertyChanged("EnterContent");
            }
        }
        /// <summary>
        /// 可用展示内容
        /// </summary>
        private object enableContent;
        /// <summary>
        /// 可用展示内容
        /// </summary>
        public object EnableContent
        {
            get
            {
                return enableContent;
            }
            set
            {
                enableContent = value;
                RaisePropertyChanged("EnableContent");
            }
        }
        /// <summary>
        /// 引用区域
        /// </summary>
        private object dataRefContent;
        public object DataRefContent
        {
            get
            {
                return dataRefContent;
            }
            set
            {
                dataRefContent = value;
                RaisePropertyChanged("DataRefContent");
            }
        }
        /// <summary>
        /// 入口实体集合
        /// </summary>
        private ObservableCollection<EntityDefinitionModel> entranceEntityModels;
        /// <summary>
        /// 入口实体集合
        /// </summary>
        public ObservableCollection<EntityDefinitionModel> EntranceEntityModels
        {
            get
            {
                if (entranceEntityModels == null)
                    entranceEntityModels = new ObservableCollection<EntityDefinitionModel>();
                return entranceEntityModels;
            }
            set
            {
                if (entranceEntityModels != value)
                {
                    entranceEntityModels = value;
                    RaisePropertyChanged("EntranceEntityModels");
                }
            }
        }
        /// <summary>
        /// 可用实体集合
        /// </summary>
        private ObservableCollection<EntityDefinitionModel> enableEntityModels;
        /// <summary>
        /// 可用实体集合
        /// </summary>
        public ObservableCollection<EntityDefinitionModel> EnableEntityModels
        {
            get
            {
                if (enableEntityModels == null)
                    enableEntityModels = new ObservableCollection<EntityDefinitionModel>();
                return enableEntityModels;
            }
            set
            {
                if (enableEntityModels != value)
                {
                    enableEntityModels = value;
                    RaisePropertyChanged("EnableEntityModels");
                }
            }
        }
        /// <summary>
        /// 编辑区列表绑定内容
        /// </summary>
        private DataTable gridDt;
        /// <summary>
        /// 编辑区列表绑定内容
        /// </summary>
        public DataTable GridDt
        {
            get { return gridDt; }
            set
            {
                gridDt = value;
                RaisePropertyChanged("GridDt");
            }
        }

        #endregion
        #region Command
        public ICommand ucMainViewLoadedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string entityRel = ReadRelationFile("rel2");
                    EntranceEntityModels.Clear();
                    allEntityModels = JsonHelper.ToObject<ObservableCollection<EntityDefinitionModel>>(entityRel);
                    foreach (EntityDefinitionModel item in allEntityModels)
                    {
                        if (item.Fields != null)
                        {
                            string fieldsStr = JsonHelper.ReadJsonString(item.Fields.ToString(), "dataArray");
                            if (!string.IsNullOrEmpty(fieldsStr))
                            {
                                item.Fields = JsonHelper.ToObject<List<EntityFieldModel>>(fieldsStr);
                            }
                        }
                        if (item.DataRef != null)
                        {
                            string relStr = JsonHelper.ReadJsonString(item.DataRef.ToString(), "dataArray");
                            if (!string.IsNullOrEmpty(relStr))
                            {
                                item.DataRef = JsonHelper.ToObject<List<RefEntityModel>>(relStr);
                            }
                        }
                        if (item.Entrance)
                        {
                            EntranceEntityModels.Add(item);
                        }
                    }
                    CreateEnterContent();
                });
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        public ICommand btnAddClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    List<RefEntityModel> refList = CurrentEntity.DataRef as List<RefEntityModel>;
                    DataRow dr = GridDt.NewRow();
                    dr["_id"] = Guid.NewGuid();
                    if (refList != null && refList.Count > 0)
                    {
                        RefEntityModel refModel = refList.FirstOrDefault(it => it.TableId == MasterEntity.Id);
                        dr[refModel.SelfField] = masterSelectedModel[refModel.SourceField];
                    }
                    GridDt.Rows.Add(dr);
                });
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        public ICommand btnDelClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    string rowKey = currentSeletecModel["_id"].ToString();
                    foreach (DataRow dr in GridDt.Rows)
                    {
                        if (dr["_id"].ToString().Equals(rowKey))
                        {
                            dr.Delete();
                            break;
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
                    DataOperation dataOp = new DataOperation();
                    bool result = dataOp.SaveData(CurrentEntity.ViewId, CurrentEntity.DataPath);
                    if (result)
                    {
                        MessageOperation messageOp = new MessageOperation();
                        Dictionary<string, object> contentDic = new Dictionary<string, object>();
                        contentDic.Add("DataChannelId", CurrentEntity.ViewId);
                        contentDic.Add("tablename", GetEntityRootTableName(CurrentEntity));
                        contentDic.Add("dbname", "test_cruiseDB");
                        contentDic.Add("systemid", "800");
                        contentDic.Add("configsystemid", "101");
                        contentDic.Add("spaceid", "tbs");
                        Dictionary<string, object> resultDic = messageOp.SendMessage("MongoDataChannelService.saveTableData", contentDic, "JSON");
                        string temp1 = resultDic["ReplyMode"].ToString();
                    }
                    string json = dataOp.GetJSONData(CurrentEntity.ViewId);
                    string curdjson = dataOp.GetCurdJSONData(CurrentEntity.ViewId);
                    string temp = curdjson;
                });
            }
        }
        /// <summary>
        /// 当前编辑区选择行改变
        /// </summary>
        public ICommand currentGridSelectionChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) => {
                    DataGrid grid = (DataGrid)x;
                    currentSeletecModel = (DataRowView)grid.SelectedItem;
                });
            }
        }

        #endregion
        #region 私有方法
        /// <summary>
        /// 创建入口展示内容
        /// </summary>
        /// <returns></returns>
        private void CreateEnterContent()
        {
            if (EntranceEntityModels.Count > 0)
            {
                VicDockPanelNormal dockPanel = new VicDockPanelNormal();
                foreach (EntityDefinitionModel item in EntranceEntityModels)
                {
                    VicButtonNormal vicBtnEnter = new VicButtonNormal();
                    vicBtnEnter.Name = item.TableName;
                    vicBtnEnter.Tag = item;
                    vicBtnEnter.Width = 100;
                    vicBtnEnter.Height = 24;
                    vicBtnEnter.Content = item.TabTitle;
                    vicBtnEnter.Margin = new Thickness(0, 0, 5, 0);
                    vicBtnEnter.Click += vicBtnEnter_Click;
                    dockPanel.Children.Add(vicBtnEnter);
                }
                EnterContent = dockPanel;
            }
        }
        /// <summary>
        /// 创建可用展示内容
        /// </summary>
        private void CreateEnableContent()
        {
            if (EnableEntityModels.Count > 0)
            {
                VicDockPanelNormal dockPanel = new VicDockPanelNormal();
                foreach (EntityDefinitionModel item in EnableEntityModels)
                {
                    VicButtonNormal vicBtnEnable = new VicButtonNormal();
                    vicBtnEnable.Name = item.TableName;
                    vicBtnEnable.Tag = item;
                    vicBtnEnable.Width = 100;
                    vicBtnEnable.Height = 24;
                    vicBtnEnable.Content = item.TabTitle;
                    vicBtnEnable.Margin = new Thickness(0, 0, 5, 0);
                    vicBtnEnable.Click += vicBtnEnable_Click;
                    dockPanel.Children.Add(vicBtnEnable);
                }
                EnableContent = dockPanel;
            }
        }
        /// <summary>
        /// 读取本地数据关系文件
        /// </summary>
        /// <param name="entityrelation"></param>
        /// <returns></returns>
        private string ReadRelationFile(string entityrelation)
        {
            string returnStr = string.Empty;
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "data";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fileFullName = Path.Combine(filePath, entityrelation + ".json");
            if (File.Exists(fileFullName))
            {
                using (StreamReader fileReader = new StreamReader(fileFullName))
                {
                    returnStr = fileReader.ReadToEnd();
                }
            }
            Dictionary<string, object> fileDic = JsonHelper.ToObject<Dictionary<string, object>>(returnStr);
            returnStr = JsonHelper.ReadJsonString(fileDic["setting"].ToString(), "dataArray");
            return returnStr;
        }

        /// <summary>
        /// 实体Model重构
        /// </summary>
        private void EntityModelReConsitution(EntityDefinitionModel EntityModel)
        {
            /* 1、选定实体的hostTable对应的实体可用(上级可用)
             * 2、hostTable与选定实体的hostTable相同的实体可用(同级可用)
             * 3、以选定实体为hostTable的实体可用(下级可用)
             * 4、当前实体的dataRef中前导实体可用(前导可用)
             * 5、入口实体一致可用
             * 6、以当前实体为前导的实体可用(跟随可用)
             */
            EnableEntityModels.Clear();
            EnableEntityModels.Add(EntityModel);
            if (!string.IsNullOrEmpty(EntityModel.HostTable))
            {
                //上级可用
                EntityDefinitionModel hostModel = allEntityModels.First(it => it.Id == EntityModel.HostTable);
                if (hostModel != null)
                {
                    hostModel.Actived = true;
                    EnableEntityModels.Add(hostModel);
                    //跟随可用
                    foreach (EntityDefinitionModel item in allEntityModels)
                    {
                        if (item.HostTable == hostModel.Id)
                        {
                            item.Actived = true;
                            if (EnableEntityModels.FirstOrDefault(it => it.Id == item.Id) == null)
                                EnableEntityModels.Add(item);
                        }
                        else
                        {
                            List<RefEntityModel> itemRefList = item.DataRef as List<RefEntityModel>;
                            if (itemRefList != null)
                            {
                                foreach (RefEntityModel refitem in itemRefList)
                                {
                                    if (refitem.TableId == hostModel.Id && refitem.ForeRunner)
                                    {
                                        if (EnableEntityModels.FirstOrDefault(it => it.Id == item.Id) == null)
                                        {
                                            item.Actived = true;
                                            EnableEntityModels.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ///同级可用
                foreach (EntityDefinitionModel item in allEntityModels.Where(it => it.HostTable == EntityModel.HostTable))
                {
                    item.Actived = true;
                    if (EnableEntityModels.FirstOrDefault(it => it.Id == item.Id) == null)
                        EnableEntityModels.Add(item);
                }
            }
            //下级可用
            foreach (EntityDefinitionModel item in allEntityModels.Where(it => it.HostTable == EntityModel.Id))
            {
                item.Actived = true;
                if (EnableEntityModels.FirstOrDefault(it => it.Id == item.Id) == null)
                    EnableEntityModels.Add(item);
            }
            //前导可用
            List<RefEntityModel> refEntityList = EntityModel.DataRef as List<RefEntityModel>;
            if (refEntityList != null)
            {
                foreach (RefEntityModel item in refEntityList)
                {
                    if (item.ForeRunner)
                    {
                        if (EnableEntityModels.FirstOrDefault(it => it.Id == item.TableId) == null)
                        {
                            allEntityModels.First(it => it.Id == item.TableId).Actived = true;
                            EnableEntityModels.Add(allEntityModels.First(it => it.Id == item.TableId));
                        }
                    }
                }
            }
            //跟随可用
            foreach (EntityDefinitionModel item in allEntityModels)
            {
                List<RefEntityModel> itemRefList = item.DataRef as List<RefEntityModel>;
                if (itemRefList != null)
                {
                    foreach (RefEntityModel refitem in itemRefList)
                    {
                        if (refitem.TableId == EntityModel.Id && refitem.ForeRunner)
                        {
                            if (EnableEntityModels.FirstOrDefault(it => it.Id == item.Id) == null)
                            {
                                EnableEntityModels.Add(item);
                            }
                        }
                    }
                }
            }
        }
        //更新MasterContent
        private void RefreshMasterContent()
        {
            DataSet ds = new DataSet();
            DataOperation dataOp = new DataOperation();
            if (string.IsNullOrEmpty(MasterEntity.HostTable))
            {
                MasterEntity.ViewId = SendFindDataMessage(MasterEntity.TableName);
                if (!string.IsNullOrEmpty(MasterEntity.ViewId))
                {
                    MasterEntity.DataPath = ConstructDataPath(MasterEntity, MasterEntity, masterSelectedModel);
                    DataTable dt = dataOp.GetData(MasterEntity.ViewId, MasterEntity.DataPath, CreateStructDataTable(MasterEntity));
                    if (!ds.Tables.Contains(dt.TableName))
                    {
                        ds.Tables.Add(dt);
                    }
                }
            }
            else
            {
                DataTable dt = dataOp.GetData(MasterEntity.ViewId, MasterEntity.DataPath, CreateStructDataTable(MasterEntity));
                if (!ds.Tables.Contains(dt.TableName))
                {
                    ds.Tables.Add(dt);
                }
            }
            switch (MasterEntity.ViewType)
            {
                case "tree":
                    VicTreeView tv = new VicTreeView();
                    tv.IDField = "_id";
                    tv.FIDField = MasterEntity.ParentId;
                    tv.DisplayField = MasterEntity.TreeDisPlay;
                    tv.ItemsSource = ds.Tables[MasterEntity.TableName].DefaultView;
                    tv.SelectedItemChanged += tv_SelectedItemChanged;
                    MasterContent = tv;
                    break;
                case "grid":
                case "dict":
                    VicDataGrid masterGrid = new VicDataGrid();
                    masterGrid.ItemsSource = ds.Tables[MasterEntity.TableName].DefaultView;
                    masterGrid.AutoGenerateColumns = false;
                    masterGrid.CanUserAddRows = false;
                    masterGrid.SelectionChanged += masterGrid_SelectionChanged;
                    MasterContent = masterGrid;
                    break;
                default:
                    break;
            }
            MasterHeader = MasterEntity.TabTitle;
        }
        /// <summary>
        /// 更新CurrentContent
        /// </summary>
        private void RefreshCurrentContent()
        {
            DataSet ds = new DataSet();
            DataOperation dataOp = new DataOperation();
            if (string.IsNullOrEmpty(CurrentEntity.HostTable))
            {
                if (CurrentEntity.DataRef != null)
                {
                    List<RefEntityModel> refEntityList = CurrentEntity.DataRef as List<RefEntityModel>;
                    if (refEntityList != null)
                    {
                        RefEntityModel refModel = refEntityList.FirstOrDefault(it => it.TableId == MasterEntity.Id);
                        if (refModel != null)
                        {
                            List<object> conList = new List<object>();
                            Dictionary<string, object> conDic = new Dictionary<string, object>();
                            conDic.Add(refModel.SelfField, masterSelectedModel[refModel.SourceField]);
                            conList.Add(conDic);
                            CurrentEntity.ViewId = SendFindDataMessage(CurrentEntity.TableName, conList);
                        }
                    }
                }
                else
                {
                    CurrentEntity.ViewId = SendFindDataMessage(CurrentEntity.TableName);
                }
                CurrentEntity.DataPath = ConstructDataPath(CurrentEntity, MasterEntity, masterSelectedModel);
                DataTable dt = dataOp.GetData(CurrentEntity.ViewId, CurrentEntity.DataPath, CreateStructDataTable(CurrentEntity));
                if (!ds.Tables.Contains(dt.TableName))
                {
                    ds.Tables.Add(dt);
                }
            }
            else
            {
                if (MasterEntity.Id == CurrentEntity.HostTable)
                {
                    List<object> pathList = JsonHelper.ToObject<List<object>>(MasterEntity.DataPath);
                    Dictionary<string, object> pathDic = new Dictionary<string, object>();
                    pathDic.Add("key", "_id");
                    pathDic.Add("value", masterSelectedModel["_id"]);
                    pathList.Add(pathDic);
                    pathList.Add(CurrentEntity.TableName);
                    CurrentEntity.ViewId = MasterEntity.ViewId;
                    CurrentEntity.DataPath = JsonHelper.ToJson(pathList);
                    DataTable dt = dataOp.GetData(CurrentEntity.ViewId, CurrentEntity.DataPath, CreateStructDataTable(CurrentEntity));
                    if (!ds.Tables.Contains(dt.TableName))
                    {
                        ds.Tables.Add(dt);
                    }
                }
            }

            GridDt = ds.Tables[CurrentEntity.TableName];
            CurrentHeader = CurrentEntity.TabTitle;
        }
        /// <summary>
        /// 刷新数据引用区域
        /// </summary>
        private void RefreshDataRefContent()
        {
            VicStackPanelNormal stackpanel = new VicStackPanelNormal();
            List<RefEntityModel> dataRefList = CurrentEntity.DataRef as List<RefEntityModel>;
            DataOperation dataOp = new DataOperation();
            if (dataRefList != null)
            {
                foreach (RefEntityModel item in dataRefList)
                {
                    if (item.TableId.Equals(MasterEntity.Id))
                        continue;
                    VicGroupBoxNormal groupBox = new VicGroupBoxNormal();
                    EntityDefinitionModel entityModel = allEntityModels.FirstOrDefault(it => it.Id == item.TableId);
                    switch (entityModel.ViewType)
                    {
                        case "tree":
                            VicTreeView vicTv = new VicTreeView();
                            vicTv.Height = 200;
                            if (string.IsNullOrEmpty(entityModel.HostTable))
                            {
                                entityModel.ViewId = SendFindDataMessage(entityModel.TableName);
                                if (!string.IsNullOrEmpty(entityModel.ViewId))
                                {
                                    DataTable dt = dataOp.GetData(entityModel.ViewId, ConstructDataPath(entityModel, null, null), CreateStructDataTable(entityModel));
                                    vicTv.IDField = "_id";
                                    vicTv.FIDField = entityModel.ParentId;
                                    vicTv.DisplayField = item.SourceText;
                                    vicTv.SelectedItemChanged += vicTv_SelectedItemChanged;
                                    vicTv.ItemsSource = dt.DefaultView;
                                }
                            }
                            entityModel.DataPath = entityModel.TableName;
                            vicTv.Tag = entityModel;
                            groupBox.Content = vicTv;
                            groupBox.Header = entityModel.TabTitle;
                            break;
                        case "grid":
                            VicListBoxNormal viclbox = new VicListBoxNormal();
                            viclbox.Height = 200;
                            if (string.IsNullOrEmpty(entityModel.HostTable))
                            {
                                entityModel.ViewId = SendFindDataMessage(entityModel.TableName);
                                if (!string.IsNullOrEmpty(entityModel.ViewId))
                                {
                                    DataTable dt = dataOp.GetData(entityModel.ViewId, ConstructDataPath(entityModel, null, null), CreateStructDataTable(entityModel));
                                    viclbox.DisplayMemberPath = item.SourceText;
                                    viclbox.SelectedValuePath = item.SourceField;
                                    viclbox.ItemsSource = dt.DefaultView;
                                }
                            }
                            entityModel.DataPath = entityModel.TableName;
                            viclbox.Tag = entityModel;
                            groupBox.Content = viclbox;
                            groupBox.Header = entityModel.TabTitle;
                            break;
                        default:
                            break;
                    }

                    stackpanel.Children.Add(groupBox);
                }
            }
            DataRefContent = stackpanel;
        }
        /// <summary>
        /// 发送检索数据消息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string SendFindDataMessage(string tableName, List<object> conditons = null)
        {
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("systemid", "800");
            contentDic.Add("configsystemid", "101");
            contentDic.Add("spaceid", "tbs");
            contentDic.Add("tablename", tableName);
            if (conditons != null)
            {
                contentDic.Add("tablecondition", conditons);
            }
            string messageType = "MongoDataChannelService.findTableData";
            MessageOperation messageOp = new MessageOperation();
            Dictionary<string, object> returnDic = messageOp.SendMessage(messageType, contentDic, "JSON");
            if (returnDic != null && !returnDic["ReplyMode"].ToString().Equals("0"))
            {
                return returnDic["DataChannelId"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 创建结构表
        /// </summary>
        /// <param name="entityFields"></param>
        /// <returns></returns>
        private DataTable CreateStructDataTable(EntityDefinitionModel entityModel)
        {
            List<EntityFieldModel> entityFields = entityModel.Fields as List<EntityFieldModel>;
            List<EntityFieldModel> extandFields = new List<EntityFieldModel>();
            if (entityModel.DynaColumn != null)
            {
                foreach (string dynCol in entityModel.DynaColumn)
                {
                    EntityDefinitionModel dynModel = allEntityModels.First(it => it.Id == dynCol);

                    if (dynModel.HostTable.Equals(MasterEntity.Id))
                    {
                        ConstructDataPath(dynModel, MasterEntity, masterSelectedModel);
                        DataOperation dataOp = new DataOperation();
                        DataTable dt = dataOp.GetData(MasterEntity.ViewId, dynModel.DataPath, CreateStructDataTable(dynModel));
                        foreach (DataRow item in dt.Rows)
                        {
                            EntityFieldModel fieldModel = new EntityFieldModel()
                            {
                                Field = item["field"].ToString(),
                                FieldType = item["fieldtype"].ToString(),
                                FieldTitle = item["fieldtitle"].ToString()
                            };
                            extandFields.Add(fieldModel);
                        }
                    }
                }
            }
            DataTable structDt = new DataTable(entityModel.TableName);
            foreach (EntityFieldModel item in entityFields)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = item.Field;
                if (item.Field.Equals("_id"))
                {
                    dc.ReadOnly = true;
                    dc.DefaultValue = Guid.NewGuid();
                }
                dc.Caption = item.FieldTitle;
                switch (item.FieldType)
                {
                    case "date":
                        dc.DataType = typeof(DateTime);
                        break;
                    case "int":
                        dc.DataType = typeof(Int32);
                        break;
                    case "long":
                        dc.DataType = typeof(Int64);
                        break;
                    case "bool":
                        dc.DataType = typeof(Boolean);
                        break;
                    case "string":
                    default:
                        dc.DataType = typeof(String);
                        break;
                }
                structDt.Columns.Add(dc);
            }
            foreach (EntityFieldModel item in extandFields)
            {
                DataColumn dc = new DataColumn();
                dc.ColumnName = item.Field;
                dc.Caption = item.FieldTitle;
                switch (item.FieldType)
                {
                    case "date":
                        dc.DataType = typeof(DateTime);
                        break;
                    case "int":
                        dc.DataType = typeof(Int32);
                        break;
                    case "long":
                        dc.DataType = typeof(Int64);
                        break;
                    case "bool":
                        dc.DataType = typeof(Boolean);
                        break;
                    case "string":
                    default:
                        dc.DataType = typeof(String);
                        break;
                }
                structDt.Columns.Add(dc);
            }
            return structDt;
        }
        /// <summary>
        /// 构建DataPath
        /// </summary>
        /// <param name="entityModel"></param>
        /// <param name="hostModel"></param>
        /// <param name="drv"></param>
        /// <returns></returns>
        private string ConstructDataPath(EntityDefinitionModel entityModel, EntityDefinitionModel hostModel, DataRowView drv)
        {
            string dataPath = string.Empty;
            if (string.IsNullOrEmpty(entityModel.HostTable))
            {
                dataPath = string.Format("[\"{0}\"]", entityModel.TableName);
                entityModel.DataPath = dataPath;
            }
            else
            {
                List<object> pathList = JsonHelper.ToObject<List<object>>(hostModel.DataPath);
                Dictionary<string, object> pathDic = new Dictionary<string, object>();
                pathDic.Add("key", "_id");
                pathDic.Add("value", drv["_id"]);
                pathList.Add(pathDic);
                pathList.Add(entityModel.TableName);
                dataPath = JsonHelper.ToJson(pathList);
                entityModel.DataPath = dataPath;
            }
            return dataPath;
        }
        /// <summary>
        /// 获取实体的RootTable
        /// </summary>
        /// <param name="entityModel"></param>
        /// <returns></returns>
        private string GetEntityRootTableName(EntityDefinitionModel entityModel)
        {
            if (string.IsNullOrEmpty(entityModel.HostTable))
            {
                return entityModel.TableName;
            }
            else
            {
                entityModel = allEntityModels.FirstOrDefault(it => it.Id == entityModel.HostTable);
                return GetEntityRootTableName(entityModel);
            }
        }
        #endregion
        #region 附加事件
        /// <summary>
        /// 入口按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void vicBtnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dataPathEntityList = new List<EntityDefinitionModel>();
                DataRefContent = null;
                CurrentContent = null;
                VicButtonNormal vicBtnEnter = (VicButtonNormal)sender;
                MasterEntity = (EntityDefinitionModel)vicBtnEnter.Tag;
                EntityModelReConsitution(MasterEntity);
                CreateEnableContent();
                RefreshMasterContent();
                CurrentEntity = MasterEntity.Copy();
                RefreshCurrentContent();
                dataPathEntityList.Add(MasterEntity);
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("入口按钮点击异常：{0}", ex.Message);
            }

        }
        /// <summary>
        /// 可用按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void vicBtnEnable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VicButtonNormal vicBtnEnable = (VicButtonNormal)sender;
                EntityDefinitionModel vicBtnEntity = (EntityDefinitionModel)vicBtnEnable.Tag;
                if (CurrentEntity.Id.Equals(vicBtnEntity.HostTable))
                {
                    MasterEntity = CurrentEntity.Copy();
                    CurrentEntity = vicBtnEntity.Copy();
                    dataPathEntityList.Add(MasterEntity);
                    RefreshMasterContent();
                }
                else if (CurrentEntity.HostTable.Equals(vicBtnEntity.Id))
                {
                    CurrentEntity = dataPathEntityList[dataPathEntityList.Count - 1].Copy();
                    MasterEntity = dataPathEntityList[dataPathEntityList.Count - 2].Copy();
                    RefreshMasterContent();
                }
                else
                {
                    CurrentEntity = vicBtnEntity.Copy();
                }
                EntityModelReConsitution(CurrentEntity);
                CreateEnableContent();
                RefreshCurrentContent();
                RefreshDataRefContent();
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("可用按钮点击异常：{0}", ex.Message);
            }
        }
        /// <summary>
        /// 主档树选择项目改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            masterSelectedModel = (DataRowView)((VicTreeView)sender).SelectedItem;
            if (masterSelectedModel == null)
                return;
            if (CurrentEntity != null)
                RefreshCurrentContent();
        }
        // <summary>
        /// 主档列表选择项目改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void masterGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            masterSelectedModel = (DataRowView)((VicDataGrid)sender).SelectedItem;
            if (masterSelectedModel == null)
                return;
            if (CurrentEntity != null)
            {
                RefreshCurrentContent();
            }
        }
        /// <summary>
        /// 数据引用中树节点选择改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void vicTv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            VicTreeView vicTv = (VicTreeView)sender;
            EntityDefinitionModel entityModel = vicTv.Tag as EntityDefinitionModel;
            if (editFlag && currentSeletecModel != null)
            {
                DataRow dr = GridDt.Select(string.Format("_id='{0}'", currentSeletecModel["_id"].ToString()))[0];
                List<RefEntityModel> refList = CurrentEntity.DataRef as List<RefEntityModel>;
                RefEntityModel refModel = refList.FirstOrDefault(it => it.TableId == entityModel.Id);
                dr[refModel.SelfField] = ((DataRowView)vicTv.SelectedItem)[refModel.SourceField];
            }
        }
        #endregion
    }
}
