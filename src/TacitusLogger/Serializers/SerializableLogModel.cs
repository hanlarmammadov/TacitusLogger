using System;

namespace TacitusLogger.Serializers
{
    public class SerializableLogModel
    {
        public string LogId;
        public string Context;
        public string[] Tags;
        public string Source;
        public LogType LogType;
        public string Description;
        public LogItem[] LogItems;
        public DateTime LogDate;

        public SerializableLogModel(LogModel logModel)
        {
            if (logModel == null)
                return;

            LogId = logModel.LogId;
            Context = logModel.Context;
            Tags = logModel.Tags;
            Source = logModel.Source;
            LogType = logModel.LogType;
            Description = logModel.Description;
            LogItems = logModel.LogItems;
            LogDate = logModel.LogDate;
        }
    }
}
