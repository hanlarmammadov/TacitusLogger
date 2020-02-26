using System;
using System.Text;
using TacitusLogger.Exceptions;

namespace TacitusLogger.LogIdGenerators
{
    /// <summary>
    /// An implementation of ILogIdGenerator that generates log id in the form of GUID string.
    /// </summary>
    public class GuidLogIdGenerator : SynchronousLogIdGeneratorBase
    {
        private readonly string _guidFormat;
        private readonly int _substringLength;

        /// <summary>
        /// Initializes a new instance of the <c>TacitusLogger.LogIdGenerators.GuidLogIdGenerator</c> class.
        /// </summary>
        /// <param name="guidFormat">The format of string representation of GUID. Default value is "N"</param>
        /// <param name="substringLength">
        /// Substring of GUID string from the beginning of the string that will constitute the
        /// generated log id. 0 means full string without sub-stringing. Default value is 0.
        /// </param>
        public GuidLogIdGenerator(string guidFormat = "N", int substringLength = 0)
        {
            this._guidFormat = guidFormat;
            this._substringLength = substringLength;
        }

        /// <summary>
        /// Gets GUID string format.
        /// </summary>
        public string GuidFormat => _guidFormat;
        /// <summary>
        /// Gets the specified length of GUID string that will constitute the generated log id.
        /// </summary>
        public int SubstringLength => _substringLength;


        /// <summary>
        /// Generates log id string.
        /// </summary>
        /// <param name="logModel">Log model.</param>
        /// <returns>Log id string.</returns>
        public override string Generate(LogModel logModel)
        {
            try
            {
                var str = Guid.NewGuid().ToString(_guidFormat); ;
                if (_substringLength == 0)
                    return str;
                else
                    return str.Substring(0, _substringLength);
            }
            catch(Exception ex)
            {
                throw new LogIdGeneratorException("Error when generating log id GUID", ex);
            }
        }
        public override void Dispose()
        {

        }
        public override string ToString()
        {
            return new StringBuilder()
               .AppendLine(this.GetType().FullName)
               .AppendLine($"Guid format: {_guidFormat}")
               .Append($"Substring length: {_substringLength}")
               .ToString(); 
        }
    }
}
