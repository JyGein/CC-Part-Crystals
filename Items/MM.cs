﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Fragments;

public class MM : Item
{
    public override List<Type> GetBaseFragmentTypes()
        => [typeof(MagentaFragment), typeof(MagentaFragment)];

    public List<PDamMod?> OGPDamMods = [];
    public override void OnTurnStart(State state, Combat combat, Part part)
    {
        base.OnTurnStart(state, combat, part);
        if (OGPDamMods.Count > 0)
        {
            foreach ((Part p, int i) in (playerOwned ? state.ship : combat.otherShip).parts.Where(p => p.type != PType.empty).ToList().WithIndex()) {
                p.damageModifierOverrideWhileActive = OGPDamMods[i]; 
            }
        }
        OGPDamMods = [];
    }
    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone)
    {
        base.OnPartHit(state, combat, part, damageDone);
        if (OGPDamMods.Count > 0) return;
        foreach ((Part p, int i) in (playerOwned ? state.ship : combat.otherShip).parts.Where(p => p.type != PType.empty).ToList().WithIndex())
        {
            OGPDamMods.Add(p.damageModifierOverrideWhileActive);
            p.damageModifierOverrideWhileActive = PDamMod.armor;
        }
    }

    public override List<Tooltip>? GetExtraTooltips()
        => [new TTGlossary("parttrait.armor")];
}
