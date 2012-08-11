using System.Globalization;
using System.Text;

namespace NDatabase.Odb
{
    /// <summary>
    ///   The abstract class to represent OID.
    /// </summary>
    /// <remarks>
    ///   The abstract class to represent OID. OID is a unique identifier for NDatabase ODB entities like objects and classes. The id is generated by NDatabase
    /// </remarks>
    /// <author>osmadja</author>
    public abstract class OdbAbstractOID : OID
    {
        protected OdbAbstractOID(long oid)
        {
            ObjectId = oid;
        }

        public long ObjectId { get; protected set; }

        public abstract int CompareTo(object obj);

        protected long UrShift(long number, int bits)
        {
            if (number >= 0)
                return number >> bits;

            return (number >> bits) + (2L << ~bits);
        }

        public override string ToString()
        {
            return ObjectId.ToString(CultureInfo.InvariantCulture);
        }
    }
}