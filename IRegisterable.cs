using Nanoray.PluginManager;
using Nickel;

namespace JyGein.PartCrystals;

internal interface IRegisterable
{
    static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}