using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Levolution.Core.Types
{
    public static partial class TypeExtensions
    {
        private static TypeInfo EnumerableTypeInfo { get; } = typeof(IEnumerable).GetTypeInfo();
        private static TypeInfo GenericEnumerableTypeInfo { get; } = typeof(IEnumerable<>).GetTypeInfo();
        private static TypeInfo NullableTypeInfo { get; } = typeof(Nullable<>).GetTypeInfo();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetNameWithoutArity(this TypeInfo info)
            => info.IsGenericType ? GetNameWithoutArity(info.Name) : info.Name;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsCollection(this TypeInfo info)
            => EnumerableTypeInfo.IsAssignableFrom(info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Type GetCollectionType(this TypeInfo info)
        {
            if (info == EnumerableTypeInfo) { return Types.Enumerable; }
            if (info == GenericEnumerableTypeInfo) { return Types.GenericEnumerable; }
            if (info.IsGenericType && info.GetGenericTypeDefinition() == Types.GenericEnumerable) { return info.AsType(); }

            return info.IsCollection() ? info.GetAllImplementedInterfaces()
                    .Where(x => x.GetTypeInfo().IsGenericType)
                    .FirstOrDefault(x => x.GetGenericTypeDefinition() == Types.GenericEnumerable) // return IEnumerable<T>
                    ?? Types.Enumerable // return IEnumerable
                : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static Type GetCollectionElementType(this TypeInfo info)
        {
            if (info != EnumerableTypeInfo && info != GenericEnumerableTypeInfo && !(info.IsGenericType && info.GetGenericTypeDefinition() == Types.GenericEnumerable))
            {
                return null;
            }

            return info.GenericTypeArguments.Any() ? info.GenericTypeArguments[0] : typeof(object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsNullable(this TypeInfo info)
            => info.IsGenericType && info.GetGenericTypeDefinition() == Types.Nullable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsPureType(this TypeInfo info)
            => !info.IsNullable() && !info.IsGenericType && !info.IsArray;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsInteger(this TypeInfo info)
            => Types.Integers.Select(x => x.GetTypeInfo()).Contains(info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsNumber(this TypeInfo info)
            => Types.Numbers.Select(x => x.GetTypeInfo()).Contains(info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllImplementedInterfaces(this Type type)
            => type.GetTypeInfo().GetAllImplementedInterfaces();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllImplementedInterfaces(this TypeInfo info)
            => info.ImplementedInterfaces.Concat(info.BaseType != null ? info.BaseType.GetTypeInfo().GetAllImplementedInterfaces() : Enumerable.Empty<Type>());
    }
}
