using System.Collections;
using System.Text;
using NDatabase.Odb.Core;
using NDatabase.Tool.Wrappers;

namespace NDatabase.Btree
{
    /// <summary>
    ///   ODB BTree Errors All @ in error description will be replaced by parameters
    /// </summary>
    /// <author>olivier s</author>
    public sealed class BTreeError : IError
    {
        public static readonly BTreeError MergeWithTwoMoreKeys = new BTreeError(500,
                                                                                "Trying to merge two node with more keys than allowed! @1 // @2");

        public static readonly BTreeError LazyLoadingNode = new BTreeError(501,
                                                                           "Error while loading node lazily with oid @1");

        public static readonly BTreeError NodeWithoutId = new BTreeError(502, "Node with id -1");

        public static readonly BTreeError NullPersisterFound = new BTreeError(503, "Null persister for PersistentBTree");

        public static readonly BTreeError InvalidIdForBtree = new BTreeError(504, "Invalid id for Btree : id=@1");

        public static readonly BTreeError InvalidNodeType = new BTreeError(505,
                                                                           "Node should be a PersistentNode but is a @1");

        public static readonly BTreeError InternalError = new BTreeError(506, "Internal error: @1");

        private readonly int _code;

        private readonly string _description;

        private IList _parameters;

        public BTreeError(int code, string description)
        {
            _code = code;
            _description = description;
        }

        #region IError Members

        public IError AddParameter(object o)
        {
            if (_parameters == null)
                _parameters = new ArrayList();

            _parameters.Add(o != null
                               ? o.ToString()
                               : "null");
            return this;
        }

        public IError AddParameter(string s)
        {
            if (_parameters == null)
                _parameters = new ArrayList();

            _parameters.Add(s);
            return this;
        }

        public IError AddParameter(int i)
        {
            if (_parameters == null)
                _parameters = new ArrayList();

            _parameters.Add(i);
            return this;
        }

        public IError AddParameter(byte i)
        {
            if (_parameters == null)
                _parameters = new ArrayList();

            _parameters.Add(i);
            return this;
        }

        public IError AddParameter(long l)
        {
            if (_parameters == null)
                _parameters = new ArrayList();

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

            var token = buffer.ToString();

            if (_parameters != null)
            {
                for (var i = 0; i < _parameters.Count; i++)
                {
                    var parameterName = string.Format("@{0}", (i + 1));
                    var parameterValue = _parameters[i].ToString();
                    var parameterIndex = token.IndexOf(parameterName, System.StringComparison.Ordinal);
                    
                    if (parameterIndex != -1)
                        token = OdbString.ReplaceToken(token, parameterName, parameterValue, 1);
                }
            }

            return token;
        }
    }
}
