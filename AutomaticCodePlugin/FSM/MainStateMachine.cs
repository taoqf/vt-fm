using System;
using Victop.Frame.CmptRuntime;
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
                .PermitIf("AddRow", "AddRowed", () => CheckSource());
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

        private bool CheckSource()
        {
            return MainView.GetPresentationBlockModel("masterPBlock").ViewBlockDataTable != null;
        }

        private void OnAddRowedEntry()
        {
            Console.WriteLine("OnAddRowedEntry");
            Fire(new OAVModel("masterPBlock", "AddData", MainView.GetPresentationBlockModel("masterPBlock").ViewBlockDataTable),
           new OAVModel("masterPBlock", "addBtn", MainView.FindName("addBtn")));
        }

        private void OnSelectedRowChangedEntry()
        {
            Console.WriteLine("OnSelectedRowChangedEntry");
            Fire(new OAVModel("masterPBlock", "selectedItem", MainView.FindName("dgridProduct")),
                new OAVModel("masterPBlock", "PBlock", MainView.GetPresentationBlockModel("masterPBlock")));
        }
        private void OnSearchBtnClickExit()
        {
            Console.WriteLine("OnSearchBtnClickExit");
        }

        private void OnSearchBtnClickEntry()
        {
            Console.WriteLine("OnSearchBtnClickEntry");
            Fire(new OAVModel("masterPBlock", "GetData", MainView.GetPresentationBlockModel("masterPBlock")));
        }

        private void OnViewLoadedExit()
        {
            Console.WriteLine("OnViewLoadedExit");
        }

        private void OnViewLoadedEntry()
        {
            Fire(new OAVModel("UCMaster", "UserControl", MainView));
        }
    }
}
