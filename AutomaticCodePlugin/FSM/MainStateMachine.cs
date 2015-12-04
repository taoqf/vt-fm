using System;
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
using AutomaticCodePlugin.ViewModels;
using NRules;
using System.Reflection;
using Victop.Server.Controls.Models;

namespace AutomaticCodePlugin.FSM
{
    public class MainStateMachine
    {
        StateMachine<MainViewState, MainViewTrigger> myFsm;
        public MainViewState currentState = MainViewState.None;
        UCMainViewViewModel mainViewModel;
        ISession session;
        public MainStateMachine(UCMainViewViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            //CreateRuleRepository();
            CreateRuleRepositoryGroup();
            myFsm = new StateMachine<MainViewState, MainViewTrigger>(() => currentState, s => currentState = s);
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
                .Permit(MainViewTrigger.SelectRow, MainViewState.SelectRowed)
                .Permit(MainViewTrigger.AddRow, MainViewState.AddRowed);
            myFsm.Configure(MainViewState.SelectRowed)
                .OnEntry(() => OnSelectedRowChangedEntry())
                .PermitReentry(MainViewTrigger.SelectRow)
                .Permit(MainViewTrigger.SearchBtnClick, MainViewState.SearchBtnClicked)
                .Permit(MainViewTrigger.AddRow, MainViewState.AddRowed);
            myFsm.Configure(MainViewState.AddRowed)
                .OnEntry(() => OnAddRowedEntry())
                .Permit(MainViewTrigger.SearchBtnClick, MainViewState.SearchBtnClicked)
                .Permit(MainViewTrigger.SelectRow, MainViewState.SelectRowed);
        }


        public void AddRow()
        {
            myFsm.Fire(MainViewTrigger.AddRow);
        }

        private void OnAddRowedEntry()
        {
            
            OAVModel oav = new OAVModel();
            oav.ObjectName = "dt";
            oav.AtrributeName = "userdt";
            oav.AtrributeValue = mainViewModel.MainPBlock.ViewBlockDataTable;

            OAVModel oavctrl = new OAVModel();
            oavctrl.ObjectName = "dt";
            oavctrl.AtrributeName = "btn";
            oavctrl.AtrributeValue = mainViewModel.MainView.addBtn;

            OAVModel oavsession = new OAVModel();
            oavsession.ObjectName = "rules";
            oavsession.AtrributeName = "session";
            oavsession.AtrributeValue = session;

            Console.WriteLine("OnAddRowedEntry");
            session.Insert(this);
            session.Insert(oav);
            session.Insert(oavctrl);
            session.Insert(oavsession);
            session.Fire();
            session.Retract(oav);
            session.Retract(oavctrl);
            session.Retract(oavsession);
            session.Retract(this);
        }

        public void SelectRow()
        {
            if (myFsm.CanFire(MainViewTrigger.SelectRow))
            {
                myFsm.Fire(MainViewTrigger.SelectRow,"a","b","c");
            }
        }

        private void OnSelectedRowChangedEntry()
        {
            Console.WriteLine("OnSelectedRowChangedEntry");
            session.TryInsert(this);
            session.TryInsert(mainViewModel.MainView);
            session.Fire();
            session.TryRetract(mainViewModel.MainView);
            session.TryRetract(this);
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
            Console.WriteLine("OnSearchBtnClickExit");
        }

        private void OnSearchBtnClickEntry()
        {
            Console.WriteLine("OnSearchBtnClickEntry");
            session.TryInsert(this);
            session.TryInsert(mainViewModel.MainView);
            session.Fire();
            session.TryRetract(mainViewModel.MainView);
            session.TryRetract(this);
        }

        private void OnViewLoadedExit()
        {
            Console.WriteLine("OnViewLoadedExit");
        }

        private void OnViewLoadedEntry()
        {
            session.TryInsert(this);
            session.TryInsert(mainViewModel.MainView);
            session.Fire();
            session.TryRetract(mainViewModel.MainView);
            session.TryRetract(this);
        }

        #region NRules
        private void CreateRuleRepository()
        {
            RuleRepository repository = new RuleRepository();
            repository.Load(x => x.From(typeof(MainViewLoadRule).Assembly));
            ISessionFactory factory = repository.Compile();
            session = factory.CreateSession();
        }

        private void CreateRuleRepositoryGroup()
        {
            RuleRepository fullRepository = new RuleRepository();
            fullRepository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(it => it.IsTagged("User")).To("UserRoles"));
            fullRepository.Load(x => x.From(Assembly.GetExecutingAssembly()).Where(it => it.IsTagged("Row")).To("RowRoles"));
            var sets = fullRepository.GetRuleSets();
            var complier = new RuleCompiler();
            var factory = complier.Compile(sets);
            session = factory.CreateSession();
            Console.WriteLine(sets.Count());
        }
        #endregion
    }
}
