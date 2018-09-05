using Prism.Navigation;
using ReactiveUI.XamForms;
using System.Linq;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ReactiveTabbedPage<MainPageViewModel>, INavigatingAware
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            // if we're given a page to navigate to, select that page
            if (parameters.TryGetValue("page", out string pageName))
            {
                var pageToView = Children.FirstOrDefault(p => p.GetType().Name == pageName);
                if (pageToView != null)
                {
                    SelectedItem = pageToView;
                }
            }
        }
    }
}