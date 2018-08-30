namespace ToolBelt.Validation.Rules
{
    /// <summary>
    /// A validation rule that verifies that a value is not null.
    /// </summary>
    /// <typeparam name="T">The type of the value validated by the rule.</typeparam>
    /// <seealso cref="ToolBelt.Validation.IValidationRule{T}" />
    public class IsNotNullRule<T> : IValidationRule<T>
        where T : class
    {
        public string ValidationMessage
        {
            get;
            set;
        } = "Should have a value";

        public bool IsValid(T value) => value != null;
    }
}
