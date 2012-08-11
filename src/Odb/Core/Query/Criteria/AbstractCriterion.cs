using System;
using NDatabase.Odb.Core.Layers.Layer2.Meta;
using NDatabase.Tool.Wrappers.List;

namespace NDatabase.Odb.Core.Query.Criteria
{
    /// <summary>
    ///   An adapter for Criterion.
    /// </summary>
    /// <remarks>
    ///   An adapter for Criterion.
    /// </remarks>
    /// <author>olivier s</author>
    [Serializable]
    public abstract class AbstractCriterion : ICriterion
    {
        /// <summary>
        ///   The name of the attribute involved by this criterion
        /// </summary>
        protected string AttributeName;

        /// <summary>
        ///   The query containing the criterion
        /// </summary>
        private IQuery _query;

        protected AbstractCriterion(string fieldName)
        {
            AttributeName = fieldName;
        }

        #region ICriterion Members

        public virtual bool CanUseIndex()
        {
            return false;
        }

        public abstract bool Match(object valueToMatch);

        /// <summary>
        ///   Gets thes whole query
        /// </summary>
        /// <returns> The owner query </returns>
        public virtual IQuery GetQuery()
        {
            return _query;
        }

        public virtual void SetQuery(IQuery query)
        {
            _query = query;
        }

        /// <summary>
        ///   An abstract criterion only restrict one field =&gt; it returns a list of one field!
        /// </summary>
        /// <returns> The list of involved field of the criteria </returns>
        public virtual IOdbList<string> GetAllInvolvedFields()
        {
            IOdbList<string> list = new OdbArrayList<string>(1);
            list.Add(AttributeName);
            return list;
        }

        public abstract AttributeValuesMap GetValues();

        public abstract void Ready();

        #endregion

        public virtual bool Match(AbstractObjectInfo aoi)
        {
            var nnoi = (NonNativeObjectInfo) aoi;
            var aoiValue = nnoi.GetValueOf(AttributeName);
            return Match(aoiValue);
        }

        public virtual bool Match(AttributeValuesMap attributeValues)
        {
            return Match(attributeValues.GetAttributeValue(AttributeName));
        }

        public virtual IExpression And(ICriterion criterion)
        {
            return new And().Add(this).Add(criterion);
        }

        public virtual IExpression Or(ICriterion criterion)
        {
            return new Or().Add(this).Add(criterion);
        }

        public virtual IExpression Not()
        {
            return new Not(this);
        }

        /// <returns> The attribute involved in the criterion </returns>
        public virtual string GetAttributeName()
        {
            return AttributeName;
        }

        public virtual void SetAttributeName(string attributeName)
        {
            AttributeName = attributeName;
        }
    }
}