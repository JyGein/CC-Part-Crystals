using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class MagentaFragment : Fragment
{
    public override void OnPartHit(State state, Combat combat)
    {
        combat.Queue(new AStatus
        {
            status = Status.tempShield,
            statusAmount = 1,
            targetPlayer = playerOwned
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(Status.tempShield, 1);
}
