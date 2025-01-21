using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class GC : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(GreenFragment), typeof(CyanFragment)];

    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        base.OnPartHit(state, combat, part, damageDone);
        List<CardAction> list = [new AStatus
        {
            status = ModEntry.Instance.KokoroApi.OxidationStatus.Status,
            statusAmount = 0,
            timer = 0,
            mode = AStatusMode.Set,
            targetPlayer = playerOwned
        }];
        if ((playerOwned ? state.ship : combat.otherShip).Get(Status.corrode) > 0)
        {
            list.Add(new AStatus
            {
                status = Status.corrode,
                statusAmount = -1,
                timer = 0,
                targetPlayer = playerOwned
            });
        }
        combat.QueueImmediate(list);
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [.. StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.OxidationStatus.Status, 0), .. StatusMeta.GetTooltips(Status.corrode, -1)];
}
