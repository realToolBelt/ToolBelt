using Prism.Navigation;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive.Subjects;

namespace ToolBelt.ViewModels
{
    /// <summary>
    /// Base class for view-models.
    /// </summary>
    /// <seealso cref="Prism.Navigation.INavigationAware" />
    /// <seealso cref="ReactiveUI.ReactiveObject" />
    /// <seealso cref="ReactiveUI.ISupportsActivation" />
    public class BaseViewModel : ReactiveObject, ISupportsActivation, INavigationAware
    {
        private string _icon = string.Empty;
        private string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseViewModel" /> class.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        /// <summary>
        /// Gets or sets the icon relating to the view-model.
        /// </summary>
        public string Icon
        {
            get => _icon;
            set => this.RaiseAndSetIfChanged(ref _icon, value);
        }

        /// <summary>
        /// Gets the navigation service used to provide page based navigation for ViewModels.
        /// </summary>
        public INavigationService NavigationService
        {
            get;
        }

        /// <summary>
        /// Gets or sets the title describing the view-model.
        /// </summary>
        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        #region INavigationAware

        private readonly Subject<NavigationParameters> _navigatedFrom = new Subject<NavigationParameters>();
        private readonly Subject<NavigationParameters> _navigatedTo = new Subject<NavigationParameters>();
        private readonly Subject<NavigationParameters> _navigatingTo = new Subject<NavigationParameters>();

        public IObservable<NavigationParameters> NavigatedFrom => _navigatedFrom;

        public IObservable<NavigationParameters> NavigatedTo => _navigatedTo;

        public IObservable<NavigationParameters> NavigatingTo => _navigatingTo;

        /// <summary>
        /// Called when the implementer is being navigated away from.
        /// </summary>
        /// <param name="parameters">The navigation parameters.</param>
        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {
            _navigatedFrom.OnNext(parameters);
        }

        /// <summary>
        /// Called when the implementer has been navigated to.
        /// </summary>
        /// <param name="parameters">The navigation parameters.</param>
        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {
            _navigatedTo.OnNext(parameters);
        }

        /// <summary>
        /// Called before the implementor has been navigated to.
        /// </summary>
        /// <param name="parameters">The navigation parameters.</param>
        /// <remarks>Not called when using device hardware or software back buttons</remarks>
        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {
            _navigatingTo.OnNext(parameters);
        }

        #endregion

        /// <summary>
        /// When called, asserts that the current code is executing on a background thread.
        /// </summary>
        [Conditional("DEBUG")]
        protected void AssertRunningOnBackgroundThread()
        {
            Debug.Assert(System.Threading.Thread.CurrentThread.IsBackground);
        }
    }
}
