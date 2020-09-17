using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmptyPage : Grid
    {
        public EmptyPage()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Invoked when view size is changed.
        /// </summary>
        /// <param name="width">The Width</param>
        /// <param name="height">The Height</param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (width > height)
            {
                if (Device.Idiom == TargetIdiom.Phone)
                {
                    EmptyItinerary.IsVisible = false;
                }
            }
            else
            {
                EmptyItinerary.IsVisible = true;
            }
        }
    }
}