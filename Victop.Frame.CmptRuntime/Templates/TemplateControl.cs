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
                    OrgnizeRuntime.InitCompnt(DefinModel);
                    PresentationBlockModel blockModel = DefinModel.CompntPresentation.PresentationBlocks.FirstOrDefault(it => it.Superiors.Equals("root"));
                    foreach (var item in DefinModel.CompntPresentation.PresentationBlocks.Where(it => it.Superiors.Equals("root")))
                    {
                        bool result = CheckPresentationBlock(this, item.BlockName);
                        if (!result)
                        {
                            return false;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    initFlag = true;
                    return true;
                }
                else
                {
                    VicErrorMsg = "组件定义内容异常";
                    return false;
                }
            }
            catch (Exception ex)
            {
                VicErrorMsg = ex.Message;
                return false;
            }
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
        #region 私有方法
        /// <summary>
        /// 检查展示层Block
        /// </summary>
        /// <param name="obj">界面元素</param>
        /// <param name="ctrlName">元素名称</param>
        /// <returns></returns>
        private bool CheckPresentationBlock(DependencyObject obj, string ctrlName)
        {
            bool result = false;
            VicGridNormal pBlockGrid = XamlTreeHelper.GetChildObjectByName<VicGridNormal>(obj, ctrlName);
            if (pBlockGrid == null)
            {
                VicErrorMsg = string.Format("未查询到名称为{0}的界面元素", ctrlName);
                return result;
            }
            else
            {
                PresentationBlockModel blockModel = DefinModel.CompntPresentation.PresentationBlocks.FirstOrDefault(it => it.Superiors.Equals(ctrlName));
                if (blockModel != null)
                {
                    result = CheckPresentationBlock(pBlockGrid, blockModel.BlockName);
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }
        #endregion
        /// <summary>
        /// 飞道状态机
        /// </summary>
        public BaseStateMachine FeiDaoFSM;
        /// <summary>
        /// 参数值列
        /// </summary>
        public static Dictionary<string, object> ParamDict
        {
            get; set;
        }
        /// <summary>
        /// 属性改变
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 属性改变通知
        /// </summary>
        /// <param name="propertyName"></param>
        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
