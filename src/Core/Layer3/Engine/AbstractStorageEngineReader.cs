using System;
using System.Collections.Generic;
using NDatabase.Api;
using NDatabase.Api.Query;
using NDatabase.Api.Triggers;
using NDatabase.Btree;
using NDatabase.Core.BTree;
using NDatabase.Core.Introspector;
using NDatabase.Core.Query.Criteria;
using NDatabase.Core.Query.Execution;
using NDatabase.Core.Trigger;
using NDatabase.Exceptions;
using NDatabase.Meta;
using NDatabase.Oid;
using NDatabase.Tool;
using NDatabase.Tool.Wrappers;

namespace NDatabase.Core.Layer3.Engine
{
    internal abstract class AbstractStorageEngineReader : IStorageEngine
    {
        private static readonly IDictionary<IStorageEngine, IInternalTriggerManager> TriggerManagers =
            new OdbHashMap<IStorageEngine, IInternalTriggerManager>();

        /// <summary>
        ///     The file parameters - if we are accessing a file, it will be a IOFileParameters that contains the file name
        /// </summary>
        protected IDbIdentification DbIdentification;

        /// <summary>
        ///     To check if database has already been closed
        /// </summary>
        protected bool IsDbClosed;

        protected IObjectReader ObjectReader;
        public abstract IObjectIntrospectionDataProvider GetClassInfoProvider();

        protected IMetaModel GetMetaModel()
        {
            return GetSession().GetMetaModel();
        }

        #region IStorageEngine Members

        public IInternalTriggerManager GetLocalTriggerManager()
        {
            // First check if trigger manager has already been built for the engine
            IInternalTriggerManager triggerManager;
            TriggerManagers.TryGetValue(this, out triggerManager);
            if (triggerManager != null)
                return triggerManager;

            triggerManager = new InternalTriggerManager(this);
            TriggerManagers[this] = triggerManager;
            return triggerManager;
        }

        public virtual IInternalObjectSet<T> GetObjects<T>(IQuery query, bool inMemory, int startIndex, int endIndex)
        {
            if (IsDbClosed)
                throw new OdbRuntimeException(NDatabaseError.OdbIsClosed.AddParameter(DbIdentification.Id));

            return ObjectReader.GetObjects<T>(query, inMemory, startIndex, endIndex);
        }

        public virtual void DefragmentTo(string newFileName)
        {
            var start = OdbTime.GetCurrentTimeInMs();
            var totalNbObjects = 0L;

            var newStorageEngine = new StorageEngine(new FileIdentification(newFileName));
            var j = 0;

            var criteriaQuery = new SodaQuery(typeof (object));
            var defragObjects = GetObjects<object>(criteriaQuery, true, -1, -1);

            foreach (var defragObject in defragObjects)
            {
                newStorageEngine.Store(defragObject);
                totalNbObjects++;

                if (OdbConfiguration.IsLoggingEnabled())
                {
                    if (j%10000 == 0)
                        DLogger.Info(string.Concat("\nStorageEngine: ", totalNbObjects.ToString(), " objects saved."));
                }

                j++;
            }

            newStorageEngine.Close();

            var time = OdbTime.GetCurrentTimeInMs() - start;

            if (!OdbConfiguration.IsLoggingEnabled())
                return;

            var nbObjectsAsString = totalNbObjects.ToString();
            var timeAsString = time.ToString();

            DLogger.Info(string.Format("StorageEngine: New storage {0} created with {1} objects in {2} ms.", newFileName,
                                       nbObjectsAsString, timeAsString));
        }

        public abstract ISession GetSession();

        public void DeleteIndex(string className, string indexName)
        {
            var classInfo = GetMetaModel().GetClassInfo(className, true);

            var classInfoIndex = GetClassInfoIndex(className, indexName, classInfo);

            if (OdbConfiguration.IsLoggingEnabled())
                DLogger.Info(string.Format("StorageEngine: Deleting index {0} on class {1}", indexName, className));

            Delete(classInfoIndex);
            classInfo.RemoveIndex(classInfoIndex);

            if (OdbConfiguration.IsLoggingEnabled())
                DLogger.Info(string.Format("StorageEngine: Index {0} deleted", indexName));
        }

        /// <summary>
        ///     Used to rebuild an index
        /// </summary>
        public void RebuildIndex(string className, string indexName)
        {
            if (OdbConfiguration.IsLoggingEnabled())
                DLogger.Info(string.Format("StorageEngine: Rebuilding index {0} on class {1}", indexName, className));

            var classInfo = GetMetaModel().GetClassInfo(className, true);
            var classInfoIndex = GetClassInfoIndex(className, indexName, classInfo);

            DeleteIndex(className, indexName);

            AddIndexOn(className, indexName, classInfo.GetAttributeNames(classInfoIndex.AttributeIds),
                       !classInfoIndex.IsUnique);
        }

        public void AddIndexOn(string className, string indexName, string[] indexFields,
                               bool acceptMultipleValuesForSameKey)
        {
            var classInfo = GetMetaModel().GetClassInfo(className, true);
            if (classInfo.HasIndex(indexName))
                throw new OdbRuntimeException(
                    NDatabaseError.IndexAlreadyExist.AddParameter(indexName).AddParameter(className));

            var classInfoIndex = classInfo.AddIndexOn(indexName, indexFields, acceptMultipleValuesForSameKey);
            IBTree btree;

            var lazyOdbBtreePersister = new LazyOdbBtreePersister(this);

            if (acceptMultipleValuesForSameKey)
                btree = new OdbBtreeMultiple(OdbConfiguration.GetIndexBTreeDegree(), lazyOdbBtreePersister);
            else
                btree = new OdbBtreeSingle(OdbConfiguration.GetIndexBTreeDegree(), lazyOdbBtreePersister);

            classInfoIndex.BTree = btree;
            Store(classInfoIndex);

            // Now The index must be updated with all existing objects.
            if (classInfo.NumberOfObjects == 0)
            {
                // There are no objects. Nothing to do
                return;
            }

            if (OdbConfiguration.IsLoggingEnabled())
            {
                var numberOfObjectsAsString = classInfo.NumberOfObjects.ToString();
                DLogger.Info(
                    string.Format(
                        "StorageEngine: Creating index {0} on class {1} - Class has already {2} Objects. Updating index",
                        indexName, className, numberOfObjectsAsString));

                DLogger.Info(string.Format("StorageEngine: {0} : loading {1} objects from database", indexName,
                                           numberOfObjectsAsString));
            }

            // We must load all objects and insert them in the index!
            var criteriaQuery = new SodaQuery(classInfo.UnderlyingType);
            var objects = GetObjectInfos(criteriaQuery);

            if (OdbConfiguration.IsLoggingEnabled())
            {
                var numberOfObjectsAsString = classInfo.NumberOfObjects.ToString();
                DLogger.Info(string.Format("StorageEngine: {0} : {1} objects loaded", indexName, numberOfObjectsAsString));
            }

            while (objects.HasNext())
            {
                var nnoi = (NonNativeObjectInfo) objects.Next();

                var odbComparable = IndexTool.BuildIndexKey(classInfoIndex.Name, nnoi, classInfoIndex.AttributeIds);
                btree.Insert(odbComparable, nnoi.GetOid());
            }

            if (OdbConfiguration.IsLoggingEnabled())
                DLogger.Info(string.Format("StorageEngine: {0} created!", indexName));
        }

        public abstract void AddCommitListener(ICommitListener arg1);

        public abstract void AddDeleteTriggerFor(Type type, DeleteTrigger arg2);

        public abstract void AddInsertTriggerFor(Type type, InsertTrigger arg2);

        public abstract void AddSelectTriggerFor(Type type, SelectTrigger arg2);

        public abstract void AddUpdateTriggerFor(Type type, UpdateTrigger arg2);

        public abstract void Close();

        public abstract void Commit();

        public abstract OID Delete<T>(T plainObject) where T : class;

        public abstract void DeleteObjectWithOid(OID arg1);

        public abstract void Disconnect<T>(T plainObject) where T : class;

        public abstract IList<long> GetAllObjectIds();

        public abstract IDbIdentification GetBaseIdentification();

        public abstract IOdbList<ICommitListener> GetCommitListeners();

        public abstract IDatabaseId GetDatabaseId();

        public abstract object GetObjectFromOid(OID arg1);

        public abstract OID GetObjectId<T>(T plainObject, bool throwExceptionIfDoesNotExist) where T : class;

        public abstract ObjectInfoHeader GetObjectInfoHeaderFromOid(OID arg1);

        public abstract IObjectReader GetObjectReader();

        public abstract IObjectWriter GetObjectWriter();

        public abstract IRefactorManager GetRefactorManager();

        public abstract IInternalTriggerManager GetTriggerManager();

        public abstract IValues GetValues(IInternalValuesQuery query, int arg2, int arg3);

        public abstract bool IsClosed();

        public abstract void ResetCommitListeners();

        public abstract void Rollback();

        public abstract void SetCurrentIdBlockInfos(CurrentIdBlockInfo currentIdBlockInfo);

        public abstract void SetDatabaseId(IDatabaseId arg1);

        public abstract OID Store<T>(OID oid, T plainObject) where T : class;

        public abstract OID Store<T>(T plainObject) where T : class;

        public abstract IIdManager GetIdManager();

        private static ClassInfoIndex GetClassInfoIndex(string className, string indexName, ClassInfo classInfo)
        {
            if (!classInfo.HasIndex(indexName))
                throw new OdbRuntimeException(
                    NDatabaseError.IndexDoesNotExist.AddParameter(indexName).AddParameter(className));

            var classInfoIndex = classInfo.GetIndexWithName(indexName);
            return classInfoIndex;
        }

        protected void RemoveLocalTriggerManager()
        {
            TriggerManagers.Remove(this);
        }

        private IObjectSet<object> GetObjectInfos(IQuery query)
        {
            // Returns the query result handler for normal query result (that return a collection of objects)
            var queryResultAction = new QueryResultAction<object>(query, false, this, false,
                                                                  GetObjectReader().GetInstanceBuilder());

            return ObjectReader.GetObjectInfos<object>(query, false, -1, -1, false,
                                                       queryResultAction);
        }

        #endregion
    }
}