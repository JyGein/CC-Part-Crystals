using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class YY : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(YellowFragment), typeof(YellowFragment)];

    public override void OnTurnStart(State state, Combat combat, Part part)
    {
        base.OnTurnStart(state, combat, part);
        combat.Queue(new AStatus
        {
            status = Status.autododgeRight,
            statusAmount = 1,
            targetPlayer = playerOwned,
            timer = 0
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(Status.autododgeRight, 1);
}
