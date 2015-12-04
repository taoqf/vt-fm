using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NRules.Fluent.Dsl;
using NRules.RuleModel;
using AutomaticCodePlugin.Views;
using Victop.Frame.CmptRuntime;
using AutomaticCodePlugin.FSM;
using AutomaticCodePlugin.ViewModels;
using System.Data;
using Victop.Server.Controls.Models;
using System.Windows.Controls;
using NRules;

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("Row")]
    public class MainViewAddRowRule : MainRule
    {
        public override void Define()
        {
            OAVModel oav = null;
            OAVModel oav1 = null;
            MainStateMachine fsm = null;
            When().Match<OAVModel>(() => oav, o => oav.AtrributeName.Equals("userdt"))
                .Match<OAVModel>(() => oav1, o => oav1.AtrributeName.Equals("btn"))
                .Match<OAVModel>(() => sessionOAV, o => sessionOAV.AtrributeName.Equals("session"))
                .Match<MainStateMachine>(() => fsm, f => f.currentState == Enums.MainViewState.AddRowed);
            Then().Do(ctx => OnAddRow(ctx, oav));
        }

        private void OnAddRow(IContext ctx, OAVModel userCtrl)
        {
            AddRow(userCtrl, string.Empty);
            OAVModel oav = new OAVModel();
            oav.AtrributeName = "abc";
            oav.AtrributeValue = "test";
            Add(oav);
        }

        #region 原子操作

        private void AddRow(OAVModel oav, string rowJson)
        {
            DataTable dt = (DataTable)oav.AtrributeValue;
            DataRow dr = dt.NewRow();
            dr["_id"] = Guid.NewGuid().ToString();
            dt.Rows.Add(dr);
        }
        #endregion
    }
}
