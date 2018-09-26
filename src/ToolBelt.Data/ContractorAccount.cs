namespace ToolBelt.Data
{
    /// <summary>
    /// The account information for a contractor.
    /// </summary>
    /// <seealso cref="ToolBelt.Services.Account" />
    // TODO: [Table("ContractorAccount")]
    public class ContractorAccount : Account
    {
        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the URL for the company.
        /// </summary>
        public string CompanyUrl { get; set; }

        /// <summary>
        /// Gets or sets the phone number for the company.
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
