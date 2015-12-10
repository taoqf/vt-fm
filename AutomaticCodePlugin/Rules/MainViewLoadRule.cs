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
using Victop.Server.Controls.Models;

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("MainPage")]
    public class MainViewLoadRule : MainRule
    {
        public override void Define()
        {
            OAVModel oav = null;
            When().Match<OAVModel>(() => oav, o => o.ObjectName.Equals("UCMaster") && o.AtrributeName.Equals("UserControl"));
            Then().Do(ctx => InitPBlockInfo(ctx, oav));
        }

        private void InitPBlockInfo(IContext ctx, OAVModel oav)
        {
            UCMainView view = oav.AtrributeValue as UCMainView;
            if (view.InitVictopUserControl(Properties.Resources.masterPVDString))
            {
                view.MainPBlock = view.GetPresentationBlockModel("masterPBlock");
            }
        }
    }
}
