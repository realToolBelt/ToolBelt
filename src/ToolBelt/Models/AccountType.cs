namespace ToolBelt.Models
{
    /// <summary>
    /// Enumeration containing the types of accounts within the system.
    /// </summary>
    public enum AccountType
    {
        // NOTE: 0 will be reserved for future use
        Contractor = 1 << 0,

        Tradesmen = 1 << 1
    }
}
