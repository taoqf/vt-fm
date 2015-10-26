using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victop.Frame.CmptRuntime;
using NRules.Fluent.Dsl;
using NRules.RuleModel;
using System.Linq.Expressions;
using AutomaticCodePlugin.Views;
using AutomaticCodePlugin.FSM;
using AutomaticCodePlugin.ViewModels;

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("User")]
    public class MainViewLoadRule : Rule
    {
        public override void Define()
        {
            TemplateControl userCtrl = null;
            MainStateMachine fsm = null;
            When().Match<TemplateControl>(() => userCtrl, tc => tc != null && tc.InitFlag != true)
                .Match<MainStateMachine>(() => fsm, f => f.currentState == Enums.MainViewState.ViewLoaded);
            Then().Do(ctx => InitPBlockInfo(ctx, userCtrl,fsm));
        }

        private void InitPBlockInfo(IContext ctx, TemplateControl userCtrl,MainStateMachine fsm)
        {
            UCMainView view = userCtrl as UCMainView;
            UCMainViewViewModel mainViewModel = view.DataContext as UCMainViewViewModel;
            if (view.InitVictopUserControl(Properties.Resources.masterPVDString))
            {
                mainViewModel.MainPBlock = view.GetPresentationBlockModel("masterPBlock");
            }
            ctx.Update(userCtrl);
        }
    }
}
