using System;
using System.Collections.Generic;
using System.Linq;

namespace Levolution.Core
{
    public static class RandomExtensions
    {
        public static T Choose<T>(this Random random, IEnumerable<T> items)
            => Choose(random, items.ToArray());

        public static T Choose<T>(this Random random, T[] items)
            => items[random.Next(items.Length)];
    }
}
