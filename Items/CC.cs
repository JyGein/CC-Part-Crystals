using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class CC : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(CyanFragment), typeof(CyanFragment)];

    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        base.OnPartHit(state, combat, part, damageDone);
        combat.QueueImmediate(new AStatus
        {
            status = playerOwned ? Status.corrode : ModEntry.Instance.KokoroApi.OxidationStatus.Status,
            statusAmount = playerOwned ? 1 : 5,
            timer = 0,
            targetPlayer = !playerOwned
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [.. playerOwned ? StatusMeta.GetTooltips(Status.corrode, 1) : StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.OxidationStatus.Status, 5)];
}
