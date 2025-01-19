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
using System.Drawing;
using PartCrystals.Actions;

namespace PartCrystals.Features;

internal sealed class AttachableToPartManager
{
    public static UK OpenShipManagerButton = ModEntry.Instance.Helper.Utilities.ObtainEnumCase<UK>();
    public static string afterFragmentSequenceKey = ".zone_first";
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
            original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.DrawTopLayer)),
            transpiler: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_DrawTopLater_Transpiler))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(ShipUpgrades), nameof(ShipUpgrades.Render)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(ShipUpgrades_Render_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(ShipUpgrades), nameof(ShipUpgrades.OnMouseDown)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(ShipUpgrades_OnMouseDown_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Events), nameof(Events.BootSequence)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Events_BootSequence_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(DB), nameof(DB.SetLocale)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(DB_SetLocale_Postfix))
        );
        DB.story.all["FragmentSequence" + ModEntry.Instance.Package.Manifest.UniqueName] = new StoryNode
        {
            type = NodeType.@event,
            bg = "BGBootSequence",
            lines = [
                    new Say {
                        who = "comp",
                        loopTag = "loading3",
                        hash = "beb77013"
                    }
                ],
            choiceFunc = "FragmentSequence" + ModEntry.Instance.Package.Manifest.UniqueName
        };
        DB.eventChoiceFns = DB.eventChoiceFns.Concat((from mi in typeof(PartCrystalEvents).GetMethods()
                                 let p = mi.GetParameters()
                                 where mi.IsStatic && p.Length == 1 && p[0].ParameterType == typeof(State) && mi.ReturnType == typeof(List<Choice>)
                                 select mi).ToDictionary((MethodInfo info) => info.Name + ModEntry.Instance.Package.Manifest.UniqueName, (MethodInfo info) => info)).ToDictionary();
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(State), nameof(State.PopulateRun)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(State_PopulateRun_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.PlayerWon)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Combat_PlayerWon_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AStartPlayerTurn), nameof(AStartPlayerTurn.Begin)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AStartPlayerTurn_Begin_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AEnemyTurn), nameof(AEnemyTurn.Begin)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AEnemyTurn_Begin_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AAfterPlayerTurn), nameof(AAfterPlayerTurn.Begin)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AAfterPlayerTurn_Begin_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AEnemyTurnAfter), nameof(AEnemyTurnAfter.Begin)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AEnemyTurnAfter_Begin_Postfix))
        );
    }

    private static void MapRoute_Render_Postfix(MapRoute __instance, G g)
    {
        if (g.state.route == __instance)
        {
            Vec localV = new Vec(100.0, 230.0);
            UIKey key = OpenShipManagerButton;
            string text = ModEntry.Instance.Localizations.Localize(["uiText", "btnPartManager"]);
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
        if(g.state.route is MapRoute && g.state.routeOverride is ShipUpgrades shipUpgrades)
        {
            if (shipUpgrades.GetHeldAttachable() != null)
            {
                Box? box = g.boxes.FirstOrDefault(b => b.key == new UIKey(StableUK.part, localX, keyPrefix));
                if (box != null) box.onMouseDown = shipUpgrades;
            }
            List<AttachableToPart> attachables = [.. part.GetAttachables().OrderBy(a => a is Fragment ? "b" : "a")];
            int num = 0;
            foreach(AttachableToPart attachable in attachables)
            {
                if (attachable is Fragment)
                {
                    Vec vec = new(localX * 16);
                    int num1 = 11;
                    vec.y += num1;
                    Rect rect = new(vec.x - 1.0 + 3 + (num % 2 * 6), vec.y + (num/2 * 8), 5, 7);
                    Box box = g.Push(attachable.UIKey(), rect, onMouseDown: shipUpgrades);
                    Vec pos = box.rect.xy + new Vec(5 + 2);
                    if (box.IsHover()) g.tooltips.Add(pos, attachable.GetTooltips().Where(tt => tt is not TTDivider));
                    num++;
                    Spr id = attachable.GetSprite();
                    Vec xy = box.rect.xy;
                    Draw.Sprite(id, xy.x, xy.y);
                    g.Pop();
                }
            }
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

        List<AttachableToPart> attachables = g.state.GetPlayerAttachables();
        int startX = g.mg.PIX_W;
        attachables.ForEach(attachable => startX -= attachable is Fragment ? 7 : 13);
        startX /= 2;
        startX -= 1;
        int num = 0;
        foreach (AttachableToPart attachable in attachables)
        {
            UIKey key = attachable.UIKey();
            Vec vec = new(startX + num, 60);
            int num3 = attachable is Fragment ? 5 : 11;
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
            AttachableToPart attachable = __instance.GetHeldAttachable()!;
            Part part = g.state.ship.parts[key.v];
            if (part.GetAttachables().Size() + attachable.GetSize() <= 4 && part.type != PType.empty)
            {
                __instance.SetHeldAttachable(null);
                g.state.SetPlayerAttachables(g.state.GetPlayerAttachables().Where((a) => a != attachable).ToList());
                part.SetAttachables([.. part.GetAttachables(), attachable]);
            }
        }
        if (key.k == Fragment.FragmentUK)
        {
            if(__instance.GetHeldAttachable() != null && __instance.GetHeldAttachable()!.UIKey() == key) __instance.SetHeldAttachable(null);
            else if (g.state.GetPlayerAttachables().Any(a => a.UIKey() == key)) __instance.SetHeldAttachable(g.state.GetPlayerAttachables().First(a => a.UIKey() == key));
            else
            {
                Part part = g.state.ship.parts.Where(p => p.GetAttachables().Where(a => a.UIKey() == key).Count() > 0).First();
                AttachableToPart attachable = part.GetAttachables().Where(a => a.UIKey() == key).First();
                g.state.SetPlayerAttachables([.. g.state.GetPlayerAttachables(), attachable]);
                part.SetAttachables(part.GetAttachables().Where(a => a != attachable).ToList());
            }
        }
    }

    private static IEnumerable<CodeInstruction> Ship_DrawTopLater_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(
                    ILMatches.LdcR8(1),
                    ILMatches.LdcR8(1),
                    ILMatches.LdcR8(1)
                )
                .Find(
                    ILMatches.Newobj(AccessTools.DeclaredConstructor(typeof(Color), [typeof(double), typeof(double), typeof(double), typeof(double)]))
                )
                .Insert(
                    SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion,
                    new CodeInstruction(OpCodes.Ldloc_S, 4),
                    new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(ChangeColorIfEmpowered)))
                )
                .AllElements();
        }
        catch (Exception ex)
        {
            ModEntry.Instance.Logger.LogError("Could not patch method {Method} - {Mod} probably won't work.\nReason: {Exception}", originalMethod, ModEntry.Instance.Package.Manifest.GetDisplayName(@long: false), ex);
            return instructions;
        }
    }

    private static Color ChangeColorIfEmpowered(Color color, Part part)
        => part.GetAttachables().Count > 0 ? new Color(254.0/255.0, 184.0/255.0, 255.0/255.0, color.a) : color;

    private static void Events_BootSequence_Postfix(ref List<Choice> __result)
    {
        foreach(Choice choice in __result)
        {
            afterFragmentSequenceKey = choice.key!;
            choice.key = "FragmentSequence" + ModEntry.Instance.Package.Manifest.UniqueName;
        }
    }

    private static void DB_SetLocale_Postfix(string locale, bool useHiRes)
    {
        DB.currentLocale.strings.TryAdd($"FragmentSequence{ModEntry.Instance.Package.Manifest.UniqueName}:beb77013", ModEntry.Instance.Localizations.Localize(["dialogue", "FragmentSequence"]));
    }

    private static void State_PopulateRun_Postfix(State __instance)
        => __instance.SetPlayerAttachables([]);

    private static void Combat_PlayerWon_Postfix(Combat __instance, G g)
    {
        State s = g.state;
        if (s.map.markers[s.map.currentLocation].contents is MapBattle mapBattle)
        {
            bool flag = mapBattle.battleType == BattleType.Boss && s.map.IsFinalZone();
            if (!__instance.noReward)
            {
                if (mapBattle.battleType == BattleType.Elite || mapBattle.battleType == BattleType.Boss)
                {
                    s.rewardsQueue.Queue(new AFragmentOffering());
                }
            }
        }
    }

    private static void AStartPlayerTurn_Begin_Postfix(State s, Combat c)
        => s.ship.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnTurnStart(s, c, p)));

    private static void AEnemyTurn_Begin_Postfix(State s, Combat c)
        => c.otherShip.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnTurnStart(s, c, p)));

    private static void AAfterPlayerTurn_Begin_Postfix(State s, Combat c)
        => s.ship.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnTurnEnd(s, c, p)));

    private static void AEnemyTurnAfter_Begin_Postfix(State s, Combat c)
        => c.otherShip.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnTurnEnd(s, c, p)));
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
