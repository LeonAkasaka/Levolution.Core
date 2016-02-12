using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Levolution.Core
{
    public static class TypeExtensions
    {
#if !Net35
        private static TypeInfo EnumerableTypeInfo { get; } = typeof(IEnumerable).GetTypeInfo();
        private static TypeInfo GenericEnumerableTypeInfo { get; } = typeof(IEnumerable<>).GetTypeInfo();
        private static TypeInfo NullableTypeInfo { get; } = typeof(Nullable<>).GetTypeInfo();
#endif

        #region IsCollection

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsCollection(this TypeInfo info)
            => EnumerableTypeInfo.IsAssignableFrom(info);
#endif

        #endregion

        #region GetCellioctionType
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
#endif

        #endregion

        #region GetCollectionElementType

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
#endif

#endregion

        #region IsNullable

        public static bool IsNullable(this Type type)
#if Net35
            => type.IsGenericType && type.GetGenericTypeDefinition() == Types.Nullable;
#else
            => type.GetTypeInfo().IsNullable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsNullable(this TypeInfo info)
            => info.IsGenericType && info.GetGenericTypeDefinition() == Types.Nullable;
#endif

        #endregion

        #region IsPureType

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsPureType(this TypeInfo info)
            => !info.IsNullable() && !info.IsGenericType && !info.IsArray;
#endif

        #endregion

        #region IsInteger

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInteger(Type type)
#if Net35
         => Types.Integers.Contains(type);
#else
         => Types.Integers.Contains(type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsInteger(TypeInfo info)
            => Types.Integers.Select(x => x.GetTypeInfo()).Contains(info);
#endif

        #endregion

#if !Net35
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

#endif
    }
}
