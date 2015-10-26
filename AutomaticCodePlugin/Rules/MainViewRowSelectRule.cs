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

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    public class MainViewRowSelectRule : NRules.Fluent.Dsl.Rule
    {
        public override void Define()
        {
            UCMainView mainView = null;
            object SelectedItem = null;
            When().Match(() => SelectedItem, p => p != null && mainView != null);
            Then().Do(ctx => UpdateDetialUI(ctx, mainView, SelectedItem));
        }

        private void UpdateDetialUI(IContext ctx, UCMainView mainView, object SelectedItem)
        {
            Console.WriteLine("temp:" + mainView.MainPBlock.BlockName);
            DataRowView drv = SelectedItem as DataRowView;
            mainView.MainPBlock.SetCurrentRow(drv.Row);
            mainView.MainPBlock.PreBlockSelectedRow = drv.Row;
            mainView.tbox.Text = "temp";
            ctx.Update(mainView);
        }
    }
}
