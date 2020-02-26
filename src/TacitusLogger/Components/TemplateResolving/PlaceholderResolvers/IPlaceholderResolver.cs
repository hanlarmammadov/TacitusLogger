
namespace TacitusLogger.Components.TemplateResolving
{
    /// <summary>
    /// Represents the interface which should be implemented by all placeholder resolvers.
    /// </summary> 
    public interface IPlaceholderResolver
    {
        void Resolve(LogModel logModel, ref string template);
    }
}
