using System;

namespace TacitusLogger.Transformers
{
    public class ManualTransformer : SynchronousTransformerBase
    {
        private readonly Action<LogModel> _transformerAction;
        private bool _isDisposed;

        public ManualTransformer(Action<LogModel> transformerAction, string name = "Manual transformer")
            : base(name)
        {
            _transformerAction = transformerAction ?? throw new ArgumentNullException("transformerAction");
        }

        public Action<LogModel> TransformerAction => _transformerAction;

        public override void Transform(LogModel logModel)
        {
            _transformerAction(logModel);
        }
        public override void Dispose()
        {
            if (_isDisposed)
                return;

            base.Dispose();

            _isDisposed = true;
        }
    }
}