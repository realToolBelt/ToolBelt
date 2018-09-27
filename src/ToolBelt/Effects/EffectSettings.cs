namespace ToolBelt.Effects
{
    /// <summary>
    /// Class to hold common settings related to effects. This ensures we don't have any typos
    /// between the shared library and the downstram (Android, iOS, ...) projects.
    /// </summary>
    public static class EffectSettings
    {
        /// <summary>
        /// The resolution group name used to resolve effects.
        /// </summary>
        /// <remarks>
        /// This must be a constant for use in the platform-specific project's effect attributes.
        /// </remarks>
        public const string ResolutionGroupName = "ToolBelt";

        /// <summary>
        /// Gets the ID for the <see cref="DatePickerLineColorEffect" />.
        /// </summary>
        public static string DatePickerLineColor { get; } = $"{ResolutionGroupName}.{nameof(DatePickerLineColorEffect)}";

        /// <summary>
        /// Gets the ID for the <see cref="EditorLineColorEffect" />.
        /// </summary>
        public static string EditorLineColor { get; } = $"{ResolutionGroupName}.{nameof(EditorLineColorEffect)}";

        /// <summary>
        /// Gets the ID for the <see cref="EntryLineColorEffect" />.
        /// </summary>
        public static string EntryLineColor { get; } = $"{ResolutionGroupName}.{nameof(EntryLineColorEffect)}";

        /// <summary>
        /// Gets the ID for the <see cref="PickerLineColorEffect" />.
        /// </summary>
        public static string PickerLineColor { get; } = $"{ResolutionGroupName}.{nameof(PickerLineColorEffect)}";
    }
}
