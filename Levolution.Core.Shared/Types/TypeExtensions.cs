using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Levolution.Core.Types
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class TypeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetNameWithoutArity(this Type type)
#if Net35
            => type.IsGenericType ? GetNameWithoutArity(type.Name) : type.Name;
#else
            => type.GetTypeInfo().GetNameWithoutArity();
#endif

        private static string GetNameWithoutArity(string name)
        {
            var len = name.LastIndexOf("`");
            return len > 0 ? name.Substring(0, len) : name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsCollection(this Type type)
#if Net35
            => Types.Enumerable.IsAssignableFrom(type);
#else
            => type.GetTypeInfo().IsCollection();
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetCollectionType(this Type type)
#if Net35
        {
            if (type == Types.Enumerable) { return Types.Enumerable; }
            if (type == Types.GenericEnumerable) { return Types.GenericEnumerable; }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == Types.GenericEnumerable) { return type; }

            return type.IsCollection() ? type.GetInterfaces()
                    .Where(x => x.IsGenericType)
                    .FirstOrDefault(x => x.GetGenericTypeDefinition() == Types.GenericEnumerable) // return IEnumerable<T>
                    ?? Types.Enumerable // return IEnumerable
                : null;
        }
#else
            => type.GetTypeInfo().GetCollectionType();
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetCollectionElementType(this Type type)
#if Net35
        {
            if (type != Types.Enumerable && type != Types.GenericEnumerable && !(type.IsGenericType && type.GetGenericTypeDefinition() == Types.GenericEnumerable))
            {
                return null;
            }
            var args = type.GetGenericArguments();
            return type.GetGenericArguments().Any() ? args[0] : typeof(object);
        }
#else
            => type.GetTypeInfo().GetCollectionElementType();
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
#if Net35
            => type.IsGenericType && type.GetGenericTypeDefinition() == Types.Nullable;
#else
            => type.GetTypeInfo().IsNullable();
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPureType(this Type type)
#if Net35
            => !type.IsNullable() && !type.IsGenericType && !type.IsArray;
#else
            => type.GetTypeInfo().IsPureType();
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInteger(this Type type)
            => Types.Integers.Contains(type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumber(this Type type)
            => Types.Numbers.Contains(type);
    }
}
