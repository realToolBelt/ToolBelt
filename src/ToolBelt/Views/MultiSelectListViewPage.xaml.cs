using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using ReactiveUI;
using ToolBelt.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultiSelectListViewPage : ContentPageBase<MultiSelectListViewPageViewModel>
    {
        public MultiSelectListViewPage ()
        {
            InitializeComponent ();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                this
                    .BindCommand(ViewModel, vm => vm.SelectAll, v => v._miSelectAll)
                    .DisposeWith(disposable);

                this
                    .BindCommand(ViewModel, vm => vm.SelectNone, v => v._miSelectNone)
                    .DisposeWith(disposable);

                this
                    .OneWayBind(ViewModel, vm => vm.Items, v => v._lstItems.ItemsSource)
                    .DisposeWith(disposable);

                _lstItems
                    .Events()
                    .ItemSelected
                    .Select(item => item.SelectedItem as SelectionViewModel)
                    .Where(item => item != null)
                    .Subscribe(item =>
                    {
                        item.IsSelected = !item.IsSelected;
                        _lstItems.SelectedItem = null;
                    })
                    .DisposeWith(disposable);
            });
        }
    }

    public class MultiSelectListViewPageViewModel : BaseViewModel
    {
        public MultiSelectListViewPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectAll = ReactiveCommand.Create(() =>
            {
                foreach (var item in Items)
                {
                    item.IsSelected = true;
                }
            });

            SelectNone = ReactiveCommand.Create(() =>
            {
                foreach (var item in Items)
                {
                    item.IsSelected = false;
                }
            });

            NavigatedTo
                .Select(args => (IEnumerable<SelectionViewModel>)args["items"])
                .Subscribe(items => Items.AddRange(items));

            NavigatedFrom
                .Subscribe(args => args.Add("selected_items", Items.Where(item => item.IsSelected).ToList()));
        }

        public ReactiveList<SelectionViewModel> Items { get; } = new ReactiveList<SelectionViewModel>();

        public ReactiveCommand SelectAll { get; }

        public ReactiveCommand SelectNone { get; }
    }
}