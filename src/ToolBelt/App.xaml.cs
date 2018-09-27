using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Splat;
using System;
using System.Threading.Tasks;
using ToolBelt.Services;
using ToolBelt.Services.Analytics;
using ToolBelt.Services.Crash;
using ToolBelt.Services.Device;
using ToolBelt.Views;
using ToolBelt.Views.About;
using ToolBelt.Views.Authentication;
using ToolBelt.Views.Authentication.Registration;
using ToolBelt.Views.Profile;
using ToolBelt.Views.Projects;
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

        protected override async void OnInitialized()
        {
            InitializeComponent();

#if DEBUG

            // NOTE: When running in debug, we want to disable analytics and crash reporting
            await Task.WhenAll(
                Crashes.SetEnabledAsync(false),
                Analytics.SetEnabledAsync(false)
            );
#endif

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Crashes.IsEnabledAsync()
                .ContinueWith(isEnabled =>
                {
                    if (isEnabled.Result)
                    {
                        Crashes.HasCrashedInLastSessionAsync()
                            .ContinueWith(hasCrashed =>
                            {
                                if (hasCrashed.Result)
                                {
                                    Crashes.GetLastSessionCrashReportAsync()
                                        .ContinueWith(crashReport =>
                                        {
                                            if (crashReport?.Exception != null)
                                            {
                                                Crashes.TrackError(crashReport.Exception);
                                            }
                                        });
                                }
                            });
                    }
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            System.AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

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

#if DEBUG

            // in the debug configuration, register our services used for debugging
            containerRegistry.RegisterInstance<IAnalyticService>(new DebugAnalyticService());
#else

            // in non-debug configurations, register the real services
            containerRegistry.RegisterInstance<IAnalyticService>(new AnalyticService());
#endif

            containerRegistry.RegisterInstance<ICrashService>(new CrashService());
            containerRegistry.RegisterInstance<IDeviceOrientation>(new DeviceOrientationImplementation());
            containerRegistry.RegisterInstance<IDateTimeProvider>(new SystemDateTimeProvider());

            containerRegistry.Register<IPermissionsService, PermissionsService>();
            containerRegistry.RegisterInstance<Acr.UserDialogs.IUserDialogs>(Acr.UserDialogs.UserDialogs.Instance);

            // TODO: REPLACE!
            containerRegistry.Register<IUserDataStore, FakeUserDataStore>();
            containerRegistry.Register<IProjectDataStore, FakeProjectDataStore>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ModalNavigationPage, ModalNavigationPageViewModel>();
            containerRegistry.RegisterForNavigation<TabbedPage>();
            containerRegistry.RegisterForNavigation<AuthenticationTabbedPage>();
            containerRegistry.RegisterForNavigation<ExtendedSplashPage, ExtendedSplashPageViewModel>();

            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<BasicInformationPage, BasicInformationPageViewModel>();
            containerRegistry.RegisterForNavigation<TradeSpecialtiesPage, TradeSpecialtiesPageViewModel>();
            containerRegistry.RegisterForNavigation<SignupPage, SignupPageViewModel>();
            containerRegistry.RegisterForNavigation<RegistrationTypeSelectionPage, RegistrationTypeSelectionPageViewModel>();
            containerRegistry.RegisterForNavigation<ContractorRegistrationPage, ContractorRegistrationPageViewModel>();

            containerRegistry.RegisterForNavigation<RootPage, RootPageViewModel>("Root");
            containerRegistry.RegisterForNavigation<RootNavigationPage, RootNavigationPageViewModel>("Details");
            containerRegistry.RegisterForNavigation<ContactUsPage, ContactUsPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutUsPage, AboutUsPageViewModel>();
            containerRegistry.RegisterForNavigation<PrivacyPolicyPage, PrivacyPolicyPageViewModel>();
            containerRegistry.RegisterForNavigation<MultiSelectListViewPage, MultiSelectListViewPageViewModel>();
            containerRegistry.RegisterForNavigation<ProjectDetailsPage, ProjectDetailsPageViewModel>();

            containerRegistry.RegisterForNavigation<ProfileViewTabbedPage, ProfileViewTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<EditableProfilePage, EditableProfilePageViewModel>();

            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ProjectsPage, ProjectsPageViewModel>();
            containerRegistry.RegisterForNavigation<MyProjectsPage, MyProjectsPageViewModel>();
            containerRegistry.RegisterForNavigation<ProjectFilterPage, ProjectFilterPageViewModel>();

            containerRegistry.RegisterForNavigation<EditProjectPage, EditProjectPageViewModel>();

            RegisterSplatDependencies();
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private void RegisterSplatDependencies()
        {
            // register the command binder for the social buttons
            Locator.CurrentMutable.Register(
                () => new Controls.SocialButtonCommandBinder(),
                typeof(ReactiveUI.ICreatesCommandBinding));
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Crashes.TrackError(e.Exception);
        }
    }
}
