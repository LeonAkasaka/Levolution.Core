using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Levolution.Core.Types
{
    public static partial class TypeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInteger(this Type type) => Types.Integers.Contains(type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumber(this Type type) => Types.Numbers.Contains(type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == Types.Nullable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetNameWithoutArity(this Type type)
            => type.IsGenericType ? GetNameWithoutArity(type.Name) : type.Name;

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
        public static bool IsCollection(this Type type) => typeof(IEnumerable).IsAssignableFrom(type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type ToEnumerable(this Type type)
        {
            if (type == typeof(IEnumerable)) { return Types.Enumerable; }
            if (type == typeof(IEnumerable<>)) { return Types.GenericEnumerable; }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == Types.GenericEnumerable) { return type; }

            return type.IsCollection() ? type.GetInterfaces()
                    .Where(x => x.IsGenericType)
                    .FirstOrDefault(x => x.GetGenericTypeDefinition() == Types.GenericEnumerable) // return IEnumerable<T>
                    ?? Types.Enumerable // return IEnumerable
                : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetElementType(this Type type)
        {
            if (type != typeof(IEnumerable) && type != typeof(IEnumerable<>) && !(type.IsGenericType && type.GetGenericTypeDefinition() == Types.GenericEnumerable))
            {
                return null;
            }

            return type.GenericTypeArguments.Any() ? type.GenericTypeArguments[0] : typeof(object);
        }
    }
}
