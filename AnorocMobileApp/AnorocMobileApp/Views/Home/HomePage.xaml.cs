using AnorocMobileApp.Models;
using AnorocMobileApp.Services;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Home
{
    /// <summary>
    /// Home page for the app to display a summary of information
    /// </summary>    
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomePage" /> class.
        /// </summary>
        private string title = "";
        private string body = "";
        public HomePage()
        {
            InitializeComponent();
        }

     
    }
}