using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MachinePlatformPlugin.Models
{
    public class CabinetInfoModel
    {
        /// <summary>
        /// 机台Id
        /// </summary>
        private string cabinetId;
        /// <summary>
        /// 机台Id
        /// </summary>
        public string CabinetId
        {
            get { return cabinetId; }
            set { cabinetId = value; }
        }
        /// <summary>
        /// 机台编号
        /// </summary>
        private string cabinetCode;
        /// <summary>
        /// 机台编号
        /// </summary>
        public string CabinetCode
        {
            get { return cabinetCode; }
            set { cabinetCode = value; }
        }
        /// <summary>
        /// 机台名称
        /// </summary>
        private string cabinetName;
        /// <summary>
        /// 机台名称
        /// </summary>
        public string CabinetName
        {
            get { return cabinetName; }
            set { cabinetName = value; }
        }
        /// <summary>
        /// 机台开始状态
        /// </summary>
        private int cabinetBeginState;
        /// <summary>
        /// 机台开始状态
        /// </summary>
        public int CabinetBeginState
        {
            get { return cabinetBeginState; }
            set { cabinetBeginState = value; }
        }
        /// <summary>
        /// 机台结束状态
        /// </summary>
        private int cabinetEndState;
        /// <summary>
        /// 机台结束状态
        /// </summary>
        public int CabinetEndState
        {
            get { return cabinetEndState; }
            set { cabinetEndState = value; }
        }
        /// <summary>
        /// 机台人员Id
        /// </summary>
        private string cabinetUserId;
        /// <summary>
        /// 机台人员Id
        /// </summary>
        public string CabinetUserId
        {
            get { return cabinetUserId; }
            set { cabinetUserId = value; }
        }
        /// <summary>
        /// 机台人员名称
        /// </summary>
        private string cabinetUserName;
        /// <summary>
        /// 机台人员名称
        /// </summary>
        public string CabinetUserName
        {
            get { return cabinetUserName; }
            set { cabinetUserName = value; }
        }
        /// <summary>
        /// 人员Id
        /// </summary>
        private string userId;
        /// <summary>
        /// 人员名称
        /// </summary>
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        /// <summary>
        /// 人员编号
        /// </summary>
        private string userCode;
        /// <summary>
        /// 人员编号
        /// </summary>
        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; }
        }
        /// <summary>
        /// 人员名称
        /// </summary>
        private string userName;
        /// <summary>
        /// 人员名称
        /// </summary>

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        /// <summary>
        /// 机台任务状态集合
        /// </summary>
        private DataTable cabinetTaskStateDt;
        /// <summary>
        /// 机台任务状态集合
        /// </summary>
        public DataTable CabinetTaskStateDt
        {
            get
            {
                if (cabinetTaskStateDt == null)
                    cabinetTaskStateDt = new DataTable();
                return cabinetTaskStateDt;
            }
            set { cabinetTaskStateDt = value; }
        }

        /// <summary>
        /// 机台参数集合
        /// </summary>
        private List<Dictionary<string, object>> cabinetParamsList;
        /// <summary>
        /// 机台参数集合
        /// </summary>
        public List<Dictionary<string, object>> CabinetParamsList
        {
            get
            {
                if (cabinetParamsList == null)
                    cabinetParamsList = new List<Dictionary<string, object>>();
                return cabinetParamsList;
            }
            set { cabinetParamsList = value; }
        }
    }
}
