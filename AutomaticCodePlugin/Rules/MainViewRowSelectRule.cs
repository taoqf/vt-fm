using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NRules.Fluent.Dsl;
using NRules.RuleModel;
using System.Data;
using Victop.Frame.CmptRuntime;
using AutomaticCodePlugin.Views;
using AutomaticCodePlugin.FSM;
using AutomaticCodePlugin.ViewModels;

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("Row")]
    public class MainViewRowSelectRule : NRules.Fluent.Dsl.Rule
    {
        public override void Define()
        {
            TemplateControl userControl = null;
            MainStateMachine fsm = null;
            When().Match<TemplateControl>(() => userControl, p => p != null && p.InitFlag == true)
                .Match<MainStateMachine>(() => fsm, f => f.currentState == Enums.MainViewState.SelectRowed);
            Then().Do(ctx => UpdateDetialUI(ctx, userControl, fsm));
        }

        private void UpdateDetialUI(IContext ctx, TemplateControl mainView, MainStateMachine fsm)
        {
            if (mainView != null)
            {
                UCMainView myView = mainView as UCMainView;
                UCMainViewViewModel mainViewModel = myView.DataContext as UCMainViewViewModel;
                if (myView.dgridProduct.SelectedItem != null)
                {
                    DataRow dr = (myView.dgridProduct.SelectedItem as DataRowView).Row;
                    mainViewModel.MainPBlock.PreBlockSelectedRow = dr;
                    mainViewModel.MainPBlock.SetCurrentRow(dr);
                    mainViewModel.Test = dr["product_name"].ToString();
                }
            }
        }
    }
}
