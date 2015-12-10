using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stateless;
using System.Windows;
using System.Windows.Controls;
using Victop.Frame.CmptRuntime;
using AutomaticCodePlugin.Views;
using NRules.Fluent;
using AutomaticCodePlugin.Rules;
using NRules;
using System.Reflection;
using Victop.Server.Controls.Models;

namespace AutomaticCodePlugin.FSM
{
    public class MainStateMachine : BaseStateMachine
    {
        public MainStateMachine() : base("MainPage", Assembly.GetExecutingAssembly())
        {
            FeiDaoFSM.Configure("None")
                .Permit("ViewLoad", "ViewLoaded");
            FeiDaoFSM.Configure("ViewLoaded")
                .OnEntry(() => OnViewLoadedEntry())
                .OnExit(() => OnViewLoadedExit())
                .Permit("SearchBtnClick", "SearchBtnClicked");
            FeiDaoFSM.Configure("SearchBtnClicked")
                .OnEntry(() => OnSearchBtnClickEntry())
                .PermitReentry("SearchBtnClick")
                .OnExit(() => OnSearchBtnClickExit())
                .Permit("SelectRow", "SelectRowed")
                .Permit("AddRow", "AddRowed");
            FeiDaoFSM.Configure("SelectRowed")
                .OnEntry(() => OnSelectedRowChangedEntry())
                .PermitReentry("SelectRow")
                .Permit("SearchBtnClick", "SearchBtnClicked")
                .Permit("AddRow", "AddRowed");
            FeiDaoFSM.Configure("AddRowed")
                .OnEntry(() => OnAddRowedEntry())
                .Permit("SearchBtnClick", "SearchBtnClicked")
                .Permit("SelectRow", "SelectRowed");
        }
        private void OnAddRowedEntry()
        {
            Console.WriteLine("OnAddRowedEntry");
            OAVModel oav = new OAVModel()
            {
                ObjectName = "masterPBlock",
                AtrributeName = "AddData",
                AtrributeValue = ((UCMainView)MainView).MainPBlock.ViewBlockDataTable
            };
            OAVModel oavbtn = new OAVModel()
            {
                ObjectName = "masterPBlock",
                AtrributeName = "addBtn",
                AtrributeValue = ((UCMainView)MainView).addBtn
            };
            InsertOAV(oav);
            InsertOAV(oavbtn);
            Fire();
            RetractOAV(oav);
            RetractOAV(oavbtn);
        }

        private void OnSelectedRowChangedEntry()
        {
            Console.WriteLine("OnSelectedRowChangedEntry");
            OAVModel oav = new OAVModel()
            {
                ObjectName = "masterPBlock",
                AtrributeName = "selectedItem",
                AtrributeValue = ((UCMainView)MainView).dgridProduct.SelectedItem
            };
            OAVModel oavP = new OAVModel()
            {
                ObjectName = "masterPBlock",
                AtrributeName = "PBlock",
                AtrributeValue = ((UCMainView)MainView).MainPBlock
            };
            InsertOAV(oav);
            InsertOAV(oavP);
            Fire();
            InsertOAV(oavP);
            RetractOAV(oav);
        }
        private void OnSearchBtnClickExit()
        {
            Console.WriteLine("OnSearchBtnClickExit");
        }

        private void OnSearchBtnClickEntry()
        {
            Console.WriteLine("OnSearchBtnClickEntry");
            OAVModel oav = new OAVModel()
            {
                ObjectName = "masterPBlock",
                AtrributeName = "GetData",
                AtrributeValue = ((UCMainView)MainView).MainPBlock
            };
            InsertOAV(oav);
            Fire();
            RetractOAV(oav);
        }

        private void OnViewLoadedExit()
        {
            Console.WriteLine("OnViewLoadedExit");
        }

        private void OnViewLoadedEntry()
        {
            OAVModel oav = new OAVModel()
            {
                ObjectName = "UCMaster",
                AtrributeName = "UserControl",
                AtrributeValue = MainView
            };
            InsertOAV(oav);
            Fire();
            RetractOAV(oav);
        }
    }
}
