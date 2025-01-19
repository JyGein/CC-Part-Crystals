using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class OrangeFragment : Fragment
{
    public override void OnPartHit(State state, Combat combat, Part part)
    {
        combat.QueueImmediate(new AStatus
        {
            status = Status.heat,
            statusAmount = 1,
            targetPlayer = !playerOwned
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(Status.heat, 1);
}
