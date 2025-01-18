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
using PartCrystals.Fragments;
using System.Runtime.CompilerServices;

namespace PartCrystals;

internal class ModEntry : SimpleMod
{
    internal static ModEntry Instance { get; private set; } = null!;
    internal Harmony Harmony;
    internal IKokoroApi.IV2 KokoroApi;
    //internal IDeckEntry DemoDeck;
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    public IStatusEntry HalfEvade;
    public IStatusEntry HalfShield;
    public IStatusEntry HalfTempShield;
    public IStatusEntry HalfDamage;
    public IStatusEntry QuarterEvade;
    public IStatusEntry HalfHeal;
    public IStatusEntry QuarterHeal;
    public Dictionary<string, ISpriteEntry> FragmentSprites; //NEED TO MAKE
    //public Dictionary<string, ISpriteEntry> ItemSprites; //NEED TO MAKE

    public List<AttachableToPart> TempFragments = new() { new BlueFragment(), new RedFragment(), new GreenFragment(), new YellowFragment(), new MagentaFragment(), new CyanFragment(), new OrangeFragment() };

    /*
     * The following lists contain references to all types that will be registered to the game.
     * All cards and artifacts must be registered before they may be used in the game.
     * In theory only one collection could be used, containing all registerable types, but it is seperated this way for ease of organization.
     */

    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        Harmony = new Harmony("JyGein.PartCrystals");

        /*
         * Some mods provide an API, which can be requested from the ModRegistry.
         * The following is an example of a required dependency - the code would have unexpected errors if Kokoro was not present.
         * Dependencies can (and should) be defined within the nickel.json file, to ensure proper mod load order.
         */
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;
        
        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );

        /*
         * A deck only defines how cards should be grouped, for things such as codex sorting and Second Opinions.
         * A character must be defined with a deck to allow the cards to be obtainable as a character's cards.
         */
        /*(DemoDeck = helper.Content.Decks.RegisterDeck("Demo", new DeckConfiguration
        {
            Definition = new DeckDef
            {
                /*
                 * This color is used in a few places:
                 * TODO On cards, it dictates the sheen on higher rarities, as well as influences the color of the energy cost.
                 * If this deck is given to a playable character, their name will be this color, and their mini will have this color as their border.
                 */
                /*color = new Color("999999"),

                titleColor = new Color("000000")
            },

            DefaultCardArt = StableSpr.cards_colorless,
            BorderSprite = RegisterSprite(package, "assets/frame_dave.png").Sprite,
            Name = AnyLocalizations.Bind(["character", "name"]).Localize
        });*/

        /*
         * All the IRegisterable types placed into the static lists at the start of the class are initialized here.
         * This snippet invokes all of them, allowing them to register themselves with the package and helper.
         */
        //foreach (var type in AllRegisterableTypes)
        //    AccessTools.DeclaredMethod(type, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);

        /*
         * Characters have required animations, recommended animations, and you have the option to add more.
         * In addition, they must be registered before the character themselves is registered.
         * The game requires you to have a neutral animation and mini animation, used for normal gameplay and the map and run start screen, respectively.
         * The game uses the squint animation for the Extra-Planar Being and High-Pitched Static events, and the gameover animation while you are dying.
         * You may define any other animations, and they will only be used when explicitly referenced (such as dialogue).
         */
        List<Fragment> fragments = [new BlueFragment(), new RedFragment(), new GreenFragment(), new YellowFragment(), new MagentaFragment(), new CyanFragment(), new OrangeFragment()];
        FragmentSprites = new();
        foreach(Fragment fragment in fragments) FragmentSprites[fragment.Key()] = RegisterSprite(package, $"assets/fragments/{fragment.Key()}.png");
        /*
         * Statuses are used to achieve many mechanics.
         * However, statuses themselves do not contain any code - they just keep track of how much you have.
         */
        HalfHeal = helper.Content.Statuses.RegisterStatus("HalfHeal", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = false,
                color = Colors.heal,
                icon = RegisterSprite(package, "assets/icons/HalfHeal.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "HalfHeal", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "HalfHeal", "desc"]).Localize
        });
        QuarterHeal = helper.Content.Statuses.RegisterStatus("QuarterHeal", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = false,
                color = Colors.heal,
                icon = RegisterSprite(package, "assets/icons/QuarterHeal.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "QuarterHeal", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "QuarterHeal", "desc"]).Localize
        });
        QuarterEvade = helper.Content.Statuses.RegisterStatus("QuarterEvade", new StatusConfiguration
        {
            Definition = new StatusDef
            {
                isGood = true,
                affectedByTimestop = false,
                color = Colors.status,
                icon = RegisterSprite(package, "assets/icons/QuarterEvade.png").Sprite
            },
            Name = AnyLocalizations.Bind(["status", "QuarterEvade", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "QuarterEvade", "desc"]).Localize
        });
        _ = new PartialStatusManager();

        /*
         * Managers are typically made to register themselves when constructed.
         * _ = makes the compiler not complain about the fact that you are constructing something for seemingly no reason.
         */
        string randallUniqueName = DSIS.RandallUniqueName;
        HalfEvade = helper.Content.Statuses.LookupByUniqueName($"{randallUniqueName}::{DSIS.HalfEvadeUniqueName}")!;
        HalfShield = helper.Content.Statuses.LookupByUniqueName($"{randallUniqueName}::{DSIS.HalfShieldUniqueName}")!;
        HalfTempShield = helper.Content.Statuses.LookupByUniqueName($"{randallUniqueName}::{DSIS.HalfTempShieldUniqueName}")!;
        HalfDamage = helper.Content.Statuses.LookupByUniqueName($"{randallUniqueName}::{DSIS.HalfDamageUniqueName}")!;

        _ = new AttachableToPartManager();
    }

    /*
     * assets must also be registered before they may be used.
     * Unlike cards and artifacts, however, they are very simple to register, and often do not need to be referenced in more than one place.
     * This utility method exists to easily register a sprite, but nothing prevents you from calling the method used yourself.
     */
    public static ISpriteEntry RegisterSprite(IPluginPackage<IModManifest> package, string dir)
    {
        return Instance.Helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile(dir));
    }

    public static int? GetPartX(Part part, State state, Combat combat)
    {
        if (state.ship.parts.Where((p) => p == part).Count() > 0) return state.ship.x + state.ship.parts.FindIndex((p) => p == part);
        else if (combat.otherShip.parts.Where((p) => p == part).Count() > 0) return combat.otherShip.x + combat.otherShip.parts.FindIndex((p) => p == part);
        return null;
    }

    /*
     * Animation frames are typically named very similarly, only differing by the number of the frame itself.
     * This utility method exists to easily register an animation.
     * It expects the animation to start at frame 0, up to frames - 1.
     * TODO It is advised to avoid animations consisting of 2 or 3 frames.
     */
    //public static void RegisterAnimation(IPluginPackage<IModManifest> package, string tag, string dir, int frames)
    //{
    //    Instance.Helper.Content.Characters.V2.RegisterCharacterAnimation(new CharacterAnimationConfigurationV2
    //    {
    //        CharacterType = Instance.DemoDeck.Deck.Key(),
    //        LoopTag = tag,
    //        Frames = Enumerable.Range(0, frames)
    //            .Select(i => RegisterSprite(package, dir + i + ".png").Sprite)
    //            .ToImmutableList()
    //    });
    //}
}

