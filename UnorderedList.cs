using HarmonyLib;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JyGein.PartCrystals;

/// <summary>
/// Describes an unordered pair of elements.
/// </summary>
/// <typeparam name="T">The type of elements.</typeparam>
public readonly struct UnorderedList<T> : IEquatable<UnorderedList<T>>, IEnumerable<T>
{
    /// <summary>
    /// The list stored by this object
    /// </summary>
    public List<T> List { get; init; }

    /// <summary>
    /// Creates a new unordered list.
    /// </summary>
    /// <param name="first">The list stored by this object.</param>
    public UnorderedList(List<T> list)
    {
        this.List = list;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
        => obj is UnorderedList<T> uList && this.Equals(uList);

    /// <inheritdoc/>
    public bool Equals(UnorderedList<T> other)
    {
        Dictionary<object, int> dict1 = [];
        Dictionary<object, int> dict2 = [];
        foreach (object? obj in List) if (obj != null) dict1[obj] = 1 + (dict1.TryGetValue(obj, out int value) ? value : 0);
        foreach (object? obj in other.List) if (obj != null) dict2[obj] = 1 + (dict2.TryGetValue(obj, out int value) ? value : 0);
        foreach (object key in dict1.Keys) if (dict1.GetValueOrDefault(key) != dict2.GetValueOrDefault(key)) return false;
        return true;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
        => (this.List?.GetHashCode() ?? 0);

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
        => List.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static bool operator ==(UnorderedList<T> left, UnorderedList<T> right)
        => Equals(left, right);

    public static bool operator !=(UnorderedList<T> left, UnorderedList<T> right)
        => !Equals(left, right);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
