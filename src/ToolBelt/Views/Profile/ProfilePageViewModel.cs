﻿using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using ToolBelt.Data;
using ToolBelt.Services;
using ToolBelt.ViewModels;

namespace ToolBelt.Views.Profile
{
    public class ProfilePageViewModel : BaseViewModel
    {
        private Account _user;

        public ProfilePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Profile";

            Edit = ReactiveCommand.CreateFromTask(async () =>
            {
                var parameters = new NavigationParameters
                {
                    { "user", User }
                };

                await NavigationService.NavigateAsync($"NavigationPage/{nameof(EditableProfilePage)}", parameters, useModalNavigation: true).ConfigureAwait(false);
            });

            NavigatedTo
                .Take(1)
                .Select(args => args["user"] as Account)
                .Subscribe(user =>
                {
                    // TODO: Add handler for when user is null
                    User = user;
                });
        }

        public ReactiveCommand Edit { get; }

        public Account User
        {
            get => _user;
            private set => this.RaiseAndSetIfChanged(ref _user, value);
        }
    }
}