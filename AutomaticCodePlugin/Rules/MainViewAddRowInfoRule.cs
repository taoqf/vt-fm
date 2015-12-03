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
    public class MainViewAddRowInfoRule : NRules.Fluent.Dsl.Rule
    {
        public override void Define()
        {
            OAVModel oavCtrl = null;
            OAVModel oavSession = null;
            OAVModel oavCtrlInfo = null;
            When().Match<OAVModel>(() => oavCtrl, o => o.AtrributeName.Equals("btn"))
                .Match<OAVModel>(() => oavSession, o => o.AtrributeName.Equals("session"))
                .Match<OAVModel>(() => oavCtrlInfo, o => o.AtrributeName.Equals("abc"));
            Then().Do(ctx => CtrlUI(ctx, oavCtrl, oavSession, oavCtrlInfo));

        }

        private void CtrlUI(IContext ctx, OAVModel atrributeValue, OAVModel session, OAVModel oavCtrlInfo)
        {
            Console.WriteLine(atrributeValue.AtrributeName);
            ISession se = session.AtrributeValue as ISession;
            Button btn = atrributeValue.AtrributeValue as Button;
            btn.IsEnabled = false;
            se.Retract(oavCtrlInfo);
        }
    }
}
