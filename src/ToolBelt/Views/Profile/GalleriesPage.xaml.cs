using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleriesPage : ContentPageBase<GalleriesPageViewModel>
    {
        public GalleriesPage()
        {
            using (this.Log().Perf($"{nameof(GalleriesPage)}: Initialize component."))
            {
                InitializeComponent();
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(GalleriesPage)}: Activate."))
                {
                    this
                        .OneWayBind(ViewModel, vm => vm.Albums, v => v._lstAlbums.ItemsSource)
                        .DisposeWith(disposable);

                    this
                        .OneWayBind(ViewModel, vm => vm.IsBusy, v => v._lstAlbums.IsRefreshing)
                        .DisposeWith(disposable);

                    this
                        .WhenAnyValue(x => x.ViewModel)
                        .Where(vm => vm?.IsInitialized != true)
                        .ToSignal()
                        .InvokeCommand(this, x => x.ViewModel.Initialize)
                        .DisposeWith(disposable);

                    _lstAlbums
                        .ItemTappedToCommandBehavior(ViewModel, vm => vm.ViewAlbum)
                        .DisposeWith(disposable);

                    var menuItemDisposable = new SerialDisposable();
                    menuItemDisposable
                        .DisposeWith(disposable);

                    this
                        .WhenAnyValue(x => x.ViewModel.CanEdit)
                        .Select(canEdit =>
                        {
                            if (canEdit)
                            {
                                if (!ToolbarItems.Contains(_miAddAlbum))
                                {
                                    ToolbarItems.Add(_miAddAlbum);
                                }

                                menuItemDisposable.Disposable = this.BindCommand(ViewModel, vm => vm.AddAlbum, v => v._miAddAlbum);
                            }
                            else
                            {
                                ToolbarItems.Remove(_miAddAlbum);
                                menuItemDisposable.Disposable = Disposable.Empty;
                            }

                            return Unit.Default;
                        })
                        .Subscribe()
                        .DisposeWith(disposable);
                }
            });
        }
    }

    public class GalleryCellDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DeletableTemplate { get; set; }

        public DataTemplate ReadOnlyTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (container.BindingContext is GalleriesPageViewModel vm)
            {
                return vm.CanEdit ? DeletableTemplate : ReadOnlyTemplate;
            }

            return null;
        }
    }
}