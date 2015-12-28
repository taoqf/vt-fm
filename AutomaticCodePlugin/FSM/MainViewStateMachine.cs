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
        public MainViewStateMachine(TemplateControl mainView) : base("MainViewRules", Assembly.GetExecutingAssembly(), mainView)
        {
            FeiDaoFSM.Configure("None")
                .Permit("Load", "Loaded");
            FeiDaoFSM.Configure("Loaded")
                .OnEntry((x) => OnFeidaoEntry(x))
                .OnExit((x) => OnFeidaoExit(x))
                .Permit("Search", "Searched");
            FeiDaoFSM.Configure("Searched")
                .OnEntry((x) => OnFeidaoEntry(x))
                .OnExit((x) => OnFeidaoExit(x))
                .PermitReentry("Search")
                .Permit("Add", "Added");
            FeiDaoFSM.Configure("Added")
                .OnEntry((x) => OnFeidaoEntry(x))
                .OnExit((x) => OnFeidaoExit(x))
                .Permit("Search", "Searched");
        }
    }
}
