namespace ToolBelt.Models
{
    /// <summary>
    /// Basic model representing a trade specialty.
    /// </summary>
    public class TradeSpecialty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TradeSpecialty" /> class.
        /// </summary>
        /// <param name="id">The unique id of the specialty.</param>
        /// <param name="name">The name of the specialty.</param>
        public TradeSpecialty(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Gets the unique id for the specialty.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the name of the specialty.
        /// </summary>
        public string Name { get; }
    }
}
