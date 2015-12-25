using NRules.Fluent.Dsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Frame.CmptRuntime;
using Victop.Server.Controls.Models;
using NRules.RuleModel;
using AutomaticCodePlugin.Views;

namespace AutomaticCodePlugin.Rules
{
    [Tag("BtnOpView")]
    public class BtnOpViewRuleSearch : BaseRule
    {
        public override void Define()
        {
            StateTransitionModel stateModel = null;
            When().Match<StateTransitionModel>(() => stateModel,s=>s.ActionName.Equals("Search"));
            Then().Do(session => OnSearch(session, stateModel));
        }

        private void OnSearch(IContext session, StateTransitionModel stateModel)
        {
            stateModel.MainView.ParentControl.FeiDaoFSM.Do("Search",stateModel.ActionSourceElement);
        }
    }
    [Tag("BtnOpView")]
    public class BtnOpViewRuleAdd : BaseRule
    {
        public override void Define()
        {
            StateTransitionModel stateModel = null;
            When().Match<StateTransitionModel>(() => stateModel,s=>s.ActionName.Equals("Add"));
            Then().Do(session => OnAdd(session, stateModel));
        }

        private void OnAdd(IContext session, StateTransitionModel stateModel)
        {
            stateModel.MainView.ParentControl.FeiDaoFSM.Do("Add",stateModel.ActionSourceElement);
        }
    }
}
