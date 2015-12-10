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
            OAVModel oavEnable = null;
            OAVModel oavCtrl = null;
            When().Match<OAVModel>(() => oavEnable, o => o.ObjectName.Equals("masterPBlock") && o.AtrributeName.Equals("AddBtnEnble"))
                .Match<OAVModel>(() => oavCtrl, o => o.ObjectName.Equals("masterPBlock") && o.AtrributeName.Equals("addBtn"));
            Then().Do(session => CtrlUI(session, oavEnable, oavCtrl));

        }

        private void CtrlUI(IContext session, OAVModel atrributeValue, OAVModel oavCtrl)
        {
            SetButtonEnabled(oavCtrl, (bool)atrributeValue.AtrributeValue);
            session.Retract(atrributeValue);

        }
        private void SetButtonEnabled(OAVModel oav, bool enabled)
        {
            Button btn = (Button)oav.AtrributeValue;
            btn.IsEnabled = enabled;
        }
    }
}
