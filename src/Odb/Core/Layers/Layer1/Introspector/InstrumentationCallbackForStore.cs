using NDatabase.Odb.Core.Trigger;

namespace NDatabase.Odb.Core.Layers.Layer1.Introspector
{
    internal sealed class InstrumentationCallbackForStore : IIntrospectionCallback
    {
        private readonly bool _isUpdate;
        private readonly IInternalTriggerManager _triggerManager;

        public InstrumentationCallbackForStore(IInternalTriggerManager triggerManager, bool isUpdate)
        {
            _triggerManager = triggerManager;
            _isUpdate = isUpdate;
        }

        #region IIntrospectionCallback Members

        public void ObjectFound(object @object)
        {
            if (!_isUpdate)
            {
                if (_triggerManager != null)
                {
                    var type = @object.GetType();
                    _triggerManager.ManageInsertTriggerBefore(type, @object);
                }
            }
        }

        #endregion
    }
}
