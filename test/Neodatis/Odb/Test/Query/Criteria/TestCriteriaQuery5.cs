using System;
using NDatabase.Odb.Core.Query.Criteria;
using NDatabase.Odb.Impl.Core.Query.Criteria;
using NUnit.Framework;
using Test.Odb.Test.VO.Attribute;

namespace Test.Odb.Test.Query.Criteria
{
    [TestFixture]
    public class TestCriteriaQuery5 : ODBTest
    {
        private DateTime correctDate;

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestCriteriaWithDate()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            for (var i = 0; i < 10; i++)
            {
                var tc = new TestClass();
                tc.SetInt1(i);
                odb.Store(tc);
            }
            odb.Close();
            odb = Open(baseName);
            var os = odb.GetObjects<TestClass>(new CriteriaQuery(Where.Ge("int1", 0)));
            AssertEquals(10, os.Count);
            var j = 0;
            while (os.HasNext())
            {
                var tc = os.Next();
                AssertEquals(j, tc.GetInt1());
                j++;
            }
            odb.Close();
        }

        [Test]
        public virtual void TestIntLongCriteriaQuery()
        {
            var baseName = GetBaseName();

            var odb = Open(baseName);
            var cwi = new ClassWithInt(1, "test");
            odb.Store(cwi);
            odb.Close();
            odb = Open(baseName);
            var os = odb.GetObjects<ClassWithInt>(new CriteriaQuery(Where.Equal("i", (long) 1)));
            AssertEquals(1, os.Count);
            odb.Close();
        }

        [Test]
        public virtual void TestLongIntCriteriaQuery()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            var cwl = new ClassWithLong(1L, "test");
            odb.Store(cwl);
            odb.Close();
            odb = Open(baseName);
            var os = odb.GetObjects<ClassWithLong>(new CriteriaQuery(Where.Equal("i", 1L)));
            AssertEquals(1, os.Count);
            odb.Close();
        }

        [Test]
        public virtual void TestLongIntCriteriaQueryGt()
        {
            var baseName = GetBaseName();
            var odb = Open(baseName);
            var cwl = new ClassWithLong(1L, "test");
            odb.Store(cwl);
            odb.Close();
            odb = Open(baseName);
            var os = odb.GetObjects<ClassWithLong>(new CriteriaQuery(Where.Ge("i", 1L)));
            AssertEquals(1, os.Count);
            os = odb.GetObjects<ClassWithLong>(new CriteriaQuery(Where.Gt("i", 1L)));
            AssertEquals(0, os.Count);
            odb.Close();
        }
    }
}