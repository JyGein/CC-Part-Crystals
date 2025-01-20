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

public class Fragment : AttachableToPart
{
    public int v = Mutil.NextRandInt();
    public static readonly UK FragmentUK = ModEntry.Instance.Helper.Utilities.ObtainEnumCase<UK>();
    public static Dictionary<Type, string> FragmentColors =
    new() {
        {typeof(BlueFragment), "4DBBF4"},
        {typeof(CyanFragment), "6DFFE2"},
        {typeof(GreenFragment), "38FF94"},
        {typeof(MagentaFragment),"F26BFF"},
        {typeof(OrangeFragment), "fc872d"},
        {typeof(RedFragment), "FF6666"},
        {typeof(YellowFragment), "FFE25B"}
    };

    public override int GetSize() => 1;

    public virtual List<Tooltip>? GetExtraTooltips()
        => null;

    public Spr GetSprite()
    {
        return ModEntry.Instance.FragmentSprites[Key()].Sprite;
    }

    public override string Name()
    {
        return ModEntry.Instance.Localizations.Localize(["fragment", Key(), "name"]);
    }

    public override string Desc()
    {
        return ModEntry.Instance.Localizations.Localize(["fragment", Key(), playerOwned ? "playerDesc" : "enemyDesc"]);
    }

    public override UIKey UIKey()
    {
        return new UIKey(FragmentUK, v, Key());
    }

    public override List<Tooltip> GetTooltips()
    {
        List<Tooltip> list = new()
        {
            new TTDivider(),
            new TTText($"<c=artifact>{Name().ToUpper()}</c>\n{Desc()}")
        };
        if (GetExtraTooltips() != null)
        {
            list.AddRange(GetExtraTooltips()!);
        }
        return list;
    }

    public override void Render(G g, Vec restingPosition, bool autoFocus = false, OnMouseDown? onMouseDown = null, Color? color = null)
    {
        UIKey key = UIKey();
        Rect? rect = new Rect(0.0, 0.0, 5.0, 7.0) + restingPosition;
        bool autoFocus2 = autoFocus;
        Box box = g.Push(key, rect, null, autoFocus2, onMouseDown: onMouseDown);
        Vec vec = box.rect.xy;
        Spr spr = GetSprite();
        Vec vec2 = vec.round();
        Draw.Sprite(spr, vec2.x, vec2.y, color: color);
        if (box.IsHover())
        {
            Vec pos = vec2 + new Vec(7.0);
            g.tooltips.Add(pos, GetTooltips().Skip(1));
        }
        g.Pop();
    }
}
