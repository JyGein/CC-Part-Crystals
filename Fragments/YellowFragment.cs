using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class YellowFragment : Fragment
{
    public override void OnTurnEnd(State state, Combat combat, Part part)
    {
        base.OnTurnEnd(state, combat, part);
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
        base.OnPartHit(state, combat, part, damageDone);
        if (playerOwned) return;
        if (timesShotThisTurn > 1) return;
        float otherShipPos = combat.otherShip.x + (combat.otherShip.parts.Count / 2f);
        float shipPos = state.ship.x + (state.ship.parts.Count / 2f);
        combat.QueueImmediate(new AMove
        {
            dir = otherShipPos == shipPos ? 0 : (otherShipPos > shipPos ? -1 : 1),
            targetPlayer = false,
            timer = 0
        });
    }
    public override void OnTurnStart(State state, Combat combat, Part part)
    {
        base.OnTurnStart(state, combat, part);
    }

    public override List<Tooltip>? GetExtraTooltips()
        => !playerOwned ? null : StatusMeta.GetTooltips(ModEntry.Instance.QuarterEvade.Status, 1);
}
