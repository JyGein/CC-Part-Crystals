using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class YM : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(YellowFragment), typeof(MagentaFragment)];

    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        if (damageDone.hitHull)
        {
            combat.QueueImmediate([new AStatus
            {
                status = playerOwned ? Status.evade : Status.lockdown,
                statusAmount = playerOwned ? 3 : 1,
                timer = 0,
                targetPlayer = true
            },
            new AStatus
            {
                status = Status.tempShield,
                statusAmount = 5,
                timer = 0,
                targetPlayer = playerOwned
            }]);
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [.. playerOwned ? StatusMeta.GetTooltips(Status.evade, 3) : StatusMeta.GetTooltips(Status.lockdown, 1), .. StatusMeta.GetTooltips(Status.tempShield, 5)];
}
