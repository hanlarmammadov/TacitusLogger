using System;

namespace TacitusLogger.Transformers
{ 
    public abstract class StringsTransformerBase : SynchronousTransformerBase
    {
        private bool _isDisposed;

        public StringsTransformerBase(string name)
            : base(name)
        {

        }

        public override void Transform(LogModel logModel)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("StringsTransformerBase");

            if (logModel.LogId != null)
                TransformString(ref logModel.LogId);
            if (logModel.Context != null)
                TransformString(ref logModel.Context);
            if (logModel.Source != null)
                TransformString(ref logModel.Source);
            if (logModel.Description != null)
                TransformString(ref logModel.Description);

            if (logModel.Tags != null)
                for (int i = 0; i < logModel.Tags.Length; i++)
                    TransformString(ref logModel.Tags[i]);
            if (logModel.LogItems != null)
                for (int i = 0; i < logModel.LogItems.Length; i++)
                {
                    var name = logModel.LogItems[i].Name;
                    TransformString(ref name);
                    logModel.LogItems[i].Name = name;
                }
        }
        public override void Dispose()
        {
            if (_isDisposed)
                return;

            base.Dispose();

            _isDisposed = true;
        }
        protected abstract void TransformString(ref string str);
    }
}
