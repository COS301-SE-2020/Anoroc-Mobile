using AnorocMobileApp.Interfaces;
using Plugin.SecureStorage;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Settings
{
    /// <summary>
    /// Page to show the setting.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingPage" /> class.
        /// </summary>
        public SettingPage()
        {
            InitializeComponent();
        }

        private void SfSwitch_OnStateChanged(object sender, SwitchStateChangedEventArgs e)
        {
            var backgroundLocationService = App.IoCContainer.GetInstance<IBackgroundLocationService>();
            if (e.NewValue == true)
            {
                CrossSecureStorage.Current.SetValue("Location", "true");
                backgroundLocationService.Start_Tracking();
            }
            else
            {
                backgroundLocationService.Stop_Tracking();
                CrossSecureStorage.Current.SetValue("Location", "false");
            }
        }
    }
}