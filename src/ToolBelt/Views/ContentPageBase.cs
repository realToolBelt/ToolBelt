using ReactiveUI.XamForms;
using Splat;
using ToolBelt.Services;
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
        private const double SizeNotAllocated = -1;
        private double _height;
        private double _width;

        public ContentPageBase()
        {
            Init();

            // bind the Title and Icon by default
            SetBinding(TitleProperty, new Binding(nameof(BaseViewModel.Title), BindingMode.OneWay));
            SetBinding(IconProperty, new Binding(nameof(BaseViewModel.Icon), BindingMode.OneWay));

            // Set the background color of the page to the primary background color for the application
            SetDynamicResource(BackgroundColorProperty, "primaryBackgroundColor");

            // make sure the page honors iOS safe areas (eg. plays nice with the notch)
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        /// <summary>
        /// Called when the orientation is changed.
        /// </summary>
        /// <param name="orientation">The orientation.</param>
        protected virtual void OnOrientationChanged(DeviceOrientations orientation)
        {
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            var oldWidth = _width;

            base.OnSizeAllocated(width, height);

            // if the sizes are the same, we don't need to do anything else
            if (Equals(_width, width) && Equals(_height, height))
            {
                return;
            }

            _width = width;
            _height = height;

            // ignore if the previous height was size unallocated
            if (Equals(oldWidth, SizeNotAllocated))
            {
                return;
            }

            // Has the device been rotated ?
            if (!Equals(width, oldWidth))
            {
                OnOrientationChanged(width < height ? DeviceOrientations.Portrait : DeviceOrientations.Landscape);
            }
        }

        private void Init()
        {
            _width = Width;
            _height = Height;
        }
    }
}
