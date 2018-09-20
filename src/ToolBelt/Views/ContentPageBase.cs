using ReactiveUI.XamForms;
using Splat;
using ToolBelt.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace ToolBelt.Views
{
    /// <summary>
    /// Base content page for the application.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <seealso cref="ReactiveUI.XamForms.ReactiveContentPage{TViewModel}" />
    public class ContentPageBase<TViewModel> : ReactiveContentPage<TViewModel>, IEnableLogger
        where TViewModel : BaseViewModel
    {
        public ContentPageBase()
        {
            // bind the Title and Icon by default
            SetBinding(TitleProperty, new Binding(nameof(BaseViewModel.Title), BindingMode.OneWay));
            SetBinding(IconProperty, new Binding(nameof(BaseViewModel.Icon), BindingMode.OneWay));

            // Set the background color of the page to the primary background color for the application
            SetDynamicResource(BackgroundColorProperty, "primaryBackgroundColor");

            // make sure the page honors iOS safe areas (eg. plays nice with the notch)
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}
