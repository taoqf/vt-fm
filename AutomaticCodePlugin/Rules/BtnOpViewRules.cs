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
            TemplateControl oavCtrl = null;
            OAVModel oavOpName = null;
            When().Match<TemplateControl>(() => oavCtrl)
                .Match<OAVModel>(() => oavOpName, op => op.ObjectName.Equals("opView") && op.AtrributeName.Equals("opName") && op.AtrributeValue.Equals("SearchBtnClick"));
            Then().Do(session => OnSearch(session, oavCtrl, oavOpName));
        }

        private void OnSearch(IContext session, TemplateControl oavCtrl, OAVModel oavOpName)
        {
            UCBtnOperationView opView = oavCtrl as UCBtnOperationView;
            if (opView.SearchBtnClick != null)
            {
                opView.SearchBtnClick(opView.searchBtn, opView.ParamDict);
            }
        }
    }
    [Tag("BtnOpView")]
    public class BtnOpViewRuleAdd : BaseRule
    {
        public override void Define()
        {
            TemplateControl oavCtrl = null;
            OAVModel oavOpName = null;
            When().Match<TemplateControl>(() => oavCtrl)
                .Match<OAVModel>(() => oavOpName, op => op.ObjectName.Equals("opView") && op.AtrributeName.Equals("opName") && op.AtrributeValue.Equals("AddBtnClick"));
            Then().Do(session => OnAdd(session, oavCtrl, oavOpName));
        }

        private void OnAdd(IContext session, TemplateControl oavCtrl, OAVModel oavOpName)
        {
            UCBtnOperationView opView = oavCtrl as UCBtnOperationView;
            if (opView.AddBtnClick != null)
            {
                opView.AddBtnClick(opView.addBtn, opView.ParamDict);
            }
        }
    }
}
