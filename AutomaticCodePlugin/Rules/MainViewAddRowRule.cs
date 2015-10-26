using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NRules.Fluent.Dsl;
using NRules.RuleModel;
using AutomaticCodePlugin.Views;
using Victop.Frame.CmptRuntime;
using AutomaticCodePlugin.FSM;
using AutomaticCodePlugin.ViewModels;
using System.Data;

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("Row")]
    public class MainViewAddRowRule : NRules.Fluent.Dsl.Rule
    {
        public override void Define()
        {
            TemplateControl userCtrl = null;
            MainStateMachine fsm = null;
            When().Match<TemplateControl>(() => userCtrl, uctrl => userCtrl.InitFlag == true)
                .Match<MainStateMachine>(() => fsm, f => f.currentState == Enums.MainViewState.AddRowed);
            Then().Do(ctx => OnAddRow(ctx, userCtrl));
        }

        private void OnAddRow(IContext ctx, TemplateControl userCtrl)
        {
            UCMainView mainView = userCtrl as UCMainView;
            UCMainViewViewModel mainViewModel = mainView.DataContext as UCMainViewViewModel;
            DataRow dr = mainViewModel.MainPBlock.ViewBlockDataTable.NewRow();
            dr["_id"] = Guid.NewGuid().ToString();
            mainViewModel.MainPBlock.ViewBlockDataTable.Rows.Add(dr);
            ctx.Update(userCtrl);
        }
    }
}
