using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewsPage : ReactiveContentView<ReviewsPageViewModel>
    {
        public ReviewsPage(ReviewsPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            this.WhenActivated(disposable =>
            {

            });
        }
    }
}