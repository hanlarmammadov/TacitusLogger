using System.ComponentModel;
using TacitusLogger.Transformers;
using static TacitusLogger.Transformers.StringsManualTransformer;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ILogTransformersBuilderExtensions
    {
        public static ILogTransformersBuilder StringsManual(this ILogTransformersBuilder self, StringDelegate transformerDelegate, Setting<bool> isActive, string name = "Strings manual transformer")
        {
            return self.Custom(new StringsManualTransformer(transformerDelegate, name), isActive);
        }
        public static ILogTransformersBuilder StringsManual(this ILogTransformersBuilder self, StringDelegate transformerDelegate, bool isActive = true, string name = "Strings manual transformer")
        {
            return self.Custom(new StringsManualTransformer(transformerDelegate, name), isActive);
        }
        
        public static ILogTransformersBuilder Custom(this ILogTransformersBuilder self, LogTransformerBase logTransformer, bool isActive = true)
        {
            return self.Custom(logTransformer, isActive);
        }
    }
}
