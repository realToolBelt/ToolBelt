using ReactiveUI;
using ReactiveUI.XamForms;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Validation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Controls
{
    public class BaseValidatedDatePicker : ReactiveContentView<ValidatableObject<DateTime?>>
    {
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ValidatedDatePicker : BaseValidatedDatePicker
    {
        public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create(
            nameof(HeaderText),
            typeof(string),
            typeof(ValidatedDatePicker),
            string.Empty);

        public static readonly BindableProperty MaximumDateProperty = BindableProperty.Create(
            nameof(MaximumDate),
            typeof(DateTime),
            typeof(ValidatedDatePicker),
            new DateTime(2100, 12, 31));

        public static readonly BindableProperty MinimumDateProperty = BindableProperty.Create(
            nameof(MinimumDate),
            typeof(DateTime),
            typeof(ValidatedDatePicker),
            new DateTime(1900, 1, 1));

        public ValidatedDatePicker()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this
                    .OneWayBind(this, vm => vm.MaximumDate, v => v._datePicker.MaximumDate)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(this, vm => vm.MinimumDate, v => v._datePicker.MinimumDate)
                    .DisposeWith(disposable);

                this
                    .Bind(ViewModel, vm => vm.Value, v => v._datePicker.NullableDate)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.IsValid, v => v._datePicker.IsValid)
                    .DisposeWith(disposable);

                var errorsChanged = this
                    .WhenAnyObservable(v => v.ViewModel.Errors.Changed)
                    .Select(_ => ViewModel.Errors)
                    .Publish()
                    .RefCount();

                errorsChanged
                    .Select(errors => errors.FirstOrDefault())
                    .StartWith(ViewModel?.Errors ?? Enumerable.Empty<string>())
                    .BindTo(this, v => v._lblPickerError.Text)
                    .DisposeWith(disposable);

                errorsChanged
                    .Select(errors => errors.Count > 0)
                    .StartWith(ViewModel?.Errors.Count > 0)
                    .BindTo(this, v => v._lblPickerError.IsVisible)
                    .DisposeWith(disposable);

                _datePicker
                    .Events()
                    .Focused
                    .Where(args => args.IsFocused)
                    .Subscribe(_ => ViewModel.ClearValidationErrors())
                    .DisposeWith(disposable);
            });
        }

        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        public DateTime MaximumDate
        {
            get => (DateTime)GetValue(MaximumDateProperty);
            set => SetValue(MaximumDateProperty, value);
        }

        public DateTime MinimumDate
        {
            get => (DateTime)GetValue(MinimumDateProperty);
            set => SetValue(MinimumDateProperty, value);
        }
    }
}