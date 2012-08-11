using System;
using NDatabase.Tool.Wrappers;

namespace NDatabase.Odb.Core.Query
{
    [Serializable]
    public abstract class CompareKey : IOdbComparable
    {
        #region IOdbComparable Members

        public abstract int CompareTo(object o);

        #endregion
    }
}