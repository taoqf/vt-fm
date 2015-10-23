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

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    public class MainViewLoadRule : Rule
    {
        public override void Define()
        {
            TemplateControl userCtrl = null;
            When().Match(() => userCtrl, tc => tc.InitVictopUserControl(Properties.Resources.masterPVDString) == true);
            Then().Do(ctx => InitPBlockInfo(ctx, userCtrl));
        }

        private void InitPBlockInfo(IContext ctx, TemplateControl userCtrl)
        {
            UCMainView view = userCtrl as UCMainView;
            view.MainPBlock = view.GetPresentationBlockModel("masterPBlock");
            ctx.Update(userCtrl);
        }
    }
}
