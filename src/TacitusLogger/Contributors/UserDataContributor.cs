using System;
using System.Collections.Generic;
using System.Text;

namespace TacitusLogger.Contributors
{
    /// <summary>
    /// Allows to add log items to generated logs.
    /// </summary>
    public class UserDataContributor : SynchronousLogContributorBase
    {
        private readonly Object _data;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Contributors.UserDataContributor</c>
        /// </summary>
        /// <param name="name">Log attachment name.</param>
        /// <param name="data">Custom log item data.</param>
        public UserDataContributor(string name, Object data)
            : base(name)
        {
            _data = data;
        }

        /// <summary>
        /// Gets the data that was specified during the initialization. For testing purposes.
        /// </summary>
        internal Object Data => _data;
         
        protected override object GenerateLogItemData()
        {
            return _data;
        }
    }
}
