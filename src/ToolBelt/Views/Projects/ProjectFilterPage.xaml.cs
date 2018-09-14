using ReactiveUI;
using Splat;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Projects
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectFilterPage : ContentPageBase<ProjectFilterPageViewModel>
    {
        public ProjectFilterPage()
        {
            using (this.Log().Perf($"{nameof(ProjectFilterPage)}: Initialize component."))
            {
                InitializeComponent();
            }

            // handle when the view is activated
            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(ProjectFilterPage)}: Activate."))
                {
                    this
                        .BindCommand(ViewModel, vm => vm.Cancel, v => v._miCancel)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Apply, v => v._btnApply)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.DateComparisonOptions, v => v._startDateComparisonTypePicker.ItemsSource)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.DateComparisonOptions, v => v._endDateComparisonTypePicker.ItemsSource)
                        .DisposeWith(disposable);

                    this
                        .Bind(ViewModel, vm => vm.SelectedStartDateComparisonType, v => v._startDateComparisonTypePicker.SelectedItem,
                            vmToViewConverter: x => x.ToString(),
                            viewToVmConverter: v => (DateComparisonType)Enum.Parse(typeof(DateComparisonType), v.ToString()))
                        .DisposeWith(disposable);

                    this
                        .Bind(ViewModel, vm => vm.SelectedEndDateComparisonType, v => v._endDateComparisonTypePicker.SelectedItem,
                            vmToViewConverter: x => x.ToString(),
                            viewToVmConverter: v => (DateComparisonType)Enum.Parse(typeof(DateComparisonType), v.ToString()))
                        .DisposeWith(disposable);

                    this
                        .Bind(ViewModel, vm => vm.SelectedStartDate, v => v._startDatePicker.NullableDate)
                        .DisposeWith(disposable);

                    this
                        .Bind(ViewModel, vm => vm.SelectedEndDate, v => v._endDatePicker.NullableDate)
                        .DisposeWith(disposable);

                    _vcTrades
                        .Events()
                        .Tapped
                        .ToSignal()
                        .InvokeCommand(this, v => v.ViewModel.SelectTrades)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}