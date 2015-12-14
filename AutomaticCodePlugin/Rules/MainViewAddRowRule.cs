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
using System.Data;
using Victop.Server.Controls.Models;
using System.Windows.Controls;
using NRules;

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    [Tag("MainPage")]
    public class MainViewAddRowRule : BaseRule
    {
        public override void Define()
        {
            OAVModel oav = null;
            When().Match<OAVModel>(() => oav, o => o.ObjectName.Equals("masterPBlock") && oav.AtrributeName.Equals("AddData"));
            Then().Do(session => OnAddRow(session, oav));
        }

        private void OnAddRow(IContext session, OAVModel userCtrl)
        {
            AddRow(userCtrl, string.Empty);
            OAVModel oav = new OAVModel();
            oav.ObjectName = userCtrl.ObjectName;
            oav.AtrributeName = "AddBtnEnble";
            oav.AtrributeValue = false;
            session.Insert(oav);
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
