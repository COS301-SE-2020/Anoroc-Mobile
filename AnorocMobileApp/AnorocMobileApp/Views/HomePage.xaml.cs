using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : TabbedPage
    {
        public HomePage()
        {
            InitializeComponent();

            var vTabbedPages = new TabbedPage();
            vTabbedPages.Children.Add(new Login());
            //vTabbedPages.Children.Add(new Settings());
            //vTabbedPages.Children.Add(new Settings());
            //Application.Current.MainPage = new TabbedPage();
            Application.Current.MainPage = vTabbedPages;
        }



    }
}