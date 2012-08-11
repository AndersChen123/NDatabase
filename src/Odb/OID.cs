using System;

namespace NDatabase.Odb
{
    public interface OID : IComparable
    {
        long ObjectId { get; }
        int GetType();
        string GetTypeName();

        /// <summary>
        ///   To retrieve a string representation of an OID
        /// </summary>
        /// <returns> </returns>
        string OidToString();

        string ToString();
    }
}