using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Levolution.Core.Pcl.UnitTest
{
    [TestClass]
    public class TypeExtensionsTests
    {
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
