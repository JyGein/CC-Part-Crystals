using JyGein.PartCrystals;
using System.Collections.Generic;

namespace JyGein.PartCrystals.Artifacts;

internal sealed class CraftArtifact : Artifact
{
    public int CraftsLeft = ModEntry.Instance.Settings.ProfileBased.Current.InitialCrafts;

    public override int? GetDisplayNumber(State s)
        => CraftsLeft;

    public override List<Tooltip>? GetExtraTooltips()
        => [
            .. base.GetExtraTooltips() ?? [],
            new TTText(ModEntry.Instance.Localizations.Localize(["artifact", "extraDescription", CraftsLeft switch
            {
                0 => "zero",
                1 => "one",
                _ => "other"
            }], new { Amount = CraftsLeft }))
        ];
}
