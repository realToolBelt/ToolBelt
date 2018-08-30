using System.ComponentModel.DataAnnotations;

namespace ToolBelt.Validation.Rules
{
    /// <summary>
    /// A validation rule that verifies that a value is an email address.
    /// </summary>
    /// <seealso cref="ToolBelt.Validation.IValidationRule{string}" />
    public class EmailRule : IValidationRule<string>
    {
        private static readonly EmailAddressAttribute _emailAddressAttribute = new EmailAddressAttribute();

        public string ValidationMessage
        {
            get;
            set;
        } = "Should be an email address";

        public bool IsValid(string value)
        {
            return _emailAddressAttribute.IsValid(value);
        }
    }
}
