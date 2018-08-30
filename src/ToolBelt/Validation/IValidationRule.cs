namespace ToolBelt.Validation
{
    /// <summary>
    /// Interface defining the necessary elements for a validation rule.
    /// </summary>
    /// <typeparam name="T">The type of the value validated by the rule.</typeparam>
    public interface IValidationRule<in T>
    {
        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        string ValidationMessage { get; set; }

        /// <summary>
        /// Checks whether the rule is valid or not when applied to the <paramref name="value" />.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>
        /// <c>true</c> if the specified <paramref name="value" /> is valid; otherwise, <c>false</c>.
        /// </returns>
        bool IsValid(T value);
    }
}
