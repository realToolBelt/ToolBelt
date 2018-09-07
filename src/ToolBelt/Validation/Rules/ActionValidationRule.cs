using System;

namespace ToolBelt.Validation.Rules
{
    /// <summary>
    /// A validation rule that uses an action to perform the validation.
    /// </summary>
    /// <typeparam name="T">The type of the value validated by the rule.</typeparam>
    /// <seealso cref="ToolBelt.Validation.IValidationRule{T}" />
    public class ActionValidationRule<T> : IValidationRule<T>
    {
        private readonly Func<T, bool> _predicate;

        public ActionValidationRule(Func<T, bool> predicate, string validationMessage)
        {
            _predicate = predicate;
            ValidationMessage = validationMessage;
        }

        public string ValidationMessage { get; set; }

        public bool IsValid(T value)
        {
            return _predicate.Invoke(value);
        }
    }
}
