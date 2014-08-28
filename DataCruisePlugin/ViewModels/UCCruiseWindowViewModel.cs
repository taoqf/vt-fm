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

namespace DataCruisePlugin.ViewModels
{
    public class UCCruiseWindowViewModel : ModelBase
    {
        #region 字段
        private object masterContent;
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
        #endregion
        #region 属性
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
                    //根据CurrentEntityModel.TableName去检索数据
                    //根据CurrentEntityModel.Fields,CurrentEntityModel.DynaColumn生成DataGrid列
                    switch (CurrentEntityModel.ViewType)
                    {
                        case "tree":
                            TreeView tv = new TreeView();
                            if (CurrentEntityModel.Fields != null)
                            {
                                List<EntityFieldModel> FieldList = CurrentEntityModel.Fields as List<EntityFieldModel>;
                                foreach (EntityFieldModel item in FieldList)
                                {
                                    tv.Items.Add(item.FieldTitle);
                                }
                            }
                            MasterContent = tv;
                            break;
                        case "grid":
                            DataGrid dgrid = new DataGrid();
                            if (CurrentEntityModel.Fields != null)
                            {
                                List<EntityFieldModel> FieldList = CurrentEntityModel.Fields as List<EntityFieldModel>;
                                foreach (EntityFieldModel item in FieldList)
                                {
                                    DataGridTextColumn dcolumn = new DataGridTextColumn();
                                    dcolumn.Header = item.FieldTitle;
                                    dgrid.Columns.Add(dcolumn);
                                }
                            }
                            MasterContent = dgrid;
                            break;
                        default:
                            break;
                    }
                });
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
                            EntityModelReConsitution(SelectedEnableEntityModel);
                    }
                    catch (Exception ex)
                    {
                        string temp = ex.Message;
                    }
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
            return messageOp.SendMessage(messageType, messageContent);
        }
        #endregion
    }
}
