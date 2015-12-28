using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Victop.Frame.CmptRuntime;
using Victop.Server.Controls.Models;

namespace AutomaticCodePlugin.FSM
{
    public class DataGridViewStateMachine : BaseStateMachine
    {
        public DataGridViewStateMachine(TemplateControl mainView) : base("DataGridView", Assembly.GetExecutingAssembly(),mainView)
        {
            FeiDaoFSM.Configure("None")
                .Permit("Load", "Loaded");
            FeiDaoFSM.Configure("Loaded")
                .OnEntry(() => OnLoadedEntry())
                .OnExit(() => OnLoadeExit())
                .Permit("Search", "Searched");
            FeiDaoFSM.Configure("Searched")
                .OnEntry(() => OnSearchedEntry())
                .OnExit(() => OnSearchExit())
                .PermitReentry("Search")
                .Permit("Add", "Added");
            FeiDaoFSM.Configure("Added")
                .OnEntry(() => OnAddedEntry())
                .OnExit(() => OnAddedExit())
                .Permit("Search", "Searched");
        }

        private void OnAddedExit()
        {
            Console.WriteLine("DataGridView:OnAddedExit");
        }

        private void OnAddedEntry()
        {
            Console.WriteLine("DataGridView:OnAddedEntry");
            Fire(new OAVModel("masterPBlock", "AddData",null));
        }

        private void OnSearchExit()
        {
            Console.WriteLine("DataGridView:OnSearchExit");
        }

        private void OnSearchedEntry()
        {
            Console.WriteLine("DataGridView:OnSearchedEntry");
            Fire(new OAVModel("masterPBlock", "GetData",null));
        }

        private void OnLoadeExit()
        {
            Console.WriteLine("DataGridView:OnLoadeExit");
        }

        private void OnLoadedEntry()
        {
            Console.WriteLine("DataGridView:OnLoadedEntry");
            Fire();
        }
    }
}
