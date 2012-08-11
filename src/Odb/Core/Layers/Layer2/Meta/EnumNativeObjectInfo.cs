using System;
using System.Collections.Generic;

namespace NDatabase.Odb.Core.Layers.Layer2.Meta
{
    /// <summary>
    ///   Meta representation of an enum.
    /// </summary>
    /// <remarks>
    ///   Meta representation of an enum. Which is internally represented by a string : Its name
    /// </remarks>
    /// <author>osmadja</author>
    [Serializable]
    public class EnumNativeObjectInfo : NativeObjectInfo
    {
        private ClassInfo _enumClassInfo;

        public EnumNativeObjectInfo(ClassInfo classInfo, string enumName) : base(enumName, Meta.OdbType.EnumId)
        {
            _enumClassInfo = classInfo;
        }

        public override string ToString()
        {
            return GetObject().ToString();
        }

        public override bool IsNull()
        {
            return GetObject() == null;
        }

        public override bool IsNative()
        {
            return true;
        }

        public override bool IsEnumObject()
        {
            return true;
        }

        public override AbstractObjectInfo CreateCopy(IDictionary<OID, AbstractObjectInfo> cache, bool onlyData)
        {
            return new EnumNativeObjectInfo(_enumClassInfo, GetObject() == null
                                                               ? null
                                                               : GetObject().ToString());
        }

        public virtual string GetEnumName()
        {
            return GetObject().ToString();
        }

        public virtual ClassInfo GetEnumClassInfo()
        {
            return _enumClassInfo;
        }

        public virtual void SetEnumClassInfo(ClassInfo enumClassInfo)
        {
            _enumClassInfo = enumClassInfo;
        }
    }
}