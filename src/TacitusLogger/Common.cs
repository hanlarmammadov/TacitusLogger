
namespace TacitusLogger
{
    public static class Common
    {
        #region Descriptions class

        public static class Descriptions
        {
            // Application
            public const string AppStartedSuccessfully = "Application started successfully.";
            public const string AppStartFailed = "Application start failed.";
            public const string AppStoppedSuccessfully = "Application stopped successfully.";
            public const string AppStopFailed = "Application stop failed.";
            public const string AppFinishedSuccessfully = "Application finished successfully.";
            // Node
            public const string NodeStartedSuccessfully = "Node started successfully.";
            public const string NodeStoppedSuccessfully = "Node stopped successfully.";
            public const string NodeFinishedSuccessfully = "Node finished successfully.";
            // Service
            public const string ServiceStartedSuccessfully = "Service started successfully.";
            public const string ServicePausedSuccessfully = "Service paused successfully.";
            public const string ServiceStoppedSuccessfully = "Service stopped successfully.";
            public const string ServiceFinishedSuccessfully = "Service finished successfully.";
            // Exception
            public const string MethodThrew = "Method threw an exception.";
            public const string Exception = "Exception was thrown.";
             
        }

        #endregion

        #region Tags class

        public static class Tags
        {
            public const string Started = "Started";
            public const string Stoped = "Stopped";
            public const string Paused = "Paused";
            public const string Restarted = "Restarted";
            public const string Finished = "Finished";
            public const string Threw = "Threw";
            public const string ShutDown = "ShutDown";
            public const string Crash = "Crash";
            public const string Exception = "Exception";
            public const string Timeout = "Timeout";

            public const string App = "App";
            public const string Service = "Service";
            public const string ExternalService = "ExternalService";
            public const string Daemon = "Daemon";
            public const string Database = "Database";
            public const string Storage = "Storage";
            public const string User = "User";

            public const string Security = "Security";
            public const string Authorization = "Authorization";
            public const string Authentication = "Authentication";
            public const string Request = "Request";
            public const string Response = "Request";

            public const string Debug = "Debug";
            public const string Bug = "Bug";

            public const string Controller = "Controller";
            public const string Model = "Model";
            public const string View = "View";
            public const string Presenter = "Presenter";
            public const string Middleware = "Middleware";
            public const string Configuration = "Configuration";
        }

        #endregion 
    } 
}
