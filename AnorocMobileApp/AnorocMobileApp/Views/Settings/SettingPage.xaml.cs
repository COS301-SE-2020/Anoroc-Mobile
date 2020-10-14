using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Services;
using Plugin.SecureStorage;
using Plugin.Toast;
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
            if (Application.Current.Properties.ContainsKey("Tracking"))
            {
                var value = (bool)Application.Current.Properties["Tracking"];
                if (value)
                    Location_Tracking_Switch.IsOn = true;
            }

            if(Application.Current.Properties.ContainsKey("Anonymity"))
            {
                var value = (bool)Application.Current.Properties["Anonymity"];
                if (value)
                    Set_Anonomous_Switch.IsOn = true;
            }

            if(Application.Current.Properties.ContainsKey("EmailNotifications"))
            {
                var value = (bool)Application.Current.Properties["EmailNotifications"];
                if (value)
                    Email_Notification_Switch.IsOn = true;
            }

            if(Application.Current.Properties.ContainsKey("Allow_Notifications"))
            {
                var value = (bool)Application.Current.Properties["Allow_Notifications"];
                Allow_Notification_Switch.IsOn = value;
            }
            else
            {
                Application.Current.Properties["Allow_Notifications"] = true;
                Allow_Notification_Switch.IsOn = true;
            }

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

        private async void Set_Anonomous_Switch_StateChanged(object sender, SwitchStateChangedEventArgs e)
        {
            var user = App.IoCContainer.GetInstance<IUserManagementService>();
            var value = await user.ToggleAnonymousUser((bool)e.NewValue);
            CrossToastPopUp.Current.ShowToastMessage($"Anonymity set to: {value}");
            Application.Current.Properties["Anonymity"] = e.NewValue;
        }

        private async void Email_Notification_Switch_StateChanged(object sender, SwitchStateChangedEventArgs e)
        {
            var user = App.IoCContainer.GetInstance<IUserManagementService>();
            var value = await user.SetEmaileNotificationSettings((bool)e.NewValue);

            Application.Current.Properties["EmailNotifications"] = value;
        }

        private void Allow_Notification_Switch_StateChanged(object sender, SwitchStateChangedEventArgs e)
        {
            var val = (bool)e.NewValue;
            if(val == false)
            {
                Email_Notification_Switch.IsOn = true;
            }
            App.Current.Properties["Allow_Notifications"] = val;
        }
    }
}