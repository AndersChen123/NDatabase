using System.Text;
using NDatabase.Tool.Wrappers;
using NDatabase.Tool.Wrappers.List;

namespace NDatabase.Odb.Core
{
    /// <summary>
    ///   All NDatabase ODB Errors.
    /// </summary>
    /// <remarks>
    ///   All NDatabase ODB Errors. Errors can be user errors or Internal errors. All @1 in error description will be replaced by parameters
    /// </remarks>
    /// <author>olivier s</author>
    public sealed class NDatabaseError : IError
    {
        public static readonly NDatabaseError NullNextObjectOid = new NDatabaseError(100,
                                                                                   "ODB has detected an inconsistency while reading instance(of @1) #@2 over @3 with oid @4 which has a null 'next object oid'");

        public static readonly NDatabaseError InstancePositionOutOfFile = new NDatabaseError(101,
                                                                                           "ODB is trying to read an instance at position @1 which is out of the file - File size is @2");

        public static readonly NDatabaseError InstancePositionIsNegative = new NDatabaseError(102,
                                                                                            "ODB is trying to read an instance at a negative position @1 , oid=@2 : @3");

        public static readonly NDatabaseError WrongTypeForBlockType = new NDatabaseError(201,
                                                                                       "Block type of wrong type : expected @1, Found @2 at position @3");

        public static readonly NDatabaseError WrongBlockSize = new NDatabaseError(202,
                                                                                "Wrong Block size : expected @1, Found @2 at position @3");

        public static readonly NDatabaseError WrongOidAtPosition = new NDatabaseError(203,
                                                                                    "Reading object with oid @1 at position @2, but found oid @3");

        public static readonly NDatabaseError BlockNumberDoesExist = new NDatabaseError(205,
                                                                                      "Block(of ids) with number @1 does not exist");

        public static readonly NDatabaseError FoundPointer = new NDatabaseError(204,
                                                                              "Found a pointer for oid @1 at position @2");

        public static readonly NDatabaseError ObjectIsMarkedAsDeletedForOid = new NDatabaseError(206,
                                                                                               "Object with oid @1 is marked as deleted");

        public static readonly NDatabaseError ObjectIsMarkedAsDeletedForPosition = new NDatabaseError(207,
                                                                                                    "Object with position @1 is marked as deleted");

        public static readonly NDatabaseError NativeTypeNotSupported = new NDatabaseError(208,
                                                                                        "Native type not supported @1 @2");

        public static readonly NDatabaseError NegativeClassNumberInHeader = new NDatabaseError(210,
                                                                                             "number of classes is negative while reading database header : @1 at position @2");

        public static readonly NDatabaseError UnknownBlockType = new NDatabaseError(211, "Unknown block type @1 at @2");

        public static readonly NDatabaseError UnsupportedIoType = new NDatabaseError(212, "Unsupported IO Type : @1");

        public static readonly NDatabaseError ObjectDoesNotExistInCache = new NDatabaseError(213,
                                                                                           "Object does not exist in cache");

        public static readonly NDatabaseError ObjectWithOidDoesNotExistInCache = new NDatabaseError(213,
                                                                                                  "Object with oid @1 does not exist in cache");

        public static readonly NDatabaseError ObjectInfoNotInTempCache = new NDatabaseError(214,
                                                                                          "ObjectInfo does not exist in temporary cache oid=@1 and position=@2");

        public static readonly NDatabaseError CanNotDeleteFile = new NDatabaseError(215, "Can not delete file @1");

        public static readonly NDatabaseError GoToPosition = new NDatabaseError(216,
                                                                              "Error while going to position @1, length = @2");

        public static readonly NDatabaseError ErrorInCoreProviderInitialization = new NDatabaseError(217,
                                                                                                   "Error while initializing CoreProvider @1");

        public static readonly NDatabaseError UndefinedClassInfo = new NDatabaseError(218, "Undefined class info for @1");

        public static readonly NDatabaseError AbstractObjectInfoTypeNotSupported = new NDatabaseError(219,
                                                                                                    "Abstract Object Info type not supported : @1");

        public static readonly NDatabaseError NegativeBlockSize = new NDatabaseError(220,
                                                                                   "Negative block size at @1 : size = @2, object=@3");

        public static readonly NDatabaseError OperationNotImplemented = new NDatabaseError(222,
                                                                                         "Operation not supported : @1");

        public static readonly NDatabaseError InstanceBuilderWrongObjectType = new NDatabaseError(223,
                                                                                                "Wrong type of object: expecting @1 and received @2");

        public static readonly NDatabaseError InstanceBuilderWrongObjectContainerType = new NDatabaseError(224,
                                                                                                         "Building instance of @1 : can not put a @2 into a @3");

        public static readonly NDatabaseError InstanceBuilderNativeTypeInCollectionNotSupported = new NDatabaseError(225,
                                                                                                                   "Native @1 in Collection(List,array,Map) not supported");

        public static readonly NDatabaseError ObjectIntrospectorNoFieldWithName = new NDatabaseError(226,
                                                                                                   "Class/Interface @1 does not have attribute '@2'");

        public static readonly NDatabaseError ClassPoolCreateClass = new NDatabaseError(228,
                                                                                      "Error while creating (reflection) class @1");

        public static readonly NDatabaseError FileInterfaceReadError = new NDatabaseError(231,
                                                                                        "Error reading @1 bytes at @2 : read @3 bytes instead");

        public static readonly NDatabaseError IndexNotFound = new NDatabaseError(233,
                                                                               "No index defined on class @1 at index position @2");

        public static readonly NDatabaseError MetaModelClassNameDoesNotExist = new NDatabaseError(235,
                                                                                                "Class @1 does not exist in meta-model");

        public static readonly NDatabaseError ClassInfoDoNotHaveTheAttribute = new NDatabaseError(238,
                                                                                                "Class @1 does not have attribute with name @2 in the database meta-model");

        public static readonly NDatabaseError OdbTypeIdDoesNotExist = new NDatabaseError(239,
                                                                                       "ODBtype with id @1 does not exist");

        public static readonly NDatabaseError QueryEngineNotSet = new NDatabaseError(241,
                                                                                   "Storage engine not set on query");

        public static readonly NDatabaseError QueryTypeNotImplemented = new NDatabaseError(242,
                                                                                         "Query type @1 not implemented");

        public static readonly NDatabaseError SerializationFromString = new NDatabaseError(247,
                                                                                         "Error while deserializing: expecting classId @1 and received @2");

        public static readonly NDatabaseError SerializationCollection = new NDatabaseError(248,
                                                                                         "Error while deserializing collection: sizes are not consistent : expected @1, found @2");

        public static readonly NDatabaseError MetamodelReadingLastObject = new NDatabaseError(249,
                                                                                            "Error while reading last object of type @1 at with OID @2");

        public static readonly NDatabaseError ObjectReaderDirectCall = new NDatabaseError(257,
                                                                                        "Generic readObjectInfo called for non native object info");

        public static readonly NDatabaseError CacheObjectInfoHeaderWithoutClassId = new NDatabaseError(258,
                                                                                                     "Object Info Header without class id ; oih.oid=@1");

        public static readonly NDatabaseError NonNativeAttributeStoredByPositionInsteadOfOid = new NDatabaseError(259,
                                                                                                                "Non native attribute (@1) of class @2 stored by position @3 instead of oid");

        public static readonly NDatabaseError CacheNullOid = new NDatabaseError(260, "Null OID");

        public static readonly NDatabaseError NegativePosition = new NDatabaseError(261, "Negative position : @1");

        public static readonly NDatabaseError UnexpectedSituation = new NDatabaseError(262, "Unexpected situation: @1");

        public static readonly NDatabaseError MethodShouldNotBeCalled = new NDatabaseError(267,
                                                                                         "Method @1 should not be called on @2");

        public static readonly NDatabaseError CacheNegativePosition = new NDatabaseError(268,
                                                                                       "Caching an ObjectInfoHeader with negative position @1");

        public static readonly NDatabaseError ErrorWhileGettingObjectFromListAtIndex = new NDatabaseError(269,
                                                                                                        "Error while getting object from list at index @1");

        public static readonly NDatabaseError BtreeSizeDiffersFromClassElementNumber = new NDatabaseError(271,
                                                                                                        "The Index has @1 element(s) whereas the Class has @2 objects. The two values should be equal");

        public static readonly NDatabaseError InstanceBuilderNativeType = new NDatabaseError(274,
                                                                                           "Native object of type @1 can not be instanciated");

        public static readonly NDatabaseError ClassIntrospectionError = new NDatabaseError(275,
                                                                                         "Class Introspectpr error for class @1");

        public static readonly IError EndOfFileReached = new NDatabaseError(276,
                                                                           "End Of File reached - position = @1 : Length = @2");

        public static readonly IError MapInstanciationError = new NDatabaseError(277,
                                                                                "Error while creating instance of MAP of class @1");

        public static readonly IError CollectionInstanciationError = new NDatabaseError(278,
                                                                                       "Error while creating instance of Collection of class @1");

        public static readonly IError InstanciationError = new NDatabaseError(279,
                                                                             "Error while creating instance of type @1");

        public static readonly NDatabaseError CacheNullObject = new NDatabaseError(286, "Null Object : @1");

        public static readonly NDatabaseError LookupKeyNotFound = new NDatabaseError(287, "Lookup key not found : @1");

        public static readonly NDatabaseError FileNotFoundOrItIsAlreadyUsed = new NDatabaseError(291, "File not found or it already used: @1");

        public static readonly NDatabaseError ParentNotFound = new NDatabaseError(283, "Parent directory doesn't exist: @1");

        public static readonly NDatabaseError IndexIsCorrupted = new NDatabaseError(292,
                                                                                  "Index '@1' of class '@2' is corrupted: class has @3 objects, index has @4 entries");

        public static readonly NDatabaseError ContainsQueryWithNoQuery = new NDatabaseError(295,
                                                                                          "Contains criteria with no query!");

        public static readonly NDatabaseError ContainsQueryWithNoStorageEngine = new NDatabaseError(296,
                                                                                                  "Contains criteria with no engine!");

        public static readonly NDatabaseError CrossSessionCacheNullOidForObject = new NDatabaseError(297,
                                                                                                   "Cross session cache does not know the object @1");

        public static readonly NDatabaseError CriteriaQueryUnknownAttribute = new NDatabaseError(1000,
                                                                                               "Attribute @1 used in criteria queria does not exist on class @2");

        public static readonly NDatabaseError RuntimeIncompatibleVersion = new NDatabaseError(1001,
                                                                                            "Incompatible ODB Version : ODB file version is @1 and Runtime version is @2");

        public static readonly NDatabaseError IncompatibleMetamodel = new NDatabaseError(1002,
                                                                                       "Incompatible meta-model : @1");

        public static readonly NDatabaseError OdbIsClosed = new NDatabaseError(1004,
                                                                             "ODB session has already been closed (@1)");

        public static readonly NDatabaseError OdbHasBeenRollbacked = new NDatabaseError(1005,
                                                                                      "ODB session has been rollbacked (@1)");

        public static readonly NDatabaseError OdbCanNotStoreNullObject = new NDatabaseError(1006,
                                                                                          "ODB can not store null object");

        public static readonly NDatabaseError OdbCanNotStoreNativeObjectDirectly = new NDatabaseError(1008,
                                                                                                    "NeoDats ODB can not store native object direclty : @1 which is or seems to be a @2. Workaround: Wrap class @3 into another class");

        public static readonly NDatabaseError ObjectDoesNotExistInCacheForDelete = new NDatabaseError(1009,
                                                                                                    "The object being deleted does not exist in cache. Make sure the object has been loaded before deleting : type=@1 object=[@2]");

        public static readonly NDatabaseError TransactionIsPending = new NDatabaseError(1010,
                                                                                      "There are pending work associated to current transaction, a commit or rollback should be executed : session id = @1");

        public static readonly NDatabaseError UnknownObjectToGetOid = new NDatabaseError(1011, "Unknown object @1");

        public static readonly NDatabaseError OdbCanNotReturnOidOfNullObject = new NDatabaseError(1012,
                                                                                                "Can not return the oid of a null object");

        public static readonly NDatabaseError TransactionAlreadyCommitedOrRollbacked = new NDatabaseError(1017,
                                                                                                        "Transaction have already been 'committed' or 'rollbacked'");

        public static readonly NDatabaseError DifferentSizeInWriteAction = new NDatabaseError(1018,
                                                                                            "Size difference in WriteAction.persist :(calculated,stored)=(@1,@2)");

        public static readonly NDatabaseError QueryBadCriteria = new NDatabaseError(1021,
                                                                                  "CollectionSizeCriteria only work with Collection or Array, and you passed a @1 instead");

        public static readonly NDatabaseError QueryCollectionSizeCriteriaNotSupported = new NDatabaseError(1022,
                                                                                                         "CollectionSizeCriterion sizeType @1 not yet implemented");

        public static readonly NDatabaseError QueryComparableCriteriaAppliedOnNonComparable = new NDatabaseError(1023,
                                                                                                               "ComparisonCriteria with greater than only work with Comparable, and you passed a @1 instead");

        public static readonly NDatabaseError QueryUnknownOperator = new NDatabaseError(1024, "Unknow operator @1");

        public static readonly NDatabaseError QueryContainsCriterionTypeNotSupported = new NDatabaseError(1025,
                                                                                                        "Where.contain can not be used with a @1, only collections and arrays are supported");

        public static readonly NDatabaseError QueryAttributeTypeNotSupportedInLikeExpression = new NDatabaseError(1026,
                                                                                                                "LikeCriteria with like expression(%) only work with String, and you passed a @1 instead");

        public static readonly NDatabaseError IndexKeysMustImplementComparable = new NDatabaseError(1027,
                                                                                                  "Unable to build index key for attribute that does not implement 'Comparable/IComparable' : Index=@1, attribute = @2 , type = @3");

        public static readonly NDatabaseError QueryNqMatchMethodNotImplemented = new NDatabaseError(1029,
                                                                                                  "ISimpleNativeQuery implementing classes must implement method: boolean match(?Object obj), class @1 does not");

        public static readonly NDatabaseError QueryNqExceptionRaisedByNativeQueryExecution = new NDatabaseError(1030,
                                                                                                              "Exception raised by the native query @1 match method");

        public static readonly NDatabaseError AttributeReferencesADeletedObject = new NDatabaseError(1033,
                                                                                                   "Object of type @1 with oid @2 has the attribute '@3' that references a deleted object");

        public static readonly NDatabaseError BeforeDeleteTriggerHasThrownException = new NDatabaseError(1034,
                                                                                                       "Before Delete Trigger @1 has thrown exception. ODB has ignored it \n<user exception>\n@2</user exception>");

        public static readonly NDatabaseError AfterDeleteTriggerHasThrownException = new NDatabaseError(1035,
                                                                                                      "After Delete Trigger @1 has thrown exception. ODB has ignored it\n<user exception>\n@2</user exception>");

        public static readonly NDatabaseError BeforeUpdateTriggerHasThrownException = new NDatabaseError(1036,
                                                                                                       "Before Update Trigger @1 has thrown exception. ODB has ignored it\n<user exception>\n@2</user exception>");

        public static readonly NDatabaseError AfterUpdateTriggerHasThrownException = new NDatabaseError(1037,
                                                                                                      "After Update Trigger @1 has thrown exception. ODB has ignored it\n<user exception>\n@2</user exception>");

        public static readonly NDatabaseError BeforeInsertTriggerHasThrownException = new NDatabaseError(1038,
                                                                                                       "Before Insert Trigger @1 has thrown exception. ODB has ignored it\n<user exception>\n@2</user exception>");

        public static readonly NDatabaseError AfterInsertTriggerHasThrownException = new NDatabaseError(1039,
                                                                                                      "After Insert Trigger @1 has thrown exception. ODB has ignored it\n<user exception>\n@2</user exception>");

        public static readonly NDatabaseError NoMoreObjectsInCollection = new NDatabaseError(1040,
                                                                                           "No more objects in collection");

        public static readonly NDatabaseError IndexAlreadyExist = new NDatabaseError(1041,
                                                                                   "Index @1 already exist on class @2");

        public static readonly NDatabaseError IndexDoesNotExist = new NDatabaseError(1042,
                                                                                   "Index @1 does not exist on class @2");

        public static readonly NDatabaseError QueryAttributeTypeNotSupportedInIequalExpression = new NDatabaseError(1043,
                                                                                                                  "EqualCriteria with case insensitive expression only work with String, and you passed a @1 instead");

        public static readonly NDatabaseError ValuesQueryAliasDoesNotExist = new NDatabaseError(1044,
                                                                                              "Alias @1 does not exist in query result. Existing alias are @2");

        public static readonly NDatabaseError ValuesQueryNotConsistent = new NDatabaseError(1045,
                                                                                          "Single row actions (like sum,count,min,max) are declared together with multi row actions : @1");

        public static readonly NDatabaseError ValuesQueryErrorWhileCloningCustumQfa = new NDatabaseError(1046,
                                                                                                       "Error while cloning Query Field Action @1");

        public static readonly NDatabaseError ExecutionPlanIsNullQueryHasNotBeenExecuted = new NDatabaseError(1047,
                                                                                                            "The query has not been executed yet so there is no execution plan available");

        public static readonly NDatabaseError ObjectWithOidDoesNotExist = new NDatabaseError(1048,
                                                                                           "Object with OID @1 does not exist in the database");

        public static readonly IError UnsupportedEncoding = new NDatabaseError(1052, "Unsupported encoding @1");

        public static readonly IError ReconnectCanReconnectNullObject = new NDatabaseError(1055,
                                                                                          "Can not reconnect null object");

        public static readonly IError CanNotGetObjectFromNullOid = new NDatabaseError(1056,
                                                                                     "Can not get object from null OID");

        public static readonly IError OperationNotAllowedInTrigger = new NDatabaseError(1056,
                                                                                       "Operation not allowed in trigger");

        public static readonly IError TriggerCalledOnNullObject = new NDatabaseError(1058,
                                                                                    "Trigger has been called on class @1 on a null object so it cannot retrieve the value of the '@2' attribute");

        public static readonly NDatabaseError OdbCanNotDeleteNullObject = new NDatabaseError(1061,
                                                                                           "NDatabase can not delete null object");

        public static readonly NDatabaseError InternalError = new NDatabaseError(10, "Internal error : @1 ");

        private readonly int _code;

        private readonly string _description;

        private IOdbList<object> _parameters;

        public NDatabaseError(int code, string description)
        {
            // Internal errors
            // *********************************************
            // User errors
            // *********************************************
            _code = code;
            _description = description;
        }

        #region IError Members

        public IError AddParameter(object o)
        {
            if (_parameters == null)
                _parameters = new OdbArrayList<object>();
            _parameters.Add(o.ToString());
            return this;
        }

        public IError AddParameter(string s)
        {
            if (_parameters == null)
                _parameters = new OdbArrayList<object>();
            
            _parameters.Add(s ?? "[null object]");
            return this;
        }

        public IError AddParameter(int i)
        {
            if (_parameters == null)
                _parameters = new OdbArrayList<object>();
            _parameters.Add(i);
            return this;
        }

        public IError AddParameter(byte i)
        {
            if (_parameters == null)
                _parameters = new OdbArrayList<object>();
            _parameters.Add(i);
            return this;
        }

        public IError AddParameter(long l)
        {
            if (_parameters == null)
                _parameters = new OdbArrayList<object>();
            _parameters.Add(l);
            return this;
        }

        #endregion

        /// <summary>
        ///   replace the @1,@2,...
        /// </summary>
        /// <remarks>
        ///   replace the @1,@2,... by their real values.
        /// </remarks>
        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.Append(_code).Append(":").Append(_description);
            var sourceString = buffer.ToString();

            if (_parameters != null)
            {
                for (var i = 0; i < _parameters.Count; i++)
                {
                    var parameterName = string.Format("@{0}", (i + 1));
                    var parameterValue = _parameters[i].ToString();
                    var parameterIndex = sourceString.IndexOf(parameterName, System.StringComparison.Ordinal);

                    if (parameterIndex != -1)
                        sourceString = OdbString.ReplaceToken(sourceString, parameterName, parameterValue, 1);
                }
            }

            return sourceString;
        }
    }
}
