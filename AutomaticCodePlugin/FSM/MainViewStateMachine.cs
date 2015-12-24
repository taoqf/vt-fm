using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Victop.Frame.CmptRuntime;
using Victop.Server.Controls.Models;

namespace AutomaticCodePlugin.FSM
{
    public class MainViewStateMachine : BaseStateMachine
    {
        public MainViewStateMachine(TemplateControl mainView) : base("MainView", Assembly.GetExecutingAssembly(),mainView)
        {
            FeiDaoFSM.Configure("None")
                .Permit("Load", "Loaded");
            FeiDaoFSM.Configure("Loaded")
                 .OnEntry(() => OnLoadedEntry())
                 .OnExit(() => OnLoadedExit())
                 .Permit("Search", "Searched");
            FeiDaoFSM.Configure("Searched")
                .OnEntry(() => OnSearchedEntry())
                .OnExit(() => OnSearchedExit())
                .PermitReentry("Search")
                .Permit("Add", "Added");
            FeiDaoFSM.Configure("Added")
                .OnEntry(() => OnAddedEntry());
        }
        private void OnAddedEntry()
        {
            Console.WriteLine("MainView:OnAddedEntry");
            Fire(new OAVModel("MainView", "GridControl", "ucdgrid"),
                new OAVModel("MainView", "State", "Add"));
        }

        private void OnSearchedExit()
        {
            Console.WriteLine("MainView:OnSearchedExit");
        }

        private void OnSearchedEntry()
        {
            Console.WriteLine("MainView:OnSearchedEntry");
            Fire(new OAVModel("MainView", "GridControl", "ucdgrid"),
                new OAVModel("MainView", "State", "Search"));
        }

        private void OnLoadedExit()
        {
            Console.WriteLine("MainView:OnLoadedExit");
        }

        private void OnLoadedEntry()
        {
            Console.WriteLine("MainView:OnLoadedEntry");
        }
    }
}
