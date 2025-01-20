using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class YellowFragment : Fragment
{
    public override void OnTurnEnd(State state, Combat combat, Part part)
    {
        if (!playerOwned) return;
        combat.QueueImmediate(new AStatus
        {
            status = ModEntry.Instance.QuarterEvade.Status,
            statusAmount = 1,
            targetPlayer = playerOwned,
            timer = 0
        });
    }

    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        if (playerOwned) return;
        combat.QueueImmediate(new AMove
        {
            dir = combat.otherShip.x + (combat.otherShip.parts.Count/2f) > state.ship.x + (state.ship.parts.Count/2f) ? -1 : 1,
            targetPlayer = false,
            timer = 0
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => !playerOwned ? null : StatusMeta.GetTooltips(ModEntry.Instance.QuarterEvade.Status, 1);
}
