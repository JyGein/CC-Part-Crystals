using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class BB : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(BlueFragment), typeof(BlueFragment)];

    public PDamMod OGPDamMod;
    public PDamMod? OGPDamModOverride;
    public override void OnCombatStart(State state, Combat combat, Part part)
    {
        base.OnCombatStart(state, combat, part);
        if (playerOwned) return;
        part.damageModifier = PDamMod.armor;
        part.damageModifierOverrideWhileActive = PDamMod.armor;
    }
    public override void OnPartAttached(State state, Part part)
    {
        base.OnPartAttached(state, part);
        OGPDamMod = part.damageModifier;
        part.damageModifier = PDamMod.armor;
        OGPDamModOverride = part.damageModifierOverrideWhileActive;
        part.damageModifierOverrideWhileActive = PDamMod.armor;
    }
    public override void OnPartDetached(State state, Part part)
    {
        base.OnPartDetached(state, part);
        part.damageModifier = OGPDamMod;
        part.damageModifierOverrideWhileActive = OGPDamModOverride;
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [new TTGlossary("parttrait.armor")];
}
