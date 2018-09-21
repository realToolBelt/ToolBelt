namespace ToolBelt.Effects
{
    public static class EffectSettings
    {
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
