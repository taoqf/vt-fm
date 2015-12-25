using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NRules.RuleModel;
using Victop.Frame.CmptRuntime;
using Victop.Server.Controls.Models;
using NRules.Fluent.Dsl;

namespace AutomaticCodePlugin.Rules
{
    [Tag("MainView")]
    public class MainViewRuleSearch : BaseRule
    {
        public override void Define()
        {
            StateTransitionModel stateModel = null;
            When().Match<StateTransitionModel>(() => stateModel);
            Then().Do(session => OnSearch(session, stateModel));
        }

        private void OnSearch(IContext session, StateTransitionModel stateModel)
        {
            TemplateControl tc = stateModel.MainView.FindName("ucdgrid") as TemplateControl;
            tc.FeiDaoFSM.Do(stateModel.ActionName, stateModel.ActionSourceElement);
        }
    }
}
