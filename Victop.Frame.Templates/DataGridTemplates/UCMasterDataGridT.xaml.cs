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

namespace Victop.Frame.Templates.DataGridTemplates
{
    /// <summary>
    /// 主档DataGrid的交互逻辑
    /// </summary>
    public partial class UCMasterDataGridT : UserControl
    {
        #region 变量
        /// <summary>
        /// 系统ID
        /// </summary>
        private string systemId = "905";        
        /// <summary>
        /// 功能号
        /// </summary>
        //private string formId = "10402";        
        /// <summary>
        /// 模型ID
        /// </summary>
        //private string modelId = "供应商柜台管理";        
        /// <summary>
        /// 数据通道ID
        /// </summary>
        private string DataChannelId = string.Empty;
        /// <summary>
        /// 数据集合
        /// </summary>
        private DataTable dtData;
        /// <summary>
        /// 每次查询后数据的数量
        /// </summary>
        private int dataNum;
        /// <summary>
        /// 主档名称
        /// </summary>
        private string masterName = "供应商柜台管理表";        
        /// <summary>
        /// 取消按钮置灰状态
        /// </summary>
        private bool cannelFlag = false;
        /// <summary>
        /// 查询条件
        /// </summary>
        private string sqlFilter = " 1=1 ";
        #endregion

        #region 无参构造函数
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public UCMasterDataGridT()
        {
            InitializeComponent();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemId
        {
            get { return systemId; }
            set { systemId = value; }
        }

        /// <summary>
        /// 功能号
        /// </summary>
        //public string FormId
        //{
        //    get { return formId; }
        //    set { formId = value; }
        //}

        /// <summary>
        /// 模型ID
        /// </summary>
        //public string ModelId
        //{
        //    get { return modelId; }
        //    set { modelId = value; }
        //}

        /// <summary>
        /// 主档名称
        /// </summary>
        public string MasterName
        {
            get { return masterName; }
            set { masterName = value; }
        }
        
        /// <summary>
        /// 取消按钮置灰状态
        /// </summary>
        public bool CannelFlag
        {
            get { return cannelFlag; }
            set { cannelFlag = value; }
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string SqlFilter
        {
            get { return sqlFilter; }
            set { sqlFilter = value; }
        }
        #endregion

        public delegate void ButtonDelegate(object sender, DataRow dr);
        public event ButtonDelegate AddClick;

        #region 方法

        #region DataGrid初始化
        /// <summary>
        /// DataGrid初始化
        /// </summary>
        /// <param name="_systemId"></param>
        /// <param name="_formId"></param>
        /// <param name="_modelId"></param>
        /// <param name="_masterName"></param>
        /// <param name="_sqlFilter"></param>
        public void UserControlInit(string _systemId,string _masterName,string _sqlFilter) 
        {
            this.SystemId = _systemId;
            //this.FormId = _formId;
            //this.ModelId = _modelId;
            this.MasterName = _masterName;
            this.SqlFilter = _sqlFilter;
            this.VerticalAlignment = VerticalAlignment.Stretch;
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
        }
        #endregion 

        #region 用户控件Load事件
        /// <summary>
        /// 用户控件Load事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetDataFromServer();
        }
        #endregion 

        #region 获取业务数据
        /// <summary>
        /// 获取业务数据
        /// </summary>
        private void GetDataFromServer()
        {
            PluginMessage pluginMessage = new PluginMessage();
            pluginMessage.SendMessage("", MasterRequestMessage(), new System.Threading.WaitCallback(SearchData));
        }
        #endregion

        #region 主档取数消息
        /// <summary>
        /// 主档取数消息
        /// </summary>
        /// <returns></returns>
        private string MasterRequestMessage()
        {
            Dictionary<string, string> messageDic = new Dictionary<string, string>();
            messageDic.Add("MessageType", "DataChannelService.getMasterPropDataAsync");
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("openType", null);
            contentDic.Add("bzsystemid", SystemId);
            contentDic.Add("formid", null);
            contentDic.Add("dataSetID", null);
            contentDic.Add("reportID", null);
            contentDic.Add("modelId", null);
            contentDic.Add("fieldName", null);
            contentDic.Add("masterOnly", "false");
            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
            paramsDic.Add("isdata", "0");
            paramsDic.Add("mastername", MasterName);            
            paramsDic.Add("wheresql", SqlFilter);

            paramsDic.Add("pageno", "-1");
            paramsDic.Add("prooplist", null);
            paramsDic.Add("proplisted", null);
            paramsDic.Add("dataed", null);
            paramsDic.Add("ispage", "1");
            paramsDic.Add("getset", "1");
            contentDic.Add("dataparam", paramsDic);
            contentDic.Add("whereArr", null);
            contentDic.Add("masterParam", null);
            contentDic.Add("deltaXml", null);
            contentDic.Add("runUser", "test4");
            contentDic.Add("shareFlag", null);
            contentDic.Add("treeStr", null);
            contentDic.Add("saveType", null);
            contentDic.Add("doccode", null);
            contentDic.Add("clientId", "byerp");
            string content = JsonHelper.ToJson(contentDic);
            messageDic.Add("MessageContent", content);
            return JsonHelper.ToJson(messageDic);
        }
        #endregion

        #region 主档/模型/业务规则 取数回调
        /// <summary>
        /// 请求成功后，根据返回结构向数据通道请求数据
        /// </summary>
        /// <param name="message"></param>
        private void SearchData(object message)
        {
            try
            {
                DataChannelId = JsonHelper.ReadJsonString(message.ToString(), "DataChannelId");
                DataOperation operateData = new DataOperation();

                DataSet ds = operateData.GetData(DataChannelId);
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
                dtData = tables.Tables["masterdata"];
                //dtGmDatareference = tables.Tables["gmDatareference"];
                dataNum = dtData.Rows.Count;
                dgrid.ItemsSource = dtData.DefaultView;
                if (dtData == null || dtData.Rows.Count == 0)
                {
                    MessageBox.Show("没有查询到相关的数据！");
                }
            }
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        private void Add()
        {
            DataRow dr = dtData.NewRow();
            //dr["vndcntguid"] = Guid.NewGuid();
            //dr["cntcode"] = Guid.NewGuid();
            //dr["id"] = Guid.NewGuid();            
            //dr["actived"] = "1";                    //是否生效（默认值：是）
            if (AddClick !=null)
            {
                AddClick(null,dr);
            }
            dtData.Rows.Add(dr);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            if (dgrid.SelectedItem == null)
            {
                MessageBox.Show("请选择一条需要删除的数据！");
                return;
            }
            DataRow dr = dtData.Rows[dtData.Rows.IndexOf(((DataRowView)dgrid.SelectedItem).Row)];
            dr.Delete();
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        private string MasterSaveRequestMessage()
        {
            Dictionary<string, object> messageDic = new Dictionary<string, object>();
            messageDic.Add("MessageType", "DataChannelService.getSaveMasterDataAsync");
            Dictionary<string, object> contentDic = new Dictionary<string, object>();
            contentDic.Add("DataChannelId", DataChannelId);
            Dictionary<string, string> paramsDic = new Dictionary<string, string>();
            paramsDic.Add("isdata", "1");
            paramsDic.Add("mastername", MasterName);
            paramsDic.Add("wheresql", "1=1");
            paramsDic.Add("pageno", "-1");
            paramsDic.Add("prooplist", null);
            paramsDic.Add("proplisted", null);
            paramsDic.Add("dataed", null);
            paramsDic.Add("ispage", "1");
            paramsDic.Add("getset", "1");
            contentDic.Add("dataparam", paramsDic);
            string content = JsonHelper.ToJson(contentDic);
            messageDic.Add("MessageContent", content);
            return JsonHelper.ToJson(messageDic);
        }
        #endregion

        #region 保存请求成功后，根据返回结构向数据通道请求数据
        /// <summary>
        /// 保存请求成功后，根据返回结构向数据通道请求数据
        /// </summary>
        /// <param name="message"></param>
        private void SaveData(object message)
        {
            try
            {
                MessageBox.Show(message.ToString());
                //Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new WaitCallback(UpdateTableList), ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 取消
        /// <summary>
        /// 取消
        /// </summary>
        public void Cancle()
        {
            if (dtData.Rows.Count > dataNum)
            {
                dtData.Rows.Remove(dtData.Rows[dtData.Rows.Count - 1]);
                if (dtData.Rows.Count == dataNum) { CannelFlag = false; }
            }
        }
        #endregion

        #region 全消
        /// <summary>
        /// 全消
        /// </summary>
        //private void AllCancle()
        //{
        //    dtData.RejectChanges();
        //    CannelFlag = false;
        //    for (int i = 0; i < dtData.Rows.Count; i++)
        //    {
        //        dtData.Rows[i]["select"] = false;
        //    }
        //}
        #endregion

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        public void Search()
        {
            GetDataFromServer();
        }
        #endregion

        #endregion

    }
}
