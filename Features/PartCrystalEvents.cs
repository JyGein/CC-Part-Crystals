using PartCrystals.Fragments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartCrystals.Actions;

namespace PartCrystals.Features;

public static class PartCrystalEvents
{
    public static List<Choice> FragmentSequence(State s)
    {
        Rand rng = new Rand(s.rngCurrentEvent.seed + 2512983);
        string key = AttachableToPartManager.afterFragmentSequenceKey;
        List<Choice> list = new List<Choice>();
        List<Type> fragmentTypes = ModEntry.Instance.fragmentTypes;
        for (int i = 0; i < 3; i++)
        {
            List<Type> chosenFragments = new()
            {
                fragmentTypes.Shuffle(rng).First(),
                fragmentTypes.Shuffle(rng).First()
            };
            list.Add(new Choice
            {
                label = ModEntry.Instance.Localizations.Localize(["dialogue", "FragmentSequence_Option"], new { first = chosenFragments[0].Name.Remove(chosenFragments[0].Name.Length-8), second = chosenFragments[1].Name.Remove(chosenFragments[1].Name.Length - 8), firstColor = Fragment.FragmentColors[chosenFragments[0]], secondColor = Fragment.FragmentColors[chosenFragments[1]], }),
                key = key,
                actions = [new AGainFragments(chosenFragments)]
            });
        }
        return list;
    }
}
