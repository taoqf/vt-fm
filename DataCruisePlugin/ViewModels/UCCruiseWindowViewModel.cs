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
        /// <summary>
        /// 选定实体类对象
        /// </summary>
        private EntityDefinitionModel currentEntityModel;
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
        #endregion
        #region Command
        public ICommand ucMainViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    string entityRel = ReadRelationFile("rel");
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
        /// 主Tab选择事件
        /// </summary>
        public ICommand lboxEnterSelectionChangedCommand
        {
            get
            {
                return new RelayCommand(() => {
                    string tableName = CurrentEntityModel.TableName;
                });
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
