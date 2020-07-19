using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Navigation
{
    /// <summary>
    /// Page to show the Me page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MePage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MePage" /> class.
        /// </summary>
        public MePage()
        {
            InitializeComponent();
        }
    }
}