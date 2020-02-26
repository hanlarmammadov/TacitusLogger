using System.Text;
using TacitusLogger.Serializers; 

namespace TacitusLogger.Destinations.Debug
{
    /// <summary>
    /// Destination that writes log model to System.Diagnostics.Debug.
    /// </summary>
    public class DebugDestination : NetDiagnosticsLogDestinationBase
    {
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.DebugDestination class using 
        /// log serializer.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logSerializer"/> is null.</exception>
        /// <param name="logSerializer">Log serializer.</param>
        public DebugDestination(ILogSerializer logSerializer)
            : base(logSerializer, new DebugConsoleFacade())
        {

        } 
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.DebugDestination class using 
        /// log string template.
        /// </summary>        
        /// <exception cref="ArgumentNullException">If <paramref name="logStringTemplate"/> is null.</exception>
        /// /// <param name="logStringTemplate">Log string template.</param>
        public DebugDestination(string logStringTemplate)
             : this(new SimpleTemplateLogSerializer(logStringTemplate))
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.DebugDestination class using 
        /// log string factory method.
        /// </summary>    
        /// <exception cref="ArgumentNullException">If <paramref name="logStringFactoryMethod"/> is null.</exception>
        /// <param name="logStringFactoryMethod">Log string factory method</param>
        public DebugDestination(LogModelFunc<string> logStringFactoryMethod)
             : this(new GeneratorFunctionLogSerializer(logStringFactoryMethod))
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.DebugDestination class using 
        /// default log serializer.
        /// </summary>
        /// 
        public DebugDestination()
             : this(new SimpleTemplateLogSerializer())
        {

        } 
    }
}
