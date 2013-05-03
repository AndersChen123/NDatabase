using NDatabase.Core;
using NDatabase.Core.Layers.Layer3;
using NUnit.Framework;

namespace Test.NDatabase.Odb.Test.Other
{
    [TestFixture]
    public class TestGetDatabaseName : ODBTest
    {
        [Test]
        public virtual void Test1()
        {
            var baseName = "name.ndb";
            DeleteBase(baseName);

            IStorageEngine engine;
            using (var odb = Open(baseName))
            {
                engine = ((OdbAdapter)odb).GetStorageEngine();
            }

            var s = engine.GetBaseIdentification().Id;

            AssertEquals(baseName, s);
        }

        [Test]
        public virtual void Test2()
        {
            var baseName = "name.ndb";
            DeleteBase(baseName);

            string s;
            using (var odb = Open(baseName))
            {
                s = odb.Ext().GetDbId();
            }
            AssertEquals(baseName, s);
        }
    }
}
