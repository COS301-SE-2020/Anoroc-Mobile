using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        void OnToggled(object sender, ToggledEventArgs e)
        {
            // Perform an action after examining e.Value
            if(e.Value == true)
            {
                DisplayAlert("Attention", "Enabled", "OK");
            }
            else
            {
                DisplayAlert("Attention", "Disabled", "OK");

            }
        }

    }
}
