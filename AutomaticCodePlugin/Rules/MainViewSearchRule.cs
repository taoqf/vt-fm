using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victop.Frame.CmptRuntime;
using NRules.Fluent.Dsl;
using NRules.RuleModel;
using AutomaticCodePlugin.Views;
using AutomaticCodePlugin.FSM;
using AutomaticCodePlugin.ViewModels;

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("User")]
    public class MainViewSearchRule : Rule
    {
        public override void Define()
        {
            TemplateControl userCtrl = null;
            MainStateMachine fsm = null;
            When().Match<TemplateControl>(() => userCtrl, p => p != null && p.InitFlag == true)
                .Match<MainStateMachine>(() => fsm, f => f.currentState == Enums.MainViewState.SearchBtnClicked);
            Then().Do(ctx => GetBlockData(ctx, userCtrl));

        }

        private void GetBlockData(IContext ctx, TemplateControl userCtrl)
        {
            UCMainView mainView = userCtrl as UCMainView;
            UCMainViewViewModel mainViewModel = mainView.DataContext as UCMainViewViewModel;
            if (mainViewModel.MainPBlock.ViewBlock.ViewId != null)
            {
                mainViewModel.MainPBlock.GetData();
            }
        }
    }
}
