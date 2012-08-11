using System;
using System.Collections.Generic;
using NDatabase.Btree;
using NDatabase.Btree.Impl.Multiplevalue;
using NDatabase.Odb.Core;
using NDatabase.Odb.Core.Layers.Layer3;
using NDatabase.Odb.Impl.Core.Btree;

namespace NDatabase.Odb.Impl.Core.Query.List.Objects
{
    /// <summary>
    ///   A collection using a BTtree as a back-end component.
    /// </summary>
    /// <remarks>
    ///   A collection using a BTtree as a back-end component. Lazy because it only keeps the oids of the objects. When asked for an object, loads it on demand and returns it
    /// </remarks>
    /// <author>osmadja</author>
    [Serializable]
    public class LazyBTreeCollection<T> : AbstractBTreeCollection<T>
    {
        private readonly bool _returnObjects;
        private readonly IStorageEngine _storageEngine;

        public LazyBTreeCollection(IStorageEngine engine, bool returnObjects) : base(OrderByConstants.OrderByAsc)
        {
            _storageEngine = engine;
            _returnObjects = returnObjects;
        }

        public LazyBTreeCollection(OrderByConstants orderByType) : base(orderByType)
        {
        }

        public override IBTree BuildTree(int degree)
        {
            return new InMemoryBTreeMultipleValuesPerKey("default", degree);
        }

        public override IEnumerator<T> Iterator(OrderByConstants orderByType)
        {
            return new LazyOdbBtreeIteratorMultiple<T>(GetTree(), orderByType, _storageEngine, _returnObjects);
        }
    }
}