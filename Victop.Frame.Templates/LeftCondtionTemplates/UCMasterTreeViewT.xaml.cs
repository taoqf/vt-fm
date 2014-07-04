using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Victop.Frame.Templates.LeftCondtionTemplates
{
    /// <summary>
    /// UCTreeViewCondtionT.xaml 的交互逻辑
    /// </summary>
    public partial class UCTreeViewCondtionT : UserControl
    {
        #region 变量
        private string systemId;
        private string masterName;
        private string DataChannelId;
        private string id;
        private string pid;
        private string displayField;
        private DataRowView treeViewDrvSelectItem;
        #endregion

        #region 属性
        /// <summary>
        /// 系统编号
        /// </summary>
        public string SystemId
        {
            get { return systemId; }
            set { systemId = value; }
        }
        /// <summary>
        /// 主档名称
        /// </summary>
        public string MasterName
        {
            get { return masterName; }
            set { masterName = value; }
        }
        /// <summary>
        /// 当前节点id
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 当前节点父id
        /// </summary>
        public string Pid
        {
            get { return pid; }
            set { pid = value; }
        }
        /// <summary>
        /// 节点显示名称
        /// </summary>
        public string DisplayField
        {
            get { return displayField; }
            set { displayField = value; }
        }
        /// <summary>
        /// 当前选中行
        /// </summary>
        public DataRowView TreeViewDrvSelectItem
        {
            get { return treeViewDrvSelectItem; }
            set { treeViewDrvSelectItem = value; }
        }
        #endregion

        #region TreeView委托及事件
        // <summary>
        /// TreeView委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void TreeViewDelegate(object sender, EventArgs e);
        /// <summary>
        /// TreeView选项改变事件。
        /// </summary>
        public event TreeViewDelegate TreeViewSelectedItemChanged;
        #endregion

        #region 无参构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCTreeViewCondtionT()
        {
            InitializeComponent();
        } 
        #endregion

        #region 为TreeView加载数据所需要参数赋值
        /// <summary>
        /// 为TreeView加载数据所需要参数赋值
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="masterName"></param>
        public void InitTreeViewParams(string systemId, string masterName, string id, string pid, string displayField)
        {
            this.systemId = systemId;
            this.masterName = masterName;
            this.id = id;
            this.pid = pid;
            this.displayField = displayField;
        } 
        #endregion

        #region TreeView加载事件
        /// <summary>
        /// TreeView加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                GetDataFromServerForTreeViewByMaster();
            }
        } 
        #endregion

        #region TreeView选择项改变事件
        /// <summary>
        /// TreeView选择项改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeView.SelectedItem!=null)
            {
                TreeViewDrvSelectItem =(DataRowView) treeView.SelectedItem;
            }
            if (TreeViewSelectedItemChanged != null)
            {
                TreeViewSelectedItemChanged(sender, e);
            }
        } 
        #endregion

        #region 获取主档数据示例(获取树型数据)
        #region 获取主档数据示例(获取树型数据)
        /// <summary>
        /// 获取主档数据示例(获取树型数据)
        /// </summary>
        private void GetDataFromServerForTreeViewByMaster()
        {
            try
            {
                PluginMessage pluginMessage = new PluginMessage();
                pluginMessage.SendMessage("", OrganizeMasterRequestMessage(), new System.Threading.WaitCallback(SearchData));
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region 组织主档取数树消息
        /// <summary>
        /// 组织主档取数消息
        /// </summary>
        /// <returns></returns>
        private string OrganizeMasterRequestMessage()
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
            paramsDic.Add("mastername", masterName);
            paramsDic.Add("wheresql", "1=1");
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

        #region 树形取数回调
        /// <summary>
        /// 获取数据
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
                string temp = ex.Message;
            }
        }
        #endregion

        #region 设置界面树形数据
        /// <summary>
        /// 设置界面数据
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateTableList(object ds)
        {
            if (ds != null && ((DataSet)ds).Tables.Contains("masterdata"))
            {
                treeView.FIDField = Pid;
                treeView.IDField = Id;
                treeView.DisplayField =DisplayField;
                treeView.ItemsSource = ((DataSet)ds).Tables["masterdata"].DefaultView;
            }
        }
        #endregion 
        #endregion
    }
}
