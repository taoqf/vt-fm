using AutomaticCodePlugin.Views;
using NRules.Fluent.Dsl;
using NRules.RuleModel;
using System;
using System.Collections.Generic;
using System.Data;
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
            Then().Do(session => InitPBlockInfo(session, oav));
        }

        private void InitPBlockInfo(IContext session, OAVModel oav)
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
            Then().Do(session => GetBlockData(session, oav));
        }
        private void GetBlockData(IContext session, OAVModel oav)
        {
            PresentationBlockModel pBlock = oav.AtrributeValue as PresentationBlockModel;
            pBlock.GetData();
        }
    }
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("DataGridView")]
    public class DataGridViewRuleAdd : BaseRule
    {
        public override void Define()
        {
            OAVModel oav = null;
            When().Match<OAVModel>(() => oav, p => p.ObjectName.Equals("masterPBlock") && p.AtrributeName.Equals("AddData"));
            Then().Do(session => GetBlockData(session, oav));
        }
        private void GetBlockData(IContext session, OAVModel oav)
        {
            DataTable mastDt = oav.AtrributeValue as DataTable;
            DataRow dr = mastDt.NewRow();
            dr["_id"] = Guid.NewGuid().ToString();
            mastDt.Rows.Add(dr);
        }
    }
}
