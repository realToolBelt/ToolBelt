namespace ToolBelt.Data
{
    /// <summary>
    /// Class representing a cross-reference between the <see cref="Account" /> and <see cref="Project" />.
    /// </summary>
    public class SavedProjects
    {
        public Account Account { get; set; }

        public string AccountId { get; set; }

        public Project Project { get; set; }

        public int ProjectId { get; set; }
    }
}
