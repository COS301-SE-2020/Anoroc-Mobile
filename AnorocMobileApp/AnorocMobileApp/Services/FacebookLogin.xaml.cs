using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Services
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FacebookLogin : ContentPage
    {
        /// <summary>
        /// Constructor to load Facebook login Page
        /// </summary>
        public FacebookLogin()
        {
            InitializeComponent();
        }
    }
}