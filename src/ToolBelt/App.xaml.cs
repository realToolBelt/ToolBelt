using System;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Splat;
using ToolBelt.Services;
using ToolBelt.Views;
using ToolBelt.Views.About;
using ToolBelt.Views.Authentication;
using ToolBelt.Views.Authentication.Registration;
using ToolBelt.Views.Messages;
using ToolBelt.Views.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace ToolBelt
{
    public partial class App : PrismApplication
    {
        public App()
           : this(null)
        {
        }

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync($"/NavigationPage/{nameof(ExtendedSplashPage)}");
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start(
                $"android={AppSettings.AppCenterAnalyticsAndroid};" +
                $"ios={AppSettings.AppCenterAnalyticsIos};",
                typeof(Analytics),
                typeof(Crashes));
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IContainerRegistry>(containerRegistry);

#if DEBUG
            var logger = new ToolBelt.Services.DebugLogger();
            Locator.CurrentMutable.RegisterConstant(logger, typeof(ILogger));
            containerRegistry.RegisterInstance<ILogger>(logger);
#endif

            // register the analytics and crash tracking services
            containerRegistry.RegisterInstance<IAnalyticService>(new AnalyticService());
            containerRegistry.RegisterInstance<ICrashService>(new CrashService());

            containerRegistry.Register<IAuthenticatorFactory, AuthenticatorFactory>();
            containerRegistry.Register<IPermissionsService, PermissionsService>();
            containerRegistry.RegisterInstance<Acr.UserDialogs.IUserDialogs>(Acr.UserDialogs.UserDialogs.Instance);

            // TODO: REPLACE!
            containerRegistry.Register<IUserDataStore, FakeUserDataStore>();
            containerRegistry.Register<IProjectDataStore, FakeProjectDataStore>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ModalNavigationPage, ModalNavigationPageViewModel>();
            containerRegistry.RegisterForNavigation<TabbedPage>();
            containerRegistry.RegisterForNavigation<ExtendedSplashPage, ExtendedSplashPageViewModel>();

            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<BasicInformationPage, BasicInformationPageViewModel>();
            containerRegistry.RegisterForNavigation<TradeSpecialtiesPage, TradeSpecialtiesPageViewModel>();
            containerRegistry.RegisterForNavigation<SignupPage, SignupPageViewModel>();

            containerRegistry.RegisterForNavigation<RootPage, RootPageViewModel>("Root");
            containerRegistry.RegisterForNavigation<RootNavigationPage, RootNavigationPageViewModel>("Details");
            containerRegistry.RegisterForNavigation<CommunitiesPage, CommunitiesPageViewModel>();
            containerRegistry.RegisterForNavigation<ContactUsPage, ContactUsPageViewModel>();
            containerRegistry.RegisterForNavigation<ItemDetailsPage, ItemDetailsPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutUsPage, AboutUsPageViewModel>();
            containerRegistry.RegisterForNavigation<PrivacyPolicyPage, PrivacyPolicyPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<EditableProfilePage, EditableProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<MultiSelectListViewPage, MultiSelectListViewPageViewModel>();
            containerRegistry.RegisterForNavigation<ProjectDetailsPage, ProjectDetailsPageViewModel>();

            containerRegistry.RegisterForNavigation<ChatPage, ChatPageViewModel>();

            RegisterSplatDependencies();
        }

        private void RegisterSplatDependencies()
        {
            // register the command binder for the social buttons
            Locator.CurrentMutable.Register(
                () => new Controls.SocialButtonCommandBinder(),
                typeof(ReactiveUI.ICreatesCommandBinding));
        }
    }
}
