using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PartCrystals.dumb_stupid_idiot_strings;
using Nanoray.Shrike.Harmony;
using Nanoray.Shrike;
using System.Reflection.Emit;
using Nickel;
using Microsoft.Extensions.Logging;
using PartCrystals.Fragments;
using Microsoft.Xna.Framework;

namespace PartCrystals.Features;

internal sealed class AttachableToPartManager
{
    public static UK OpenShipManagerButton = ModEntry.Instance.Helper.Utilities.ObtainEnumCase<UK>();
    //public static readonly string AttachableToPartKey = "PartManager";
    public AttachableToPartManager()
    {
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(MapRoute), nameof(MapRoute.Render)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(MapRoute_Render_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(MapRoute), nameof(MapRoute.OnMouseDown)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(MapRoute_OnMouseDown_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.RenderPartUI)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_RenderPartUI_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(ShipUpgrades), nameof(ShipUpgrades.Render)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(ShipUpgrades_Render_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(ShipUpgrades), nameof(ShipUpgrades.OnMouseDown)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(ShipUpgrades_OnMouseDown_Postfix))
        );
    }

    private static void MapRoute_Render_Postfix(MapRoute __instance, G g)
    {
        if (g.state.route == __instance)
        {
            Vec localV = new Vec(100.0, 230.0);
            UIKey key = OpenShipManagerButton;
            string text = ModEntry.Instance.Localizations.Localize(["uiShared", "btnPartManager"]);
            Color? textColor = Colors.textMain;
            Btn? platformButtonHint = null;
            SharedArt.ButtonText(g, localV, key, text, textColor, null, inactive: false, __instance, null, null, null, null, autoFocus: false, showAsPressed: false, gamepadUntargetable: false, hasDownState: false, platformButtonHint);
        }
    }

    private static void MapRoute_OnMouseDown_Postfix(MapRoute __instance, G g, Box b)
    {
        if (!(__instance.uiTimer < 0.5) || (FeatureFlags.Debug && Input.shift))
        {
            if (b.key == OpenShipManagerButton)
            {
                g.state.routeOverride = new ShipUpgrades() { outroTimer = 0.75 };
            }
        }
    }

    private static void Ship_RenderPartUI_Postfix(G g, Combat? combat, Part part, int localX, string keyPrefix, bool isPreview)
    {
        AddAttachableTTs(g, part, localX, keyPrefix);
        if(g.state.route is MapRoute && g.state.routeOverride is ShipUpgrades shipUpgrades && shipUpgrades.GetHeldAttachable() != null)
        {
            /*if (part.xLerped != (double)localX)
            {
                return;
            }
            Vec vec = new(localX * 16);
            int num = (isPreview ? 25 : 34);
            vec.y -= num - 6;
            Rect rect = new(vec.x - 1.0, vec.y, 17.0, num*2);
            Rect value = rect;
            value.h -= 8.0;
            g.Push(new UIKey(StableUK.part, localX, AttachableToPartKey), rect, value, onMouseDown: shipUpgrades);
            g.Pop();*/

            Box? box = g.boxes.FirstOrDefault(b => b.key == new UIKey(StableUK.part, localX, keyPrefix));
            if (box != null) box.onMouseDown = shipUpgrades;
        }
    }

    private static void AddAttachableTTs(G g, Part part, int localX, string keyPrefix)
    {
        if (g.boxes.FirstOrDefault(b => b.key == new UIKey(StableUK.part, localX, keyPrefix)) is not { } box)
            return;
        if (!box.IsHover())
            return;
        foreach (AttachableToPart attachableToPart in part.GetAttachables()) g.tooltips.Add(g.tooltips.pos, attachableToPart.GetTooltips());
    }

    private static void ShipUpgrades_Render_Postfix(ShipUpgrades __instance, G g)
    {
        if (g.state.route is not MapRoute || __instance.actionQueue.Count > 0 || __instance.completedActions.Count > 0) return;

        g.state.SetPlayerAttachables(g.state.GetPlayerAttachables().Count == 0 ? ModEntry.Instance.TempFragments : g.state.GetPlayerAttachables());

        List<AttachableToPart> attachables = g.state.GetPlayerAttachables();
        int startX = g.mg.PIX_W;
        attachables.ForEach(attachable => startX -= attachable is Fragment ? 7 : 12);
        startX /= 2;
        int num = 0;
        foreach (AttachableToPart attachable in attachables)
        {
            UIKey key = attachable.UIKey();
            Vec vec = new(startX + num, 60);
            int num3 = attachable is Fragment ? 5 : 10;
            num += num3 + 4;
            Rect rect = new Rect(0.0, 0.0, num3, 7) + vec;
            Box box = g.Push(key, rect, onMouseDown: __instance);
            Spr id = attachable.GetSprite();
            Color? color = attachable == __instance.GetHeldAttachable() ? Colors.smoke[0] : null;
            Draw.Sprite(id, vec.x, vec.y, color: color);
            Vec pos = box.rect.xy + new Vec(num3+2);
            if (box.IsHover()) g.tooltips.Add(pos, attachable.GetTooltips().Where(tt => tt is not TTDivider));
            g.Pop();
        }
    }

    private static void ShipUpgrades_OnMouseDown_Postfix(ShipUpgrades __instance, G g, Box b)
    {
        if (!b.key.HasValue) return;
        UIKey key = b.key.Value;
        if (key.k == StableUK.part && __instance.GetHeldAttachable() != null)
        {
            ModEntry.Instance.Logger.LogInformation("YIPPEE");
            AttachableToPart attachable = __instance.GetHeldAttachable()!;
            if (g.state.ship.parts[key.v].GetAttachables().Size() + attachable.GetSize() <= 4)
            {
                __instance.SetHeldAttachable(null);
                Part part = g.state.ship.parts[key.v];
                g.state.SetPlayerAttachables(g.state.GetPlayerAttachables().Where((a) => a != attachable).ToList());
                part.SetAttachables([.. part.GetAttachables(), attachable]);
                attachable.AttachedPart = part;
            }
        }
        if (key.k == Fragment.FragmentUK)
        {
            if(__instance.GetHeldAttachable() != null && __instance.GetHeldAttachable()!.UIKey() == key) __instance.SetHeldAttachable(null);
            else __instance.SetHeldAttachable(g.state.GetPlayerAttachables().First(a => a.UIKey() == key));
        }
    }
}

internal static class AttachableToPartExt
{
    public static List<AttachableToPart> GetAttachables(this Part part)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<List<AttachableToPart>>(part, DSIS.ATPMDS) ?? [];
    public static void SetAttachables(this Part part, List<AttachableToPart> attachables)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData<List<AttachableToPart>>(part, DSIS.ATPMDS, attachables);
    public static List<AttachableToPart> GetPlayerAttachables(this State state)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<List<AttachableToPart>>(state, DSIS.ATPMDS) ?? [];
    public static void SetPlayerAttachables(this State state, List<AttachableToPart> attachables)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData<List<AttachableToPart>>(state, DSIS.ATPMDS, attachables);
    public static AttachableToPart? GetHeldAttachable(this ShipUpgrades state)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<AttachableToPart>(state, DSIS.ATPMDS);
    public static void SetHeldAttachable(this ShipUpgrades state, AttachableToPart? attachable)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData<AttachableToPart>(state, DSIS.ATPMDS, attachable);
    public static int Size(this List<AttachableToPart> attachables)
    {
        int num = 0;
        foreach(AttachableToPart attachable in attachables) num += attachable.GetSize();
        return num;
    }
}
