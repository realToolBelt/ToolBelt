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
    public class BaseValidatedEditor : ReactiveContentView<ValidatableObject<string>>
    {
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ValidatedEditor : BaseValidatedEntry
    {
        public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create(
            nameof(HeaderText),
            typeof(string),
            typeof(ExtendedEntry),
            string.Empty);

        public ValidatedEditor()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this
                    .Bind(ViewModel, vm => vm.Value, v => v._txtEntry.Text)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.IsValid, v => v._txtEntry.IsValid)
                    .DisposeWith(disposable);

                var errorsChanged = this
                    .WhenAnyObservable(v => v.ViewModel.Errors.Changed)
                    .Select(_ => ViewModel.Errors)
                    .Publish()
                    .RefCount();

                errorsChanged
                    .Select(errors => errors.FirstOrDefault())
                    .StartWith(ViewModel?.Errors ?? Enumerable.Empty<string>())
                    .BindTo(this, v => v._lblEntryError.Text)
                    .DisposeWith(disposable);

                errorsChanged
                    .Select(errors => errors.Count > 0)
                    .StartWith(ViewModel?.Errors.Count > 0)
                    .BindTo(this, v => v._lblEntryError.IsVisible)
                    .DisposeWith(disposable);

                _txtEntry
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
    }
}