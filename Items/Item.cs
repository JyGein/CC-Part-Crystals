using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
//using PartCrystals.Actions;
//using PartCrystals.Cards;
using PartCrystals.External;
using PartCrystals.dumb_stupid_idiot_strings;
using System.Reflection;
using PartCrystals.Features;
using Microsoft.Xna.Framework.Graphics;

namespace PartCrystals.Fragments;

public class Item : AttachableToPart
{
    public int v = Mutil.NextRandInt();
    public static readonly UK ItemUK = ModEntry.Instance.Helper.Utilities.ObtainEnumCase<UK>();
    public override int GetSize() => 2;
    public virtual List<Type> GetBaseFragmentTypes() => [];
    public virtual List<Tooltip>? GetExtraTooltips()
        => null;

    public List<Spr> GetSprites()
    {
        return GetBaseFragmentTypes().Select(f => ModEntry.Instance.ItemSprites[f.Name].Sprite).ToList();
    }

    public override string Name()
    {
        return ModEntry.Instance.Localizations.Localize(["item", Key(), "name"]);
    }

    public override string Desc()
    {
        return ModEntry.Instance.Localizations.Localize(["item", Key(), playerOwned ? "playerDesc" : "enemyDesc"]);
    }

    public override UIKey UIKey()
    {
        return new UIKey(ItemUK, v, Key());
    }

    public override List<Tooltip> GetTooltips()
    {
        List<Tooltip> list =
        [
            new TTDivider(),
            new TTText($"<c={Fragment.FragmentColors[GetBaseFragmentTypes()[0]]}>{Name().ToUpper()[0]}</c><c={Fragment.FragmentColors[GetBaseFragmentTypes()[1]]}>{Name().ToUpper()[1]}</c>\n{Desc()}")
        ];
        if (GetExtraTooltips() != null)
        {
            list.AddRange(GetExtraTooltips()!);
        }
        foreach(Type type in GetBaseFragmentTypes())
        {
            Fragment f = (Fragment)AccessTools.CreateInstance(type);
            f.playerOwned = playerOwned;
            list.AddRange(f.GetTooltips());
        }
        return list;
    }

    public override void Render(G g, Vec restingPosition, bool autoFocus = false, OnMouseDown? onMouseDown = null, Color? color = null)
    {
        UIKey key = UIKey();
        Rect? rect = new Rect(0.0, 0.0, 11.0, 7.0) + restingPosition;
        bool autoFocus2 = autoFocus;
        Box box = g.Push(key, rect, null, autoFocus2, onMouseDown: onMouseDown);
        Vec vec = box.rect.xy;
        List<Spr> sprs = GetSprites();
        Vec vec2 = vec.round();
        Draw.Sprite(sprs[0], vec2.x, vec2.y, color: color);
        Draw.Sprite(sprs[1], vec2.x, vec2.y, true, true, color: color);
        if (box.IsHover())
        {
            Vec pos = vec2 + new Vec(13.0);
            g.tooltips.Add(pos, GetTooltips().Skip(1));
        }
        g.Pop();
    }
    public override void OnCombatStart(State state, Combat combat, Part part) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnCombatStart(state, combat, part)); }
    public override void OnTurnStart(State state, Combat combat, Part part) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnTurnStart(state, combat, part)); }
    public override void OnOtherShipTurnStart(State state, Combat combat, Part part) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnOtherShipTurnStart(state, combat, part)); }
    public override void OnTurnEnd(State state, Combat combat, Part part) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnTurnEnd(state, combat, part)); }
    public override void OnPartHit(State state, Combat combat, Part part, DamageDone damageDone) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnPartHit(state, combat, part, damageDone)); }
    public override void BeforePartHit(State state, Combat combat, Part part, int incomingDamage) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).BeforePartHit(state, combat, part, incomingDamage)); }
    public override void OnPlayerShipShoots(State state, Combat combat, Part part) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnPlayerShipShoots(state, combat, part)); }
    public override void AlterAttackFromPart(State state, Combat combat, Part part, AAttack aAttack) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).AlterAttackFromPart(state, combat, part, aAttack)); }
    public override void AlterHullDamage(State state, Combat combat, Ship ship, ref int amt) { foreach (Type t in GetBaseFragmentTypes()) ((Fragment)AccessTools.CreateInstance(t)).AlterHullDamage(state, combat, ship, ref amt); }
    public override void OnPartAttacks(State state, Combat combat, Part part) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnPartAttacks(state, combat, part)); }
    public override void OnShipMoves(State state, Combat combat, Part part) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnShipMoves(state, combat, part)); }
    public override void OnOtherShipMoves(State state, Combat combat, Part part) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnOtherShipMoves(state, combat, part)); }
    public override void OnShipOverheats(State state, Combat combat, Part part) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnShipOverheats(state, combat, part)); }
    public override void OnPartDamages(State state, Combat combat, Part part, DamageDone damageDone, Ship ship) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnPartDamages(state, combat, part, damageDone, ship)); }
    public override void OnPartAttached(State state, Part part) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnPartAttached(state, part)); }
    public override void OnPartDetached(State state, Part part) { GetBaseFragmentTypes().ForEach(t => ((Fragment)AccessTools.CreateInstance(t)).OnPartDetached(state, part)); }
}
