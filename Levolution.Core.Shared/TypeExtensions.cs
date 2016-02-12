using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Levolution.Core
{
    public static class TypeExtensions
    {
        private static Type EnumerableType { get; } = typeof(IEnumerable);
        private static Type GenericEnumerableType { get; } = typeof(IEnumerable<>);
        private static Type NullableType { get; } = typeof(Nullable<>);

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
            => EnumerableType.IsAssignableFrom(type);
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
            if (type == EnumerableType) { return EnumerableType; }
            if (type == GenericEnumerableType) { return GenericEnumerableType; }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == GenericEnumerableType) { return type; }

            return type.IsCollection() ? type.GetInterfaces()
                    .Where(x => x.IsGenericType)
                    .FirstOrDefault(x => x.GetGenericTypeDefinition() == GenericEnumerableType) // return IEnumerable<T>
                    ?? EnumerableType // return IEnumerable
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
            if (info == EnumerableTypeInfo) { return EnumerableType; }
            if (info == GenericEnumerableTypeInfo) { return GenericEnumerableType; }
            if (info.IsGenericType && info.GetGenericTypeDefinition() == GenericEnumerableType) { return info.AsType(); }

            return info.IsCollection() ? info.GetAllImplementedInterfaces()
                    .Where(x => x.GetTypeInfo().IsGenericType)
                    .FirstOrDefault(x => x.GetGenericTypeDefinition() == GenericEnumerableType) // return IEnumerable<T>
                    ?? EnumerableType // return IEnumerable
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
            if (type != EnumerableType && type != GenericEnumerableType && !(type.IsGenericType && type.GetGenericTypeDefinition() == GenericEnumerableType))
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
            if (info != EnumerableTypeInfo && info != GenericEnumerableTypeInfo && !(info.IsGenericType && info.GetGenericTypeDefinition() == GenericEnumerableType))
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
            => type.IsGenericType && type.GetGenericTypeDefinition() == NullableType;
#else
            => type.GetTypeInfo().IsNullable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool IsNullable(this TypeInfo info)
            => info.IsGenericType && info.GetGenericTypeDefinition() == NullableType;
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
