using System;
using System.Collections;
using System.Collections.Generic;
using NDatabase.Odb;
using NDatabase.Odb.Core.Layers.Layer2.Instance;
using NDatabase.Odb.Core.Query;
using NDatabase.Odb.Main;
using NDatabase.Tool.Wrappers;
using NDatabase2.Odb;
using NUnit.Framework;
using Test.NDatabase.Odb.Test.VO.Login;

namespace Test.NDatabase.Odb.Test.Query.Values
{
    [TestFixture]
    public class TestSubList : ODBTest
    {
        private static Parameter AsParameter(object nonNativeObjectInfo)
        {
            return (Parameter) nonNativeObjectInfo;
        }

        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test1()
        {
            DeleteBase("valuesSubList");
            var odb = Open("valuesSubList");
            var handler = new Handler();
            for (var i = 0; i < 10; i++)
                handler.AddParameter(new Parameter("test " + i, "value " + i));
            odb.Store(handler);
            odb.Close();
            odb = Open("valuesSubList");
            var valuesQuery =
                odb.ValuesQuery<Handler>().Field("parameters").Sublist("parameters", "sub1", 1, 5, true).Sublist(
                    "parameters", "sub2", 1, 10).Size("parameters", "size");
            
            var values = odb.GetValues(valuesQuery);
            Println(values);
            var ov = values.NextValues();
            var fulllist = (IList) ov.GetByAlias("parameters");
            AssertEquals(10, fulllist.Count);
            var size = (long) ov.GetByAlias("size");
            AssertEquals(10, size);

            var p = AsParameter(fulllist[0]);
            AssertEquals("value 0", p.GetValue());
            var p2 = AsParameter(fulllist[9]);
            AssertEquals("value 9", p2.GetValue());
            var sublist = (IList) ov.GetByAlias("sub1");
            AssertEquals(5, sublist.Count);
            p = AsParameter(sublist[0]);
            AssertEquals("value 1", p.GetValue());
            p2 = AsParameter(sublist[4]);
            AssertEquals("value 5", p2.GetValue());
            var sublist2 = (IList) ov.GetByAlias("sub2");
            AssertEquals(9, sublist2.Count);
            odb.Close();
        }

        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test11()
        {
            using (var odb = OdbFactory.Open("valuesSubList"))
            {
                var handler = new Handler();
                for (var i = 0; i < 10; i++)
                    handler.AddParameter(new Parameter("test " + i, "value " + i));
                odb.Store(handler);
            }

            using (var odb = Open("valuesSubList"))
            {
                var valuesQuery =
                    odb.ValuesQuery<Handler>().Field("parameters").Sublist("parameters", "sub1", 1, 5, true).Sublist(
                        "parameters", "sub2", 1, 10).Size("parameters", "size");

                var values =
                    odb.GetValues(valuesQuery);
                var ov = values.NextValues();
                // Retrieve Result values
                var fulllist = (IList) ov.GetByAlias("parameters");

                Assert.That(fulllist, Has.Count.EqualTo(10));

                var size = (long) ov.GetByAlias("size");
                Assert.That(size, Is.EqualTo(10));

                var sublist = (IList) ov.GetByAlias("sub1");
                Assert.That(sublist, Has.Count.EqualTo(5));
            }
        }

        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test2()
        {
            DeleteBase("valuesSubList2");
            var odb = Open("valuesSubList2");
            var handler = new Handler();
            for (var i = 0; i < 500; i++)
                handler.AddParameter(new Parameter("test " + i, "value " + i));
            var oid = odb.Store(handler);
            odb.Close();
            odb = Open("valuesSubList2");
            var h = (Handler) odb.GetObjectFromId(oid);
            Println("size of list = " + h.GetListOfParameters().Count);
            var start = OdbTime.GetCurrentTimeInMs();
            var values =
                odb.GetValues(
                    odb.ValuesQuery<Handler>().Sublist("parameters", "sub", 490, 5, true).Size(
                        "parameters", "size"));
            var end = OdbTime.GetCurrentTimeInMs();
            Println("time to load sublist of 5 itens from 40000 : " + (end - start));
            Println(values);
            var ov = values.NextValues();
            var sublist = (IList) ov.GetByAlias("sub");
            AssertEquals(5, sublist.Count);
            var size = (long) ov.GetByAlias("size");
            AssertEquals(500, size);

            var instanceBuilder = new InstanceBuilder(((OdbAdapter)odb).GetStorageEngine());

            var p = AsParameter(sublist[0]);
            AssertEquals("value 490", p.GetValue());
            var p2 = AsParameter(sublist[4]);
            AssertEquals("value 494", p2.GetValue());
            odb.Close();
        }

        /// <summary>
        ///   Using Object representation instead of real object
        /// </summary>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test3()
        {
            var sublistSize = 10000;
            DeleteBase("valuesSubList3");
            var odb = Open("valuesSubList3");
            var handler = new Handler();
            for (var i = 0; i < sublistSize; i++)
                handler.AddParameter(new Parameter("test " + i, "value " + i));
            odb.Store(handler);
            odb.Close();
            odb = Open("valuesSubList3");
            var start = OdbTime.GetCurrentTimeInMs();
            var q = odb.ValuesQuery<Handler>().Sublist("parameters", "sub", 9990, 5, true);
            q.SetReturnInstance(false);
            var values = odb.GetValues(q);
            var end = OdbTime.GetCurrentTimeInMs();
            Println("time to load sublist of 5 itens from 40000 : " + (end - start));
            Println(values);
            var ov = values.NextValues();
            var sublist = (IList) ov.GetByAlias("sub");
            AssertEquals(5, sublist.Count);
            var parameter = (Parameter) sublist[0];
            AssertEquals("value 9990", parameter.GetValue());
            var parameter1 = (Parameter)sublist[4];
            AssertEquals("value 9994", parameter1.GetValue());
            odb.Close();
        }

        [Test]
        public virtual void Test4()
        {
            DeleteBase("sublist4");
            var odb = Open("sublist4");
            int i;
            IList<VO.Login.Function> functions1 = new List<VO.Login.Function>();
            for (i = 0; i < 30; i++)
                functions1.Add(new VO.Login.Function("f1-" + i));
            IList<VO.Login.Function> functions2 = new List<VO.Login.Function>();
            for (i = 0; i < 60; i++)
                functions2.Add(new VO.Login.Function("f2-" + i));
            IList<VO.Login.Function> functions3 = new List<VO.Login.Function>();
            for (i = 0; i < 90; i++)
                functions3.Add(new VO.Login.Function("f3-" + i));
            var user1 = new User("User1", "user1@neodtis.org", new Profile("profile1", functions1));
            var user2 = new User("User2", "user1@neodtis.org", new Profile("profile2", functions2));
            var user3 = new User("User3", "user1@neodtis.org", new Profile("profile3", functions3));
            odb.Store(user1);
            odb.Store(user2);
            odb.Store(user3);
            odb.Close();
            odb = Open("sublist4");
            var query = odb.Query<User>();
            var u = query.Execute<User>().GetFirst();
            Console.Out.WriteLine(u);
            var q =
                odb.ValuesQuery<Profile>().Field("name").Sublist("functions", 1, 2, false).Size(
                    "functions", "fsize");
            var v = odb.GetValues(q);
            i = 0;
            while (v.HasNext())
            {
                var ov = v.NextValues();
                var profileName = (string) ov.GetByAlias("name");
                Println(profileName);
                AssertEquals("profile" + (i + 1), profileName);
                AssertEquals(Convert.ToInt64(30 * (i + 1)), ov.GetByAlias("fsize"));
                var l = (IList) ov.GetByAlias("functions");
                Println(l);
                AssertEquals(2, l.Count);
                i++;
            }
            odb.Close();
        }

        /// <summary>
        ///   Using Object representation instead of real object
        /// </summary>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test5()
        {
            var sublistSize = 400;

            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            var handler = new Handler();
            for (var i = 0; i < sublistSize; i++)
                handler.AddParameter(new Parameter("test " + i, "value " + i));
            odb.Store(handler);
            odb.Close();
            odb = Open("valuesSubList3");
            var start = OdbTime.GetCurrentTimeInMs();
            var q = odb.ValuesQuery<Handler>().Sublist("parameters", "sub", 0, 2, true);
            var values = odb.GetValues(q);
            var end = OdbTime.GetCurrentTimeInMs();
            Println("time to load sublist of 5 itens for " + sublistSize + " : " + (end - start));
            Println(values);
            var ov = values.NextValues();
            var sublist = (IList) ov.GetByAlias("sub");
            AssertEquals(2, sublist.Count);

            var instanceBuilder = new InstanceBuilder(((OdbAdapter)odb).GetStorageEngine());

            var parameter = AsParameter(sublist[1]);
            AssertEquals("value 1", parameter.GetValue());
            var oid = odb.GetObjectId(parameter);
            Println(oid);
            odb.Close();
        }

        /// <summary>
        ///   Check if objects of list are known by ODB
        /// </summary>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="System.Exception"></exception>
        [Test]
        public virtual void Test6()
        {
            const int sublistSize = 400;

            var baseName = GetBaseName();
            DeleteBase(baseName);
            var odb = Open(baseName);
            var handler = new Handler();
            for (var i = 0; i < sublistSize; i++)
                handler.AddParameter(new Parameter("test " + i, "value " + i));
            odb.Store(handler);
            odb.Close();
            odb = Open("valuesSubList3");
            var start = OdbTime.GetCurrentTimeInMs();
            IQuery q = odb.Query<Handler>();
            var objects = q.Execute<Handler>();
            var end = OdbTime.GetCurrentTimeInMs();

            Console.WriteLine("Query time: {0} ms", end - start);
            var h = objects.GetFirst();
            var parameter = (Parameter) h.GetListOfParameters()[0];
            AssertEquals("value 0", parameter.GetValue());
            var oid = odb.GetObjectId(parameter);
            AssertNotNull(oid);
            odb.Close();
        }
    }
}
