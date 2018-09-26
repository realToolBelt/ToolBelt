using System.Collections.Generic;

namespace ToolBelt.Data
{
    /// <summary>
    /// Basic model representing a trade.
    /// </summary>
    public class Trade
    {
        //public List<AccountTrade> AccountTrades { get; } = new List<AccountTrade>();

        /// <summary>
        /// Gets the name of the trade.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the specialties associated with this trade.
        /// </summary>
        public List<TradeSpecialty> Specialties { get; } = new List<TradeSpecialty>();

        public List<AccountTrade> AccountTrades { get; }

        /// <summary>
        /// Gets the unique id for the trade.
        /// </summary>
        public int TradeId { get; set; }
    }
}
