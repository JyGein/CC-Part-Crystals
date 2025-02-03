using JyGein.PartCrystals.Fragments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JyGein.PartCrystals.Actions;
using JyGein.PartCrystals.Routes;

namespace JyGein.PartCrystals.Features;

public static class PartCrystalEvents
{
    public static List<Choice> FragmentSequence(State s)
    {
        Rand rng = new Rand(s.rngCurrentEvent.seed + 2512983);
        string key = AttachableToPartManager.afterFragmentSequenceKey;
        List<Choice> list = [];
        List<List<Fragment>> fragmentList = FragmentReward.GetOffering(s, 3, 2, rng);
        for (int i = 0; i < 3; i++)
        {
            list.Add(new Choice
            {
                label = ModEntry.Instance.Localizations.Localize(["dialogue", "FragmentSequence_Option"], new { first = fragmentList[i][0].Name().Remove(fragmentList[i][0].Name().Length-9), second = fragmentList[i][1].Name().Remove(fragmentList[i][1].Name().Length - 9), firstColor = Fragment.FragmentColors[fragmentList[i][0].GetType()], secondColor = Fragment.FragmentColors[fragmentList[i][1].GetType()], }),
                key = key,
                actions = [new AGainFragments(fragmentList[i].Cast<AttachableToPart>().ToList()), new AAttachSequence()]
            });
        }
        return list;
    }
}
