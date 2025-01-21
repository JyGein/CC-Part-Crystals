using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class RY : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(RedFragment), typeof(YellowFragment)];

    public override void OnShipMoves(State state, Combat combat, Part part)
    {
        base.OnShipMoves(state, combat, part);
        combat.QueueImmediate(new AAttack
        {
            damage = Card.GetActualDamage(state, 0, !playerOwned),
            status = ModEntry.Instance.HalfDamage.Status,
            statusAmount = 1,
            targetPlayer = !playerOwned,
            fromX = (playerOwned ? state.ship : combat.otherShip).parts.FindIndex(p => p == part),
            fast = true
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(ModEntry.Instance.HalfDamage.Status, 1);
}
