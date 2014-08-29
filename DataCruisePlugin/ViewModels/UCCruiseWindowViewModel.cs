using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Input;
using Victop.Server.Controls;
using Victop.Server.Controls.Models;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using DataCruisePlugin.Models;
using Victop.Frame.PublicLib.Helpers;
using System.Windows.Navigation;
using System.Reflection;
using System.Collections.ObjectModel;
using Victop.Frame.SyncOperation;
using Victop.Wpf.Controls;
using Victop.Frame.DataChannel;
using System.Data;
using System.Windows.Data;
using System.Windows;

namespace DataCruisePlugin.ViewModels
{
    public class UCCruiseWindowViewModel : ModelBase
    {
        #region 字段
        private string viewId;
        private object masterContent;
        private object currentContent;
        private string treeGroupHeader;
        private string treeDataPath;
        private string treeViewId;
        private string gridGroupHeader;
        /// <summary>
        /// 选定实体类对象
        /// </summary>
        private EntityDefinitionModel currentEntityModel;
        /// <summary>
        /// 选定可用实体类对象
        /// </summary>
        private EntityDefinitionModel selectedEnableEntityModel;
        /// <summary>
        /// 主实体类对象
        /// </summary>
        private EntityDefinitionModel masterEntityModel;
        /// <summary>
        /// 完整实体集合
        /// </summary>
        private ObservableCollection<EntityDefinitionModel> allEntityList;

        /// <summary>
        /// 入口实体集合
        /// </summary>
        private ObservableCollection<EntityDefinitionModel> enterEntityList;
        /// <summary>
        /// 可用实体集合
        /// </summary>
        private ObservableCollection<EntityDefinitionModel> enableEntityList;

        private DataTable gridDt;

        private object gridSelectedItem;
        private object treeSelectedItem;
        #endregion
        #region 属性
        public string TreeGroupHeader
        {
            get
            {
                return treeGroupHeader;
            }
            set
            {
                treeGroupHeader = value;
                RaisePropertyChanged("TreeGroupHeader");
            }
        }
        public string TreeDataPath
        {
            get { return treeDataPath; }
            set
            {
                treeDataPath = value;
                RaisePropertyChanged("TreeDataPath");
            }
        }
        public string GridGroupHeader
        {
            get { return gridGroupHeader; }
            set
            {
                gridGroupHeader = value;
                RaisePropertyChanged("GridGroupHeader");
            }
        }
        /// <summary>
        /// 完整实体集合
        /// </summary>
        public ObservableCollection<EntityDefinitionModel> AllEntityList
        {
            get
            {
                if (allEntityList == null)
                    allEntityList = new ObservableCollection<EntityDefinitionModel>();
                return allEntityList;
            }
            set
            {
                if (allEntityList != value)
                {
                    allEntityList = value;
                    RaisePropertyChanged("AllEntityList");
                }
            }
        }
        /// <summary>
        /// 入口实体集合
        /// </summary>
        public ObservableCollection<EntityDefinitionModel> EnterEntityList
        {
            get
            {
                if (enterEntityList == null)
                    enterEntityList = new ObservableCollection<EntityDefinitionModel>();
                return enterEntityList;
            }
            set
            {
                if (enterEntityList != value)
                {
                    enterEntityList = value;
                    RaisePropertyChanged("EnterEntityList");
                }
            }
        }
        /// <summary>
        /// 可用实体集合
        /// </summary>
        public ObservableCollection<EntityDefinitionModel> EnableEntityList
        {
            get
            {
                if (enableEntityList == null)
                    enableEntityList = new ObservableCollection<EntityDefinitionModel>();
                return enableEntityList;
            }
            set
            {
                if (enableEntityList != value)
                {
                    enableEntityList = value;
                    RaisePropertyChanged("EnableEntityList");
                }
            }
        }
        /// <summary>
        /// 选定实体类对象
        /// </summary>
        public EntityDefinitionModel CurrentEntityModel
        {
            get
            {
                return currentEntityModel;
            }
            set
            {
                if (currentEntityModel != value)
                {
                    currentEntityModel = value;
                    RaisePropertyChanged("CurrentEntityModel");
                }
            }
        }
        /// <summary>
        /// 主实体对象
        /// </summary>
        public EntityDefinitionModel MasterEntityModel
        {
            get
            {
                return masterEntityModel;
            }
            set
            {
                if (masterEntityModel != value)
                {
                    masterEntityModel = value;
                    RaisePropertyChanged("MasterEntityModel");
                }
            }
        }
        /// <summary>
        /// 选定可用实体类对象
        /// </summary>
        public EntityDefinitionModel SelectedEnableEntityModel
        {
            get
            {
                return selectedEnableEntityModel;
            }
            set
            {
                if (selectedEnableEntityModel != value)
                {
                    selectedEnableEntityModel = value;
                    RaisePropertyChanged("SelectedEnableEntityModel");
                }
            }
        }
        public object MasterContent
        {
            get
            {
                return masterContent;
            }
            set
            {
                if (masterContent != value)
                {
                    masterContent = value;
                    RaisePropertyChanged("MasterContent");
                }
            }
        }

        public object CurrentContent
        {
            get
            {
                return currentContent;
            }
            set
            {
                if (currentContent != value)
                {
                    currentContent = value;
                    RaisePropertyChanged("CurrentContent");
                }
            }
        }

        public DataTable GridDt
        {
            get
            {
                if (gridDt == null)
                    gridDt = new DataTable();
                return gridDt;
            }
            set
            {
                gridDt = value;
                RaisePropertyChanged("GridDt");
            }
        }
        public object GridSelectedItem
        {
            get
            {
                return gridSelectedItem;
            }
            set
            {
                gridSelectedItem = value;
                RaisePropertyChanged("GridSelectedItem");
            }
        }
        #endregion
        #region Command
        public ICommand ucMainViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    string entityRel = ReadRelationFile("rel2");
                    AllEntityList = JsonHelper.ToObject<ObservableCollection<EntityDefinitionModel>>(entityRel);
                    foreach (EntityDefinitionModel item in AllEntityList)
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
                            EnterEntityList.Add(item);
                        }
                    }
                });
            }
        }
        /// <summary>
        /// 入口Tab选择事件
        /// </summary>
        public ICommand lboxEnterSelectionChangedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (CurrentEntityModel == null)
                        return;
                    EntityModelReConsitution(CurrentEntityModel);
                    CreateContent(CurrentEntityModel, MasterContent);
                });
            }
        }
        /// <summary>
        /// 创建树形Content
        /// </summary>
        private void CreateContent(EntityDefinitionModel entityModel, object ContentId)
        {
            try
            {
                DataOperation dataOp = new DataOperation();
                DataSet ds = new DataSet();
                if (string.IsNullOrEmpty(entityModel.HostTable))
                {
                    Dictionary<string, object> contentDic = new Dictionary<string, object>();
                    contentDic.Add("systemid", "800");
                    contentDic.Add("configsystemid", "101");
                    contentDic.Add("spaceid", "tbs");
                    contentDic.Add("tablename", entityModel.TableName);
                    Dictionary<string, object> returnDic = SendMessage("MongoDataChannelService.findTableData", contentDic);
                    viewId = returnDic["DataChannelId"].ToString();
                    string dataPath = string.Format("[\"{0}\"]", entityModel.TableName);
                    DataTable dt = dataOp.GetData(viewId, dataPath, CreateStructDataTable(entityModel));
                    ds.Tables.Add(dt);
                }
                else
                {
                    string hostName = AllEntityList.First(it => it.Id == entityModel.HostTable).TableName;
                    DataRowView temp = (DataRowView)GridSelectedItem;
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("key", "_id");
                    dic.Add("value", temp["_id"].ToString());
                    string dataPath = string.Format("[\"{0}\",{1},\"{2}\"]", hostName, JsonHelper.ToJson(dic), entityModel.TableName);
                    DataTable dt = dataOp.GetData(viewId, dataPath, CreateStructDataTable(entityModel));
                    if (ds.Tables.Contains(entityModel.TableName))
                    {
                        ds.Tables.Remove(entityModel.TableName);
                    }
                    ds.Tables.Add(dt);
                }
                switch (entityModel.ViewType)
                {
                    case "tree":
                        VicTreeView tv = new VicTreeView();
                        tv.IDField = "_id";
                        tv.FIDField = entityModel.ParentId;
                        tv.DisplayField = entityModel.TreeDisPlay;
                        tv.ItemsSource = ds.Tables[entityModel.TableName].DefaultView;
                        tv.SelectedItemChanged += tv_SelectedItemChanged;
                        MasterContent = tv;
                        TreeGroupHeader = entityModel.TabTitle;
                        TreeDataPath = entityModel.Id;
                        treeViewId = viewId;
                        break;
                    case "grid":
                        List<RefEntityModel> refList = entityModel.DataRef as List<RefEntityModel>;
                        RefEntityModel refModel= refList.Find(it => it.TableId == TreeDataPath);
                        if (refModel != null)
                        {
                            DataRowView drv = (DataRowView)treeSelectedItem;
                            GridDt = ds.Tables[entityModel.TableName].Copy();
                            DataRow[] drs=ds.Tables[entityModel.TableName].Select(string.Format("{0}='{1}'", refModel.SelfField, drv[refModel.SourceField].ToString()));
                            if (drs != null && drs.Count() > 0)
                            {
                                GridDt.Clear();
                                drs.CopyToDataTable(GridDt, LoadOption.OverwriteChanges);
                            }
                            else
                            {
                                GridDt = CreateStructDataTable(entityModel);
                            }
                        }
                        else
                        {
                            GridDt = ds.Tables[entityModel.TableName];
                        }
                        GridGroupHeader = entityModel.TabTitle;
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {
                //throw;
            }
        }

        void tv_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            treeSelectedItem = ((VicTreeView)sender).SelectedItem;
            if (SelectedEnableEntityModel != null)
            {
                CreateContent(SelectedEnableEntityModel, CurrentContent);
                EntityModelReConsitution(SelectedEnableEntityModel);
            }
        }
        /// <summary>
        /// 可用Tab选择事件
        /// </summary>
        public ICommand lboxEnableSelectionChangedCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    try
                    {
                        if (SelectedEnableEntityModel != null)
                        {
                            CreateContent(SelectedEnableEntityModel, CurrentContent);
                            EntityModelReConsitution(SelectedEnableEntityModel);
                            SelectedEnableEntityModel = EnableEntityList[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        string temp = ex.Message;
                    }
                });
            }
        }
        public ICommand currentGridSelectionChangedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    GridSelectedItem = x;
                });
            }
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
            EnableEntityList.Clear();
            EnableEntityList.Add(EntityModel);
            if (!string.IsNullOrEmpty(EntityModel.HostTable))
            {
                //上级可用
                EntityDefinitionModel hostModel = AllEntityList.First(it => it.Id == EntityModel.HostTable);
                if (hostModel != null)
                {
                    hostModel.Actived = true;
                    EnableEntityList.Add(hostModel);
                    //跟随可用
                    foreach (EntityDefinitionModel item in AllEntityList)
                    {
                        if (item.HostTable == hostModel.Id)
                        {
                            item.Actived = true;
                            if (EnableEntityList.FirstOrDefault(it => it.Id == item.Id) == null)
                                EnableEntityList.Add(item);
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
                                        if (EnableEntityList.FirstOrDefault(it => it.Id == item.Id) == null)
                                        {
                                            item.Actived = true;
                                            EnableEntityList.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ///同级可用
                foreach (EntityDefinitionModel item in AllEntityList.Where(it => it.HostTable == EntityModel.HostTable))
                {
                    item.Actived = true;
                    if (EnableEntityList.FirstOrDefault(it => it.Id == item.Id) == null)
                        EnableEntityList.Add(item);
                }
            }
            //下级可用
            foreach (EntityDefinitionModel item in AllEntityList.Where(it => it.HostTable == EntityModel.Id))
            {
                item.Actived = true;
                if (EnableEntityList.FirstOrDefault(it => it.Id == item.Id) == null)
                    EnableEntityList.Add(item);
            }
            //前导可用
            List<RefEntityModel> refEntityList = EntityModel.DataRef as List<RefEntityModel>;
            if (refEntityList != null)
            {
                foreach (RefEntityModel item in refEntityList)
                {
                    if (item.ForeRunner)
                    {
                        if (EnableEntityList.FirstOrDefault(it => it.Id == item.TableId) == null)
                        {
                            AllEntityList.First(it => it.Id == item.TableId).Actived = true;
                            EnableEntityList.Add(AllEntityList.First(it => it.Id == item.TableId));
                        }
                    }
                }
            }
            //跟随可用
            foreach (EntityDefinitionModel item in AllEntityList)
            {
                List<RefEntityModel> itemRefList = item.DataRef as List<RefEntityModel>;
                if (itemRefList != null)
                {
                    foreach (RefEntityModel refitem in itemRefList)
                    {
                        if (refitem.TableId == EntityModel.Id && refitem.ForeRunner)
                        {
                            if (EnableEntityList.FirstOrDefault(it => it.Id == item.Id) == null)
                            {
                                EnableEntityList.Add(item);
                                item.Actived = true;
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region 私有方法
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
        ///浏览器转跳事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void browser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            SuppressScriptErrors((WebBrowser)sender, true);
        }
        /// <summary>
        /// 屏蔽脚本异常
        /// </summary>
        /// <param name="webBrowser"></param>
        /// <param name="Hide"></param>
        private void SuppressScriptErrors(WebBrowser webBrowser, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;

            object objComWebBrowser = fiComWebBrowser.GetValue(webBrowser);
            if (objComWebBrowser == null) return;

            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        }

        private Dictionary<string, object> SendMessage(string messageType, Dictionary<string, object> messageContent)
        {
            MessageOperation messageOp = new MessageOperation();
            return messageOp.SendMessage(messageType, messageContent, "JSON");
        }

        /// <summary>
        /// 创建结构表
        /// </summary>
        /// <param name="entityFields"></param>
        /// <returns></returns>
        private DataTable CreateStructDataTable(EntityDefinitionModel entityModel)
        {
            DataRowView drv = (DataRowView)treeSelectedItem;
            List<EntityFieldModel> entityFields = entityModel.Fields as List<EntityFieldModel>;
            List<EntityFieldModel> extandFields = new List<EntityFieldModel>();
            if (entityModel.DynaColumn != null)
            {
                List<string> tempList = new List<string>();
                foreach (string dynCol in entityModel.DynaColumn)
                {
                    EntityDefinitionModel dynModel = AllEntityList.FirstOrDefault(it => it.Id == dynCol);
                    tempList.Add(dynModel.TableName);
                    if (!string.IsNullOrEmpty(dynModel.HostTable) && dynModel.HostTable == TreeDataPath)
                    {
                        EntityDefinitionModel hostModel = AllEntityList.First(it => it.Id == TreeDataPath);
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("key", "_id");
                        dic.Add("value", drv["_id"]);
                        tempList.Add(JsonHelper.ToJson(dic));
                        tempList.Add(hostModel.TableName);
                        List<string> pathList = new List<string>();
                        for (int i = tempList.Count - 1; i >= 0; i--)
                        {
                            pathList.Add(tempList[i]);
                        }
                        DataOperation dataOp = new DataOperation();
                        DataTable dt = dataOp.GetData(treeViewId, JsonHelper.ToJson(pathList), CreateStructDataTable(dynModel));
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

        #endregion
    }
}
