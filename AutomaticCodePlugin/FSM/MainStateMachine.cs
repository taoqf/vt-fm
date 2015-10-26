﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stateless;
using AutomaticCodePlugin.Enums;
using System.Windows;
using System.Windows.Controls;
using Victop.Frame.CmptRuntime;
using AutomaticCodePlugin.Views;
using NRules.Fluent;
using AutomaticCodePlugin.Rules;
using NRules;

namespace AutomaticCodePlugin.FSM
{
    public class MainStateMachine
    {
        StateMachine<MainViewState, MainViewTrigger> myFsm;
        TemplateControl mainView;
        ISession session;
        public MainStateMachine(TemplateControl mainView)
        {
            this.mainView = mainView;
            myFsm = new StateMachine<MainViewState, MainViewTrigger>(MainViewState.None);
            myFsm.Configure(MainViewState.None)
                .Permit(MainViewTrigger.ViewLoad, MainViewState.ViewLoaded);
            myFsm.Configure(MainViewState.ViewLoaded)
                .OnEntry(() => OnViewLoadedEntry())
                .OnExit(() => OnViewLoadedExit())
                .Permit(MainViewTrigger.SearchBtnClick, MainViewState.SearchBtnClicked);
            myFsm.Configure(MainViewState.SearchBtnClicked)
                .OnEntry(() => OnSearchBtnClickEntry())
                .PermitReentry(MainViewTrigger.SearchBtnClick)
                .OnExit(() => OnSearchBtnClickExit())
                .Permit(MainViewTrigger.SelectedRowChange, MainViewState.SelectedRowChanged);
            myFsm.Configure(MainViewState.SelectedRowChanged)
                .OnEntry(() => OnSelectedRowChangedEntry())
                .PermitReentry(MainViewTrigger.SelectedRowChange);
            CreateRuleRepository();
        }


        public void SelectedRow()
        {
            myFsm.Fire(MainViewTrigger.SelectedRowChange);
        }

        private void OnSelectedRowChangedEntry()
        {
            UCMainView view = mainView as UCMainView;
            session.Insert(view.dgridProduct.SelectedItem);
            session.Fire();
            session.Retract(view.dgridProduct.SelectedItem);
        }

        public void MainLoad()
        {
            myFsm.Fire(MainViewTrigger.ViewLoad);
        }

        public void Search()
        {
            myFsm.Fire(MainViewTrigger.SearchBtnClick);
        }

        private void OnSearchBtnClickExit()
        {
            Console.WriteLine("OnViewLoadedExit");
        }

        private void OnSearchBtnClickEntry()
        {
            UCMainView myView = mainView as UCMainView;
            session.Insert(myView.MainPBlock);
            session.Fire();
            session.Retract(myView.MainPBlock);
        }

        private void OnViewLoadedExit()
        {
            Console.WriteLine("OnViewLoadedExit");
        }

        private void OnViewLoadedEntry()
        {
            session.Insert(mainView);
            session.Fire();
            session.Retract(mainView);
        }

        #region NRules
        private void CreateRuleRepository()
        {
            RuleRepository repository = new RuleRepository();
            repository.Load(x => x.From(typeof(MainViewLoadRule).Assembly));
            ISessionFactory factory = repository.Compile();
            session = factory.CreateSession();
        }
        #endregion
    }
}
