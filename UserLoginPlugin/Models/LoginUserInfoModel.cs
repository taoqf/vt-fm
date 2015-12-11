using Victop.Server.Controls.Models;

namespace UserLoginPlugin.Models
{
    public class LoginUserInfoModel : ModelBase
    {
        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (_UserName != value)
                {
                    _UserName = value;
                    RaisePropertyChanged(()=> UserName);
                }
            }
        }
        private string _UserPwd;

        public string UserPwd
        {
            get { return _UserPwd; }
            set
            {
                if (_UserPwd != value)
                {
                    _UserPwd = value;
                    RaisePropertyChanged(()=> UserPwd);
                }
            }
        }
        private string clientId;

        public string ClientId
        {
            get { return clientId; }
            set { clientId = value; }
        }
        /// <summary>
        /// 企业SOA
        /// </summary>
        private string _SOAIP;
        /// <summary>
        /// 企业SOA
        /// </summary>
        public string SOAIP
        {
            get { return _SOAIP; }
            set
            {
                if (_SOAIP != value)
                {
                    _SOAIP = value;
                    RaisePropertyChanged(()=> SOAIP);
                }
            }
        }
        /// <summary>
        /// MiniSOA
        /// </summary>
        private string _MiniSOAIP;
        /// <summary>
        /// MiniSOA
        /// </summary>
        public string MiniSOAIP
        {
            get { return _MiniSOAIP; }
            set
            {
                if (_MiniSOAIP != value)
                {
                    _MiniSOAIP = value;
                    RaisePropertyChanged(()=> MiniSOAIP);
                }
            }
        }
        
    }

}
