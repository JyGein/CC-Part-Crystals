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
            status = ModEntry.Instance.HalfEvade.Status,
            statusAmount = 1,
            targetPlayer = playerOwned,
            timer = 0
        });
    }

    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        if (playerOwned) return;
        float otherShipPos = combat.otherShip.x + (combat.otherShip.parts.Count / 2f);
        float shipPos = state.ship.x + (state.ship.parts.Count / 2f);
        combat.QueueImmediate(new AMove
        {
            dir = otherShipPos == shipPos ? 0 : (otherShipPos > shipPos ? -1 : 1),
            targetPlayer = false,
            timer = 0
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => !playerOwned ? null : StatusMeta.GetTooltips(ModEntry.Instance.HalfEvade.Status, 1);
}
