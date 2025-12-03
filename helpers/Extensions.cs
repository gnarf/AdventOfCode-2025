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

}