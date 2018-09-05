using Prism.Navigation;
using ReactiveUI.XamForms;
using Splat;
using ToolBelt.Extensions;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileViewTabbedPage : ReactiveTabbedPage<ProfileViewTabbedPageViewModel>, IEnableLogger, INavigatingAware
    {
        public ProfileViewTabbedPage()
        {
            using (this.Log().Perf($"{nameof(ProfileViewTabbedPage)}: Initialize component."))
            {
                InitializeComponent();
            }
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            foreach (var child in Children)
            {
                (child as INavigatingAware)?.OnNavigatingTo(parameters);
                (child?.BindingContext as INavigatingAware)?.OnNavigatingTo(parameters);
            }
        }
    }
}