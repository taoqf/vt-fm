using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;
 


namespace ModifyPassWordPlugin.Models
{
    public class ModifyPassWordModel : ModelBase
    {
        /// <summary>
        /// 原始密码
        /// </summary>
        private string oldUserPwd;
        public string OldUserPwd
        {
            get { return oldUserPwd; }
            set
            {
                if (oldUserPwd != value)
                {
                    oldUserPwd = value;
                    RaisePropertyChanged(()=> OldUserPwd);
                }
            }
        }
        /// <summary>
        /// 新密码
        /// </summary>
        private string newUserPwd;
        public string NewUserPwd
        {
            get { return newUserPwd; }
            set
            {
                if (newUserPwd != value)
                {
                    newUserPwd = value;
                    RaisePropertyChanged(()=> NewUserPwd);
                }
            }
        }
        private string oldUserPrompt = "6-16个字符,区分大小写";
        public string OldUserPrompt
        {
            get { return oldUserPrompt; }
            set
            {
                if (oldUserPrompt != value)
                {
                    oldUserPrompt = value;
                    RaisePropertyChanged(()=> OldUserPrompt);
                }
            }
        }
        private string newUserPrompt = "6-16个字符,区分大小写";
        public string NewUserPrompt
        {
            get { return newUserPrompt; }
            set
            {
                if (newUserPrompt != value)
                {
                    newUserPrompt = value;
                    RaisePropertyChanged(()=> NewUserPrompt);
                }
            }
        }
        private string affirmUserPrompt = "请再次填写密码";
        public string AffirmUserPrompt
        {
            get { return affirmUserPrompt; }
            set
            {
                if (affirmUserPrompt != value)
                {
                    affirmUserPrompt = value;
                    RaisePropertyChanged(()=> AffirmUserPrompt);
                }
            }
        }
        /// <summary>
        /// 确认密码
        /// </summary>
        private string affirmUserPwd;
        public string AffirmUserPwd
        {
            get { return affirmUserPwd; }
            set
            {
                if (affirmUserPwd != value)
                {
                    affirmUserPwd = value;
                    RaisePropertyChanged(()=> AffirmUserPwd);
                }
            }
        }
        public bool affirmIsEnabled=false;
        public bool AffirmIsEnabled
        {
            get { return affirmIsEnabled; }
            set { if(affirmIsEnabled!=value)
            {
                affirmIsEnabled = value;
                RaisePropertyChanged(()=> AffirmIsEnabled);
            }}
        }
        private string systemid;
        public string SystemId
        {
            get { return systemid; }
            set
            {
                if (systemid != value)
                {
                    systemid = value;
                    RaisePropertyChanged(()=> SystemId);
                }
            }
        }

        private string spaceid;
        public string SpaceId
        {
            get { return spaceid; }
            set
            {
                if (spaceid != value)
                {
                    spaceid = value;
                    RaisePropertyChanged(()=> SpaceId);
                }
            }
        }

        private string userCode;
        public string UserCode
        {
            get { return userCode; }
            set
            {
                if (userCode != value)
                {
                    userCode = value;
                    RaisePropertyChanged(()=> UserCode);
                }
            }
        }

        private string edit_Type;
        public string Edit_Type
        {
            get { return edit_Type; }
            set
            {
                if (edit_Type != value)
                {
                    edit_Type = value;
                    RaisePropertyChanged(()=> Edit_Type);
                }
            }
        }
    }
}
