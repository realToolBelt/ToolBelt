using Acr.UserDialogs;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
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
        private readonly ObservableAsPropertyHelper<bool> _isInitialized;
        private readonly IUserService _userService;
        private bool _canEdit;

        // TODO: Allow delete and add only if current user is whose albums are being viewed

        public GalleriesPageViewModel(
            INavigationService navigationService,
            IAlbumDataStore albumDataStore,
            IUserDialogs dialogService,
            IUserService userService,
            IAnalyticService analyticService) : base(navigationService)
        {
            Title = "Galleries";
            _userService = userService;

            Initialize = ReactiveCommand.CreateFromTask(async () =>
            {
                analyticService.TrackScreen("profile-gallery");
                var random = new Random();
                await Task.Delay(random.Next(400, 2000));

                var albums = await albumDataStore.GetAlbumsAsync(1).ConfigureAwait(false);
                foreach (var album in albums)
                {
                    Albums.Add(new AlbumImageCellViewModel(album));
                }
            });

            // when the command is executing, update the busy state
            Initialize.IsExecuting
              .StartWith(false)
              .ToProperty(this, x => x.IsBusy, out _isBusy);

            Initialize.IsExecuting
              .StartWith(false)
              .Where(isExecuting => isExecuting)
              .Take(1)
              .ToProperty(this, v => v.IsInitialized, out _isInitialized);

            Delete = ReactiveCommand.CreateFromTask<AlbumImageCellViewModel, Unit>(async vm =>
            {
                bool shouldDelete = await dialogService.ConfirmAsync(
                    new ConfirmConfig
                    {
                        Message = "Are you sure you want to delete the album?",
                        OkText = "Delete",
                        CancelText = "Cancel"
                    });
                if (!shouldDelete)
                {
                    return Unit.Default;
                }

                analyticService.TrackTapEvent("delete-album");

                // TODO: Perform delete (and log with analytics service)
                return Unit.Default;
            });

            AddAlbum = ReactiveCommand.CreateFromTask(async () =>
            {
                var response = await dialogService.PromptAsync(
                    new PromptConfig
                    {
                        Title = "Create Album",
                        OkText = "Create",
                        CancelText = "Cancel"
                    });
                if (response.Ok)
                {
                    analyticService.TrackTapEvent("new-album");

                    // TODO: ...
                }
            });

            ViewAlbum = ReactiveCommand.CreateFromTask<AlbumImageCellViewModel, Unit>(async album =>
            {
                analyticService.TrackTapEvent("view-album");
                await NavigationService.NavigateAsync(
                    nameof(GalleryPage),
                    new NavigationParameters
                    {
                        {"album", album.Album.Id }
                    })
                    .ConfigureAwait(false);

                return Unit.Default;
            });

            Initialize.ThrownExceptions
                .Merge(Delete.ThrownExceptions)
                .Merge(AddAlbum.ThrownExceptions)
                .Merge(ViewAlbum.ThrownExceptions)
                .Subscribe(ex => System.Diagnostics.Debug.WriteLine(ex.ToString()));
        }

        public ReactiveCommand AddAlbum { get; }

        public ReactiveList<AlbumImageCellViewModel> Albums { get; } = new ReactiveList<AlbumImageCellViewModel>();

        public bool CanEdit
        {
            get => _canEdit;
            private set => this.RaiseAndSetIfChanged(ref _canEdit, value);
        }

        public ReactiveCommand<AlbumImageCellViewModel, Unit> Delete { get; }

        public ReactiveCommand Initialize { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is busy performing work.
        /// </summary>
        public bool IsBusy => _isBusy?.Value ?? false;

        public bool IsInitialized => _isInitialized?.Value ?? false;

        public ReactiveCommand<AlbumImageCellViewModel, Unit> ViewAlbum { get; }

        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            var user = parameters["user"] as User;
            if (user?.Id == _userService.AuthenticatedUser.Id)
            {
                CanEdit = true;
                this.RaisePropertyChanged(nameof(Albums));
            }
        }

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