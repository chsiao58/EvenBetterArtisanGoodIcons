using System;
using System.Collections.Generic;

namespace BetterArtisanGoodIcons.Extensions
{
    static class ListExtensions
    {
        /// <summary>Create lists of tuples more easily.</summary>
        public static void Add<T1, T2, T3>(this List<Tuple<T1, T2, T3>> list, T1 item1, T2 item2, T3 item3)
        {
            list.Add(new Tuple<T1, T2, T3>(item1, item2, item3));
        }
    }
}
