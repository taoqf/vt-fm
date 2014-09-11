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
using DataCruisePlugin.Conververs;

namespace DataCruisePlugin.ViewModels
{
    public class UCCruiseWindowViewModel : ModelBase
    {
        #region 私有变量定义
        /// <summary>
        /// 编辑tab
        /// </summary>
        private BlockModel currentBlock;

        /// <summary>
        /// 主窗体实例
        /// </summary>
        private UserControl ucMainView;
        /// <summary>
        /// 实体定义集合
        /// </summary>
        private List<EntityDefinitionModel> allEntityDefList = new List<EntityDefinitionModel>();
        /// <summary>
        /// 入口Block集合
        /// </summary>
        private List<BlockModel> entranceBlockList = new List<BlockModel>();
        /// <summary>
        /// 可用Block集合
        /// </summary>
        private List<BlockModel> enableBlockList = new List<BlockModel>();
        /// <summary>
        /// 块路径集合
        /// </summary>
        private List<BlockModel> blockPathList = new List<BlockModel>();
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
        /// 主Tab
        /// </summary>
        private BlockModel masterBlock;
        /// <summary>
        /// 主Tab数据集合
        /// </summary>
        public BlockModel MasterBlock
        {
            get
            {
                return masterBlock;
            }
            set
            {
                if (masterBlock != value)
                {
                    masterBlock = value;
                    RaisePropertyChanged("MasterBlock");
                }
            }
        }
        private object entranceContent;
        /// <summary>
        /// 入口区域
        /// </summary>
        public object EntranceContent
        {
            get
            {
                return entranceContent;
            }
            set
            {
                if (entranceContent != value)
                {
                    entranceContent = value;
                    RaisePropertyChanged("EntranceContent");
                }
            }
        }
        private object enableContent;
        /// <summary>
        /// 可用区域
        /// </summary>
        public object EnableContent
        {
            get
            {
                return enableContent;
            }
            set
            {
                if (enableContent != value)
                {
                    enableContent = value;
                    RaisePropertyChanged("EnableContent");
                }
            }
        }

        #endregion
        #region Command定义
        /// <summary>
        /// 窗体加载
        /// </summary>
        public ICommand ucMainViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    ucMainView = (UserControl)x;
                    string entityRel = ReadRelationFile("rel2");
                    entranceBlockList.Clear();
                    allEntityDefList = JsonHelper.ToObject<List<EntityDefinitionModel>>(entityRel);
                    foreach (EntityDefinitionModel item in allEntityDefList)
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
                            BlockModel block = new BlockModel()
                            {
                                EntityDefModel = item,
                                TableId = item.Id,
                                TableName = item.TableName
                            };
                            entranceBlockList.Add(block);
                        }
                    }
                    CreateEnterContent();
                });
            }
        }
        /// <summary>
        /// 编辑区添加
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
        /// 编辑区删除
        /// </summary>
        public ICommand btnDelClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                });
            }
        }
        /// <summary>
        /// 编辑区保存
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
        #endregion
        #region 私有方法

        /// <summary>
        /// 创建可用展示内容
        /// </summary>
        private void CreateEnableContent()
        {
            if (enableBlockList.Count > 0)
            {
                VicDockPanelNormal dockPanel = new VicDockPanelNormal();
                dockPanel.HorizontalAlignment = HorizontalAlignment.Left;

                foreach (BlockModel item in enableBlockList)
                {
                    if (!string.IsNullOrEmpty(item.EntityDefModel.HostTable))
                    {
                        item.ParentBlock = enableBlockList.FirstOrDefault(it => it.TableId == item.EntityDefModel.Id);
                        if (item.ParentBlock == null)
                        {
                            EntityDefinitionModel hostModel = allEntityDefList.FirstOrDefault(it => it.Id == item.EntityDefModel.HostTable);
                            BlockModel hostBlock = new BlockModel()
                            {
                                ViewId = hostModel.ViewId,
                                TableName = hostModel.TableName,
                                TableId = hostModel.Id,
                                EntityDefModel = hostModel
                            };
                            item.ParentBlock = hostBlock;
                        }
                    }
                    VicButtonNormal vicBtnEnable = new VicButtonNormal();
                    vicBtnEnable.Name = item.TableName;
                    vicBtnEnable.Tag = item;
                    vicBtnEnable.Width = 100;
                    vicBtnEnable.Height = 24;
                    vicBtnEnable.Content = item.EntityDefModel.TabTitle;
                    vicBtnEnable.Margin = new Thickness(0, 0, 5, 0);
                    vicBtnEnable.Click += vicBtnEnable_Click;
                    dockPanel.Children.Add(vicBtnEnable);
                }
                EnableContent = dockPanel;
            }
        }

        /// <summary>
        /// 实体Model重构
        /// </summary>
        private void EntityModelReConsitution(BlockModel blockModel)
        {
            /* 1、选定实体的hostTable对应的实体可用(上级可用)
             * 2、hostTable与选定实体的hostTable相同的实体可用(同级可用)
             * 3、以选定实体为hostTable的实体可用(下级可用)
             * 4、当前实体的dataRef中前导实体可用(前导可用)
             * 5、入口实体一致可用
             * 6、以当前实体为前导的实体可用(跟随可用)
             */
            enableBlockList.Clear();
            enableBlockList.Add(blockModel);
            if (!string.IsNullOrEmpty(blockModel.EntityDefModel.HostTable))
            {
                //上级可用
                EntityDefinitionModel hostModel = allEntityDefList.First(it => it.Id == blockModel.EntityDefModel.HostTable);
                if (hostModel != null)
                {
                    BlockModel hostBlock = enableBlockList.FirstOrDefault(it => it.EntityDefModel.Id == hostModel.Id);
                    if (hostBlock == null)
                    {
                        enableBlockList.Add(new BlockModel()
                        {
                            EntityDefModel = hostModel,
                            TableId = hostModel.Id,
                            TableName = hostModel.TableName
                        });
                    }
                    //跟随可用
                    foreach (EntityDefinitionModel item in allEntityDefList)
                    {
                        if (item.HostTable == hostModel.Id)
                        {
                            item.Actived = true;
                            if (enableBlockList.FirstOrDefault(it => it.EntityDefModel.Id == item.Id) == null)
                                enableBlockList.Add(new BlockModel()
                                {
                                    EntityDefModel = item,
                                    TableId = item.Id,
                                    TableName = item.TableName
                                });
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
                                        if (enableBlockList.FirstOrDefault(it => it.EntityDefModel.Id == item.Id) == null)
                                            enableBlockList.Add(new BlockModel()
                                            {
                                                EntityDefModel = item,
                                                TableId = item.Id,
                                                TableName = item.TableName
                                            });
                                    }
                                }
                            }
                        }
                    }
                }
                ///同级可用
                foreach (EntityDefinitionModel item in allEntityDefList.Where(it => it.HostTable == blockModel.EntityDefModel.HostTable))
                {
                    if (enableBlockList.FirstOrDefault(it => it.EntityDefModel.Id == item.Id) == null)
                        enableBlockList.Add(new BlockModel()
                        {
                            EntityDefModel = item,
                            TableId = item.Id,
                            TableName = item.TableName
                        });
                }
            }
            //下级可用
            foreach (EntityDefinitionModel item in allEntityDefList.Where(it => it.HostTable == blockModel.EntityDefModel.Id))
            {
                if (enableBlockList.FirstOrDefault(it => it.EntityDefModel.Id == item.Id) == null)
                    enableBlockList.Add(new BlockModel()
                    {
                        EntityDefModel = item,
                        TableId = item.Id,
                        TableName = item.TableName
                    });
            }
            //前导可用
            List<RefEntityModel> refEntityList = blockModel.EntityDefModel.DataRef as List<RefEntityModel>;
            if (refEntityList != null)
            {
                foreach (RefEntityModel item in refEntityList)
                {
                    if (item.ForeRunner)
                    {
                        if (enableBlockList.FirstOrDefault(it => it.EntityDefModel.Id == item.TableId) == null)
                        {
                            EntityDefinitionModel refModel = allEntityDefList.First(it => it.Id == item.TableId);
                            enableBlockList.Add(new BlockModel()
                            {
                                EntityDefModel = refModel,
                                TableId = refModel.Id,
                                TableName = refModel.TableName
                            });
                        }
                    }
                }
            }
            //跟随可用
            foreach (EntityDefinitionModel item in allEntityDefList)
            {
                List<RefEntityModel> itemRefList = item.DataRef as List<RefEntityModel>;
                if (itemRefList != null)
                {
                    foreach (RefEntityModel refitem in itemRefList)
                    {
                        if (refitem.TableId == blockModel.EntityDefModel.Id && refitem.ForeRunner)
                        {
                            if (enableBlockList.FirstOrDefault(it => it.TableId == item.Id) == null)
                            {
                                enableBlockList.Add(new BlockModel() {
                                    EntityDefModel = item,
                                    TableId = item.Id,
                                    TableName = item.TableName
                                });
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 创建入口展示内容
        /// </summary>
        /// <returns></returns>
        private void CreateEnterContent()
        {
            if (entranceBlockList.Count > 0)
            {
                VicDockPanelNormal dockPanel = new VicDockPanelNormal();
                dockPanel.HorizontalAlignment = HorizontalAlignment.Left;
                foreach (BlockModel item in entranceBlockList)
                {
                    VicButtonNormal vicBtnEnter = new VicButtonNormal();
                    vicBtnEnter.Name = item.TableName;
                    vicBtnEnter.Tag = item;
                    vicBtnEnter.Width = 100;
                    vicBtnEnter.Height = 24;
                    vicBtnEnter.Content = item.EntityDefModel.TabTitle;
                    vicBtnEnter.Margin = new Thickness(0, 0, 5, 0);
                    vicBtnEnter.Click += vicBtnEnter_Click;
                    dockPanel.Children.Add(vicBtnEnter);
                }
                EntranceContent = dockPanel;
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
        /// 刷新主Tab内容
        /// </summary>
        private void RefreshMasterContent()
        {
            MasterBlock.BlockDt = CreateStructDataTable(masterBlock);
            MasterBlock.RebuildDataPath(enableBlockList, allEntityDefList);
            VicDataGrid mastergrid = new VicDataGrid();
            mastergrid.AutoGenerateColumns = false;
            mastergrid.CanUserAddRows = false;
            mastergrid.IsUserSetColumn = true;
            foreach (DataColumn dc in masterBlock.BlockDt.Columns)
            {
                DataGridTextColumn txtCol = new DataGridTextColumn();
                txtCol.Header = dc.Caption;
                Binding colBinding = new Binding(dc.ColumnName);
                if (!dc.ReadOnly)
                {
                    colBinding.Mode = BindingMode.TwoWay;
                    colBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                }
                else
                {
                    txtCol.IsReadOnly = true;
                }
                txtCol.Binding = colBinding;
                mastergrid.Columns.Add(txtCol);
            }
            Binding masterBinding = new Binding("MasterBlock.BlockDt");
            masterBinding.Mode = BindingMode.TwoWay;
            masterBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding(mastergrid, DataGrid.ItemsSourceProperty, masterBinding);
            MasterContent = mastergrid;
            MasterHeader = masterBlock.EntityDefModel.TabTitle;
        }

        /// <summary>
        /// 创建结构表
        /// </summary>
        /// <param name="entityFields"></param>
        /// <returns></returns>
        private DataTable CreateStructDataTable(BlockModel entityModel)
        {
            List<EntityFieldModel> entityFields = entityModel.EntityDefModel.Fields as List<EntityFieldModel>;
            List<EntityFieldModel> extandFields = new List<EntityFieldModel>();
            if (entityModel.EntityDefModel.DynaColumn != null)
            {
                foreach (string dynCol in entityModel.EntityDefModel.DynaColumn)
                {
                    EntityDefinitionModel dynModel = allEntityDefList.First(it => it.Id == dynCol);

                    //if (dynModel.HostTable.Equals(MasterEntity.Id))
                    //{
                    //    ConstructDataPath(dynModel, MasterEntity, masterSelectedModel);
                    //    DataOperation dataOp = new DataOperation();
                    //    DataTable dt = dataOp.GetData(MasterEntity.ViewId, dynModel.DataPath, CreateStructDataTable(dynModel));
                    //    foreach (DataRow item in dt.Rows)
                    //    {
                    //        EntityFieldModel fieldModel = new EntityFieldModel()
                    //        {
                    //            Field = item["field"].ToString(),
                    //            FieldType = item["fieldtype"].ToString(),
                    //            FieldTitle = item["fieldtitle"].ToString()
                    //        };
                    //        extandFields.Add(fieldModel);
                    //    }
                    //}
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
        #endregion

        #region 附件事件
        /// <summary>
        /// 入口Block按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void vicBtnEnter_Click(object sender, RoutedEventArgs e)
        {
            VicButtonNormal vicBtn = (VicButtonNormal)sender;
            BlockModel btnTag = (BlockModel)vicBtn.Tag;
            EntityModelReConsitution(btnTag);
            masterBlock = btnTag;
            CreateEnableContent();
            RefreshMasterContent();
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
                BlockModel vicBtnEntity = (BlockModel)vicBtnEnable.Tag;
                if (vicBtnEntity.EntityDefModel.Entrance)
                {
                    vicBtnEnter_Click(sender, e);
                    return;
                }
                else
                {
                    currentBlock = vicBtnEntity;
                    EntityModelReConsitution(vicBtnEntity);
                    CreateEnableContent();
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.InfoFormat("可用异常：{0}", ex.Message);
            }
        }
        #endregion
    }
}
