using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using ToolBelt.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToolBelt.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectDetailsPage : ContentPageBase<ProjectDetailsPageViewModel>
    {
        public ProjectDetailsPage ()
        {
            InitializeComponent ();
        }
    }
}