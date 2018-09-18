﻿using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ToolBelt.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.Authentication.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationTypeSelectionPage : ContentPageBase<RegistrationTypeSelectionPageViewModel>
    {
        public RegistrationTypeSelectionPage()
        {
            using (this.Log().Perf($"{nameof(RegistrationTypeSelectionPage)}: Initialize component."))
            {
                InitializeComponent();
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(RegistrationTypeSelectionPage)}: Activate."))
                {
                    _tgrTradesmen
                        .Events()
                        .Tapped
                        .ToSignal()
                        .InvokeCommand(this, v => v.ViewModel.Tradesman)
                        .DisposeWith(disposable);

                    _tgrContractor
                        .Events()
                        .Tapped
                        .ToSignal()
                        .InvokeCommand(this, v => v.ViewModel.Contractor)
                        .DisposeWith(disposable);
                }
            });
        }
    }
}