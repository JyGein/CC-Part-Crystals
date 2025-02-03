using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JyGein.PartCrystals;

public static class Util
{
    public static List<(T, int)> WithIndex<T>(this List<T> list)
        => list.Select((value, index) => (value, index)).ToList();

    public static IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
    {
        return assembly.GetTypes().Where(t => t != baseType &&
                                              baseType.IsAssignableFrom(t));
    }
}
