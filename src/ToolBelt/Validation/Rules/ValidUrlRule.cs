using System.ComponentModel.DataAnnotations;

namespace ToolBelt.Validation.Rules
{
    /// <summary>
    /// A validation rule that verifies that a value is a valid URL.
    /// </summary>
    /// <seealso cref="ToolBelt.Validation.IValidationRule{string}" />
    public class ValidUrlRule : IValidationRule<string>
    {
        private static readonly UrlAttribute _validationAttribute = new UrlAttribute();

        public string ValidationMessage
        {
            get;
            set;
        } = "Should be a URL";

        public bool IsValid(string value)
        {
            return _validationAttribute.IsValid(value);
        }
    }
}
