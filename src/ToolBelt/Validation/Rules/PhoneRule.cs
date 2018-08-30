using System.ComponentModel.DataAnnotations;

namespace ToolBelt.Validation.Rules
{
    /// <summary>
    /// A validation rule that verifies that a string is a phone number.
    /// </summary>
    /// <seealso cref="ToolBelt.Validation.IValidationRule{string}" />
    public class PhoneRule : IValidationRule<string>
    {
        private static readonly PhoneAttribute _validator = new PhoneAttribute();

        public string ValidationMessage
        {
            get;
            set;
        } = "Should be a phone number";

        public bool IsValid(string value) => _validator.IsValid(value);
    }
}
