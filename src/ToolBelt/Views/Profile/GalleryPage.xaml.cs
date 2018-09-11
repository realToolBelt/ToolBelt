using ReactiveUI;
using Splat;
using ToolBelt.Extensions;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryPage : ContentPageBase<GalleryPageViewModel>
    {
        public GalleryPage()
        {
            using (this.Log().Perf($"{nameof(GalleryPage)}: Initialize component."))
            {
                InitializeComponent();
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(GalleryPage)}: Activate."))
                {
                }
            });
        }
    }
}