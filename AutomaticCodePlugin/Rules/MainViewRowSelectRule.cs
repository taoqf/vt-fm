using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NRules.Fluent.Dsl;
using NRules.RuleModel;
using System.Data;
using Victop.Frame.CmptRuntime;
using AutomaticCodePlugin.Views;
using AutomaticCodePlugin.FSM;
using Victop.Server.Controls.Models;

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("Row")]
    public class MainViewRowSelectRule : MainRule
    {
        public override void Define()
        {
            OAVModel oav = null;
            OAVModel oavP = null;
            When().Match<OAVModel>(() => oav, o => o.ObjectName.Equals("masterPBlock") && o.AtrributeName.Equals("selectedItem"))
                .Match<OAVModel>(()=>oavP,p=>p.ObjectName.Equals("masterPBlock")&&p.AtrributeName.Equals("PBlock"));
            Then().Do(ctx => UpdateDetialUI(ctx, oav,oavP));
        }

        private void UpdateDetialUI(IContext ctx, OAVModel oav,OAVModel oavP)
        {
            if (oav.AtrributeValue != null)
            {
                DataRow dr = (oav.AtrributeValue as DataRowView).Row;
                PresentationBlockModel pBlock = oavP.AtrributeValue as PresentationBlockModel;
                pBlock.PreBlockSelectedRow = dr;
                pBlock.SetCurrentRow(dr);
            }
        }
    }
}
