using Prism.Navigation;
using System.Reactive.Linq;
using ToolBelt.ViewModels;
using System;

namespace ToolBelt.Views.Profile
{
    public class GalleryPageViewModel : BaseViewModel
    {
        public GalleryPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            // TODO: Update this to be the incoming gallery
            Title = "Gallery";

            NavigatingTo
                .Select(args => (int)args["album"])
                .Subscribe(albumId =>
                {
                    // TODO: Load the album...
                });
        }
    }
}