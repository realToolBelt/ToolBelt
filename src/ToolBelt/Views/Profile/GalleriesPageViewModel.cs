using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ToolBelt.Models;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Profile
{
    public class GalleriesPageViewModel : BaseViewModel
    {
        private readonly ObservableAsPropertyHelper<bool> _isBusy;

        public GalleriesPageViewModel(
            INavigationService navigationService,
            IAlbumDataStore albumDataStore) : base(navigationService)
        {
            Title = "Galleries";

            Initialize = ReactiveCommand.CreateFromTask(async () =>
            {
                var random = new Random();
                await Task.Delay(random.Next(400, 2000));

                var albums = await albumDataStore.GetAlbumsAsync(1).ConfigureAwait(false);

                //Projects.Reset(projects);
                foreach (var album in albums)
                {
                    Albums.Add(new AlbumImageCellViewModel(album));
                }
            });

            // when the command is executing, update the busy state
            Initialize.IsExecuting
              .StartWith(false)
              .ToProperty(this, x => x.IsBusy, out _isBusy);
        }

        public ReactiveList<AlbumImageCellViewModel> Albums { get; } = new ReactiveList<AlbumImageCellViewModel>();

        public ReactiveCommand Initialize { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is busy performing work.
        /// </summary>
        public bool IsBusy => _isBusy?.Value ?? false;

        // TODO: Pull this class out and return it as a summary when selecting album summaries
        // TODO: Include number of photos in the album, album picture, album name
        public class AlbumImageCellViewModel : IImageCellViewModel
        {
            public AlbumImageCellViewModel(Album album)
            {
                Album = album;
                Text = album.Name;
                Detail = album.Description;
            }

            public Album Album { get; }

            public string Detail { get; }

            public byte[] ImageSource { get; }

            public string Text { get; }
        }
    }
}