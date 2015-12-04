using NRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Victop.Server.Controls.Models;

namespace AutomaticCodePlugin.Rules
{
    public abstract class MainRule : NRules.Fluent.Dsl.Rule
    {
        public OAVModel sessionOAV;
        public void Add(OAVModel OAV)
        {
            ISession session = (ISession)sessionOAV.AtrributeValue;
            session.TryInsert(OAV);
        }
        public void Remove(OAVModel OAV)
        {
            ISession session = (ISession)sessionOAV.AtrributeValue;
            session.TryRetract(OAV);
        }
    }
}
