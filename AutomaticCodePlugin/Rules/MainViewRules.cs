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
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("MainView")]
    public class MainViewRuleSearch : BaseRule
    {
        public override void Define()
        {
            OAVModel oavCtrl = null;
            OAVModel oavState = null;
            When().Match<OAVModel>(() => oavCtrl, ctrl => ctrl.ObjectName.Equals("MainView") && ctrl.AtrributeName.Equals("GridControl"))
                .Match<OAVModel>(() => oavState, state => state.ObjectName.Equals("MainView") && state.AtrributeName.Equals("State") && state.AtrributeValue.Equals("Search"));
            Then().Do(session => OnSearch(session, oavCtrl, oavState));
        }

        private void OnSearch(IContext session, OAVModel oavCtrl, OAVModel oavState)
        {
            TemplateControl tc = oavCtrl.AtrributeValue as TemplateControl;
            Dictionary<string, object> paramDic = new Dictionary<string, object>();
            paramDic.Add(oavState.AtrributeName, oavState.AtrributeValue);
            tc.Excute(paramDic);
        }
    }
}
