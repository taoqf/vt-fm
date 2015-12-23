using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Victop.Frame.CmptRuntime;
using Victop.Server.Controls.Models;

namespace AutomaticCodePlugin.FSM
{
    public class BtnOpViewStateMachine : BaseStateMachine
    {
        public BtnOpViewStateMachine() : base("BtnOpView", Assembly.GetExecutingAssembly())
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
                .OnEntry(() => OnAddedEntry())
                .OnExit(() => OnAddedExit());
        }

        private void OnAddedExit()
        {
            Console.WriteLine("BtnOpView:OnAddedExit");
        }

        private void OnAddedEntry()
        {
            Console.WriteLine("BtnOpView:OnAddedEntry");
            Fire(new OAVModel("opView", "TemplateControl", MainView),
                new OAVModel("opView", "opName", "AddBtnClick"));
        }

        private void OnSearchedExit()
        {
            Console.WriteLine("BtnOpView:OnSearchedExit");
        }

        private void OnSearchedEntry()
        {
            Console.WriteLine("BtnOpView:OnSearchedEntry");
            Fire(new OAVModel("opView", "TemplateControl", MainView),
                new OAVModel("opView", "opName", "SearchBtnClick"));
        }

        private void OnLoadedExit()
        {
            Console.WriteLine("BtnOpView:OnLoadedExit");
        }

        private void OnLoadedEntry()
        {
            Console.WriteLine("BtnOpView:OnLoadedEntry");
        }
    }
}
