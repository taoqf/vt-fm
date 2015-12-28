using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Victop.Frame.CmptRuntime
{
    /// <summary>
    /// 飞道原子操作映射类
    /// </summary>
    public class FeiDaoOperation
    {
        private TemplateControl MainView;
        /// <summary>
        /// 构造函数，页面/组件实体
        /// </summary>
        /// <param name="mainView"></param>
        public FeiDaoOperation(TemplateControl mainView)
        {
            MainView = mainView;
        }
        /// <summary>
        /// 设置按钮文本
        /// </summary>
        /// <param name="btnName"></param>
        /// <param name="btnContent"></param>
        public void SetButtonText(string btnName, string btnContent)
        {
            Button btn = MainView.FindName(btnName) as Button;
            btn.Content = btnContent;
        }
        /// <summary>
        /// 系统输出
        /// </summary>
        /// <param name="consoleText"></param>
        public void SysConsole(string consoleText)
        {
            Console.WriteLine(consoleText);
        }
        /// <summary>
        /// 获取BlockData的数据
        /// </summary>
        /// <param name="blockName"></param>
        public void GetPBlockData(string blockName)
        {
            PresentationBlockModel pBlock = MainView.GetPresentationBlockModel(blockName);
            pBlock.GetData();
        }
        /// <summary>
        /// 执行页面动作
        /// </summary>
        /// <param name="pageTrigger"></param>
        /// <param name="paramInfo"></param>
        public void ExcutePageTrigger(string pageTrigger, object paramInfo)
        {
            MainView.ParentControl.FeiDaoFSM.Do(pageTrigger, paramInfo);
        }
        /// <summary>
        /// 执行组件动作
        /// </summary>
        /// <param name="compntName"></param>
        /// <param name="compntTrigger"></param>
        /// <param name="paramInfo"></param>
        public void ExcuteComponentTrigger(string compntName, string compntTrigger, object paramInfo)
        {
            TemplateControl tc = MainView.FindName("compntName") as TemplateControl;
            tc.FeiDaoFSM.Do(compntTrigger, paramInfo);
        }
    }
}
