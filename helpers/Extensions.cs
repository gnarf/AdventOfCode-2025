using System.Collections;
using System.Collections.Generic;

public static class ListExtensions
{

    public static List<T> Sorted<T>(this List<T> list, IComparer<T>? comparer = null)
    {
        list.Sort(comparer);
        return list;
    }

    public static List<T> Reversed<T>(this List<T> list)
    {
        list.Reverse();
        return list;
    }

    public static void AddRange<T>(this ICollection<T> List, IEnumerable<T> Add)
    {
        foreach (var item in Add)
        {
            List.Add(item);
        }
    }

    public static void AddRange<T,K>(this Dictionary<T, K> Dict, IEnumerable<KeyValuePair<T, K>> Add) where T : notnull
    {
        foreach (var entry in Add)
        {
            Dict[entry.Key] = entry.Value;
        }
    }

}