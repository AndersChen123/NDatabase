using System;
using System.Collections;

namespace NDatabase.Odb.Core.Layers.Layer2.Meta
{
    /// <summary>
    ///   A Map to contain values of attributes of an object.
    /// </summary>
    /// <remarks>
    ///   A Map to contain values of attributes of an object. It is used to optimize a criteria query execution where ODB , while reading an instance data, tries to retrieve only values of attributes involved in the query instead of reading the entire object.
    /// </remarks>
    /// <author>olivier s</author>
    [Serializable]
    public class AttributeValuesMap : Hashtable

    {
        /// <summary>
        ///   The Object Info Header of the object being represented
        /// </summary>
        private ObjectInfoHeader _objectInfoHeader;

        /// <summary>
        ///   The oid of the object.
        /// </summary>
        /// <remarks>
        ///   The oid of the object. This is used when some criteria (example is equalCriterion) is on an object, in this case the comparison is done on the oid of the object and not on the object itself.
        /// </remarks>
        private OID _oid;

        public virtual ObjectInfoHeader GetObjectInfoHeader()
        {
            return _objectInfoHeader;
        }

        public virtual void SetObjectInfoHeader(ObjectInfoHeader objectInfoHeader)
        {
            _objectInfoHeader = objectInfoHeader;
        }

        public virtual object GetAttributeValue(string attributeName)
        {
            return this[attributeName];
        }

        public virtual IComparable GetComparable(string attributeName)
        {
            return (IComparable) GetAttributeValue(attributeName);
        }

        public virtual bool HasOid()
        {
            return _oid != null;
        }

        public virtual OID GetOid()
        {
            return _oid;
        }

        public virtual void SetOid(OID oid)
        {
            _oid = oid;
        }

        public bool PutAll(IDictionary map)
        {
            var keys = map.Keys;
            foreach (var key in keys)
                Add(key, map[key]);

            return true;
        }
    }
}