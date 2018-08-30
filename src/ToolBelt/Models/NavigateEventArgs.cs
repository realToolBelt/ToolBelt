using Prism.Navigation;

namespace ToolBelt.Models
{
    public class NavigateEventArgs
    {
        public NavigateEventArgs(string name, NavigationParameters parameters = null, bool? useModalNavigation = null, bool animated = true)
        {
            Name = name;
            Parameters = parameters;
            UseModalNavigation = useModalNavigation;
            Animated = animated;
        }

        public bool Animated { get; }

        public string Name { get; }

        public NavigationParameters Parameters { get; }

        public bool? UseModalNavigation { get; }
    }
}
