using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class RedFragment : Fragment
{
    public override void OnShipShoots(State state, Combat combat)
    {
        combat.QueueImmediate(new AAttack
        {
            damage = Card.GetActualDamage(state, playerOwned ? 0 : 1, !playerOwned),
            status = playerOwned ? ModEntry.Instance.HalfDamage.Status : null,
            statusAmount = playerOwned ? 1 : default,
            targetPlayer = !playerOwned,
            fromX = ModEntry.GetPartX(AttachedPart!, state, combat) ?? 0,
            fast = true
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => !playerOwned ? null : StatusMeta.GetTooltips(ModEntry.Instance.HalfDamage.Status, 1);
}
