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
    public List<Type> GetBaseFragmentTypes() => [];
    public List<Type> baseFragmentTypes = [];
    public virtual List<Tooltip>? GetExtraTooltips()
        => null;

    public List<Spr> GetSprites()
    {
        return baseFragmentTypes.Select(f => ModEntry.Instance.ItemSprites[f.Name].Sprite).ToList();
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
        List<Tooltip> list = new()
        {
            new TTDivider(),
            new TTText($"<c={Fragment.FragmentColors[baseFragmentTypes[0]]}>{Name().ToUpper()[0]}</c><c={Fragment.FragmentColors[baseFragmentTypes[1]]}>{Name().ToUpper()[1]}</c>\n{Desc()}")
        };
        if (GetExtraTooltips() != null)
        {
            list.AddRange(GetExtraTooltips()!);
        }
        foreach(Type type in baseFragmentTypes)
        {
            Fragment f = (Fragment)AccessTools.CreateInstance(type);
            f.playerOwned = playerOwned;
            list.AddRange(f.GetTooltips());
        }
        return list;
    }

    public override void Render(G g, Vec restingPosition, bool autoFocus = false, OnMouseDown? onMouseDown = null, Color? color = null)
    {
        UIKey? key = UIKey();
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
}
