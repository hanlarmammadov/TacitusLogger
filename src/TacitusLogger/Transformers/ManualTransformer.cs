using System;

namespace TacitusLogger.Transformers
{
    public class ManualTransformer : SynchronousTransformerBase
    {
        private readonly Action<LogModel> _transformerAction;

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
    }
}