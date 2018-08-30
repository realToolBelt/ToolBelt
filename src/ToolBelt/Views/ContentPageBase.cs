using ReactiveUI.XamForms;
using Splat;
using ToolBelt.ViewModels;
using Xamarin.Forms;

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
        }
    }
}
