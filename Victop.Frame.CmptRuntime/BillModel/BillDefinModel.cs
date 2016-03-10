using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace Victop.Frame.CmptRuntime
{
    public class BillDefinModel : PropertyModelBase
    {
        /// <summary>
        /// 主键
        /// </summary>
        private string id;
        /// <summary>
        /// 主键
        /// </summary>
        [JsonProperty(PropertyName = "_id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 功能号
        /// </summary>
        private string formId;
        /// <summary>
        /// 功能号
        /// </summary>
        [JsonProperty(PropertyName = "formid")]
        public string FormId
        {
            get { return formId; }
            set { formId = value; }
        }

        /// <summary>
        /// ControlID
        /// </summary>
        private string controlID;
        /// <summary>
        /// ControlID
        /// </summary>
        [JsonProperty(PropertyName = "ControlID")]
        public string ControlID
        {
            get { return controlID; }
            set { controlID = value; }
        }

        /// <summary>
        /// 功能名称
        /// </summary>
        private string formName;
        /// <summary>
        /// 功能名称
        /// </summary>
        [JsonProperty(PropertyName = "formname")]
        public string FormName
        {
            get { return formName; }
            set { formName = value; }
        }

        /// <summary>
        /// 说明
        /// </summary>
        private string instructions;
        /// <summary>
        /// 说明
        /// </summary>
        [JsonProperty(PropertyName = "instructions")]
        public string Instructions
        {
            get { return instructions; }
            set { instructions = value; }
        }

        /// <summary>
        /// 确认前单据状态
        /// </summary>
        private string predocstatus;
        /// <summary>
        /// 确认前单据状态
        /// </summary>
        [JsonProperty(PropertyName = "predocstatus")]
        public string Predocstatus
        {
            get { return predocstatus; }
            set { predocstatus = value; }
        }
        
        /// <summary>
        /// 单据状态
        /// </summary>
        private string docstatus;
        /// <summary>
        /// 单据状态
        /// </summary>
        [JsonProperty(PropertyName = "docstatus")]
        public string Docstatus
        {
            get { return docstatus; }
            set { docstatus = value; }
        }

        /// <summary>
        /// 日期
        /// </summary>
        private int days;
        /// <summary>
        /// 日期
        /// </summary>
        [JsonProperty(PropertyName = "days")]
        public int Days
        {
            get { return days; }
            set { days = value; }
        }

        /// <summary>
        /// 是否审核
        /// </summary>
        private int isAudit;
        /// <summary>
        /// 是否审核
        /// </summary>
        [JsonProperty(PropertyName = "is_audit")]
        public int IsAudit
        {
            get { return isAudit; }
            set { isAudit = value; }
        }

        /// <summary>
        /// 是否新单
        /// </summary>
        private string disableNewBill;
        /// <summary>
        /// 是否新单
        /// </summary>
        [JsonProperty(PropertyName = "disable_new_bill")]
        public string DisableNewBill
        {
            get { return disableNewBill; }
            set { disableNewBill = value; }
        }

        /// <summary>
        /// 抽象字段
        /// </summary>
        private string abstractField;
        /// <summary>
        /// 抽象字段
        /// </summary>
        [JsonProperty(PropertyName = "abstract_field")]
        public string AbstractField
        {
            get { return abstractField; }
            set { abstractField = value; }
        }

        /// <summary>
        /// 产品ID
        /// </summary>
        private string productid;
        /// <summary>
        /// 产品
        /// </summary>
        [JsonProperty(PropertyName = "productid")]
        public string Productid
        {
            get { return productid; }
            set { productid = value; }
        }

        /// <summary>
        /// 服务名称
        /// </summary>
        private string serverName;
        /// <summary>
        /// 服务名称
        /// </summary>
        [JsonProperty(PropertyName = "servername")]
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        /// <summary>
        /// cdt参数
        /// </summary>
        private string cdtparam;
        /// <summary>
        /// cdt参数
        /// </summary>
        [JsonProperty(PropertyName = "cdtparam")]
        public string Cdtparam
        {
            get { return cdtparam; }
            set { cdtparam = value; }
        }

        /// <summary>
        /// 结束点编号
        /// </summary>
        private string endpointNo;
        /// <summary>
        /// 结束点编号
        /// </summary>
        [JsonProperty(PropertyName = "endpoint_no")]
        public string EndpointNo
        {
            get { return endpointNo; }
            set { endpointNo = value; }
        }

        /// <summary>
        /// 架构图编号
        /// </summary>
        private string structureNo;
        /// <summary>
        /// 架构图编号
        /// </summary>
        [JsonProperty(PropertyName = "structure_no")]
        public string StructureNo
        {
            get { return structureNo; }
            set { structureNo = value; }
        }

        /// <summary>
        /// 包编号
        /// </summary>
        private string packageNo;
        /// <summary>
        /// 包编号
        /// </summary>
        [JsonProperty(PropertyName = "package_no")]
        public string PackageNo
        {
            get { return packageNo; }
            set { packageNo = value; }
        }

        /// <summary>
        /// 数据模式插件dll
        /// </summary>
        private string fileUrl;
        /// <summary>
        /// 数据模式插件dll
        /// </summary>
        [JsonProperty(PropertyName = "file_url")]
        public string FileUrl
        {
            get { return fileUrl; }
            set { fileUrl = value; }
        }

        /// <summary>
        /// 单据状态分类
        /// </summary>
        private object gsdocstate;
        /// <summary>
        /// 单据状态分类
        /// </summary>
        [JsonProperty(PropertyName = "gsdocstate")]
        public object Gsdocstate
        {
            get { return gsdocstate; }
            set { gsdocstate = value; }
        }

        /// <summary>
        /// 编码服务
        /// </summary>
        private object encodedService;
        /// <summary>
        /// 编码服务
        /// </summary>
        [JsonProperty(PropertyName = "encoded_service")]
        public object EncodedService
        {
            get { return encodedService; }
            set { encodedService = value; }
        }

        /// <summary>
        /// 动态字段
        /// </summary>
        private object dynamicFields;
        /// <summary>
        /// 动态字段
        /// </summary>
        [JsonProperty(PropertyName = "dynamic_fields")]
        public object DynamicFields
        {
            get { return dynamicFields; }
            set { dynamicFields = value; }
        }
    }
}
