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
    public partial class ValidatedEditor : BaseValidatedEditor
    {
        public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create(
            nameof(HeaderText),
            typeof(string),
            typeof(ExtendedEntry),
            string.Empty);

        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
            nameof(MaxLength),
            typeof(int),
            typeof(ValidatedEditor),
            defaultValue: int.MaxValue);

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

                this
                    .WhenAnyValue(x => x.MaxLength)
                    .Select(length => length != (int)MaxLengthProperty.DefaultValue)
                    .BindTo(this, v => v._lblCharacters.IsVisible)
                    .DisposeWith(disposable);

                this
                    .WhenAnyValue(v => v.ViewModel.Value, v => v.MaxLength, (vm, length) => $"{(vm?.Length ?? 0)}/{length}")
                    .BindTo(this, v => v._lblCharacters.Text)
                    .DisposeWith(disposable);
            });
        }

        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }
    }
}