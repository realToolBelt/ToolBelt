using System.Collections.Generic;

namespace ToolBelt.Data
{
    /// <summary>
    /// The basic information for an account in the system.
    /// </summary>
    public abstract class Account
    {
        /// <summary>
        /// Gets or sets the unique identifier for the account.
        /// </summary>
        public string AccountId { get; set; }

        public List<AccountTrade> AccountTrades { get; }

        /// <summary>
        /// Gets or sets the email address for the account.
        /// </summary>
        public string EmailAddress { get; set; }
    }
}
