using Nanoray.PluginManager;
using Nickel;

namespace PartCrystals;

internal interface IRegisterable
{
    static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}