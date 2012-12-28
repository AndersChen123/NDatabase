using System;
using NDatabase.Odb.Core.Trigger;
using NDatabase2.Odb;

namespace Test.NDatabase.Odb.Test.Trigger
{
    public class MySelectTrigger : SelectTrigger
    {
        public int nbCalls;

        public override void AfterSelect(object @object, OID oid)
        {
            nbCalls++;
            Console.Out.WriteLine("Select on object with oid " + oid);
        }
    }
}
