using NRules;
using NRules.Fluent.Dsl;
using NRules.RuleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Victop.Server.Controls.Models;

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("Row")]
    public class MainViewAddRowInfoRule : MainRule
    {
        public override void Define()
        {
            OAVModel oavCtrl = null;
            OAVModel oavCtrlInfo = null;
            When().Match<OAVModel>(() => oavCtrl, o => o.AtrributeName.Equals("btn"))
                .Match<OAVModel>(() => sessionOAV, o => o.AtrributeName.Equals("session"))
                .Match<OAVModel>(() => oavCtrlInfo, o => o.AtrributeName.Equals("abc"));
            Then().Do(ctx => CtrlUI(ctx, oavCtrl, oavCtrlInfo));

        }

        private void CtrlUI(IContext ctx, OAVModel atrributeValue,OAVModel oavCtrlInfo)
        {
            SetButtonEnabled(atrributeValue, false);
            Remove(oavCtrlInfo);
        }
        private void SetButtonEnabled(OAVModel oav, bool enabled)
        {
            Button btn = (Button)oav.AtrributeValue;
            btn.IsEnabled = enabled;
        }
    }
}
