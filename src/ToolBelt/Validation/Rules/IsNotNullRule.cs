using System;

namespace ToolBelt.Validation.Rules
{
    /// <summary>
    /// A validation rule that verifies that a value is not null.
    /// </summary>
    /// <typeparam name="T">The type of the value validated by the rule.</typeparam>
    /// <seealso cref="ToolBelt.Validation.IValidationRule{T}" />
    public class IsNotNullRule<T> : IValidationRule<T>
    {
        public IsNotNullRule()
        {
            // JIT-compile time check, so it doesn't even have to evaluate.
            if (default(T) != null)
            {
                throw new InvalidOperationException("IsNotNullRule<T> requires T to be a nullable type.");
            }
        }

        public string ValidationMessage
        {
            get;
            set;
        } = "Should have a value";

        public bool IsValid(T value) => value != null;
    }
}
