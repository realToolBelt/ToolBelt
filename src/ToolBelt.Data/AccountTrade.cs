namespace ToolBelt.Data
{
    /// <summary>
    /// Class representing a cross-reference between the <see cref="Account" /> and <see cref="Trade" />.
    /// </summary>
    public class AccountTrade
    {
        public Account Account { get; set; }

        public string AccountId { get; set; }

        public Trade Trade { get; set; }

        public int TradeId { get; set; }
    }
}
