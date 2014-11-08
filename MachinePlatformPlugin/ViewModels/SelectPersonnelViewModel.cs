using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using MachinePlatformPlugin.Views;
using Victop.Frame.Components;
using Victop.Frame.PublicLib.Helpers;
using Victop.Server.Controls.Models;
using Victop.Server.Controls.Runtime;
using Victop.Wpf.Controls;
namespace MachinePlatformPlugin.ViewModels
{
   public class SelectPersonnelViewModel:ModelBase
    {

        #region 字段
        /// <summary>
        /// 用户控件
        /// </summary>
        private VicWindowNormal winChooseStaff;
        /// <summary>
        /// 选择人员DataGrid
        /// </summary>
        private CompntSingleDataGrid comdgrid;
        private VicTextBoxNormal tboxName;
        /// <summary>
        /// 选中派工人员
        /// </summary>
        public static string staffName;
        public static string guid;
        public static bool chooseFlag = false;//默认未选中任何人进行派工
        #endregion

        #region 属性
        #endregion

        #region 命令

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public ICommand ChooseStaffViewLoadedCommand
        {
            get
            {
                return new RelayCommand<object>((x) =>
                {
                    try
                    {
                        winChooseStaff = (VicWindowNormal)x;
                        comdgrid = (CompntSingleDataGrid)winChooseStaff.FindName("comdgridSaff");
                        tboxName = (VicTextBoxNormal)winChooseStaff.FindName("tboxStaffNname");

                        //注册事件
                        comdgrid.MouseLeftButtonUp += comdgrid_MouseLeftButtonUp;

                        comdgrid.CompntDataGridParams.SystemId = UCMachinePlatform.ParamDict["systemid"].ToString(); ;
                        comdgrid.CompntDataGridParams.SettingModel = JsonHelper.ToObject<CompntSettingModel>(FileHelper.ReadFitData("CompntSingleDataGrid_ChooseStaff_Cabinet_staff_Settings"));
                        string JsonStr = comdgrid.DoRender();
                        string ResultMess = JsonHelper.ReadJsonString(JsonStr, "ReplyAlertMessage");
                        string ResultMode = (JsonHelper.ReadJsonString(JsonStr, "ReplyMode")).ToString();//键"ReplyMode"的值等于1，表示操作成功；等于0，表示当前操作失败。
                        if (ResultMode != "1" && !string.IsNullOrEmpty(ResultMess))//键"ReplyMode"的值不等于1，则抛出操作失败的原因；反之，程序没有提示
                        {
                            VicMessageBoxNormal.Show(ResultMess, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {

                        }
                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        dict.Add("cabinet_id", UCMachinePlatform.ParamDict["cabinet_id"].ToString());
                        comdgrid.CompntDataGridParams.ConditionList = GetSearchCondition(dict);
                        comdgrid.Search();
                    }
                    catch { }

                });
            }
        }
        #endregion

        #region 确认
        /// <summary>
        /// 确认按钮
        /// </summary>
        public ICommand btnAffirmClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Affirm();
                });
            }
        }

        #endregion

        #region 取消
        /// <summary>
        /// 取消按钮
        /// </summary>
        public ICommand btnCancelClickCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Cancel();
                });
            }
        }

        #endregion

        #endregion

        #region 方法

        #region 确认
        /// <summary>
        /// 确认方法
        /// </summary>
        public void Affirm()
        {
            if (comdgrid.CompntDataGridParams.GridSelectedRow != null)
            {
                chooseFlag = true;                
                winChooseStaff.Close();
            }
            else
            {
                VicMessageBoxNormal.Show("请选择要派工的人员", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        #region 取消
        /// <summary>
        /// 取消方法
        /// </summary>
        public void Cancel()
        {
            MessageBoxResult result = VicMessageBoxNormal.Show("确定要退出么？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                winChooseStaff.Close();
            }
        }
        #endregion

        #region 鼠标移入事件
        /// <summary>
        ///鼠标移入事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void comdgrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            staffName = comdgrid.CompntDataGridParams.GridSelectedRow["cabinet_staff_name"].ToString();
            guid = comdgrid.CompntDataGridParams.GridSelectedRow["_id"].ToString();
            tboxName.VicText = staffName;
        }
        #endregion

        #region 查询条件集合
        /// <summary>
        /// 查询条件集合
        /// </summary>
        /// <param name="dict">查询条件字典，必填</param>
        /// <returns></returns>
        private List<object> GetSearchCondition(Dictionary<string, object> dict)
        {
            List<object> conCitionsList = new List<object>();//查询参数Conditions集合（集合中放置“字段=值”的字典）
            conCitionsList.Add(dict);
            return conCitionsList;
        }
        #endregion
        #endregion
    }
}
