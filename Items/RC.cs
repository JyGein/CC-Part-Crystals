using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class RC : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(RedFragment), typeof(CyanFragment)];

    public override void OnPartDamages(State state, Combat combat, Part part, DamageDone damageDone, Ship ship)
    {
        base.OnPartDamages(state, combat, part, damageDone, ship);
        combat.QueueImmediate(new AStatus
        {
            status = ModEntry.Instance.KokoroApi.OxidationStatus.Status,
            statusAmount = 3,
            targetPlayer = !playerOwned,
            timer = 0
        });
    }

    public override List<Tooltip>? GetExtraTooltips()
        => StatusMeta.GetTooltips(ModEntry.Instance.KokoroApi.OxidationStatus.Status, 2);
}
