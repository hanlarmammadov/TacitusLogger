using System;
using System.Collections.Generic;
using System.Text;

namespace TacitusLogger.Contributors
{
    public class StackTraceContributor : SynchronousLogContributorBase
    {
        public StackTraceContributor(string name = "Stack trace")
            : base(name)
        {

        }

        protected override Object GenerateLogItemData()
        {
            var split = Environment.StackTrace.Trim().Split(new string[] { "at " }, StringSplitOptions.None);
            KeyValuePair<string, string>[] data = new KeyValuePair<string, string>[split.Length - 1];
            for (int i = 1; i < split.Length; i++)
                data[i - 1] = new KeyValuePair<string, string>((split.Length - i).ToString(), split[i].Trim());
            return data;
        }
    }
}
