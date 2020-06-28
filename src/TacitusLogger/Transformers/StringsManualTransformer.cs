using System;
using System.Text;

namespace TacitusLogger.Transformers
{
    public class StringsManualTransformer : StringsTransformerBase
    {
        private readonly StringDelegate _transformerDelegate;

        public StringsManualTransformer(StringDelegate transformerDelegate, string name = "Strings manual transformer")
            : base(name)
        {
            _transformerDelegate = transformerDelegate ?? throw new ArgumentNullException("transformerDelegate");
        }

        public StringDelegate TransformerDelegate => _transformerDelegate;

        public delegate void StringDelegate(ref string str);

        protected override void TransformString(ref string str)
        {
            _transformerDelegate(ref str);
        }
    }
}
