using System.Collections.Generic;

namespace ToolBelt.Data
{
    /// <summary>
    /// Basic model representing a trade specialty.
    /// </summary>
    public class TradeSpecialty
    {
        /// <summary>
        /// Gets the name of the specialty.
        /// </summary>
        public string Name { get; set; }

        public List<ProjectTradeSpecialty> ProjectTradeSpecialties { get; } = new List<ProjectTradeSpecialty>();

        /// <summary>
        /// Gets or sets the <see cref="Trade" /> this specialty is associated to.
        /// </summary>
        public Trade Trade { get; set; }

        /// <summary>
        /// Gets or sets the id of the <see cref="Trade" /> this specialty is associated to.
        /// </summary>
        public int TradeId { get; set; }

        /// <summary>
        /// Gets the unique id for the specialty.
        /// </summary>
        public int TradeSpecialtyId { get; set; }
    }
}
