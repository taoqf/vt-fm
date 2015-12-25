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
        public BtnOpViewStateMachine(TemplateControl mainView) : base("BtnOpView", Assembly.GetExecutingAssembly(), mainView)
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
                .OnExit(() => OnAddedExit())
                .Permit("Search", "Searched");
        }
        private void OnAddedExit()
        {
            Console.WriteLine("BtnOpView:OnAddedExit");
            Console.WriteLine("CurrentState:{0}", FeiDaoFSM.State);
        }

        private void OnAddedEntry()
        {
            Console.WriteLine("BtnOpView:OnAddedEntry");
            Fire();
        }

        private void OnSearchedExit()
        {
            Console.WriteLine("BtnOpView:OnSearchedExit");
            Console.WriteLine("CurrentState:{0}", FeiDaoFSM.State);
        }

        private void OnSearchedEntry()
        {
            Console.WriteLine("BtnOpView:OnSearchedEntry");
            Console.WriteLine("CurrentState:{0}", FeiDaoFSM.State);
            Fire();
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
