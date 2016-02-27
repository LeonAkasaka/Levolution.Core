using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Levolution.Core.Types;

namespace Levolution.Core.UnitTest
{
    class A<T>
    {
        public class B { }
        public class C<U, V>
        {
            public class D<W> { }
        }
    }

    public class X
    {
        public class Y<T> {}
    }

    [TestClass]
    public class TypeExtensionsTests
    {
        [TestMethod]
        public void GetNameWithoutArityTest()
        {
            Assert.AreEqual("A", typeof(A<>).GetNameWithoutArity());
            Assert.AreEqual("B", typeof(A<>.B).GetNameWithoutArity());
            Assert.AreEqual("C", typeof(A<>.C<,>).GetNameWithoutArity());
            Assert.AreEqual("D", typeof(A<>.C<,>.D<>).GetNameWithoutArity());
            Assert.AreEqual("X", typeof(X).GetNameWithoutArity());
            Assert.AreEqual("Y", typeof(X.Y<>).GetNameWithoutArity());
        }

        [TestMethod]
        public void IsCollectionTest()
        {
            Assert.IsTrue(typeof(int[]).IsCollection());
            Assert.IsTrue(typeof(int?[]).IsCollection());
            Assert.IsTrue(typeof(string).IsCollection());
            Assert.IsTrue(typeof(IEnumerable).IsCollection());
            Assert.IsTrue(typeof(IEnumerable<>).IsCollection());
            Assert.IsTrue(typeof(IEnumerable<int>).IsCollection());

            Assert.IsFalse(typeof(int).IsCollection());
            Assert.IsFalse(typeof(int?).IsCollection());
            Assert.IsFalse(typeof(object).IsCollection());
            Assert.IsFalse(typeof(Action).IsCollection());
            Assert.IsFalse(typeof(Action<>).IsCollection());
        }

        [TestMethod]
        public void GetCollectionTypeTest()
        {
            Assert.AreEqual(typeof(IEnumerable<int>), typeof(int[]).GetCollectionType());
            Assert.AreEqual(typeof(IEnumerable<char>), typeof(string).GetCollectionType());
            Assert.AreEqual(typeof(IEnumerable), typeof(IEnumerable).GetCollectionType());
            Assert.AreEqual(typeof(IEnumerable<>), typeof(IEnumerable<>).GetCollectionType());
            Assert.AreEqual(typeof(IEnumerable<int>), typeof(IEnumerable<int>).GetCollectionType());
            Assert.AreEqual(typeof(IEnumerable), typeof(ArrayList).GetCollectionType());
            Assert.AreEqual(typeof(IEnumerable<int>), typeof(List<int>).GetCollectionType());

        }

        [TestMethod]
        public void GetCollectionElementTypeTest()
        {
            Assert.AreEqual(typeof(int), typeof(int[]).GetCollectionType().GetCollectionElementType());
            Assert.AreEqual(typeof(int?), typeof(int?[]).GetCollectionType().GetCollectionElementType());
            Assert.AreEqual(typeof(char), typeof(string).GetCollectionType().GetCollectionElementType());
            Assert.AreEqual(typeof(string), typeof(string[]).GetCollectionType().GetCollectionElementType());
            Assert.AreEqual(typeof(object), typeof(IEnumerable).GetCollectionType().GetCollectionElementType());
            Assert.AreEqual(typeof(int), typeof(IEnumerable<int>).GetCollectionType().GetCollectionElementType());
            Assert.AreEqual(typeof(int?), typeof(IEnumerable<int?>).GetCollectionType().GetCollectionElementType());

            Assert.AreEqual(null, typeof(int).GetCollectionElementType());

        }

        [TestMethod]
        public void IsNullableTest()
        {
            Assert.IsTrue(typeof(bool?).IsNullable());
            Assert.IsTrue(typeof(int?).IsNullable());
            Assert.IsTrue(typeof(DateTime?).IsNullable());

            Assert.IsFalse(typeof(int).IsNullable());
            Assert.IsFalse(typeof(string).IsNullable());
            Assert.IsFalse(typeof(object).IsNullable());
            Assert.IsFalse(typeof(IEnumerable).IsNullable());
        }

        [TestMethod]
        public void IsPureTypeTest()
        {
            Assert.IsTrue(typeof(int).IsPureType());
            Assert.IsTrue(typeof(DateTime).IsPureType());
            Assert.IsTrue(typeof(string).IsPureType());
            Assert.IsTrue(typeof(object).IsPureType());
            Assert.IsTrue(typeof(IEnumerable).IsPureType());
            Assert.IsTrue(typeof(Action).IsPureType());

            Assert.IsFalse(typeof(int?).IsPureType());
            Assert.IsFalse(typeof(int[]).IsPureType());
            Assert.IsFalse(typeof(IEnumerable<>).IsPureType());
            Assert.IsFalse(typeof(IEnumerable<int>).IsPureType());
            Assert.IsFalse(typeof(Action<int>).IsPureType());
        }


        [TestMethod]
        public void IsIntegerTest()
        {
            Assert.IsTrue(typeof(byte).IsInteger());
            Assert.IsTrue(typeof(sbyte).IsInteger());
            Assert.IsTrue(typeof(short).IsInteger());
            Assert.IsTrue(typeof(ushort).IsInteger());
            Assert.IsTrue(typeof(int).IsInteger());
            Assert.IsTrue(typeof(uint).IsInteger());
            Assert.IsTrue(typeof(long).IsInteger());
            Assert.IsTrue(typeof(ulong).IsInteger());

            Assert.IsFalse(typeof(float).IsInteger());
            Assert.IsFalse(typeof(double).IsInteger());
            Assert.IsFalse(typeof(decimal).IsInteger());
            Assert.IsFalse(typeof(bool).IsInteger());
            Assert.IsFalse(typeof(char).IsInteger());
            Assert.IsFalse(typeof(string).IsInteger());
            Assert.IsFalse(typeof(object).IsInteger());
            Assert.IsFalse(typeof(int?).IsInteger());
            Assert.IsFalse(typeof(int[]).IsInteger());
        }

        [TestMethod]
        public void IsNumberTest()
        {
            Assert.IsTrue(typeof(byte).IsNumber());
            Assert.IsTrue(typeof(sbyte).IsNumber());
            Assert.IsTrue(typeof(short).IsNumber());
            Assert.IsTrue(typeof(ushort).IsNumber());
            Assert.IsTrue(typeof(int).IsNumber());
            Assert.IsTrue(typeof(uint).IsNumber());
            Assert.IsTrue(typeof(long).IsNumber());
            Assert.IsTrue(typeof(ulong).IsNumber());

            Assert.IsTrue(typeof(float).IsNumber());
            Assert.IsTrue(typeof(double).IsNumber());
            Assert.IsTrue(typeof(decimal).IsNumber());

            Assert.IsFalse(typeof(bool).IsNumber());
            Assert.IsFalse(typeof(char).IsNumber());
            Assert.IsFalse(typeof(string).IsNumber());
            Assert.IsFalse(typeof(object).IsNumber());
            Assert.IsFalse(typeof(int?).IsNumber());
            Assert.IsFalse(typeof(int[]).IsNumber());
        }

#if !Net35

        interface IA { }
        interface IB { }

        class A : IA { };
        class B : A, IB { }

        [TestMethod]
        public void GetAllImplementedInterfacesTest()
        {
            var resultA = typeof(A).GetAllImplementedInterfaces();
            var resultB = typeof(B).GetAllImplementedInterfaces();

            Assert.IsTrue(resultA.Contains(typeof(IA)));
            Assert.IsTrue(resultB.Contains(typeof(IA)) && resultB.Contains(typeof(IB)));

        }
#endif
    }
}
