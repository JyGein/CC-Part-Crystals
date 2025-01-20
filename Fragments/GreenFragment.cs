using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class GreenFragment : Fragment
{
    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        combat.QueueImmediate(playerOwned ?
            new AStatus
            {
                status = ModEntry.Instance.QuarterHeal.Status,
                statusAmount = 1,
                targetPlayer = true,
                timer = 0
            } :
            new AHeal
            {
                healAmount = 1,
                targetPlayer = false,
                timer = 0
            });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => !playerOwned ? null : StatusMeta.GetTooltips(ModEntry.Instance.QuarterHeal.Status, 1);
}
