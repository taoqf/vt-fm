using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Victop.Wpf.Controls;
using Victop.Frame.CmptRuntime;
using Victop.Frame.PublicLib.Helpers;
using System.Linq.Expressions;
using Victop.Server.Controls;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 用户控件模板类
    /// </summary>
    public class TemplateControl : UserControl, INotifyPropertyChanged
    {
        private bool initFlag;
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
        #region 公用属性
        /// <summary>
        /// 组件定义
        /// </summary>
        private CompntDefinModel DefinModel;
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
        /// <param name="spaceId">SpaceId</param>
        public bool InitVictopUserControl(string cmpntDefineContent, string spaceId = "")
        {
            try
            {
                DefinModel = JsonHelper.ToObject<CompntDefinModel>(cmpntDefineContent);
                if (DefinModel != null)
                {
                    if (!string.IsNullOrEmpty(spaceId))
                    {
                        foreach (var item in DefinModel.CompntViews)
                        {
                            item.SpaceId = spaceId;
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
        #endregion
        /// <summary>
        /// 飞道状态机
        /// </summary>
        public BaseStateMachine FeiDaoFSM;
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
        /// 执行通用方法
        /// </summary>
        /// <param name="paramDic">通用参数</param>
        public virtual void Excute(Dictionary<string, object> paramDic)
        {

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
    }
}
