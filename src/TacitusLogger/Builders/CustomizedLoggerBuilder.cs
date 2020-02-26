using System;
using System.ComponentModel;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class CustomizedLoggerBuilder : LoggerBuilder
    {
        private readonly Action _preBuildAction;
        private readonly Action<Logger> _postBuildAction;

        public CustomizedLoggerBuilder(string loggerName, Action preBuildAction = null, Action<Logger> postBuildAction = null)
            : base(loggerName)
        {
            _preBuildAction = preBuildAction;
            _postBuildAction = postBuildAction;
        }

        public Action PreBuildActionDelegate => _preBuildAction;
        public Action<Logger> PostBuildActionDelegate => _postBuildAction;

        protected override void PreBuildAction()
        {
            if (_preBuildAction != null)
                _preBuildAction();
        }
        protected override void PostBuildAction(Logger logger)
        {
            if (_postBuildAction != null)
                _postBuildAction(logger);
        }
    }
}
