using Prism.Navigation;
using ReactiveUI;
using Splat;
using ToolBelt.Extensions;
using ToolBelt.ViewModels;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TradesmenPage : ContentPageBase<TradesmenPageViewModel>
    {
        public TradesmenPage()
        {
            using (this.Log().Perf($"{nameof(TradesmenPage)}: Initialize component."))
            {
                InitializeComponent();
            }

            this.WhenActivated(disposable =>
            {
                using (this.Log().Perf($"{nameof(TradesmenPage)}: Activate."))
                {
                    // TODO: ...
                }
            });
        }
    }
}