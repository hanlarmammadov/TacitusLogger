
namespace TacitusLogger.LogIdGenerators
{
    public class NullLogIdGenerator : SynchronousLogIdGeneratorBase
    {
        public override string Generate(LogModel logModel)
        {
            return null;
        } 
    }
}
