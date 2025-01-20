using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class RO : Item
{
    public override void OnPartDamages(State state, Combat combat, Part part, DamageDone damageDone, Ship ship)
    {
        combat.QueueImmediate(new AStatus
        {
            status = Status.heat,
            statusAmount = 3,
            targetPlayer = !playerOwned,
            timer = 0
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(Status.heat, 2);
}
