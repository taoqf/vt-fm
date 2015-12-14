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
using Victop.Server.Controls.Models;

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("MainPage")]
    public class MainViewSearchRule : BaseRule
    {
        public override void Define()
        {
            OAVModel oav = null;
            When().Match<OAVModel>(() => oav, p => p.ObjectName.Equals("masterPBlock") && p.AtrributeName.Equals("GetData"));
            Then().Do(ctx => GetBlockData(ctx, oav));

        }
        private void GetBlockData(IContext ctx, OAVModel oav)
        {
            PresentationBlockModel pBlock = oav.AtrributeValue as PresentationBlockModel;
            pBlock.GetData();
        }
    }
}
