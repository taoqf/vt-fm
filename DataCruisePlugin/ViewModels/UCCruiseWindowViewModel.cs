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

namespace DataCruisePlugin.ViewModels
{
    public class UCCruiseWindowViewModel : ModelBase
    {
        #region 字段
        /// <summary>
        /// 内置浏览器
        /// </summary>
        private WebBrowser myBrowser;

        private List<EntityDefinitionModel> enterEntityList;
        private List<string> testList;
        #endregion
        #region 属性
        public List<EntityDefinitionModel> EnterEntityList
        {
            get
            {
                if (enterEntityList == null)
                    enterEntityList = new List<EntityDefinitionModel>();
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
        public List<string> TestList
        {
            get
            {
                if (testList == null)
                    testList = new List<string>();
                return testList;
            }
            set
            {
                testList = value;
                RaisePropertyChanged("TestList");
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
                    //myBrowser = (WebBrowser)x;
                    //myBrowser.Navigating += browser_Navigating;
                    //myBrowser.Source = new Uri("http://localhost/VictopPartner/aaa.html");
                    string entityRel = ReadRelationFile("rel");
                    EnterEntityList = JsonHelper.ToObject<List<EntityDefinitionModel>>(entityRel);
                    foreach (EntityDefinitionModel item in EnterEntityList)
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

                    }
                });
            }
        }
        #endregion
        #region 私有方法
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
        void browser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            SuppressScriptErrors((WebBrowser)sender, true);
        }

        private void SuppressScriptErrors(WebBrowser webBrowser, bool Hide)
        {
            FieldInfo fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;

            object objComWebBrowser = fiComWebBrowser.GetValue(webBrowser);
            if (objComWebBrowser == null) return;

            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { Hide });
        }
        #endregion
    }
}
