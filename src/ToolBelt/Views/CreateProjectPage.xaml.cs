using ReactiveUI;
using Splat;
using System;
using System.Reactive.Disposables;
using ToolBelt.Extensions;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateProjectPage : ContentPageBase<CreateProjectPageViewModel>
    {
        public CreateProjectPage()
        {
            using (this.Log().Perf($"{nameof(CreateProjectPage)}: Initialize component."))
            {
                InitializeComponent();
                _startDateControl.MinimumDate = DateTime.Today;
                _endDateControl.MinimumDate = DateTime.Today;
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(CreateProjectPage)}: Activate."))
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.ProjectName, v => v._projectNameControl.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.StartDate, v => v._startDateControl.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.EndDate, v => v._endDateControl.ViewModel)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Save, v => v._miSave)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Cancel, v => v._miCancel)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}