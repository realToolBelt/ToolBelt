﻿using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Projects
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectsPage : ContentPageBase<ProjectsPageViewModel>
    {
        public ProjectsPage()
        {
            using (this.Log().Perf($"{nameof(ProjectsPage)}: Initialize component."))
            {
                InitializeComponent();
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(ProjectsPage)}: Activate."))
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.Projects, v => v._lstProjects.ItemsSource)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.AddProject, v => v._miAddProject)
                        .DisposeWith(disposable);

                    this
                        .BindCommand(ViewModel, vm => vm.Filter, v => v._miFilter)
                        .DisposeWith(disposable);

                    _lstProjects
                        .ItemTappedToCommandBehavior(ViewModel, vm => vm.ViewProjectDetails)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.IsBusy, v => v._lstProjects.IsRefreshing)
                        .DisposeWith(disposable);

                    _lstProjects
                        .Events()
                        .Refreshing
                        .ToSignal()
                        .Merge(
                            this
                                .WhenAnyValue(x => x.ViewModel)
                                .Where(vm => vm?.Projects.Count == 0)
                                .Take(1)
                                .ToSignal()
                        )
                        .ObserveOn(RxApp.TaskpoolScheduler)
                        .InvokeCommand(this, x => x.ViewModel.RefreshProjects)
                        .DisposeWith(disposable);

                    _lstProjects
                        .Events()
                        .ItemAppearing
                        .Where(_ => !ViewModel.IsBusy && ViewModel.Projects.Count > 0)
                        .Where(args => args.Item == ViewModel.Projects[ViewModel.Projects.Count - 1])
                        .ToSignal()
                        .ObserveOn(RxApp.TaskpoolScheduler)
                        .InvokeCommand(this, x => x.ViewModel.LoadProjects)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}