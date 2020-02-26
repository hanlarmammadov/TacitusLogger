using System.IO;

namespace TacitusLogger.Serializers
{
    public static class Templates
    {
        public static class Simple
        {
            public const string Default = "[$LogDate]-[$LogType]-[$Description]-[From: $Context]-[Src: $Source]-[Tags: $Tags]-[Id: $LogId]";
            public const string Template1 = "[$LogDate]-[$LogType]-[$Description]-[From: $Context]-[Src: $Source]-[Id: $LogId]";
            public const string Template2 = "[$LogDate]-[$LogType]-[$Description]-[From: $Context]-[Id: $LogId]";
            public const string Template3 = "[$LogDate]-[$LogType]-[$Description]-[From: $Context]-[Id: $LogId(6)]";
            public const string Template4 = "[$LogDate]-[$LogType]-[$Description]-[Id: $LogId(6)]";
            public const string Template5 = "[$LogDate(HH:mm)]-[$LogType]-[$Description]-[Id: $LogId(6)]";
            public const string Template6 = "[$LogDate(HH:mm)]-[$LogType(4)]-[$Description]-[Id: $LogId(6)]";
            public const string Template7 = "[$LogId(6)]-[$LogDate(HH:mm)]-[$LogType(4)]-[$Description]";
            public const string Template8 = "[$LogDate]-[$LogType]-[$Description]-[$Context]";
            public const string Template9 = "[$LogDate]-[$LogType(4)]-[$Description]-[$Context]";
            public const string Template10 = "[$LogDate]-[$LogType]-[$Description]";
            public const string Template11 = "[$LogDate]-[$LogType(4)]-[$Description]";
        }
        public static class Extended
        {
            public const string Default = "Log Id:        $LogId$NewLineLog type:      $LogType$NewLineDescription:   $Description$NewLineSource:        $Source$NewLineContext:       $Context$NewLineTags:          $Tags$NewLineLog date:      $LogDate$NewLine$LogItems$NewLine-------------------------------------$NewLine";
        }
        public static class FilePath
        {
            public static readonly string Default = $".{Path.DirectorySeparatorChar}Logs-$LogDate.log";
            public static readonly string Template1 = $".{Path.DirectorySeparatorChar}$LogTypeLogs-$LogDate.log";
            public static readonly string Template2 = $".{Path.DirectorySeparatorChar}$LogTypeLogs.log";
            public static readonly string Template3 = $".{Path.DirectorySeparatorChar}Logs.log";
            public static readonly string Template4 = $".{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}$LogTypeLogs{Path.DirectorySeparatorChar}$LogTypeLogs-$LogDate.log";
            public static readonly string Template5 = $".{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}$LogId.log";
            public static readonly string Template6 = $".{Path.DirectorySeparatorChar}Logs{Path.DirectorySeparatorChar}$LogId(8).log";
        }
        public static class DateFormats
        {
            /// <summary>
            /// Example: 14-Jan-20 15:31:24
            /// </summary>
            public const string Default = "dd-MMM-yy HH:mm:ss";
            /// <summary>
            /// Example: 14-Jan-2020
            /// </summary>
            public const string DefaultFileName = "dd-MMM-yyyy";
            /// <summary>
            /// Example: 22:53
            /// </summary>
            public const string VeryShortTime = "HH:mm";
        }
    }
}
