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
using System.Xml.Serialization;
using FSPRO;
using PartCrystals.Routes;

namespace PartCrystals.Features;

internal sealed class AttachableToPartManager
{
    public static UK OpenShipManagerButton = ModEntry.Instance.Helper.Utilities.ObtainEnumCase<UK>();
    public static UK CodexItems = ModEntry.Instance.Helper.Utilities.ObtainEnumCase<UK>();
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
            original: AccessTools.DeclaredMethod(typeof(Events), nameof(Events.NewShop)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Events_NewShop_Postfix))
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
        DB.eventChoiceFns = DB.eventChoiceFns
            .Concat(
                (from mi in typeof(PartCrystalEvents).GetMethods()
                    let p = mi.GetParameters()
                    where mi.IsStatic && 
                        p.Length == 1 && 
                        p[0].ParameterType == typeof(State) && 
                        mi.ReturnType == typeof(List<Choice>)
                    select mi
                ).ToDictionary(
                    info => info.Name + ModEntry.Instance.Package.Manifest.UniqueName, 
                    info => info
                )
            ).ToDictionary();
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
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.NormalDamage)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_NormalDamage_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.NormalDamage)),
            prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_NormalDamage_Prefix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.Make)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Combat_Make_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AAttack), nameof(AAttack.Begin)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AAttack_Begin_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(State), nameof(State.SeedRand)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(State_SeedRand_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(MapArtifact), nameof(MapArtifact.MakeRoute)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(MapArtifact_MakeRoute_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AAttack), nameof(AAttack.Begin)),
            prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AAttack_Begin_Prefix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.DirectHullDamage)),
            prefix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Ship_DirectHullDamage_Prefix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AMove), nameof(AMove.Begin)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AMove_Begin_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(AOverheat), nameof(AOverheat.Begin)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(AOverheat_Begin_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(G), nameof(G.Render)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(G_Render_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Codex), nameof(Codex.Render)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Codex_Render_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Codex), nameof(Codex.OnMouseDown)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Codex_OnMouseDown_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Dialogue), nameof(Dialogue.GetMusic)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Dialogue_GetMusic_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(ARailCannonStartPhase), nameof(ARailCannonStartPhase.Begin)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(ARailCannonStartPhase_Begin_Postfix))
        );
    }

    private static void ARailCannonStartPhase_Begin_Postfix(State s, Combat c) { if (c.otherShip.ai is RailCannon railCannon) AttachStuffToEnemies.Begin(s, railCannon, c, true); }

    private static void Dialogue_GetMusic_Postfix(Dialogue __instance, ref MusicState? __result)
    {
        if(__instance.ctx.script.StartsWith("FragmentSequence"))
        {
            __result = Music.@void;
        }
    }

    private static void G_Render_Postfix(G __instance)
    {
        //if (__instance.metaRoute != null)
        //{
        //    ModEntry.Instance.Logger.LogInformation(__instance.metaRoute.GetType().Name);
        //    if (__instance.metaRoute.subRoute != null)
        //    {
        //        ModEntry.Instance.Logger.LogInformation(__instance.metaRoute.subRoute.GetType().Name);
        //        if (__instance.metaRoute.subRoute is Codex codex && codex.subRoute != null)
        //        {
        //            ModEntry.Instance.Logger.LogInformation(codex.subRoute.GetType().Name);
        //        }
        //    }
        //}
        //ModEntry.Instance.Logger.LogInformation(__instance.state.route.GetType().Name);
        //if (__instance.state.routeOverride != null) ModEntry.Instance.Logger.LogInformation(__instance.state.routeOverride.GetType().Name);
    }

    private static void Codex_Render_Postfix(Codex __instance, G g)
    {
        if (__instance.subRoute != null)
        {
            return;
        }
        Rect? rect = new Rect(0.0, 0.0, 0.0, 0.0);
        int num = 180;
        rect = new Rect(240 - num / 2, 100.0);
        g.Push(null, rect);
        SharedArt.MenuItem(g, new Vec(-10.0, 126.0), num, isBig: false, CodexItems, ModEntry.Instance.Localizations.Localize(["uiText", "codex", "items"]), __instance);
        g.Pop();
    }

    private static void Codex_OnMouseDown_Postfix(Codex __instance, G g, Box b)
    {
        if (b.key == CodexItems)
        {
            Audio.Play(Event.Click);
            __instance._lastSelected = Input.currentGpKey;
            __instance.subRoute = new ItemBrowse();
        }
    }

    private static void MapRoute_Render_Postfix(MapRoute __instance, G g)
    {
        if (g.state.route == __instance)
        {
            Vec localV = new Vec(100.0 + (g.state.ship.parts.Count > 5 ? (g.state.ship.parts.Count - 5) * 16 : 0 ), 230.0);
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

    private static void Ship_RenderPartUI_Postfix(Ship __instance, G g, Combat? combat, Part part, int localX, string keyPrefix, bool isPreview)
    {
        AddAttachableTTs(g, part, localX, keyPrefix);
        if (combat != null && !isPreview)
        {
            List<AttachableToPart> attachables = [.. part.GetAttachables().OrderBy(a => a is Fragment ? "b" : "a")];
            int num = 0;
            foreach (AttachableToPart attachable in attachables)
            {
                Vec vec = new(localX * 16);
                int num1 = 11 + (__instance.isPlayerShip ? 10 : -37);
                vec.y += num1;
                Rect rect = new(vec.x - 1.0 + 3 + (num % 2 * 6), vec.y + (num / 2 * 8), 11, 7);
                attachable.Render(g, rect.xy);
                num += attachable.GetSize();
            }
        }
        if (keyPrefix != "shipUpgrade_shipPreview") return;
        ShipUpgrades? shipUpgrades = null;
        if (g.state.route is MapRoute && g.state.routeOverride is ShipUpgrades upgrades1) shipUpgrades = upgrades1;
        else if (g.state.route is Dialogue dialogue && dialogue.routeOverride is ShipUpgrades upgrades && (upgrades.GetIsActuallyCrafting() || upgrades.GetIsSpecialAttachSequence())) shipUpgrades = upgrades;
        if (shipUpgrades != null)
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
                Vec vec = new(localX * 16);
                int num1 = 11;
                vec.y += num1;
                Rect rect = new(vec.x - 1.0 + 3 + (num % 2 * 6), vec.y + (num / 2 * 8), 11, 7);
                Color? color = attachable == shipUpgrades.GetHeldAttachable() ? Colors.smoke[0] : null;
                attachable.Render(g, rect.xy, false, shipUpgrades, color);
                num += attachable.GetSize();
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
        if ((g.state.route is not MapRoute || __instance.actionQueue.Count > 0 || __instance.completedActions.Count > 0) && !__instance.GetIsActuallyCrafting() && !__instance.GetIsSpecialAttachSequence()) return;

        List<AttachableToPart> attachables = g.state.GetPlayerAttachables();
        int startX = g.mg.PIX_W;
        attachables.ForEach(attachable => startX -= attachable is Fragment ? 9 : 15);
        startX /= 2;
        //startX -= 1;
        int num = 0;
        foreach (AttachableToPart attachable in attachables)
        {
            Vec vec = new(startX + num, 60);
            int num3 = attachable is Fragment ? 5 : 11;
            num += num3 + 4;
            Color? color = attachable == __instance.GetHeldAttachable() ? Colors.smoke[0] : null;
            attachable.Render(g, vec, onMouseDown:__instance, color: color);
        }
    }

    private static void ShipUpgrades_OnMouseDown_Postfix(ShipUpgrades __instance, G g, Box b)
    {
        if (!b.key.HasValue) return;
        UIKey key = b.key.Value;
        if (key.k == StableUK.part && __instance.GetHeldAttachable() != null && !__instance.GetIsActuallyCrafting())
        {
            AttachableToPart attachable = __instance.GetHeldAttachable()!;
            Part part = g.state.ship.parts[key.v];
            if (part.GetAttachables().Size() + attachable.GetSize() <= 4 && part.type != PType.empty)
            {
                __instance.SetHeldAttachable(null);
                g.state.SetPlayerAttachables(g.state.GetPlayerAttachables().Where(a => a != attachable).ToList());
                part.SetAttachables([.. part.GetAttachables(), attachable]);
                attachable.OnPartAttached(g.state, part);
            }
        }
        if (key.k == Fragment.FragmentUK || key.k == Item.ItemUK)
        {
            if (__instance.GetIsActuallyCrafting() && key.k == Item.ItemUK) return;
            if (__instance.GetHeldAttachable() != null && __instance.GetHeldAttachable()!.UIKey() == key) __instance.SetHeldAttachable(null);
            else if (__instance.GetIsActuallyCrafting() && __instance.GetHeldAttachable() != null)
            {
                Fragment fragment1 = (Fragment)__instance.GetHeldAttachable()!;
                Fragment fragment2 = (Fragment)(g.state.GetPlayerAttachables()
                    .FirstOrDefault(
                        f => f.UIKey() == key
                    ) ?? g.state.ship.parts
                    .First(
                        p => p.GetAttachables()
                            .Any(
                                a => a.UIKey() == key
                            )
                    ).GetAttachables()
                        .First(
                            a => a.UIKey() == key
                        )
                );
                Item item = (Item)AccessTools.CreateInstance(ModEntry.Instance.fragmentFragmentToItem[fragment1.GetType()][fragment2.GetType()]);
                g.state.SetPlayerAttachables([.. g.state.GetPlayerAttachables().Where(a => a.UIKey() != key && a.UIKey() != __instance.GetHeldAttachable()!.UIKey()), item]);
                g.state.ship.parts.ForEach(p => p.SetAttachables(p.GetAttachables().Where(a => a.UIKey() != key && a.UIKey() != __instance.GetHeldAttachable()!.UIKey()).ToList()));
                g.state.route.SetHasCraftedHere(true);
                g.CloseRoute(__instance);
            }
            else if (g.state.GetPlayerAttachables().Any(a => a.UIKey() == key) || (g.state.ship.parts.Any(p => p.GetAttachables().Any(a => a.UIKey() == key)) && __instance.GetIsActuallyCrafting())) __instance.SetHeldAttachable(g.state.GetPlayerAttachables().Any(a => a.UIKey() == key) ? g.state.GetPlayerAttachables().First(a => a.UIKey() == key) : g.state.ship.parts.First(p => p.GetAttachables().Any(a => a.UIKey() == key)).GetAttachables().First(a => a.UIKey() == key));
            else
            {
                Part part = g.state.ship.parts.First(p => p.GetAttachables().Count(a => a.UIKey() == key) > 0);
                AttachableToPart attachable = part.GetAttachables().First(a => a.UIKey() == key);
                g.state.SetPlayerAttachables([.. g.state.GetPlayerAttachables(), attachable]);
                part.SetAttachables(part.GetAttachables().Where(a => a != attachable).ToList());
                attachable.OnPartDetached(g.state, part);
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

    private static void Events_NewShop_Postfix(State s, ref List<Choice> __result)
    {
        if (s.GetPlayerAttachables().Count(a => a is Fragment) + s.ship.parts.SelectMany(p => p.GetAttachables()).Count(a => a is Fragment) < 2 || s.route.GetHasCraftedHere()) return;
        __result.Insert(__result.Count-2, new Choice
        {
            label = ModEntry.Instance.Localizations.Localize(["dialogue", "CraftanItem"]),
            key = "NewShop",
            actions = { new AItemCrafting() }
        });
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
    {
        s.ship.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnTurnStart(s, c, p)));
        c.otherShip.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnOtherShipTurnStart(s, c, p)));
    }

    private static void AEnemyTurn_Begin_Postfix(State s, Combat c)
    {
        c.otherShip.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnTurnStart(s, c, p)));
        s.ship.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnOtherShipTurnStart(s, c, p)));
    }

    private static void AAfterPlayerTurn_Begin_Postfix(State s, Combat c)
        => s.ship.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnTurnEnd(s, c, p)));

    private static void AEnemyTurnAfter_Begin_Postfix(State s, Combat c)
    {
        if (c.turn != 0) c.otherShip.parts.Reverse<Part>().ToList().ForEach(p => p.GetAttachables().ForEach(a => a.OnTurnEnd(s, c, p)));
    }

    private static void Ship_NormalDamage_Postfix(State s, Combat c, int? maybeWorldGridX, DamageDone __result, Ship __instance)
    {
        if(maybeWorldGridX.HasValue)
        {
            Part? part = __instance.GetPartAtWorldX(maybeWorldGridX.GetValueOrDefault())!;
            part?.GetAttachables().ForEach(a => a.OnPartHit(s, c, part, __result));

            if (!c.stuff.TryGetValue(maybeWorldGridX.GetValueOrDefault(), out _))
            {
                Ship attackingShip = s.ship == __instance ? c.otherShip : s.ship;
                Part? part2 = attackingShip.GetPartAtWorldX(maybeWorldGridX.GetValueOrDefault());
                part2?.GetAttachables().ForEach(a => a.OnPartDamages(s, c, part2, __result, __instance));
            }
        }
    }

    private static void Ship_NormalDamage_Prefix(State s, Combat c, int incomingDamage, int? maybeWorldGridX, Ship __instance)
    {
        if (maybeWorldGridX.HasValue)
        {
            Part? part = __instance.GetPartAtWorldX(maybeWorldGridX.GetValueOrDefault())!;
            part?.GetAttachables().ForEach(a => a.BeforePartHit(s, c, part, incomingDamage));
        }
    }

    private static void Combat_Make_Postfix(State s, AI ai, bool doForReal, Combat __result)
    {
        AttachStuffToEnemies.Begin(s, ai, __result);
        if(doForReal)
        {
            __result.otherShip.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnCombatStart(s, __result, p)));
            s.ship.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnCombatStart(s, __result, p)));
        }
    }

    private static void AAttack_Begin_Postfix(AAttack __instance, G g, State s, Combat c)
    {
        Ship ship = __instance.targetPlayer ? c.otherShip : s.ship;
        if (!__instance.fromDroneX.HasValue && !__instance.fromX.HasValue && !__instance.multiCannonVolley && !__instance.targetPlayer)
        {
            ship.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnPlayerShipShoots(s, c, p)));
        }
        if (__instance.fromX.HasValue || ship.GetPartTypeCount(PType.cannon) == 1)
        {
            int? x = __instance.GetFromX(s, c);
            Part? part = ship.GetPartAtLocalX(x.GetValueOrDefault());
            part?.GetAttachables().ForEach(a => a.OnPartAttacks(s, c, part));
        }
    }

    private static void State_SeedRand_Postfix(State __instance, uint seed)
        => __instance.SetrngFragmentOfferings(new Rand(seed).Offshoot());

    private static void MapArtifact_MakeRoute_Postfix(State s)
        => s.rewardsQueue.Add(new AFragmentOffering());

    private static void AAttack_Begin_Prefix(AAttack __instance, G g, State s, Combat c)
    {
        int? x = __instance.GetFromX(s, c);
        Ship ship = __instance.targetPlayer ? c.otherShip : s.ship;
        if (x.HasValue || (ship.GetPartTypeCount(PType.cannon) == 1 && !__instance.targetPlayer))
        {
            Part? part = ship.GetPartAtLocalX(x.GetValueOrDefault());
            if (part != null)
            {
                part.GetAttachables().ForEach(a => a.AlterAttackFromPart(s, c, part, __instance));
            }
        }
    }

    private static void Ship_DirectHullDamage_Prefix(Ship __instance, State s, Combat c, ref int amt)
    {
        foreach (Part part in __instance.parts)
        {
            foreach (AttachableToPart attachable in part.GetAttachables())
            {
                attachable.AlterHullDamage(s, c, __instance, ref amt);
            }
        }
    }

    private static void AMove_Begin_Postfix(AMove __instance, G g, State s, Combat c)
    {
        Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
        ship.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnShipMoves(s, c, p)));
        Ship ship2 = __instance.targetPlayer ? c.otherShip : s.ship;
        ship2.parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnOtherShipMoves(s, c, p)));
    }

    private static void AOverheat_Begin_Postfix(AOverheat __instance, G g, State s, Combat c)
        => (__instance.targetPlayer ? s.ship : c.otherShip).parts.ForEach(p => p.GetAttachables().ForEach(a => a.OnShipOverheats(s, c, p)));
}

internal static partial class AttachableToPartExt
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
    public static Rand GetrngFragmentOfferings(this State state)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<Rand>(state, DSIS.rngFragmentOfferings) ?? new();
    public static void SetrngFragmentOfferings(this State state, Rand rand)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData<Rand>(state, DSIS.rngFragmentOfferings, rand);
    public static int Size(this List<AttachableToPart> attachables)
    {
        int num = 0;
        foreach(AttachableToPart attachable in attachables) num += attachable.GetSize();
        return num;
    }
    public static bool GetIsActuallyCrafting(this ShipUpgrades shipUpgrades)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<bool>(shipUpgrades, DSIS.ShipUpgradesIsActuallyCrafting) ?? false;
    public static void SetIsActuallyCrafting(this ShipUpgrades shipUpgrades, bool b)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData<bool>(shipUpgrades, DSIS.ShipUpgradesIsActuallyCrafting, b);
    public static bool GetHasCraftedHere(this Route route)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<bool>(route, DSIS.HasCraftedHere) ?? false;
    public static void SetHasCraftedHere(this Route route, bool b)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData<bool>(route, DSIS.HasCraftedHere, b);
    public static bool GetIsSpecialAttachSequence(this ShipUpgrades shipUpgrades)
        => ModEntry.Instance.Helper.ModData.GetOptionalModData<bool>(shipUpgrades, DSIS.IsSpecialAttachSequence) ?? false;
    public static void SetIsSpecialAttachSequence(this ShipUpgrades shipUpgrades, bool b)
        => ModEntry.Instance.Helper.ModData.SetOptionalModData<bool>(shipUpgrades, DSIS.IsSpecialAttachSequence, b);
    public static Fragment ChangedPlayerOwned(this Fragment fragment, bool playerOwned = false)
    {
        fragment.playerOwned = playerOwned;
        return fragment;
    }
}
