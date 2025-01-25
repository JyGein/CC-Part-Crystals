using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartCrystals.Fragments;

public class CC : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(CyanFragment), typeof(CyanFragment)];

    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        base.OnPartHit(state, combat, part, damageDone);
        combat.QueueImmediate(new AStatus
        {
            status = ModEntry.Instance.KokoroApi.OxidationStatus.Status,
            statusAmount = playerOwned ? 3 : 5,
            timer = 0,
            targetPlayer = !playerOwned
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.OxidationStatus.Status, 3);
}
