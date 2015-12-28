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
    [Tag("DataGridView")]
    public class DataGridViewRuleLoad : BaseRule
    {
        public override void Define()
        {
            StateTransitionModel stateModel = null;
            When().Match<StateTransitionModel>(() => stateModel, s => s.ActionName.Equals("Load"));
            Then().Do(session => InitPBlockInfo(session, stateModel));
        }

        private void InitPBlockInfo(IContext session, StateTransitionModel stateModel)
        {

        }
    }
    [Tag("DataGridView")]
    public class DataGridViewRuleSearch : BaseRule
    {
        public override void Define()
        {
            
            StateTransitionModel stateModel = null;
            OAVModel oavModel = null;
            When().Match<StateTransitionModel>(() => stateModel, p => p.ActionName.Equals("Search"))
                .Match<OAVModel>(() => oavModel, o => o.ObjectName.Equals("masterPBlock"));
            Then().Do(session => GetBlockData(session, stateModel, oavModel));
        }
        private void GetBlockData(IContext session, StateTransitionModel stateModel, OAVModel oavModel)
        {
            PresentationBlockModel pBlock = stateModel.MainView.GetPresentationBlockModel(oavModel.ObjectName);
            pBlock.GetData();
        }
    }
    [Repeatability(RuleRepeatability.Repeatable)]
    [Tag("DataGridView")]
    public class DataGridViewRuleAdd : BaseRule
    {
        public override void Define()
        {
            StateTransitionModel stateModel = null;
            OAVModel oavModel = null;
            When().Match<StateTransitionModel>(() => stateModel, p => p.ActionName.Equals("Add"))
                .Match<OAVModel>(() => oavModel, o => o.ObjectName.Equals("masterPBlock"));
            Then().Do(session => GetBlockData(session, stateModel, oavModel));
        }
        private void GetBlockData(IContext session, StateTransitionModel stateModel, OAVModel oavModel)
        {
            DataTable mastDt = stateModel.MainView.GetPresentationBlockModel(oavModel.ObjectName).ViewBlockDataTable;
            DataRow dr = mastDt.NewRow();
            dr["_id"] = Guid.NewGuid().ToString();
            mastDt.Rows.Add(dr);
        }
    }
}
