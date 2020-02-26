using System; 
using System.Text;

namespace TacitusLogger.Components.Strings
{
   public static class LoggerDescriptionHelper
    {
        public static void AddObjectToNumberedList(Object obj, int index, StringBuilder stringBuilder)
        {
            var str = $"{Environment.NewLine}{index.ToString()}. {obj.ToString()}";
            str = str.Replace(Environment.NewLine, Environment.NewLine + Defaults.ToStringIndentation);
            stringBuilder.Append(str);
        } 
    }

    public static class StringExtensions
    {
        public static string AddIndentationToLines(this string str)
        {
            str = str.Replace(Environment.NewLine, Environment.NewLine + Defaults.ToStringIndentation);
            return str;
        }
    }
}
