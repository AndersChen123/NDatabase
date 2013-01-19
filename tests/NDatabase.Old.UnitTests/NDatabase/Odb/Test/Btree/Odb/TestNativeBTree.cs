using NDatabase.Btree;
using NDatabase.Odb.Core;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Btree.Odb
{
    [TestFixture]
    public class TestNativeBTree : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            var size = 100;
            IBTree tree = new InMemoryBTreeMultipleValuesPerKey(2);
            for (var i = 0; i < size; i++)
                tree.Insert(i + 1, "value " + (i + 1));
            AssertEquals(size, tree.GetSize());
            var iterator = tree.Iterator<object>(OrderByConstants.OrderByAsc);
            var j = 0;
            while (iterator.MoveNext())
            {
                var o = iterator.Current;
                j++;
                AssertEquals("value " + j, o);
                if (j == size)
                    AssertEquals("value " + size, o);
            }
        }

        [Test]
        public virtual void Test2()
        {
            var size = 100000;
            IBTree tree = new InMemoryBTreeMultipleValuesPerKey(2);
            for (var i = 0; i < size; i++)
                tree.Insert(i + 1, "value " + (i + 1));
            AssertEquals(size, tree.GetSize());
            var iterator = tree.Iterator<object>(OrderByConstants.OrderByDesc);
            var j = 0;
            while (iterator.MoveNext())
            {
                var o = iterator.Current;
                // println(o);
                j++;
                if (j == size)
                    AssertEquals("value " + 1, o);
            }
        }
    }
}
