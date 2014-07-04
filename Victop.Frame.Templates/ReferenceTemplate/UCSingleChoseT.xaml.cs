using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Victop.Frame.DataChannel;
using Victop.Frame.MessageManager;
using Victop.Frame.PublicLib.Helpers;

namespace Victop.Frame.Templates.ReferenceTemplate
{
    /// <summary>
    /// 数据引用用户控件的交互逻辑
    /// </summary>
    public partial class UCSingleChoseT : UserControl
    {
        #region 无参构造函数
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public UCSingleChoseT()
        {
            InitializeComponent();
        }
        #endregion 

        #region 变量

        private string systemId = "";     
        private string modelId = "";
        private string fieldName = "";
        private string dataChannelId = "";
        private string datasourcetype = "";
        private string datasourcename = "";
        private string bsname = ""; //数据集名称
        private string isdata = "";
        private DataTable dtDataParm = null;
        /// <summary>
        /// 点击“确定”，返回的结果
        /// </summary>
        private Dictionary<string, string> returnResult;
        /// <summary>
        /// 返回列
        /// </summary>
        private List<Dictionary<string, string>> fieldreturn;
        /// <summary>
        /// 数据引用数据集
        /// </summary>
        private DataTable dtData = null;
        /// <summary>
        /// 委托类型
        /// </summary>
        /// <param name="sender">“确定”对象</param>
        /// <param name="_returnResult">返回结果字典数据</param>
        public delegate void ReferenceResult(object sender,Dictionary<string, string> _returnResult);
        /// <summary>
        /// 委托事件（返回结果）
        /// </summary>
        public event ReferenceResult RefrenceResultClick;
        #endregion 

        #region 属性
        /// <summary>
        /// SystemId
        /// </summary>
        public string SystemId
        {
            get { return systemId; }
            set { systemId = value; }
        }
        /// <summary>
        /// ModelId
        /// </summary>
        public string ModelId
        {
            get { return modelId; }
            set { modelId = value; }
        }
        /// <summary>
        /// 列名关键字
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }
        /// <summary>
        /// 数据通道Id
        /// </summary>
        public string DataChannelId
        {
            get { return dataChannelId; }
            set { dataChannelId = value; }
        }
        /// <summary>
        /// 数据引用数据集
        /// </summary>
        public DataTable DtDataParm
        {
            get { return dtDataParm; }
            set { dtDataParm = value; }
        }   
        #endregion         

        #region 方法

        #region 数据引用初始化事件
        /// <summary>
        /// 数据引用初始化事件
        /// </summary>
        /// <param name="_systemId">系统ID</param>
        /// <param name="_modelId">插件ID</param>
        /// <param name="_fieldName">列名关键字</param>
        /// <param name="_dataChannelId">数据通道ID</param>
        /// <param name="_dtGmDatareference">数据引用数据集</param>
        /// <param name="_referenceResult">返回结果回调函数（委托）</param>
        public void UserControlInit(string _systemId, string _modelId, string _fieldName, string _dataChannelId, DataTable _dtGmDatareference, ReferenceResult _referenceResult) 
        {
            this.SystemId = _systemId;
            this.ModelId = _modelId;
            this.FieldName = _fieldName;
            this.DataChannelId = _dataChannelId;
            this.DtDataParm = _dtGmDatareference;
            this.RefrenceResultClick += _referenceResult;
            this.VerticalAlignment = VerticalAlignment.Stretch;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
        }
        #endregion 

        #region 获取返回列
        /// <summary>
        /// 获取返回列
        /// </summary>
        private void GetFieldReturn() 
        {
            if(!string.IsNullOrEmpty(FieldName) && DtDataParm !=null && DtDataParm.Rows.Count >0)
            {
                foreach(DataRow dr in DtDataParm.Rows)
                {
                    if (dr["columnid"].ToString().ToLower() == FieldName.ToLower()) 
                    {
                        fieldreturn = JsonHelper.ToObject<List<Dictionary<string, string>>>(dr["fieldreturn"].ToString().Replace(";top;", "\""));
                    }
                }
            }
        }
        #endregion 

        #region 获取数据引用参数
        /// <summary>
        /// 获取数据引用参数
        /// </summary>
        private void GetDataParm() 
        {
            DataRow[] drs = DtDataParm.Select(" columnid='" + FieldName + "'");
            if (drs.Length > 0)
            {
                bsname = drs[0]["bsname"].ToString();
                datasourcetype = drs[0]["datasourcetype"].ToString();
                datasourcename = drs[0]["datasourcename"].ToString();
                isdata = JsonHelper.ReadJsonString(drs[0]["operation"].ToString().Replace(";top;", "\""), "isdata");
            }
        }
        #endregion 

        #region 用户控件加载事件
        /// <summary>
        /// 用户控件加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DtDataParm = new DataTable();
                if (DtDataParm != null && DtDataParm.Rows.Count > 0)
                {
                    GetFieldReturn();
                    GetDataParm();
                }
                else
                {
                    DataOperation operateData = new DataOperation();
                    DataSet ds = operateData.GetData(DataChannelId);
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DtDataParm = ds.Tables["gmDatareference"];
                        GetFieldReturn();
                        GetDataParm();
                    }
                }
                PluginMessage pluginMessage = new PluginMessage();
                pluginMessage.SendMessage("", ReferenceRequestMessage(), new System.Threading.WaitCallback(SearchData));                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }      
        }        
        #endregion 

        #region 数据引用取数消息
        /// <summary>
        /// 数据引用取数消息
        /// </summary>
        /// <returns></returns>
        private string ReferenceRequestMessage()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "DataChannelService.getFormReferenceSpecial");
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("bzsystemid", SystemId);
            contentDic.Add("modelId", ModelId);
            contentDic.Add("fieldName", FieldName);

            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
            paramsDic.Add("datasourcetype", datasourcetype);
            paramsDic.Add("datasourcename", datasourcename);
            paramsDic.Add("bsname", bsname);
            paramsDic.Add("bsgroupname", null);
            paramsDic.Add("isdata", isdata);
            paramsDic.Add("isbs", null);
            paramsDic.Add("pageno", "-1");

            contentDic.Add("dataparam", paramsDic);

            string content = JsonHelper.ToJson(contentDic);
            messageDic.Add("MessageContent", content);
            string message = JsonHelper.ToJson(messageDic);
            return message;
        }
        #endregion

        #region 数据引用取数回调
        /// <summary>
        /// 请求成功后，根据返回结构向数据通道请求数据
        /// </summary>
        /// <param name="message"></param>
        private void SearchData(object message)
        {
            try
            {
                string searchDataChannelId = JsonHelper.ReadJsonString(message.ToString(), "DataChannelId");
                DataOperation operateData = new DataOperation();

                DataSet ds = operateData.GetData(searchDataChannelId);
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new WaitCallback(UpdateTableList), ds);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 更新数据表列表
        /// <summary>
        /// 更新数据表列表
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateTableList(object ds)
        {
            DataSet tables = (DataSet)ds;
            if (tables != null && tables.Tables.Count > 0)
            {
                dtData = tables.Tables["returndata"];
                dgridCustomerData.ItemsSource = dtData.DefaultView;
                dgridCustomerData.Focus();
            }           
        }
        #endregion

        #region 确认事件
        /// <summary>
        /// 确认事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (dgridCustomerData.SelectedItem != null)
            {
                returnResult = new Dictionary<string, string>();
                foreach(Dictionary<string,string> key in fieldreturn)
                {
                    if(!returnResult.ContainsKey(key["data"]))
                    {
                        string keystr = key["data"].ToLower();
                        DataRowView drv = (DataRowView)dgridCustomerData.SelectedItem;
                        string value = drv[key["linkdata"]].ToString();
                        returnResult.Add(keystr, value);
                    }
                }

                if (RefrenceResultClick != null) 
                {
                    RefrenceResultClick(sender,returnResult);
                }
            }
            else 
            {
                MessageBox.Show("请先选中一条数据！");
            }
        }
        #endregion 

        #region DataGrid双击事件
        /// <summary>
        /// DataGrid双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgridCustomerData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgridCustomerData.SelectedItem != null)
            {
                returnResult = new Dictionary<string, string>();
                foreach (Dictionary<string, string> key in fieldreturn)
                {
                    if (!returnResult.ContainsKey(key["data"]))
                    {
                        string keystr = key["data"].ToLower();
                        DataRowView drv = (DataRowView)dgridCustomerData.SelectedItem;
                        string value = drv[key["linkdata"]].ToString();
                        returnResult.Add(keystr, value);
                    }
                }

                if (RefrenceResultClick != null)
                {
                    RefrenceResultClick(sender, returnResult);
                }
            }
            else
            {
                MessageBox.Show("请先选中一条数据！");
            }
        }
        #endregion       

        #endregion
    }
}
