using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.About
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutUsPage : ContentPageBase<AboutUsPageViewModel>
    {
        public AboutUsPage()
        {
            InitializeComponent();
        }
    }
}