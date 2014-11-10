using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MachinePlatformPlugin.Models
{
    /// <summary>
    /// 机台信息实体
    /// </summary>
    public class CabinetInfoModel
    {
        /// <summary>
        /// SystemId
        /// </summary>
        private string systemId;
        /// <summary>
        /// SystemId
        /// </summary>
        public string SystemId
        {
            get
            {
                return systemId;
            }
            set
            {
                systemId = value;
            }
        }
        /// <summary>
        /// configSystemId
        /// </summary>
        private string configSystemId;
        /// <summary>
        /// ConfigSystemId
        /// </summary>
        public string ConfigSystemId
        {
            get { return configSystemId; }
            set { configSystemId = value; }
        }
        /// <summary>
        /// spaceId
        /// </summary>
        private string spaceId;
        /// <summary>
        /// SpaceId
        /// </summary>
        public string SpaceId
        {
            get { return spaceId; }
            set { spaceId = value; }
        }
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
        /// <summary>
        /// 机台配置数据
        /// </summary>
        private List<Dictionary<string, object>> cabinetFitData;
        /// <summary>
        /// 机台配置数据
        /// </summary>
        public List<Dictionary<string, object>> CabinetFitData
        {
            get { return cabinetFitData; }
            set { cabinetFitData = value; }
        }
        /// <summary>
        /// 机台CAD名称
        /// </summary>
        private string cabinetCADName;
        /// <summary>
        /// 机台CAD名称
        /// </summary>
        public string CabinetCADName
        {
            get { return cabinetCADName; }
            set { cabinetCADName = value; }
        }
    }
}
