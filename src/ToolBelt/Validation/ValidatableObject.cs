using ReactiveUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace ToolBelt.Validation
{
    /// <summary>
    /// An object that can be validated.
    /// </summary>
    /// <typeparam name="T">The type of the value held by the object.</typeparam>
    /// <seealso cref="ReactiveUI.ReactiveObject" />
    /// <seealso cref="ToolBelt.Validation.IValidity" />
    public class ValidatableObject<T> : ReactiveObject, IValidity, IChangeTracking
    {
        private readonly ObservableAsPropertyHelper<bool> _isValid;
        private T _initialValue;
        private T _value;

        public ValidatableObject()
        {
            Errors.Changed
                .Select(_ => Errors.Count == 0)
                .ToProperty(this, x => x.IsValid, out _isValid, initialValue: true, scheduler: Scheduler.Immediate);
        }

        public ReactiveList<string> Errors { get; } = new ReactiveList<string>();

        /// <summary>
        /// Gets the object's changed status.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the object’s content has changed since the last call to
        /// <see cref="IChangeTracking.AcceptChanges" />; otherwise, <c>false</c>.
        /// </returns>
        public bool IsChanged => !EqualityComparer<T>.Default.Equals(_initialValue, Value);

        /// <summary>
        /// Gets a value indicating whether or not the value held by this instance is valie.
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid => _isValid?.Value ?? true;

        public List<IValidationRule<T>> Validations { get; } = new List<IValidationRule<T>>();

        /// <summary>
        /// Gets or sets the value held by the object.
        /// </summary>
        public T Value
        {
            get => _value;
            set => this.RaiseAndSetIfChanged(ref _value, value);
        }

        /// <summary>
        /// Resets the object’s state to unchanged by accepting the modifications.
        /// </summary>
        public void AcceptChanges()
        {
            // NOTE: We're using extremely simple change tracking here. Simply set the "initial
            //       value" to the current value. When determining whether the object has changes,
            //       we'll simply compare these two values.
            _initialValue = Value;
        }

        /// <summary>
        /// Clears the validation errors on this instance.
        /// </summary>
        public void ClearValidationErrors()
        {
            Errors.Clear();
        }

        /// <summary>
        /// Validates the value held by this instance.
        /// </summary>
        /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
        public bool Validate()
        {
            IEnumerable<string> errors = Validations
                .Where(v => !v.IsValid(Value))
                .Select(v => v.ValidationMessage);

            Errors.Clear();
            Errors.AddRange(errors);

            return this.IsValid;
        }
    }
}