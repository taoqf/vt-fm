using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Victop.Frame.PublicLib.Helpers;
using System.Linq.Expressions;
using System.Reflection;
using System.Configuration;
using Victop.Frame.PublicLib.Managers;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 用户控件模板类
    /// </summary>
    public class TemplateControl : UserControl, INotifyPropertyChanged
    {
        #region 私有变量
        /// <summary>
        /// 组件定义
        /// </summary>
        private CompntDefinModel DefinModel;
        /// <summary>
        /// 内置浏览器
        /// </summary>
        public WebBrowser BuiltBrowser = new WebBrowser();
        private bool initFlag;
        private int businessModel;
        private Dictionary<string, TemplateControl> listCompnt = new Dictionary<string, TemplateControl>();
        #endregion
        #region 公用属性
        /// <summary>
        /// 业务模式(0:C#,1:JS)
        /// </summary>
        public int BusinessModel
        {
            get { return businessModel; }
            set { businessModel = value; }
        }
        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool InitFlag
        {
            get
            {
                return initFlag;
            }
        }
        /// <summary>
        /// SpaceId
        /// </summary>
        public string SpaceId { get; set; }
        /// <summary>
        /// 飞道引擎
        /// </summary>
        public BaseBusinessMachine FeiDaoMachine;
        /// <summary>
        /// 参数键值对
        /// </summary>
        public Dictionary<string, object> ParamDict { get; set; }
        /// <summary>
        /// 展示方式
        /// </summary>
        public int ShowType { get; set; }
        /// <summary>
        /// 模板委托事件
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="paramDic">事件参数</param>
        public delegate void TemplateDelegateEvent(object sender, Dictionary<string, object> paramDic);
        /// <summary>
        /// 系统Id
        /// </summary>
        public string SystemId
        {
            get { return (string)GetValue(SystemIdProperty); }
            set { SetValue(SystemIdProperty, value); }
        }
        /// <summary>
        /// 功能Id
        /// </summary>
        public string FormId
        {
            get { return (string)GetValue(FormIdProperty); }
            set { SetValue(FormIdProperty, value); }
        }
        /// <summary>
        /// 引用数据Id
        /// </summary>
        public string RefSystemId
        {
            get { return (string)GetValue(RefSystemIdProperty); }
            set { SetValue(RefSystemIdProperty, value); }
        }
        /// <summary>
        /// Vic错误信息
        /// </summary>
        public string VicErrorMsg
        {
            get { return (string)GetValue(VicErrorMsgProperty); }
            set { SetValue(VicErrorMsgProperty, value); }
        }
        /// <summary>
        /// 父级控件
        /// </summary>
        public TemplateControl ParentControl
        {
            get { return (TemplateControl)GetValue(ParentControlProperty); }
            set { SetValue(ParentControlProperty, value); }
        }
        #endregion
        #region 依赖属性
        /// <summary>
        /// 系统Id
        /// </summary>
        public static readonly DependencyProperty SystemIdProperty = DependencyProperty.Register("SystemId", typeof(string), typeof(TemplateControl));
        /// <summary>
        /// 功能Id
        /// </summary>
        public static readonly DependencyProperty FormIdProperty = DependencyProperty.Register("FormId", typeof(string), typeof(TemplateControl));
        /// <summary>
        /// 引用数据Id
        /// </summary>
        public static readonly DependencyProperty RefSystemIdProperty = DependencyProperty.Register("RefSystemId", typeof(string), typeof(TemplateControl));
        /// <summary>
        /// Vic错误信息
        /// </summary>
        public static readonly DependencyProperty VicErrorMsgProperty = DependencyProperty.Register("VicErrorMsg", typeof(string), typeof(TemplateControl));
        /// <summary>
        /// 父级控件
        /// </summary>
        public static readonly DependencyProperty ParentControlProperty = DependencyProperty.Register("ParentControl", typeof(TemplateControl), typeof(TemplateControl));
        #endregion
        #region 公用方法
        /// <summary>
        /// 初始化飞道用户控件
        /// </summary>
        /// <param name="cmpntDefineContent">组件定义内容</param>
        public bool InitVictopUserControl(string cmpntDefineContent)
        {
            try
            {
                DefinModel = JsonHelper.ToObject<CompntDefinModel>(cmpntDefineContent);
                if (DefinModel != null)
                {
                    if (!string.IsNullOrEmpty(SpaceId))
                    {
                        foreach (var item in DefinModel.CompntViews)
                        {
                            item.SpaceId = SpaceId;
                        }
                    }
                    OrgnizeRuntime.InitCompnt(DefinModel);
                    initFlag = true;
                }
                else
                {
                    VicErrorMsg = "组件定义内容异常";
                    initFlag = false;
                }
            }
            catch (Exception ex)
            {
                VicErrorMsg = ex.Message;
                initFlag = false;
            }
            return initFlag;
        }

        /// <summary>
        /// 获取展示层实体
        /// </summary>
        /// <param name="blockName">展示层名称</param>
        /// <returns></returns>
        public PresentationBlockModel GetPresentationBlockModel(string blockName)
        {
            try
            {
                PresentationBlockModel blockModel = DefinModel.CompntPresentation.PresentationBlocks.FirstOrDefault(it => it.BlockName.Equals(blockName));
                return blockModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 执行通用方法
        /// </summary>
        /// <param name="paramDic">通用参数</param>
        public virtual void Excute(Dictionary<string, object> paramDic)
        {

        }

        /// <summary>
        /// 获取组件实例，用于独立窗体展示组件
        /// </summary>
        /// <param name="componetName">组件名称</param>
        /// <returns></returns>
        public virtual TemplateControl GetComponentInstanceByName(string componetName)
        {
            return this;
        }
        /// <summary>
        /// 组件集合添加组件实例
        /// </summary>
        /// <param name="componetName">组件名称</param>
        /// <param name="templateControl">组件实例</param>
        public void AddListCompnt(string componetName, TemplateControl templateControl)
        {
            if (listCompnt.ContainsKey(componetName))
            {
                listCompnt[componetName] = templateControl;
            }
            else
            {
                listCompnt.Add(componetName, templateControl);
            }
        }
        /// <summary>
        /// 得到组件实例
        /// </summary>
        /// <param name="componetName">组件名称</param>
        /// <returns>组件实例</returns>
        public TemplateControl GetCompntInstance(string componetName)
        {
            object obj = FindName(componetName);
            if (obj == null && listCompnt.ContainsKey(componetName))
            {
                return listCompnt[componetName];
            }
            return obj as TemplateControl;
        }
        /// <summary>
        /// 属性改变
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性值变化时发生
        /// </summary>
        /// <param name="propertyExpression">属性表达式</param>
        public void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = (propertyExpression.Body as MemberExpression).Member.Name;
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        /// <summary>
        /// 内置webbrowser加载完成委托
        /// </summary>
        public delegate void BrowserLoadComplateDelegate();
        /// <summary>
        /// 内置webbrowser加载完成
        /// </summary>
        public event BrowserLoadComplateDelegate BrowserLoadComplate;
        #endregion
        #region WebBrowser相关
        /// <summary>
        /// 初始化内置浏览器
        /// </summary>
        /// <param name="fileUrl"></param>
        private void InitWebBrowser(string fileUrl)
        {
            BuiltBrowser.ObjectForScripting = new FeiDaoOperation(this);
            BuiltBrowser.Navigating += BuiltBrowser_Navigating;
            BuiltBrowser.Source = new Uri(fileUrl);
            BuiltBrowser.Visibility = Visibility.Collapsed;
            BuiltBrowser.Name = string.Format("{0}BuiltBrowser", this.Name);
            Grid grid = this.FindName(string.Format("{0}BuiltGrid", this.Name)) as Grid;
            if (grid != null)
            {
                grid.Children.Clear();
                grid.Children.Add(BuiltBrowser);
            }
        }

        private void BuiltBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
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
        /// <summary>
        /// 向内置浏览器传入规则及状态
        /// </summary>
        internal void BuiltBrowserInit()
        {
            if (BusinessModel.Equals(1))
            {
                FeiDaoMachine.Init();
                FeiDaoMachine.Do("beforeinit", this);
                if (BrowserLoadComplate != null)
                {
                    BrowserLoadComplate();
                }
            }
        }
        /// <summary>
        /// 执行js方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object MainViewInvokeScript(string methodName, params object[] args)
        {
            return BuiltBrowser.InvokeScript(methodName, args);
        }
        #endregion
        #region 重写事件
        /// <summary>
        /// 初始化完成
        /// </summary>
        public override void EndInit()
        {
            base.EndInit();
            if (BusinessModel.Equals(1) && !DesignerProperties.GetIsInDesignMode(this))
            {
                string url = string.Format("{0}/{1}", ConfigManager.GetLocalHttpServerBaseUrl(), ConfigurationManager.AppSettings["businesspath"]);
                InitWebBrowser(url);
            }
        }
        #endregion
    }
}
