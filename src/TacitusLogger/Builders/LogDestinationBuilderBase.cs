using System;
using System.ComponentModel;
using TacitusLogger.Destinations;
using TacitusLogger.Serializers;

namespace TacitusLogger.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class LogDestinationBuilderBase
    { 
        private readonly ILogGroupDestinationsBuilder _logGroupDestinationsBuilder;

        protected LogDestinationBuilderBase(ILogGroupDestinationsBuilder logGroupDestinationsBuilder)
        { 
            _logGroupDestinationsBuilder = logGroupDestinationsBuilder;
        }

        public ILogGroupDestinationsBuilder LogGroupDestinationsBuilder =>_logGroupDestinationsBuilder;

        protected ILogGroupDestinationsBuilder AddToLogGroup(ILogDestination logDestination)
        {
            return _logGroupDestinationsBuilder.CustomDestination(logDestination);
        }
    }
}
