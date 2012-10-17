using System;
using NDatabase2.Odb;
using NDatabase2.Odb.Core.Layers.Layer2.Meta;
using NDatabase2.Odb.Core.Query.Values;

namespace Test.NDatabase.Odb.Test.Query.Values
{
    
    public class TestCustomQueryFieldAction : CustomQueryFieldAction
    {
        private Decimal myValue;

        public TestCustomQueryFieldAction()
        {
            myValue = new Decimal(0);
        }

        public override void Execute(OID oid, AttributeValuesMap values)
        {
            var n = ValuesUtil.Convert(Convert.ToDecimal(values[oid]));
            var multiply = decimal.Multiply(new Decimal(2), Convert.ToDecimal(n.ToString()));
            myValue = decimal.Add(multiply, myValue);
        }

        public override object GetValue()
        {
            return myValue;
        }

        public override bool IsMultiRow()
        {
            return false;
        }

        public override void Start()
        {
        }

        public override void End()
        {
        }
    }
}
