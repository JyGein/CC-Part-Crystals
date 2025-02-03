using HarmonyLib;
using JyGein.PartCrystals;
using JyGein.PartCrystals.Artifacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals.Features;

internal sealed class CraftsChargeManager
{
    public CraftsChargeManager()
    {
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Events), nameof(Events.NewShop)),
            postfix: new HarmonyMethod(GetType(), nameof(Events_NewShop_Postfix))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(MapExit), nameof(MapExit.MakeRoute)),
            postfix: new HarmonyMethod(GetType(), nameof(MapExit_MakeRoute_Postfix))
        );
    }

    private static void GrantCrafts(State state, int amount)
    {
        if (amount <= 0)
            return;
        if (state.EnumerateAllArtifacts().OfType<CraftArtifact>().FirstOrDefault() is not { } artifact)
            return;

        artifact.CraftsLeft += amount;
        artifact.Pulse();
    }

    private static void Events_NewShop_Postfix(State s, ref List<Choice> __result)
    {
        if (!ModEntry.Instance.Settings.ProfileBased.Current.ShopCrafts)
            return;
        if (s.EnumerateAllArtifacts().OfType<CraftArtifact>().FirstOrDefault() is null)
            return;

        var indexToAppend = __result.FindIndex(c => c.key == ".shopAboutToDestroyYou");
        if (indexToAppend == -1)
            indexToAppend = __result.FindIndex(c => c.key == ".shopSkip_Confirm");

        __result.Insert(indexToAppend, new Choice
        {
            label = ModEntry.Instance.Localizations.Localize(["dialogue", "CraftanItem"]),
            key = ".shopUpgradeCard",
            actions = [new AGrantCraft()]
        });
    }

    private static void MapExit_MakeRoute_Postfix(State s)
        => GrantCrafts(s, ModEntry.Instance.Settings.ProfileBased.Current.CraftsAfterZone);

    private sealed class AGrantCraft : CardAction
    {
        public override void Begin(G g, State s, Combat c)
        {
            base.Begin(g, s, c);
            timer = 0;
            GrantCrafts(s, 1);
        }
    }
}
