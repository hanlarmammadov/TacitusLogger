
namespace TacitusLogger.Transformers
{ 
    public abstract class StringsTransformerBase : SynchronousTransformerBase
    {
        public StringsTransformerBase(string name)
            : base(name)
        {

        }

        public override void Transform(LogModel logModel)
        {
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
        protected abstract void TransformString(ref string str);
    }
}
