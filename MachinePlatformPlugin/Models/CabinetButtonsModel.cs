using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace MachinePlatformPlugin.Models
{
    /// <summary>
    /// 机台按钮集合实体
    /// </summary>
    public class CabinetButtonsModel : ModelBase
    {
        #region 任务列表按钮
        /// <summary>
        /// 查询按钮状态
        /// </summary>
        private Visibility searchBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 查询按钮状态
        /// </summary>
        public Visibility SearchBtnVisibility
        {
            get { return searchBtnVisibility; }
            set
            {
                searchBtnVisibility = value;
                RaisePropertyChanged("SearchBtnVisibility");
            }
        }

        /// <summary>
        /// 添加按钮状态
        /// </summary>
        private Visibility addBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 添加按钮状态
        /// </summary>
        public Visibility AddBtnVisibility
        {
            get
            {
                return addBtnVisibility;
            }
            set
            {
                addBtnVisibility = value;
                RaisePropertyChanged("AddBtnVisibility");
            }
        }
        /// <summary>
        /// 删除按钮状态
        /// </summary>
        private Visibility delBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 删除按钮状态
        /// </summary>
        public Visibility DelBtnVisibility
        {
            get { return delBtnVisibility; }
            set
            {
                delBtnVisibility = value;
                RaisePropertyChanged("DelBtnVisibility");
            }
        }
        /// <summary>
        /// 保存按钮状态
        /// </summary>
        private Visibility saveBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 保存按钮状态
        /// </summary>
        public Visibility SaveBtnVisibility
        {
            get { return saveBtnVisibility; }
            set
            {
                saveBtnVisibility = value;
                RaisePropertyChanged("SaveBtnVisibility");
            }
        }
        /// <summary>
        /// 派工按钮状态
        /// </summary>
        private Visibility divideBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 派工按钮状态
        /// </summary>
        public Visibility DivideBtnVisibility
        {
            get
            {
                return divideBtnVisibility;
            }
            set
            {
                divideBtnVisibility = value;
                RaisePropertyChanged("DivideBtnVisibility");
            }
        }
        /// <summary>
        /// 开工按钮状态
        /// </summary>
        private Visibility workBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 开工按钮状态
        /// </summary>
        public Visibility WorkBtnVisibility
        {
            get { return workBtnVisibility; }
            set
            {
                workBtnVisibility = value;
                RaisePropertyChanged("WorkBtnVisibility");
            }
        }
        /// <summary>
        /// 挂起按钮状态
        /// </summary>
        private Visibility suspendBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 挂起按钮状态
        /// </summary>
        public Visibility SuspendBtnVisibility
        {
            get { return suspendBtnVisibility; }
            set
            {
                suspendBtnVisibility = value;
                RaisePropertyChanged("SuspendBtnVisibility");
            }
        }
        /// <summary>
        /// 解挂按钮状态
        /// </summary>
        private Visibility continueBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 解挂按钮状态
        /// </summary>
        public Visibility ContinueBtnVisibility
        {
            get { return continueBtnVisibility; }
            set
            {
                continueBtnVisibility = value;
                RaisePropertyChanged("ContinueBtnVisibility");
            }
        }
        /// <summary>
        /// 交工按钮状态
        /// </summary>
        private Visibility endWorkBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 交工按钮状态
        /// </summary>
        public Visibility EndWorkBtnVisibility
        {
            get { return endWorkBtnVisibility; }
            set
            {
                endWorkBtnVisibility = value;
                RaisePropertyChanged("EndWorkBtnVisibility");
            }
        }
        /// <summary>
        /// 审核按钮状态
        /// </summary>
        private Visibility examineBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 审核按钮状态
        /// </summary>
        public Visibility ExamineBtnVisibility
        {
            get { return examineBtnVisibility; }
            set
            {
                examineBtnVisibility = value;
                RaisePropertyChanged("ExamineBtnVisibility");
            }
        }
        /// <summary>
        /// 驳回按钮状态
        /// </summary>
        private Visibility rejectBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 驳回按钮状态
        /// </summary>
        public Visibility RejectBtnVisibility
        {
            get { return rejectBtnVisibility; }
            set
            {
                rejectBtnVisibility = value;
                RaisePropertyChanged("RejectBtnVisibility");
            }
        }
        /// <summary>
        /// 问题反馈按钮状态
        /// </summary>
        private Visibility feedbackBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 问题反馈按钮状态
        /// </summary>
        public Visibility FeedbackBtnVisibility
        {
            get { return feedbackBtnVisibility; }
            set
            {
                feedbackBtnVisibility = value;
                RaisePropertyChanged("FeedbackBtnVisibility");
            }
        }
        #endregion
        #region 传入区/指南区按钮
        /// <summary>
        /// 传入区下载按钮状态
        /// </summary>
        private Visibility inputDownBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 传入区下载按钮状态
        /// </summary>
        public Visibility InputDownBtnVisibility
        {
            get { return inputDownBtnVisibility; }
            set
            {
                inputDownBtnVisibility = value;
                RaisePropertyChanged("InputDownBtnVisibility");
            }
        }
        #endregion
        #region 传出区/上传区按钮
        /// <summary>
        /// 生产日志按钮状态
        /// </summary>
        private Visibility proLogBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 生产日志按钮状态
        /// </summary>
        public Visibility ProLogBtnVisibility
        {
            get { return proLogBtnVisibility; }
            set
            {
                proLogBtnVisibility = value;
                RaisePropertyChanged("ProLogBtnVisibility");
            }
        }
        /// <summary>
        /// 问题日志按钮状态
        /// </summary>
        private Visibility issueLogBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 问题日志按钮状态
        /// </summary>
        public Visibility IssueLogBtnVisibility
        {
            get { return issueLogBtnVisibility; }
            set
            {
                issueLogBtnVisibility = value;
                RaisePropertyChanged("IssueLogBtnVisibility");
            }
        }
        /// <summary>
        /// 设计按钮状态
        /// </summary>
        private Visibility deviseBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 设计按钮状态
        /// </summary>
        public Visibility DeviseBtnVisibility
        {
            get { return deviseBtnVisibility; }
            set
            {
                deviseBtnVisibility = value;
                RaisePropertyChanged("DeviseBtnVisibility");
            }
        }
        /// <summary>
        /// 传出区下载按钮状态
        /// </summary>
        private Visibility outPutDownBtnVisibility = Visibility.Collapsed;
        /// <summary>
        /// 传出区下载按钮状态
        /// </summary>
        public Visibility OutPutDownBtnVisibility
        {
            get { return outPutDownBtnVisibility; }
            set
            {
                outPutDownBtnVisibility = value;
                RaisePropertyChanged("OutPutDownBtnVisibility");
            }
        }
        #endregion

    }
}
