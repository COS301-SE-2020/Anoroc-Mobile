using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Auth;

namespace AnorocMobileApp.Models
{
    public static class FacebookPageHelper
    {
        static FacebookPageHelper() { }
      /* public static void PresentUILoginScreen(this Page page, Authenticator authenticator)
        {
            if (Settings.IsFormsImplementationRenderers)
            {
                // Renderers Implementaion

                Xamarin.Auth.XamarinForms.AuthenticatorPage ap;
                ap = new Xamarin.Auth.XamarinForms.AuthenticatorPage()
                {
                    Authenticator = authenticator,
                };

                NavigationPage np = new NavigationPage(ap);

                if (Settings.IsFormsNavigationPushModal)
                {
                    System.Diagnostics.Debug.WriteLine("Presenting");
                    System.Diagnostics.Debug.WriteLine("        PushModal");
                    System.Diagnostics.Debug.WriteLine("        Custom Renderers");

                    page.Navigation.PushModalAsync(np);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Presenting");
                    System.Diagnostics.Debug.WriteLine("        Push");
                    System.Diagnostics.Debug.WriteLine("        Custom Renderers");

                    page.Navigation.PushAsync(np);
                }
            }
            else
            {
                // Presenters Implementation

                if (Settings.IsFormsNavigationPushModal)
                {
                    System.Diagnostics.Debug.WriteLine("Presenting");
                    System.Diagnostics.Debug.WriteLine("        PushModal");
                    System.Diagnostics.Debug.WriteLine("        Presenters");

                    Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
                    presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                    presenter.Login(authenticator);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Presenting");
                    System.Diagnostics.Debug.WriteLine("        Push");
                    System.Diagnostics.Debug.WriteLine("        Presenters");

                    Xamarin.Auth.Presenters.OAuthLoginPresenter presenter = null;
                    presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
                    presenter.Login(authenticator);
                }
            }

            return;
        }*/
    }
}
