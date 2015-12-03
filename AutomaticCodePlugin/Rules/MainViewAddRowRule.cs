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
    public class MainViewAddRowRule : NRules.Fluent.Dsl.Rule
    {
        public override void Define()
        {
            OAVModel oav = null;
            OAVModel oav1 = null;
            MainStateMachine fsm = null;
            OAVModel oavsession = null;
            When().Match<OAVModel>(() => oav, o => oav.AtrributeName.Equals("userdt"))
                .Match<OAVModel>(() => oav1, o => oav1.AtrributeName.Equals("btn"))
                .Match<OAVModel>(() => oavsession, o => oavsession.AtrributeName.Equals("session"))
                .Match<MainStateMachine>(() => fsm, f => f.currentState == Enums.MainViewState.AddRowed);
            Then().Do(ctx => OnAddRow(ctx, oav, oavsession));
                //.Do(ctx => OnUpdateUI(ctx, oav1));
        }

        private void OnUpdateUI(IContext ctx, OAVModel oav)
        {
            SetButtonEnabled(oav, false);
        }

        private void OnAddRow(IContext ctx, OAVModel userCtrl, OAVModel session)
        {
            ISession temp = (ISession)session.AtrributeValue;
            AddRow(userCtrl, string.Empty);
            //OAVModel oav = new OAVModel();
            //oav.AtrributeName = "abc";
            //oav.AtrributeValue = "test";
            //temp.Insert(oav);
        }

        #region 原子操作

        private void SetButtonEnabled(OAVModel oav, bool enabled)
        {
            Button btn = (Button)oav.AtrributeValue;
            btn.IsEnabled = enabled;
        }

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
