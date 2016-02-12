using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Levolution.Core.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class Types
    {
        /// <summary>
        /// 
        /// </summary>
        public static Type Boolean { get; } = typeof(bool);

        /// <summary>
        /// 
        /// </summary>
        public static Type Char { get; } = typeof(char);

        /// <summary>
        /// 
        /// </summary>
        public static Type Enum { get; } = typeof(Enum);

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<Type> Integers { get; } = new Type[]
        {
            typeof(byte), typeof(sbyte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong),
        };

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<Type> FloatingPoints { get; } = new Type[]
        {
            typeof(float), typeof(double)
        };

        /// <summary>
        /// 
        /// </summary>
        public static Type Decimal { get; } = typeof(decimal);

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<Type> Numbers { get; } = Integers.Concat(FloatingPoints).Concat(new Type[] { Decimal });

        /// <summary>
        /// 
        /// </summary>
        public static Type String { get; } = typeof(string);

        /// <summary>
        /// 
        /// </summary>
        public static Type Object { get; } = typeof(object);

        /// <summary>
        /// 
        /// </summary>
        public static Type Enumerable { get; } = typeof(IEnumerable);

        /// <summary>
        /// 
        /// </summary>
        public static Type GenericEnumerable { get; } = typeof(IEnumerable<>);

        /// <summary>
        /// 
        /// </summary>
        public static Type Nullable { get; } = typeof(Nullable<>);

    }
}
