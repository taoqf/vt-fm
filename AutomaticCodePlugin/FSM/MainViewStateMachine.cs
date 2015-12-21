﻿using System;
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
        public MainViewStateMachine() : base("MainView", Assembly.GetExecutingAssembly())
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
                .PermitReentry("Search");
        }

        private void OnSearchedExit()
        {
            Console.WriteLine("MainView:OnSearchedExit");
        }

        private void OnSearchedEntry()
        {
            Console.WriteLine("MainView:OnSearchedEntry");
            Fire(new OAVModel() { ObjectName = "MainView", AtrributeName = "GridControl", AtrributeValue = MainView.FindName("ucdgrid") },
                new OAVModel() { ObjectName = "MainView", AtrributeName = "State", AtrributeValue = "Search" });
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
