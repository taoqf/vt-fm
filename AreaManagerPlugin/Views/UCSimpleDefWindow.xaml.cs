using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Victop.Frame.DataChannel;
using Victop.Frame.PublicLib.Helpers;
using Victop.Frame.SyncOperation;
using Victop.Server.Controls.WeChat;

namespace AreaManagerPlugin.Views
{
    /// <summary>
    /// UCSimpleDefWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UCSimpleDefWindow : UserControl
    {
        public UCSimpleDefWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool result = WeChatOperation.UserLogin(tboxName.Text.Trim(), tboxPwd.Text.Trim(),tboxImgCode.Text.Trim());
                if (result)
                {
                    WeChatOperation.UpdateAppInfo();
                    MessageBox.Show("登陆成功！");
                    panelLogin.Visibility = Visibility.Collapsed;
                    panelOperation.Visibility = Visibility.Visible;
                }
                else
                {
                    MessageBox.Show("登陆失败");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnChangImgCode_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tboxName.Text.Trim()))
            {
                imgImgCode.Source = WeChatOperation.GetimgeCode(tboxName.Text.Trim());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
        /// <summary>
        /// 获取人员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetUserInfo_Click(object sender, RoutedEventArgs e)
        {
            List<SingleGroup> groupList = WeChatOperation.getAllGroupInfo();
            if (groupList != null && groupList.Count > 0)
            {
                DataTable dt = new DataTable("usergroup");
                dt.Columns.Add("groupname");
                foreach (string item in groupList[0].groupdata[0].Keys)
                {
                    dt.Columns.Add(item);
                }
                foreach (SingleGroup item in groupList)
                {
                    foreach (Dictionary<string,object> dataitem in item.groupdata)
                    {
                        DataRow dr = dt.NewRow();
                        dr["groupname"] = item.group_name;
                        foreach (string useritem in dataitem.Keys)
                        {
                            dr[useritem] = dataitem[useritem];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                datagridInfo.ItemsSource = dt.DefaultView;
            }
        }
        /// <summary>
        /// 获取菜单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetMenuInfo_Click(object sender, RoutedEventArgs e)
        {
            string MenuInfo = WeChatOperation.GetMenuInfo();
            tboxMenuInfo.Text = MenuInfo;
            MenuInfo = JsonHelper.ReadJsonString(MenuInfo, "menu");
            List<WeChatMenuItemModel> MenuList = JsonHelper.ToObject<List<WeChatMenuItemModel>>(JsonHelper.ReadJsonString(MenuInfo, "button"));
        }

        private void tboxName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tboxName.Text.Trim()))
            {
                imgImgCode.Source = WeChatOperation.GetimgeCode(tboxName.Text.Trim());
            }
        }

        private void btnCreateMenuInfo_Click(object sender, RoutedEventArgs e)
        {
            List<WeChatMenuItemModel> menuList = new List<WeChatMenuItemModel>();
            WeChatMenuItemModel menuModel1 = new WeChatMenuItemModel() {
            Name="简介",
            Type=WeChatMenuItemTypeEnum.CLICK.ToString().ToLower(),
            Key="jj",
            };
            WeChatMenuItemModel menuModel2 = new WeChatMenuItemModel()
            {
                Name = "机构",
                Type = WeChatMenuItemTypeEnum.CLICK.ToString().ToLower(),
                Key = "jg",
            };
            WeChatMenuItemModel menuModel3 = new WeChatMenuItemModel()
            {
                Name = "搜索",
                Type = WeChatMenuItemTypeEnum.CLICK.ToString().ToLower(),
                Key = "ss",
            };
            menuList.Add(menuModel1);
            menuList.Add(menuModel2);
            menuList.Add(menuModel3);
            string resultStr = WeChatOperation.CreateMenuInfo(menuList);
            if (!string.IsNullOrEmpty(resultStr))
            {
                WeChatAPIReturnModel returnModel = JsonHelper.ToObject<WeChatAPIReturnModel>(resultStr);
                MessageBox.Show(returnModel.ErrorMsg);
            }
        }

        private void btnDelMenuInfo_Click(object sender, RoutedEventArgs e)
        {
            string resultStr = WeChatOperation.DelMenuInfo();
            if (!string.IsNullOrEmpty(resultStr))
            {
                WeChatAPIReturnModel returnModel = JsonHelper.ToObject<WeChatAPIReturnModel>(resultStr);
                MessageBox.Show(returnModel.ErrorMsg);
            }
        }

        private void btnGetGroupInfo_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            dt.Columns.Add("count");
            string resultStr = WeChatOperation.GetGroupInfo();
            List<Dictionary<string, object>> groupList = JsonHelper.ToObject<List<Dictionary<string, object>>>(JsonHelper.ReadJsonString(resultStr, "groups"));
            foreach (Dictionary<string,object> item in groupList)
            {
                DataRow dr = dt.NewRow();
                dr["id"] = item["id"];
                dr["name"] = item["name"];
                dr["count"] = item["count"];
                dt.Rows.Add(dr);
            }
            datagridInfo.ItemsSource = dt.DefaultView;
        }

        private void btnGetGroupUserInfo_Click(object sender, RoutedEventArgs e)
        {
            string resultStr = WeChatOperation.GetUserInfo();
        }

    }
}
