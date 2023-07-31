using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> NullIfEmpty<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable is null)
        {
            return null;
        }
        
        var list = enumerable.ToList();

        return list.Some()
            ? list
            : null;
    }

    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable is null)
        {
            return new List<T>();
        }
        
        var list = enumerable.ToList();

        return list.Some()
            ? list
            : new List<T>();
    }
    
    public static bool None<T>(this IEnumerable<T> enumerable, bool throwIfNull = false)
    {
        switch (throwIfNull)
        {
            case true:
                ArgumentNullException.ThrowIfNull(enumerable);
                break;
            
            default:
            {
                if (enumerable is null)
                {
                    return true;
                }

                break;
            }
        }

        return !enumerable.Any();
    }
    
    public static bool Some<T>(this IEnumerable<T> enumerable, bool throwIfNull = false)
    {
        switch (throwIfNull)
        {
            case true:
                ArgumentNullException.ThrowIfNull(enumerable);
                break;
            
            default:
            {
                if (enumerable is null)
                {
                    return false;
                }

                break;
            }
        }

        return enumerable.Any();
    }
    
    public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate) 
    {
        var index = 0;
        
        foreach (var item in source) 
        {
            if (predicate.Invoke(item)) 
            {
                return index;
            }
            
            index++;
        }

        return -1;
    }    
}