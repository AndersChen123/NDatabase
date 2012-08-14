using System;
using NDatabase.Btree;
using NDatabase.Odb.Core;
using NDatabase.Odb.Impl.Core.Btree;
using NDatabase.Odb.Impl.Core.Layers.Layer3.Engine;
using NDatabase.Tool.Wrappers;
using NUnit.Framework;
using Test.Odb.Test;

namespace Btree.Odb
{
    [TestFixture]
    public class TestODBLazyBTree : ODBTest
    {
        private const int Size = 200;

        /// <exception cref="System.Exception"></exception>
        private IBTreePersister GetPersister(string baseName)
        {
            var odb = Open(baseName);
            return new LazyOdbBtreePersister(odb);
        }

        /// <exception cref="System.Exception"></exception>
        public static void Main2(string[] args)
        {
            var t = new TestODBLazyBTree();
            for (var i = 0; i < 1000; i++)
            {
                try
                {
                    t.Test1a();
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("ERROR On loop " + i);
                    throw;
                }
            }
        }

        // return new InMemoryBTreePersister();
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test01()
        {
            var baseName = GetBaseName();
            DeleteBase(baseName);
            var persister = GetPersister(baseName);
            IBTree tree = new OdbBtreeMultiple("test1", 2, persister);
            var start = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < Size; i++)
                tree.Insert(i + 1, "value " + (i + 1));
            // println(i);
            // println(new BTreeDisplay().build(tree,true));
            var end = OdbTime.GetCurrentTimeInMs();
            Println("time/object=" + (end - start) / (float) Size);
            AssertTrue((end - start) < 0.34 * Size);
            // println("insert of "+SIZE+" elements in BTREE = " +
            // (end-start)+"ms");
            // persister.close();
            // persister = getPersister();
            AssertEquals(Size, tree.GetSize());
            var iterator = tree.Iterator<object>(OrderByConstants.OrderByAsc);
            var j = 0;
            while (iterator.MoveNext())
            {
                var o = iterator.Current;
                AssertEquals("value " + (j + 1), o);
                j++;
                if (j % 10 == 0)
                    Println(j);
            }
            persister.Close();
            DeleteBase(baseName);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test1()
        {
            var baseName = GetBaseName();
            var persister = GetPersister(baseName);
            IBTree tree = new OdbBtreeMultiple("test1", 2, persister);
            var start = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < Size; i++)
                tree.Insert(i + 1, "value " + (i + 1));
            var end = OdbTime.GetCurrentTimeInMs();
            Println(end - start);
            if (testPerformance)
                AssertTrue((end - start) < 0.34 * Size);
            // println("insert of "+SIZE+" elements in BTREE = " +
            // (end-start)+"ms");
            // persister.close();
            // persister = getPersister();
            AssertEquals(Size, tree.GetSize());
            var iterator = tree.Iterator<object>(OrderByConstants.OrderByAsc);
            var j = 0;
            while (iterator.MoveNext())
            {
                var o = iterator.Current;
                AssertEquals("value " + (j + 1), o);
                j++;
                if (j % 10 == 0)
                    Println(j);
            }
            persister.Close();
            DeleteBase(baseName);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test1a()
        {
            var baseName = GetBaseName();
            // Configuration.setInPlaceUpdate(true);
            var persister = GetPersister(baseName);
            IBTree tree = new OdbBtreeMultiple("test1a", 2, persister);
            for (var i = 0; i < Size; i++)
                tree.Insert(i + 1, "value " + (i + 1));
            // println(new BTreeDisplay().build(tree,true).toString());
            persister.Close();
            persister = GetPersister(baseName);
            tree = persister.LoadBTree(tree.GetId());
            // println(new BTreeDisplay().build(tree,true).toString());
            AssertEquals(Size, tree.GetSize());
            var iterator = tree.Iterator<object>(OrderByConstants.OrderByAsc);
            var j = 0;
            while (iterator.MoveNext())
            {
                var o = iterator.Current;
                AssertEquals("value " + (j + 1), o);
                j++;
                if (j == Size)
                    AssertEquals("value " + Size, o);
            }
            persister.Close();
            DeleteBase(baseName);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test2()
        {
            var baseName = GetBaseName();
            var persister = GetPersister(baseName);
            IBTree tree = new OdbBtreeMultiple("test2", 2, persister);
            for (var i = 0; i < Size; i++)
                tree.Insert(i + 1, "value " + (i + 1));
            AssertEquals(Size, tree.GetSize());
            var iterator = tree.Iterator<object>(OrderByConstants.OrderByDesc);
            var j = 0;
            while (iterator.MoveNext())
            {
                var o = iterator.Current;
                // println(o);
                j++;
                if (j == Size)
                    AssertEquals("value " + 1, o);
            }
            persister.Close();
            DeleteBase(baseName);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test2a()
        {
            var baseName = GetBaseName();
            AbstractObjectWriter.ResetNbUpdates();
            // LogUtil.allOn(true);
            DeleteBase(baseName);
            var persister = GetPersister(baseName);
            IBTreeMultipleValuesPerKey tree = new OdbBtreeMultiple("test2a", 20, persister);
            var start0 = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < Size; i++)
                tree.Insert(i + 1, "value " + (i + 1));
            // println("Commiting");
            persister.Close();
            var end0 = OdbTime.GetCurrentTimeInMs();
            // println("insert of "+SIZE+" elements in BTREE = " +
            // (end0-start0)+"ms");
            // println("end Commiting");
            // println("updates : IP="+ObjectWriter.getNbInPlaceUpdates()+" , N="+ObjectWriter.getNbNormalUpdates());
            // ODB odb = open(baseName);
            // odb.getObjects(LazyNode.class);
            // odb.close();
            persister = GetPersister(baseName);
            // println("reloading btree");
            tree = (IBTreeMultipleValuesPerKey) persister.LoadBTree(tree.GetId());
            // println("end reloading btree , size="+tree.size());
            AssertEquals(Size, tree.GetSize());
            long totalSearchTime = 0;
            long oneSearchTime = 0;
            long minSearchTime = 10000;
            long maxSearchTime = -1;
            for (var i = 0; i < Size; i++)
            {
                var start = OdbTime.GetCurrentTimeInMs();
                var o = tree.Search(i + 1);
                var end = OdbTime.GetCurrentTimeInMs();
                AssertEquals("value " + (i + 1), o[0]);
                oneSearchTime = (end - start);
                // println("Search time for "+o+" = "+oneSearchTime);
                if (oneSearchTime > maxSearchTime)
                    maxSearchTime = oneSearchTime;
                if (oneSearchTime < minSearchTime)
                    minSearchTime = oneSearchTime;
                totalSearchTime += oneSearchTime;
            }
            persister.Close();
            // println("total search time="+totalSearchTime +
            // " - mean st="+((double)totalSearchTime/SIZE));
            // println("min search time="+minSearchTime + " - max="+maxSearchTime);
            // Median search time must be smaller than 1ms
            DeleteBase(baseName);
            AssertTrue(totalSearchTime < 1 * Size);
        }

        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void TestLazyCache()
        {
            var baseName = GetBaseName();
            var persister = GetPersister(baseName);
            IBTree tree = new OdbBtreeMultiple("test1", 2, persister);
            var start = OdbTime.GetCurrentTimeInMs();
            for (var i = 0; i < Size; i++)
                tree.Insert(i + 1, "value " + (i + 1));
            var end = OdbTime.GetCurrentTimeInMs();
            if (testPerformance)
                AssertTrue((end - start) < 0.34 * Size);
            // println("insert of "+SIZE+" elements in BTREE = " +
            // (end-start)+"ms");
            // persister.close();
            // persister = getPersister();
            // /assertEquals(SIZE,tree.size());
            var iterator = tree.Iterator<object>(OrderByConstants.OrderByAsc);
            var j = 0;
            while (iterator.MoveNext())
            {
                var o = iterator.Current;
                j++;
                if (j == Size)
                    AssertEquals("value " + Size, o);
            }
            persister.Close();
            DeleteBase(baseName);
        }
    }
}
