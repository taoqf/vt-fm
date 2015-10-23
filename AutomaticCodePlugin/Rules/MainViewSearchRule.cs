using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victop.Frame.CmptRuntime;
using NRules.Fluent.Dsl;
using NRules.RuleModel;

namespace AutomaticCodePlugin.Rules
{
    [Repeatability(RuleRepeatability.NonRepeatable)]
    public class MainViewSearchRule : Rule
    {
        public override void Define()
        {
            PresentationBlockModel pBlock = null;
            When().Match<PresentationBlockModel>(() => pBlock, p => p.ViewBlock.ViewId != null);
            Then().Do(_ => pBlock.GetData())
                .Do(ctx => ctx.Update(pBlock));

        }
    }
}
