﻿using Xamarin.Forms.Xaml;

namespace ToolBelt.Views.About
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrivacyPolicyPage : ContentPageBase<PrivacyPolicyPageViewModel>
    {
        public PrivacyPolicyPage()
        {
            InitializeComponent();
        }
    }
}