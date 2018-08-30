namespace ToolBelt.Validation.Rules
{
    /// <summary>
    /// A validation rule that verifies that a string is not null or empty.
    /// </summary>
    /// <seealso cref="ToolBelt.Validation.IValidationRule{string}" />
    public class IsNotNullOrEmptyRule : IValidationRule<string>
    {
        public string ValidationMessage
        {
            get;
            set;
        } = "Should not be empty";

        public bool IsValid(string value) => !string.IsNullOrWhiteSpace(value);
    }
}
