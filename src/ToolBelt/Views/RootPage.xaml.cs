﻿using Prism.Navigation;
using ReactiveUI;
using ReactiveUI.XamForms;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootPage : ReactiveMasterDetailPage<RootPageViewModel>, IMasterDetailPageOptions
    {
        public RootPage()
        {
            InitializeComponent();

            _imgProfile.GestureRecognizers.Add(new TapGestureRecognizer());
        }

        public bool IsPresentedAfterNavigation => Device.Idiom != TargetIdiom.Phone;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.WhenActivated(disposable =>
            {
                this
                    .OneWayBind(ViewModel, vm => vm.MenuItems, v => v._lstMenu.ItemsSource)
                    .DisposeWith(disposable);

                this
                    .WhenAnyValue(x => x.ViewModel.User.Name, x => x.ViewModel.User.LastName, (firstName, lastName) => $"{firstName} {lastName}")
                    .BindTo(this, v => v._lblUserName.Text)
                    .DisposeWith(disposable);

                _lstMenu
                    .Events()
                    .ItemSelected
                    .Select(item => item.SelectedItem as CustomMenuItem)
                    .Where(item => item != null)
                    .Subscribe(item =>
                    {
                        if (item.TapCommand != null)
                        {
                            item.TapCommand?.Execute().Subscribe();
                        }

                        _lstMenu.SelectedItem = null;
                        IsPresented = false;
                    })
                    .DisposeWith(disposable);

                // NOTE: BindCommand isn't working for the cached image.  Need to handle another way
                ((TapGestureRecognizer)_imgProfile.GestureRecognizers[0])
                    .Events()
                    .Tapped
                    .ToSignal()
                    .InvokeCommand(ViewModel, vm => vm.ViewProfile)
                    .DisposeWith(disposable);
            });
        }
    }
}