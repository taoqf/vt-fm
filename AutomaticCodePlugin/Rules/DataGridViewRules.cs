using AutomaticCodePlugin.Views;
using NRules.Fluent.Dsl;
using NRules.RuleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.CmptRuntime;
using Victop.Server.Controls.Models;

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("DataGridView")]
    public class DataGridViewRuleLoad : BaseRule
    {
        public override void Define()
        {
            OAVModel oav = null;
            When().Match<OAVModel>(() => oav, o => o.ObjectName.Equals("UCDataGridView") && o.AtrributeName.Equals("TemplateControl"));
            Then().Do(ctx => InitPBlockInfo(ctx, oav));
        }

        private void InitPBlockInfo(IContext ctx, OAVModel oav)
        {
            UCDataGridView view = oav.AtrributeValue as UCDataGridView;
            if (view.InitVictopUserControl(Properties.Resources.masterPVDString))
            {
                view.MainPBlock = view.GetPresentationBlockModel("masterPBlock");
            }
        }
    }
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("DataGridView")]
    public class DataGridViewRuleSearch : BaseRule
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
