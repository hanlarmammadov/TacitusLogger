# TacitusLogger (Alpha)

> A simple yet powerful .NET logging library.

Tacitus logger helps to organize your logging process to save vital execution information of your application to the variety of logging destinations.

> Attention: TacitusLogger is currently in **Alpha phase**. This means you should not use it in any production code.

## Installation


The NuGet <a href="https://www.nuget.org/packages/TacitusLogger" target="_blank">package</a>:

```powershell
PM> Install-Package TacitusLogger
``` 
Dependencies:  
* NET Standard >= 1.3   
* <a href="https://www.nuget.org/packages/Newtonsoft.Json/" target="_blank">Newtonsoft.Json</a>  >= 9.0.1
  
## Quickstart
Due to implemented convention over configuration principle, using TacitusLogger could be as simple as:
```cs
// Configure and build your logger.
ILogger logger = LoggerBuilder.Logger().ForAllLogs()
                                       .File().WithPath(@".\logs.txt").Add()
                                       .BuildLogger();
// Write your logs.
logger.LogError("Something bad happened!");

// Or write them asynchronously.
await logger.LogErrorAsync("Something bad happened!");
```

TacitusLogger provides an opportunity to:
- Use several built-in, various extension and custom log destinations.
- Group log destinations to several units called log groups.
- Configure each log group with rule predicate to filter out logs that is eligible to be sent to this log group.
- Configure each log group with log cache.
- Select built-in or use custom destination feeding strategy for each log group.
- Select built-in or use custom exception handling strategy for the logger.
- Use built-in and custom log contributors to add runtime info to your logs.
- Use built-in and custom log transformers to modify all logs from single place.
- Use built-in or custom log ID generator to enrich logs with IDs.
- Use built-in or custom log serializers to get different textual representation of the log for each log destination.
- Change some settings like logger log level, log group status in runtime without restarting the app.  

## Content
  
- [More examples](#More-examples)
  - [Ways to write logs](#Ways-to-write-logs)
    - [Using methods of ILogger interface:](#Using-methods-of-ILogger-interface)
    - [Using Log class builder methods:](#Using-Log-class-builder-methods)
  - [Examples of logger configuration](#Examples-of-logger-configuration)
    - [Logger with single console log destination](#Logger-with-single-console-log-destination)
    - [Logger with several log destinations](#Logger-with-several-log-destinations)
    - [Custom log file path template](#Custom-log-file-path-template)
  - [Custom log text template](#Custom-log-text-template)
  - [Several log groups](#Several-log-groups)
  - [Constant log group status](#Constant-log-group-status)
  - [Log group with status that can be changed at runtime](#Log-group-with-status-that-can-be-changed-at-runtime)
  - [Logger with log caching](#Logger-with-log-caching)
  - [Logger with custom log cache](#Logger-with-custom-log-cache)
  - [Logger with several groups each with its own log cache](#Logger-with-several-groups-each-with-its-own-log-cache)
  - [Logger with log contributors](#Logger-with-log-contributors)
  - [Logger with custom log contributor](#Logger-with-custom-log-contributor)
  - [Logger with log transformers](#Logger-with-log-transformers)
  - [Logger with custom log transformers](#Logger-with-custom-log-transformers)
  - [Logger destination feeding strategy](#Logger-destination-feeding-strategy)
  - [Logger custom destination feeding strategy](#Logger-custom-destination-feeding-strategy)
  - [Logger custom log creation strategy](#Logger-custom-log-creation-strategy)
  - [Logger exception handling strategy](#Logger-exception-handling-strategy)
  - [Logger exception handling strategy of type Log](#Logger-exception-handling-strategy-of-type-Log)
  - [Configuring logger with the custom exception handling strategy](#Configuring-logger-with-the-custom-exception-handling-strategy)
  - [Logger with constant log level](#Logger-with-constant-log-level)
  - [Logger with mutable log level that can be changed at runtime](#Logger-with-mutable-log-level-that-can-be-changed-at-runtime)
  - [Custom log serializer implementation](#Custom-log-serializer-implementation)
  - [GUID based log ID](#GUID-based-log-ID)
  - [Null log ID](#Null-log-ID)
  - [Custom log ID generator implementation](#Custom-log-ID-generator-implementation)
  - [More advanced configuration](#More-advanced-configuration)
- [Main components and definitions](#Main-components-and-definitions)
  - [Logger](#Logger)
  - [Log event](#Log-event)
    - [Log type](#Log-type)
    - [Log description.](#Log-description)
    - [Log context.](#Log-context)
    - [Log tags.](#Log-tags)
    - [Log items.](#Log-items)
  - [Log model](#Log-model)
    - [Log ID](#Log-ID)
    - [Source](#Source)
    - [Log date](#Log-date)
  - [Log destinations](#Log-destinations)
    - [Built-in destinations](#Built-in-destinations)
  - [Log groups](#Log-groups)
    - [Log group name](#Log-group-name)
    - [Log group status](#Log-group-status)
    - [Log filtering](#Log-filtering)
    - [Destination feeding strategy](#Destination-feeding-strategy)
    - [Log caching](#Log-caching)
  - [Log creation strategy](#Log-creation-strategy)
    - [Resetting default time provider](#Resetting-default-time-provider)
    - [Custom log creation strategy](#Custom-log-creation-strategy)
  - [Log ID generators](#Log-ID-generators)
  - [Self diagnostics](#Self-diagnostics)
  - [Exception handling strategy](#Exception-handling-strategy)
  - [Log serializers](#Log-serializers)
  - [Setting providers](#Setting-providers)
  - [Log contributors](#Log-contributors)
      - [StackTraceContributor](#StackTraceContributor)
      - [UserDataContributor](#UserDataContributor)
  - [Log Transformers](#Log-Transformers)
  - [Logger as a log destination](#Logger-as-a-log-destination)
- [General log flow of TacitusLogger](#General-log-flow-of-TacitusLogger)
 
# More examples

## Ways to write logs
In TacitusLogger there are several convenient styles to write logs to the logger:

### Using methods of ILogger interface:
 ```cs
logger.LogInfo("Operation has completed.");
logger.LogError("Something bad has happened!");
logger.Log("UserController", LogType.Info, "Request from the client", new { data1 = "", data2 = "" }); 
logger.Log("UserService", LogType.Error, "Some error occurred");
 ```
 ### Using ILogger log builder extensions:

 ```cs 
logger.Error("Some error occurred")
      .From(this)
      .WithEx(exception)
      .Tagged("Error", "Exception", "Bug")
      .Log();
 ```

 ```cs 
await logger.Error("Some error occurred")
            .From(this)
            .WithEx(exception)
            .Tagged("Error", "Exception", "Bug")
            .LogAsync();
 ```

### Using Log class builder methods:
 
 ```cs 
Log error = Log.Error("Some error occurred")
               .From(this)
               .WithEx(exception)
               .Tagged("Error", "Exception", "Bug");
error.To(logger);
 ```

 ```cs 
Log error = Log.Error("Some error occurred")
               .From(this)
               .WithEx(exception)
               .Tagged("Error", "Exception", "Bug");
await error.ToAsync(logger);
 ```
 
## Examples of logger configuration

### Logger with single console log destination
In the simplest form, logger could contain a single log group that proceeds all logs. The following code snippet the logger with a single log group containing one `ConsoleDestination` is configured to take all logs:

```cs
ILogger logger = LoggerBuilder.Logger().ForAllLogs()
                                       .Console().Add()
                                       .BuildLogger();
```
Actually, more precise syntax for this configuration would be like the following:
```cs
ILogger logger = LoggerBuilder.Logger().NewLogGroup()
                                       .ForAllLogs()
                                       .Console().Add()
                                       .BuildLogGroup()
                                       .BuildLogger();
```
Nevertheless, when you have a logger that contains only one log group, you can omit `NewLogGroup()` and `BuildLogGroup()`.
 
### Logger with several log destinations

In this example logger is configured with single log group containing two log destinations - Console and Debug:

```cs
ILogger logger = LoggerBuilder.Logger("App1").ForAllLogs()
                                                .Console().Add()
                                                .Debug().Add()
                                             .BuildLogger();
```
Same example:
```cs
ILoggerBuilder loggerBuilder = LoggerBuilder.Logger("App1");
ILogGroupDestinationsBuilder groupDestinations = loggerBuilder.ForAllLogs();
groupDestinations.Console().Add();
groupDestinations.Debug().Add();
groupDestinations.BuildLogGroup();
Logger logger = loggerBuilder.BuildLogger();
```

### Custom log file path template
In this example file destination is configured with file path template that will be used to save logs. Template contains placeholders `$Source`, `$LogType`, `$LogDate(dd-MM-yyyy)` that will be replaced with according properties of log models thus forming various file paths. If file exists it will be appended, otherwise, a new file will be created with all preceding directories. When selecting file path template make sure the application has the according write permissions. 

```cs
ILogger logger = LoggerBuilder.Logger("Main logger").ForAllLogs()
                                                    .File().WithPath(@".\$Source\$LogType logs\Logs-$LogDate(dd-MM-yyyy).log")
                                                           .Add()
                                                    .BuildLogger();
```

### Custom log text template
 
```cs
ILogger logger = LoggerBuilder.Logger().ForAllLogs()
                                       .Console().WithSimpleTemplateLogText("Log of type: $LogType, Description: $Description, Date: $LogDate")
                                                 .Add()
                                       .BuildLogger();
```
### Several log groups

Here a logger with two log groups is configured. First log group named `Not important` receives `Info` type logs and sends them to console and debug destinations. Second log group - `Important` writes `Error`, `Failure` and `Critical` logs to file and console.

```cs
ILogger logger = LoggerBuilder.Logger().NewLogGroup("Not important")
                                            .ForInfoLogs()
                                            .Console().Add()
                                            .Debug().Add()
                                        .BuildLogGroup()
                                        .NewLogGroup("Important")
                                            .ForRule(x => x.LogTypeIsIn(LogType.Error, LogType.Failure, LogType.Critical))
                                            .File().WithPath(@".\ImportantLogs.txt").Add()
                                            .Console().Add()
                                        .BuildLogGroup()
                                        .BuildLogger();
```

Same configuration as above:
```cs
var loggerBuilder = LoggerBuilder.Logger();

var notImpLogs = loggerBuilder.NewLogGroup("Not important");
var notImpLogsDestinations = notImpLogs.ForInfoLogs();
notImpLogsDestinations.Console().Add();
notImpLogsDestinations.Debug().Add();
notImpLogsDestinations.BuildLogGroup();

var impLogsGroup = loggerBuilder.NewLogGroup("Important");
var impLogsDestinations = impLogsGroup.ForRule(x => x.LogTypeIsIn(LogType.Error, LogType.Failure, LogType.Critical));
impLogsDestinations.File().WithPath(@".\ImportantLogs.txt").Add();
impLogsDestinations.Console().Add();
impLogsDestinations.BuildLogGroup();

ILogger logger = loggerBuilder.BuildLogger();
```

### Constant log group status

In the following code snippet log group `"group1"` is configured to be inactive:
```cs
var logGroupStatus = LogGroupStatus.Inactive;

ILogger logger = LoggerBuilder.Logger().NewLogGroup("group1")
                                           .SetStatus(logGroupStatus)
                                           .ForInfoLogs()
                                           .Console().Add()
                                       .BuildLogGroup()
                                       .BuildLogger();
```

### Log group with status that can be changed at runtime
In this example value provider of type `TacitusLogger.MutableSetting<LogGroupStatus>` is used to manage the value for log group status. After building the logger the value provider used to change log group status to `Inactive` at runtime:
```cs
MutableSetting<LogGroupStatus> logGroupStatus = Setting<LogGroupStatus>.From.Variable(LogGroupStatus.Active);

ILogger logger = LoggerBuilder.Logger().NewLogGroup("group1")
                                            .SetStatus(logGroupStatus)
                                            .ForInfoLogs()
                                            .Console().Add()
                                        .BuildLogGroup()
                                        .BuildLogger();

logger.LogInfo("This log will be processed by group1");
logGroupStatus.SetValue(LogGroupStatus.Inactive);
logger.LogInfo("This log will not, because group1 is now inactive");
```
### Logger with log caching

The following example configures log caching for the log group `"Groups with cache"`: Cache size to 20 logs and cache time to 60 seconds.

```cs
ILogger logger = LoggerBuilder.Logger("Main logger").NewLogGroup("Groups with cache")
                                                        .WithCaching(20, 60000, isActive: true)
                                                        .ForAllLogs()
                                                        .Console().Add()
                                                    .BuildLogger();
```

### Logger with custom log cache
In this example the custom log cache implementation is registered with the log group `"Groups with cache"`.

```cs
ILogCache customLogCache = new MyCustomLogCache();
ILogger logger = LoggerBuilder.Logger("Main logger").NewLogGroup("Groups with cache")
                                                        .WithCaching(customLogCache, isActive: true)
                                                        .ForAllLogs()
                                                        .Console().Add()
                                                    .BuildLogger();
```
### Logger with several groups each with its own log cache
Each log group can have its own caching configuration:
```cs
ILogger logger = LoggerBuilder.Logger("Main logger").NewLogGroup("Group1")
                                                        .WithCaching(100)
                                                        .ForAllLogs()
                                                            .File()
                                                            .WithPath(@".\AllLogs.txt")
                                                            .Add()
                                                    .BuildLogGroup()
                                                    .NewLogGroup("Group2")
                                                        .WithCaching(10)
                                                        .ForErrorLogs()
                                                            .File()
                                                            .WithPath(@".\Errors.txt")
                                                            .Add()
                                                    .BuildLogGroup()
                                                    .BuildLogger();
```

### Logger with log contributors

The following code snippet adds `StackTraceContributor` log contributor to the logger. Log contributors add additional specific data to each log:

```cs
ILogger logger = LoggerBuilder.Logger()
                              .Contributors()
                                    .StackTrace()
                              .BuildContributors()
                              .NewLogGroup("group1")
                                   .ForAllLogs()
                                   .File()
                                   .WithPath(@".\logs.txt")
                                   .Add()
                               .BuildLogGroup()
                               .BuildLogger();
```
### Logger with custom log contributor
  
```cs
LogContributorBase customContributor = new MyCustomContributor();

ILogger logger = LoggerBuilder.Logger()
                                .LogContributors()
                                    .Custom(customContributor)
                                .BuildContributors()
                                .NewLogGroup("group1")
                                    .ForAllLogs()
                                    .File().WithPath(@".\logs.txt").Add()
                                .BuildLogGroup()
                                .BuildLogger();
```
### Logger with log transformers
The following code snippet adds `TacitusLogger.Transformers.StringsManualTransformer` log transformer to the logger. Log transformers allow to modify all logs before they are sent to log groups. In this case `StringsManualTransformer` is used to modify all strings to lower case (Note that no null checks are performed inside the delegate, that is because it  `StringsManualTransformer` filters out null strings and does not sends them to the delegate):
```cs
ILogger logger = LoggerBuilder.Logger()
                              .Transformers()
                                  .StringsManual((ref string s) => s.ToLower(), true, "Lowercase")
                              .BuildTransformers()
                              .NewLogGroup("group1")
                                  .ForAllLogs()
                                  .Console()
                                  .Add()
                              .BuildLogGroup()
                              .BuildLogger();
```

### Logger with custom log transformers
In the following example in addition to the transformer from the previous example, another custom transformer is added to the logger: 

```cs
var myCustomLogTransformer = new MyCustomLogTransformer("some transformer");

ILogger logger = LoggerBuilder.Logger()
                              .Transformers()
                                  .StringsManual((ref string s) => s.ToLower(), true, "Lowercase")
                                  .Custom(myCustomLogTransformer, true)
                              .BuildTransformers()
                              .NewLogGroup("group1")
                                  .ForAllLogs()
                                  .Console().Add()
                              .BuildLogGroup()
                              .BuildLogger();
```


### Logger destination feeding strategy
Each log group has its own destination feeding strategy which defines how log group sends provided logs to its destinations. There are two built-in strategies - Greedy and FirstSuccess. If not specified, greedy is the default strategy for all log groups. The following example configures two log groups with feeding strategies specified explicitly:
```cs
ILogger logger = LoggerBuilder.Logger()
                              .NewLogGroup("non-greedy group")
                                  .WithDestinationFeeding(DestinationFeedingStrategy.FirstSuccess)
                                  .ForAllLogs()
                                      .File().WithPath(@".\logs1.txt").Add()
                                      .File().WithPath(@".\logs2.txt").Add()
                              .BuildLogGroup()
                              .NewLogGroup("greedy group")
                                  .WithDestinationFeeding(DestinationFeedingStrategy.Greedy)
                                  .ForErrorLogs()
                                      .File().WithPath(@".\errors.txt").Add()
                                      .Console().Add()
                              .BuildLogGroup()
                              .BuildLogger();
```

### Logger custom destination feeding strategy

It is possible to implement your own destination feeding strategy and use it with log groups as in the following code snippet:

```cs
DestinationFeedingStrategyBase customDestinationFeedingStrategy = new CustomDestinationFeedingStrategy();

ILogger logger = LoggerBuilder.Logger()
                              .NewLogGroup("group1")
                                  .WithDestinationFeeding(customDestinationFeedingStrategy)
                                  .ForAllLogs()
                                  .File()
                                      .WithPath(@".\logs1.txt")
                                      .Add()
                                  .File()
                                      .WithPath(@".\logs2.txt")
                                      .Add()
                              .BuildLogGroup()
                              .BuildLogger();
```

### Logger custom log creation strategy

Creation strategy defines how an instance of `TacitusLogger.LogModel` is created. You can override the default log creation strategy by creating your own one and providing it to logger builder:

```cs
LogCreationStrategyBase customLogCreationStrategy = new CustomLogCreationStrategy();

ILogger logger = LoggerBuilder.Logger()
                              .WithLogCreation(customLogCreationStrategy)
                              .ForAllLogs()
                                  .Console().Add()
                              .BuildLogger();
```

### Logger exception handling strategy

Exception handling strategy defines how the logger copes with its own exceptions. There are three built-in strategies: `Silent`, `Log` and `Rethrow`. If not specified, `Silent` is the default strategy for all loggers. In the following code snippet the logger is explicitly coed with the exception handling strategy:

```cs
ILogger logger = LoggerBuilder.Logger()
                              .WithExceptionHandling(ExceptionHandling.Rethrow)
                              .ForAllLogs()
                                  .Console().Add()
                              .BuildLogger();
```

### Logger exception handling strategy of type Log
In the following example the exception handling strategy of type `Log` is used. Please note, that along to the exception handling strategy the diagnostics destination also should be set as a place where the error logs from the strategy will be directed; otherwise, `Log` exception handling strategy will behave just like the `Silent`:

```cs
FileDestination diagnosticsDestination = new FileDestination("./Logger-Errors.txt");

ILogger logger = LoggerBuilder.Logger()
                                .WithDiagnostics(diagnosticsDestination)
                                .WithExceptionHandling(ExceptionHandling.Log)
                                .ForAllLogs()
                                    .Console().Add()
                                .BuildLogger();
```

### Configuring logger with the custom exception handling strategy

You can implement and register your own exception handling strategy as shown in the following example:

```cs
ExceptionHandlingStrategyBase customExceptionHandlingStrategy = new CustomExceptionHandlingStrategy();

ILogger logger = LoggerBuilder.Logger()
                              .WithExceptionHandling(customExceptionHandlingStrategy)
                              .ForAllLogs()
                                  .Console().Add()
                              .BuildLogger();
```

### Logger with constant log level

Log level shows the minimal value of log type that will be proceeded by the logger. The following example sets log level to `Warning` which means that logs with log level less than `Warning` (that are `Info`, `Success` and `Event`) will be ignored by the logger.

```cs
LogLevel logLevel = LogLevel.Warning;

ILogger logger = LoggerBuilder.Logger()
                              .WithLogLevel(logLevel)
                              .ForAllLogs()
                                  .Console().Add()
                              .BuildLogger();
```
### Logger with mutable log level that can be changed at runtime

If you intend to modify logger's log level during the runtime, you can use `VariableValueProvider<LogLevel>` wrapper as shown in the following example. Using this class allows you to modify log level without restarting the application.

```cs
MutableSetting<LogLevel> logLevel = Setting<LogLevel>.From.Variable(LogLevel.Info);

ILogger logger = LoggerBuilder.Logger()
                              .WithLogLevel(logLevel)
                              .ForAllLogs()
                                  .Console().Add()
                              .BuildLogger();

logger.LogInfo("This log will be processed by logger");
logLevel.SetValue(LogLevel.None);
logger.LogInfo("This log will be ignored");
```
 
### Custom log serializer implementation

Log serializers are used by destinations that need some textual representation of log models. For example `ConsoleDestination` needs that log text to send it to Console, `FileDestination` - to generate file path and log text to save it to the file etc. 

```cs
ILogSerializer myCustomLogSerializer = new MyCustomLogSerializer();

ILogger logger = LoggerBuilder.Logger().ForAllLogs()
                                       .File().WithLogSerializer(myCustomLogSerializer)
                                              .Add()
                                       .BuildLogger();
```

### GUID based log ID

Log ID generator is used by logger to add `string` log IDs to log models. The default log ID generator for the logger is `TacitusLogger.LogIdGenerators.GuidLogIdGenerator` and it will be added if no log ID generator is specified explicitly. Nevertheless, if you want to add it with some customizations like different GUID format or substring length you may register it explicitly as shown in the following example:
```cs
ILogger logger = LoggerBuilder.Logger().WithGuidLogId("N", 6)
                                       .ForAllLogs()
                                           .File().WithPath(@".\logs.txt").Add()
                                       .BuildLogger(); 
```

### Null log ID
In some cases you do not want your logs to contain any log IDs. In this case you may consider registering the logger with `TacitusLogger.LogIdGenerators.NullLogIdGenerator`. `NullLogIdGenerator` is a typical implementation of null object pattern and always generates NULL  log IDs:
```cs
ILogger logger = LoggerBuilder.Logger().WithNullLogId()
                                       .ForAllLogs()
                                           .File().WithPath(@".\logs.txt").Add()
                                       .BuildLogger();
```

### Custom log ID generator implementation

If you need some custom log ID generation logic you can create and register your own log ID generator as in the following example:

```cs
ILogIdGenerator myCustomLogIdGenerator = new MyCustomLogIdGenerator();

ILogger logger13 = LoggerBuilder.Logger().WithLogIdGenerator(myCustomLogIdGenerator)
                                         .ForAllLogs()
                                         .File().WithPath(@".\logs.txt").Add()
                                         .BuildLogger();
```

### More advanced configuration

This is more advanced logger configuration that combines most of the previous ones:

```cs
var debugLogGroupStatus = LogGroupStatus.Inactive;
var userServiceLogGroupStatus = LogGroupStatus.Inactive;
 
ILogger logger = LoggerBuilder.Logger("App1 logs")
                              .Contributors()
                                  .StackTrace()
                              .BuildContributors()
                              .Transformers()
                                  .StringsManual((ref string s) => s.ToLower(), true, "Lowercase")
                              .BuildTransformers()
                              .WithExceptionHandling(ExceptionHandling.Silent)
                              .WithLogCreation(LogCreation.Standard)
                              .WithLogIdGenerator(new MyCustomLogIdGenerator())
                              .NewLogGroup("Debug")
                                  .SetStatus(debugLogGroupStatus)
                                  .ForAllLogs()
                                      .Debug()
                                          .WithSimpleTemplateLogText("$LogTypeLog: $Description | Date: $LogDate | Id: $LogId(8)")
                                          .Add()
                              .BuildLogGroup()
                              .NewLogGroup("UserService logs")
                                  .SetStatus(userServiceLogGroupStatus)
                                  .ForRule(x => x.Context != null && x.Context.Contains("UserService"))
                                      .File()
                                          .WithPath(@".\UserServiceLogs-$LogDate(dd-MM-yyyy).log")
                                          .WithExtendedTemplateLogText()
                                          .Add()
                              .BuildLogGroup()
                              .NewLogGroup("Informational logs")
                                  .WithCaching(30, 60000)
                                  .ForRule(x => x.LogType == LogType.Info || x.LogType == LogType.Event || x.LogType == LogType.Warning)
                                      .File()
                                          .WithPath(@".\InformationalLogs-$LogDate(dd-MM-yyyy).log")
                                          .WithGeneratorFuncLogText(x => $"Id: {x.LogId} | {x.LogTypeName}Log: {x.Description} | Date: {x.LogDate.ToString("dd-MM-yyyy hh.mm.ss")}")
                                          .Add()
                                      .Console()
                                          .WithGeneratorFuncLogText(x => $"{x.LogTypeName}Log: {x.Description}")
                                          .Add()
                              .BuildLogGroup()
                              .NewLogGroup("Important logs")
                                  .ForRule(x => x.LogType == LogType.Error || x.LogType == LogType.Critical)
                                      .File()
                                          .WithPath(@".\$LogTypeLogs-$LogDate(dd-MM-yyyy).log")
                                          .WithGeneratorFuncLogText(x => $"{x.LogTypeName}Log: {x.Description} | Date: {x.LogDate.ToString("dd-MM-yyyy hh.mm.ss")}")
                                          .Add()
                              .BuildLogGroup()
                              .BuildLogger();
```

 
# Main components and definitions

### Logger

The central definition in TacitusLogger library is the Logger - an implementation of `TacitusLogger.ILogger` interface:

```cs
public interface ILogger
{
    string Log(Log log);
    Task<string> LogAsync(Log log, CancellationToken cancellationToken = default(CancellationToken));
}
```
TacitusLogger contains an only implementation of this interface - `TacitusLogger.Logger` class which is the default logger. In all further documentation unless explicitly stated otherwise, when we say logger we mean exactly this implementation of the `TacitusLogger.ILogger` interface.

Logger is the main entry point for all writing logs and usually it is the only dependency you want to inject into your classes. Logger's creation could be an expensive operation (depending on used components) and it's lifetime should be singleton. Its implementation of `ILogger` interface if fully thread safe, which means you are free to use singleton logger as an `ILogger` from several threads. As a contrary, logger's configuration (adding log groups, destinations etc) is not thread safe.

------------------------------------------------------------------
### Log event
`TacitusLogger.Log` class contains user-provided information about the logging event. These are the properties shown in the following listing:

```cs
    public class Log 
    {
        // Constructors go here... 

        public string Context { get; }
        public LogType Type { get; }
        public string Description { get; }
        public IList<string> Tags { get; }
        public IList<LogItem> Items { get; }

        // Other members go here...
    }
```
Lets discuss them briefly:

#### Log type
`TacitusLogger.LogType` is an `enum` defining the following names:
```cs
    public enum LogType
    { 
        Info = 10, 
        Success = 11,  
        Event = 12, 
        Warning = 20, 
        Error = 30, 
        Failure = 31,  
        Critical = 32
    }
```
Every log has its type. Log type classify log events to 7 categories that show what type of event is being described:

Log Type | Value | Description
---------| ------|-----------------
Info     | 10    | Informational logs.
Success  | 11    | Logs representing successful operation.
Event    | 12    | Logs representing some unexceptional event occurrence.
Warning  | 20    | Warning logs.
Error    | 30    | Error logs.
Failure  | 31    | Logs representing failure.
Critical | 32    | Logs representing some critical situations.


As with all `Log` properties, log type meaning is closely related to user's interpretations - it is up to user, who decides, for example, what is difference between Error logs and Failure logs.
There is another enum closely related with `LogType` - `LogLevel` that will be discussed #####.

#### Log description. 
String property containing log event description.

#### Log context.
Log context is a string property that is used to provide information related to the place and circumstances of the logging event. It can contain any string representation of what user considers as "context of the logging event" - usually these are class and method names, or some custom piece of data. Well chosen log context easily localize "the place" of the event thus easing troubleshooting.
 
#### Log tags.
Tags are supposed as short strings that beautify the log with additional information useful in the following classification and research. Every log contains a `Tags` property - a list of tags, that can contain any number of tag strings, meaningful to logger user. For convenience, TacitusLogger comes with common tag strings in class `TacitusLogger.Common.Tags`.

#### Log items. 
In the most circumstances log description string is not enough to describe the log event: Usually you want to include additional information in form of textual data, various objects, exceptions and so on. In TacitusLogger this could be achieved by means of Log items. Every log object contains a list of `LogItem` objects which store some peace of named data. Every log item has `Name` property that describes the item and `Value` property that contains related data.
 
```cs
  public class LogItem
    {
        public LogItem(string name, object value);

        public string Name { get; }
        public object Value { get; }

        // Methods go here
    }
```
 Final representation of log item's value could be various and completely depends on log serializer. Usually, when textual representation is needed, log items values are represented in JSON format.

------------------------------------------------------------------
### Log model
While **log event** contains user provided information about the event, **log model** contains full information about the writing log ready to be consumed by log destinations. Log model is defined by `TacitusLogger.LogModel` class, which have the following fields:

```cs 
    public class LogModel
    {
        // Other members goes here

        public string LogId;
        public string Context;
        public string[] Tags;
        public string Source;
        public LogType LogType;
        public string Description;
        public LogItem[] LogItems;
        public DateTime LogDate; 

        // Other members goes here
    }

```
As you can see, additional information includes: `LogId`, `Source` and `LogDate`. Besides that, `LogItems` may contain additional items if logger is configured with log contributors (for additional information see log contributors section of this document).

#### Log ID

Often when dealing with logs you want them to contain some sort of ID information to identify them later. This can be achieved with string `LogId` property and log ID generators. When creating LogModel object from provided `Log` object, the logger (More precisely, log creation strategy assigned to logger) populates `LogId` property with string ID using assigned log ID generator. More on log ID generators and log creation strategies see below.

#### Source
Source property represents more global information about where the logging event is originated from. In default implementation this is the name of the logger which has produced current log. This means that if you have several loggers (probably, from the different applications) which send logs to the same destination, you can name them in some way that will help you when dealing with logs.

#### Log date

`LogDate`, obviously, is a DateTime field that contains date of creation of log model. As with log id, this field is set by log creation strategy assigned to each logger. Depending on settings, it could hold local or UTC date and time. 

------------------------------------------------------------------

### Log destinations

In TacitusLogger log destination is an object that represents a target where the log will be written to. It could be a file system, console, email, database etc. Log destinations are feeded with `LogModel` models and encapsulate all logic related to how these models will be transformed to specific form (file record, console output, email body and subject, db record etc) and sent to the specified targets - the logger have no idea about destination-specific logic. It is important to note that log destinations NEVER mutate provided log models. Violating this rule when implementing your custom destinations can lead to various undesirable consequences. You can register as many log destinations as you need with a single logger. 

All log destinations implement the `TacitusLogger.Destinations.ILogDestination` interface:
```cs
public interface ILogDestination : IDisposable
{
    void Send(LogModel[] logs);
    Task SendAsync(LogModel[] logs, CancellationToken cancellationToken = default(CancellationToken));
}
```
`ILogDestination` interface contains two methods, both of them takes the `LogModel[]` models array. Second method is an asynchronous counterpart of first one and takes an optional cancellation token parameter.
 
#### Built-in destinations 

The TacitusLogger has 4 built-in log destinations:   
`TacitusLogger.Destinations.Console.ConsoleDestination`  
`TacitusLogger.Destinations.File.FileDestination`  
`TacitusLogger.Destinations.Debug.DebugDestination`  
`TacitusLogger.Destinations.TextWriter.TextWriterDestination`  
   
First one, `ConsoleDestination`, writes logs to the standard output device. `FileDestination` writes to file system, `DebugDestination` writes to the `System.Diagnostics.Debug`, `TextWriterDestination` as the names implies, writes to the given `System.IO.TextWriter`.

Logger does not own and manage log destinations by self. There are intermediate players - **log groups** which we are going to introduce next.

------------------------------------------------------------------

### Log groups

Log groups represents an additional layer in TacitusLogger which gives the potential to configure loggers to more advanced log flow including log filtering and caching. Each log group contains log group status which can be used to toggle the group on-off. Every log group implements the `TacitusLogger.LogGroupBase` abstract class:
```cs
    public abstract class LogGroupBase: IDisposable
    {
        public abstract string Name { get; }
        public abstract LogGroupStatus Status { get; } 
            
        public abstract bool IsEligible(LogModel log);
        public abstract void Send(LogModel log);
        public abstract Task SendAsync(LogModel log, CancellationToken cancellationToken = default(CancellationToken));
        public virtual void Dispose();
    }
```
TacitusLogger contains an only implementation of this interface - `TacitusLogger.LogGroup` class which is the default implementation. In all further documentation unless explicitly stated otherwise, when we say log group we mean exactly this implementation of the `TacitusLogger.LogGroupBase` abstract class.

#### Log group name
Every log group has its name that can be useful during the configuration process. If log group name was not specified during the creation of log group, a random GUID-based name will be used. Name should be unique within the given logger which means no two log groups with the same name can be attached to the same logger. Log groups names do not affect the log models in any way.

#### Log group status
Every log group has its status of enum type `TacitusLogger.LogGroupStatus`:

```cs
public enum LogGroupStatus
{
    Active = 0,
    Inactive
}
```

Only active groups are considered by logger for log sending, inactive groups are ignored.  
If not specified explicitly, by default, all log groups are created with `Active` status.


#### Log filtering

The main purpose of log groups is to group log destinations by filtering rule. Here is how it works. Every log group contains a **rule predicate** which is simply a delegate of type `TacitusLogger.LogModelFunc<bool>` that takes `LogModel` object as a single parameter and returns the `Boolean`. When the `Log` object has been received and `LogModel` object has been constructed, the logger iterates over all **active** log groups and checks their eligibility to take the log model by calling its `IsEligible()` method. It is inside this method where the log rule predicate is executed and its `Boolean` result is returned to the logger.

#### Destination feeding strategy

The strategy of sending logs to destinations inside a single log group is implemented by means of destination feeding strategies assigned to that log group. List of possible strategies could be found in `TacitusLogger.DestinationFeeding` enum:
```cs
public enum DestinationFeeding
{
    Greedy = 0,
    FirstSuccess
}
```
 Currently, there are two destination feeding strategies out of box in TacitusLogger - `Greedy` and `FirstSuccess` strategies. If not specified explicitly, by default, the greedy strategy is used in all log groups and as its name implies, it sends logs to all log destinations registered with the given log group. As the opposite, the FirstSuccess strategy keeps sending logs to destinations until first successful send operation. Sending operation is considered successful if no exception was thrown by log destination methods. In other words, it iterates through log destinations list of its log group consequently sending log models to destinations until it reaches the first destination that does not throw an exception or until destinations list is exhausted depending which happens first. After that, it stops iterating and returns, leaving all subsequent log destinations unfed if there are any.  

You can also implement your own feeding strategy by implementing `TacitusLogger.Strategies.DestinationFeeding.DestinationFeedingStrategyBase` abstract class:
```cs
public abstract class DestinationFeedingStrategyBase
{
    public abstract void Feed(LogModel[] logs, IList<ILogDestination> logDestinations);
    public abstract Task FeedAsync(LogModel[] logs, IList<ILogDestination> logDestinations, CancellationToken cancellationToken = default(CancellationToken));
}
```
This strategy could be useful if you have several destination one of which is primary and others just in case if first one fails to write log.  
Also, you should take into consideration that some times a destination could fail silently without throwing. For example a destination that sends mails using SMTP server may not know that the mail it sent failed to reach the recipient and will not throw any exception. In such circumstances the `FirstSuccess` strategy, obviously, will behave not as expected to.
 
#### Log caching 
Another thing that is done on the log group level is log caching. Usually when there are a lot of logs per minute, sending each log to log destinations could be expensive. Especially if writing to destinations is related with opening-closing and holding resources. This is where log caching can help. 
When log group has cache activated, it serves logs differently. Instead of sending each log to destinations immediately after receiving, log group places them into the cache. Each time the cache receives a log model it checks fulfillment of some condition (for example the specific amount of logs have been cached or specified cache time has been expired) and if the check succeeds it immediately **flushes** returning all collected logs which are then sent to destinations **at once** within an array (saying it more precisely, not to destinations directly but to destination feeding strategy object).
An important thing here to note is that sending logs to destinations at once in the form of array **by no means guarantee** that the given destination will not process them *separately*. Performance gain here is strictly related to log destination type and its implementation. For some types of destinations logs caching could be completely useless, for example for destination that sends emails, or destination that writes to console. But in many cases caching could be quite useful in terms of performance, for example if the destination writes to the file (or files) or to MongoDb database.

As it was mentioned, log caching is performed on log group level, that is every log group could have its own cache configured specifically or disabled.  
By default, if not enabled and configured explicitly, log caching is disabled for all created log groups.

All log caches implement `TacitusLogger.LogCaching.ILogCache` interface:
```cs 
public interface ILogCache : IDisposable
{
    LogModel[] AddToCache(LogModel logModel, bool forceToFlush = false);
}
```
TacitusLogger contains only one implementation of `ILogCache` that is `TacitusLogger.LogCaching.InMemoryLogCache` that is configured to flush on the specific amount of logs have been cached or specified cache time has been expired depending which happens first.

Custom implementations of `ILogCache` have to implement `AddToCache` in a way that:
1. When called with `forceToFlush = false` it should return either log data array (including the current log) when flushed or null otherwise. The moment and reason of flushing is related to implementation.
2. When called with `forceToFlush = true` it should always flush.
3. Flushing should clear the inner collection of the cache.
 
#### Summary

To summarize, every log group has:
1. Unique **name** that could be used during configuration.
2. **Status**, which could be `Active` or `Inactive`. Only active groups are considered by logger as targets for logs, inactive log groups are ignored.
3. **Rule predicate** which is used to decide if this log group is eligible to take the given log data model.
4. **Destination feeding strategy** which defines how the log group should feed its log destinations when provided by log data model.
5. Optional **log cache** which delays destination feeding until some time in future, for example, until some amount of them will have been collected in cache.



------------------------------------------------------------------
### Log creation strategy
After the logger gets `Log` object that contains log event data provided by user, it should create `LogModel` object which contains complete picture about the logging event and which will be delivered to log destinations through the TacitusLogger pipeline.  The TacitusLogger currently contains only one log creation strategy - `Standard`:
```cs
    public enum LogCreation
    {
        Standard = 0,
    }

```
This log creation strategy is implemented in `TacitusLogger.Strategies.LogCreation.StandardLogCreationStrategy`.
The `Standard` log creation strategy works this way: it creates an instance of `LogModel` populates it with data from the `Log` object, populates `Source` property with the provided one, adds log date using an instance of `TacitusLogger.Components.Time.ITimeProvider` and considering `UseUtcTime` flag provided during the logger configuration; if logger was configured with (active) log contributors, the strategy retrieves their log items and adds to the `LogItems` array of the log model.


#### Resetting default time provider
As it was mentioned above time provider that is an implementation of `TacitusLogger.Components.Time.ITimeProvider` is used by standard log creation strategy to set log dates:

```cs
public interface ITimeProvider
{
    DateTime GetLocalTime();
    DateTime GetUtcTime();
}
```

 In some situations (for example while testing custom log destinations) you may want to change the default time provider that is `TacitusLogger.Components.Time.SystemTimeProvider` to some custom implementation. You can do this by using `ResetTimeProvider` method of `TacitusLogger.Strategies.LogCreation.StandardLogCreationStrategy` object.

#### Custom log creation strategy

If you need some custom logic for creating `LogModel` instances, you can create and register your own implementation of log creation strategy by deriving from `TacitusLogger.Strategies.LogCreation.LogCreationStrategyBase` abstract class:

```cs
public abstract class LogCreationStrategyBase
{
    protected ILogIdGenerator _logIdGenerator;
    protected IList<LogContributorBase> _logContributors;
    protected ExceptionHandlingStrategyBase _exceptionHandlingStrategy;

    public LogCreationStrategyBase();

    public ILogIdGenerator LogIdGenerator { get; }
    public IEnumerable<LogContributorBase> LogContributors { get; }
    public ExceptionHandlingStrategyBase ExceptionHandlingStrategy { get; }

    public abstract LogModel CreateLogModel(Log log, string source);
    public abstract Task<LogModel> CreateLogModelAsync(Log log, string source, CancellationToken cancellationToken = default(CancellationToken));
    public void InitStrategy(ILogIdGenerator logIdGenerator, IList<LogContributorBase> logContributors, ExceptionHandlingStrategyBase exceptionHandlingStrategy);
}
```
As you see from the above snippet, to do this you need to implement two abstract methods - `CreateLogModel` and `CreateLogModelAsync`.

------------------------------------------------------------------

### Log ID generators
 
As it was mentioned all logs contain LogId field of type `string`. It is not mandatory for logs to have this field populated - it can contain `null` value for all logs. Nevertheless, in most situations you want your logs to contain some sort of ID to identify logs. Log ID generator as the name implies, used to generate string log IDs that are added to logs. If not specified explicitly, `TacitusLogger.LogIdGenerators.GuidLogIdGenerator` is used with the logger by default.  
All log ID generators implement `TacitusLogger.LogIdGenerators.ILogIdGenerator` interface:

```cs
public interface ILogIdGenerator: IDisposable
{
    string Generate(LogModel logModel);
    Task<string> GenerateAsync(LogModel logModel, CancellationToken cancellationToken = default(CancellationToken));
}
```

Log ID generator is used by log creation strategy when creating `LogModel` objects from `Log` objects. With default log creation strategy, by the time when log ID generator methods are called and provided with `LogModel` object, the latter already contains all other data with the log ID being the only missing datum. This means that in your custom log ID generator implementations you can leverage all `LogModel` fields if you need them during the log generation.

This interface contains two methods - the second one is an asynchronous counterpart of the first one. They take log model and returns generated log id. Please take into consideration that these methods should not mutate log model taken. They are provided with log model only to have the opportunity to use it for log id string generation. 
Asynchronous counterpart is called when asynchronous method of `ILogger` is called. In most cases when creating custom log id generator your log generation represents a quick synchronous operation and you do not want to implement and fill `GenerateAsync(...)` method. For such cases you can inherit from `SynchronousLogIdGeneratorBase` abstract class. This class has the only abstract methods to be overridden - `Generate(...)`. Async counterpart of `Generate(...)` method has already implemented within the abstract class itself to call `Generate(...)` synchronously and to return a completed task afterwards:

```cs
public abstract class SynchronousLogIdGeneratorBase : ILogIdGenerator
{
    public abstract string Generate(LogModel logModel);
    public Task<string> GenerateAsync(LogModel logModel, CancellationToken cancellationToken = default(CancellationToken)); 
    public abstract void Dispose();
}
```

------------------------------------------------------------------

### Self diagnostics

TacitusLogger provides the ability to log different information regarding to the logger's itself health. Every logger has diagnostics manager of type `TacitusLogger.Diagnostics.DiagnosticsManagerBase` that is used by logger itself or injected into its exception handling strategy. If not specified explicitly, by default, it is `TacitusLogger.Diagnostics.DiagnosticsManager`. Besides the diagnostics manager, logger can be provided with diagnostics destination using `SetDiagnosticsDestination(...)` method of logger class or `WithDiagnostics(...)`. If set, this destination is used by diagnostics manager to send logs related to the logger itself, which could be logger start configuration description and logger errors sent by exception handling strategy.
 
------------------------------------------------------------------

### Exception handling strategy

Exception handling strategy defines how the logger should handle its own exceptions in `Log(...)` and `LogAsync(...)` methods. There are three built-in exception handling strategies as seen from `TacitusLogger.ExceptionHandling`:
```cs
public enum ExceptionHandling
{
    Silent = 0,
    Log = 1,
    Rethrow = 2
}
```
Just keep in mind that exception handling strategy handles an exceptions **only** in `Log(...)` and `LogAsync(...)` methods. Particularly, it does not affect exceptions that is thrown during configuration of the logger.

* `Silent` exception handling strategy is implemented in `TacitusLogger.Strategies.ExceptionHandling.SilentExceptionHandlingStrategy` class. When `Silent` strategy is used, all exceptions that flows up to `Log(...)` and `LogAsync(...)` methods from logger's components are swallowed.
* `Log` exception handling strategy is implemented in `TacitusLogger.Strategies.ExceptionHandling.LogExceptionHandlingStrategy` class. When using this strategy, exceptions caught in `Log(...)` and `LogAsync(...)` methods are send to logger's *self monitoring destination*, which was discussed above. This means that to use this strategy your logger should have self monitoring destination set. Otherwise, `Log` strategy, not having destination to sends its logs, will behave just like `Silent`. Also consider that if self monitoring destination throws any exception when being feed by `Log` strategy that exceptions will be swallowed by the strategy.
* `Rethrow` implemented in `TacitusLogger.Strategies.ExceptionHandling.RethrowExceptionHandlingStrategy` class just rethrows all exception that are caught in `Log(...)` and `LogAsync(...)` methods wrapped in `TacitusLogger.Exceptions.LoggerException` class.

Important note: The only exception that is not affected by exception handling strategy is `System.OperationCanceledException` thrown from `LogAsync(...)`. It is always rethrown and there is no way to silence or log it.


If the above strategies does not fit your needs, you can create your own exception handling strategy by deriving from `TacitusLogger.Strategies.ExceptionHandling.ExceptionHandlingStrategyBase` abstract class:
```cs
public abstract class ExceptionHandlingStrategyBase
{
    protected DiagnosticsManagerBase _diagnosticsManager;

    protected ExceptionHandlingStrategyBase();

    public abstract bool ShouldRethrow { get; }
    public DiagnosticsManagerBase DiagnosticsManager { get; }

    public abstract void HandleException(Exception exception, string context);
    public abstract Task HandleExceptionAsync(Exception exception, string context, CancellationToken cancellationToken = default(CancellationToken));
    public void SetDiagnosticsManager(DiagnosticsManagerBase diagnosticsManager);
}
```
And register it with the logger using `ResetExceptionHandlingStrategy(...)` method of the latter.

To implement a custom strategy, it is useful to briefly discuss how its methods are used by the logger instance:
When `ResetExceptionHandlingStrategy(...)` method is called and provided with an instance of strategy, inside this method the `SetHandler(...)` method of the strategy is called provided with reference to self monitoring destination. It is OK if by the time of calling `ResetExceptionHandlingStrategy(...)` self monitoring destination has not specified yet because it is passed to `SetHandler(...)` method by reference.
Every time exception is caught in `Log(...)` and `LogAsync(...)` methods, according (synchronous or asynchronous) handler method of strategy is called. After that, `ShouldRethrow` property is examined by the logger to decide should the exception be rethrown or not.


------------------------------------------------------------------

### Log serializers

Log serializers' responsibility is to create some textual representation of provided log model. Depending on serializer's implementation, this could be plain text, JSON or XML. Log serializers are used within log destinations, and help to decouple creating textual representation of the log from the specific log destination logic. You are not obligated to use serializers in your custom log destination but if you need some textual representation within your destination it would be a good practice to delegate the work to serializers and give users opportunity to inject them to your log destination.  
 
Every log serializer implements `TacitusLogger.Serializers.ILogSerializer` interface:
```cs
public interface ILogSerializer : IDisposable
{
    string Serialize(LogModel logModel); 
}
```
TacitusLogger has five built in log serializers out of the box:

`TacitusLogger.Serializers.JsonLogSerializer`
`TacitusLogger.Serializers.SimpleTemplateLogSerializer`
`TacitusLogger.Serializers.ExtendedTemplateLogSerializer`
`TacitusLogger.Serializers.FilePathTemplateLogSerializer`
`TacitusLogger.Serializers.GeneratorFunctionLogSerializer`

`JsonLogSerializer` transforms log model object into its JSON representation, `SimpleTemplateLogSerializer`, `ExtendedTemplateLogSerializer` and `FilePathTemplateLogSerializer` uses templates with placeholders to get plaintext representation of log model, `GeneratorFunctionLogSerializer` uses delegate of type `TacitusLogger.LogDataFunc<string>` provided by user to generate strings from log models.

------------------------------------------------------------------
### Setting providers

Sometimes you want to make change to different settings of logger without restarting the whole application and that is where setting providers can help. Setting providers sets some property to the logger or its components in the form of injection thus providing the ability to manage the value of this property during the runtime.

All setting providers inherit from `TacitusLogger.Setting<TValue>` class which TValue parameter represents the value itself:
```cs
public class Setting<TValue> : IDisposable
{
    protected Setting();

    public static SettingBuilder<TValue> From { get; }
    public virtual TValue Value { get; }

    public virtual void Dispose();

    public static implicit operator Setting<TValue>(TValue value);
    public static implicit operator TValue(Setting<TValue> setting);
}
```
As you see, derivatives of `TacitusLogger.Setting<TValue>` are intended to encapsulate the logic of retrieving an instance of `TValue` (from memory, config file, database etc) and/or are able to make changes to it thus affecting the components of logger which use that value during the runtime. 

`TacitusLogger.Setting<TValue>` does not have a public constructor, and can be created only by implicit conversion from the `TValue` instance:
```cs
Setting<LogLevel> logLevel = LogLevel.All;
Setting<LogGroupStatus> logGroupStatus = LogGroupStatus.Active;
```
This is useful when all you need is a constant value and you want to avoid bulky expressions in your logger configurations.

If you need more control over the value of the setting, you should use derived subclasses of `TacitusLogger.Setting<TValue>`. Out of the box TacitusLogger comes with a single subclass of the `TacitusLogger.Setting<TValue>`, that is `TacitusLogger.MutableSetting<TValue>`:
```cs
public class MutableSetting<TValue> : Setting<TValue>
{
    public MutableSetting(TValue initialValue = default(TValue));

    public override TValue Value { get; }

    public void SetValue(TValue value);

    public static implicit operator MutableSetting<TValue>(TValue value);
}
```

As you can see from the above code snippet, `TacitusLogger.MutableSetting<TValue>` has the method `SetValue()` which is the exact one that is used to change the wrapped value during the execution.

------------------------------------------------------------------

### Log contributors

Log contributors are lightweight plugins that are attached to logger and automatically add various runtime information to every created log model as additional log items. Every contributor produces a single log item that is added to the collection of log items of log model by standard log creation strategy.

Every log contributor should inherit from `TacitusLogger.Contributors.LogContributorBase` abstract class:
```cs
public abstract class LogContributorBase
{
    protected LogContributorBase(string name);

    public string Name { get; }
    public Setting<bool> IsActive { get; }

    public virtual void Dispose();
    public virtual LogItem ProduceLogItem();
    public void SetActive(Setting<bool> isActive);
    protected abstract object GenerateLogItemData();
}
```
and implement `GenerateLogItemData()` method that is supposed to return specific log item value.

When creating a log model, log creation strategy strategy iterates through all **Active** log contributors (inactive ones are ignored) and harvests log items calling `ProduceLogItem()` method of each contributor.
 
TacitusLogger package contains four log contributors out of the box:   
2. `TacitusLogger.Contributors.StackTraceContributor`
4. `TacitusLogger.Contributors.UserDataContributor`
 
#### StackTraceContributor

`StackTraceContributor` produces a log item with the default name `Stack trace` and the value containing stack trace information at the moment of writing log.
 
#### UserDataContributor

`UserDataContributor` defines an only public constructor that takes user defined name and value, and uses them to add log item to all created logs.

------------------------------------------------------------------
 
### Log Transformers

Log transformers are added to the logger and intended to make arbitrary modifications to log model after the latter is created and before is sent to log groups. Log transformers could be useful in various situations, such as strings localization, beautifying, making custom changes that should be applicable to all logs etc. Log transformers are function at logger level before the created log models reach the log groups. Logger can have zero or many log transformers and they are applied to log model is the same order in which they were added to the logger.
  
All log transformers should implement `TacitusLogger.Transformers.LogTransformerBase` abstract class:
```cs
    public abstract class LogTransformerBase
    {
        protected LogTransformerBase(string name);

        public string Name { get; }
        public Setting<bool> IsActive { get; }

        public void SetActive(Setting<bool> isActive);
        public abstract void Transform(LogModel logModel);
        public abstract Task TransformAsync(LogModel logModel, CancellationToken cancellationToken = default(CancellationToken));
    }
```
Every transformer has its name which is useful to make configuration more readable and `IsActive` status of type `Setting<bool>` which shows if this transformer is active or not. Only active transformers are applied by logger to logs, inactive ones are completely ignored. Being of type `Setting<bool>` `IsActive` status can be modified during the runtime thus allowing the logger user to toggle on/off various transformers.  

When creating your own transformers, as it was said, you have to implement  `TacitusLogger.Transformers.LogTransformerBase`  which defines two abstract methods `Transform(...)` and `TransformAsync(...)` which are intended to be called during the synchronous and asynchronous uses of logger: That is `Transform(...)` is called within the `Log(...)` method of the logger, and `TransformAsync(...)` - within the `LogAsync(...)` method. In most situations when creating your custom transformers your logic represents some quick synchronous manipulations over the log model and thus you do not have any special async logic to put into `TransformAsync(...)`. Is such scenarios you may consider to derive from `TacitusLogger.Transformers.SynchronousTransformerBase` abstract class which has already implemented `TransformAsync(...)` by calling `Transform(...)` method synchronously inside of it and returning completed task afterwards:
```cs
public abstract class SynchronousTransformerBase : LogTransformerBase
{
    public SynchronousTransformerBase(string name);

    public override Task TransformAsync(LogModel logModel, CancellationToken cancellationToken = default(CancellationToken));
}
```
in this case you will have to implement only the `Transform(...)` method.  

In some cases you may want to create a transformer that makes some custom modifications on main string properties of `LogModel` object. In such cases you may want to derive from the `TacitusLogger.Transformers.StringsTransformerBase`:
```cs
public abstract class StringsTransformerBase : SynchronousTransformerBase
{
    public StringsTransformerBase(string name);

    public override void Transform(LogModel logModel);
    protected abstract void TransformString(ref string str);
}
```
This class, inside its `Transform(...)` method goes over string properties of `LogModel` and sends each by reference to `TransformString(...)` method that you should implement with your string transformation logic. String properties which the class deal with are the following: Log ID, context, source, description, tags, name properties of log items. Take into consideration that this class does not deal with log item values.
 
TacitusLogger contains the following log transformers out of the box:  
`TacitusLogger.Transformers.ManualTransformer`
`TacitusLogger.Transformers.StringsManualTransformer`

The first one - `TacitusLogger.Transformers.ManualTransformer` - provides a way to inject a transformation logic into its constructor in the form of a transformer action - a delegate of type `Action<LogModel>` with a single `LogModel` parameter. This delegate will be called and provided with `LogModel` action on each call of `Transform(...)` method:
```cs
    public class ManualTransformer : SynchronousTransformerBase
    {
        public ManualTransformer(Action<LogModel> transformerAction, string name = "Manual transformer");

        public Action<LogModel> TransformerAction { get; }

        public override void Transform(LogModel logModel);
    }
```

The second one, `TacitusLogger.Transformers.StringsManualTransformer` can be used for similar purposes as `StringsTransformerBase` that was explained above, but with a single difference: Instead of subclassing it and implementing abstract `TransformString(...)` method, you inject `StringTransformerDelegate` delegate, containing the logic for modifying strings into its constructor. This delegate will be called for each string:
```cs
public class StringsManualTransformer : StringsTransformerBase
{
    public StringsManualTransformer(StringDelegate transformerDelegate, string name = "Strings manual transformer");

    public StringDelegate TransformerDelegate { get; }

    protected override void TransformString(ref string str);

    public delegate void StringDelegate(ref string str);
}
```

### Logger as a log destination

As you can see from `TacitusLogger.Logger` class definition, along with other interfaces it implements `TacitusLogger.Destinations.ILogDestination`:
```cs
public class Logger : ILogger, ILogDestination, IDisposable
{
    //...
    public void Send(LogModel[] logs);
    public Task SendAsync(LogModel[] logs, CancellationToken cancellationToken = default(CancellationToken));
    //...
}
```
This means that you can register an instance of `TacitusLogger.Logger` class as a log destinations with other loggers - as destinations within their log groups and/or a self monitoring destination:
```cs
Logger loggerAsDestination1 = LoggerBuilder.Logger().ForAllLogs()
                                           .Console().WithSimpleTemplateLogText("Log of type: $LogType, Description: $Description, Date: $LogDate").Add()
                                           .File().WithPath(Templates.FilePath.Template1).Add()
                                           .BuildLogger();

Logger loggerAsDestination2 = LoggerBuilder.Logger().ForAllLogs() 
                                           .Debug().Add()
                                           .BuildLogger();

// This logger uses loggerAsDestination1 and loggerAsDestination2 as log destinations for its only log group.
Logger logger = LoggerBuilder.Logger("Main").ForAllLogs()
                                            .CustomDestination(loggerAsDestination1)
                                            .CustomDestination(loggerAsDestination2)
                                            .BuildLogGroup()
                                            .BuildLogger(); 
```


## General log flow of TacitusLogger
In this section we will discuss in general the journey which is made by log event and log model after the former is sent to the logger.
#### Synchronous flow.
Synchronous flow is performed when synchronous `Log(...)` method is called by user either directly or through different extension methods that finally utilize this entry point.

1. An instance of `TacitusLogger.Log` class is received by `Log(...)` method of the logger.
2. The type of the received log is compared with the logger's log level. If the log type is less that the logger's log level then the log is ignored and no further action is taken - the `Log(...)` method returns. Otherwise, subsequent steps are performed.
3. Using *log creation strategy*, a log model instance of type `TacitusLogger.LogModel` is created and filled with data from the log event instance.
4. Additional log items are produced using *log contributors* (if any) and attached to the log model.
5. Log ID string is generated by the *log ID generator* and set to the log model.
6. After the log model is created and returned from the *log creation strategy*, the logger applies all registered log transformers to it giving the last chance to make modifications to log model before it will be sent to log groups.
7. Logger iterates through all log groups, and sends log model to active and eligible ones. Eligibility is checked by sending log model to `IsEligible(...)` method of the log group that returns `Boolean` value indicating whether or not the log group should filter out this log model. If the log model is considered eligible, it is sent to the log group.
8. Flow inside the log group depends on whether the log caching is active or not. If caching is not active, the log model is placed to an array and sent directly to *log destination feeding strategy*. Otherwise, if caching is active, log model is sent to cache using `AddToCache(...)` method of the log cache instance. This method, depending on its logic either returns null or array of all cached logs (that is said that cache flushed). In the former case, log group method returns without sending any data. In the latter case, the flushed array is sent to *log destination feeding strategy*.
9. In the log feeding strategy, log models array is eventually send to log destinations of this log group according to implementation of this feeding strategy.
10. Log destinations take the array and make actions on each log model depending on their internal logic: This could include various serializations, connecting to database, file or SMTP server etc, sending data to them. 

#### Asynchronous flow.
Asynchronous flow is performed when asynchronous `LogAsync(...)` method is called and its result is awaited. The flow is generally similar to the synchronous one described above. The main difference is that for each involved component the async versions of methods (if any) are called and awaited. For example, instead of sync  `Generate(...)` method of log ID generator its async method is called - `GenerateAsync(...)`; instead of sync `Send(...)` method of log group its async method `SendAsync(...)` is called and so on.
