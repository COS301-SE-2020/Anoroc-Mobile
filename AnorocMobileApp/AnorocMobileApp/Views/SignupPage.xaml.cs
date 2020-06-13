using AnorocMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        User user;
        public SignupPage()
        {
            InitializeComponent();
        }

        private void btn_Back_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        // TODO:
        // Change Alerts to red highlights of the entry missing
        private void btn_signup_Clicked(object sender, EventArgs e)
        {
            if(UserEmail.Text != "")
            {
                try
                {
                    MailAddress m = new MailAddress(UserEmail.Text);
                    if(UsersName.Text != "")
                    {
                        if (UserSurname.Text != "")
                        {
                            if (Password.Text != "")
                            {
                                if (ConfrimPassword.Text != "")
                                {
                                    if (Password.Text.Equals(ConfrimPassword.Text))
                                    {
                                        User.Email = UserEmail.Text;
                                        User.UserName = UsersName.Text;
                                        User.UserSurname = UserSurname.Text;
                                        User.loggedInAnoroc = true;
                                        user = new User();
                                        user.Password = Password.Text;
                                        user.registerAsync();
                                    }
                                    else
                                        DisplayAlert("Passwords Don't Match", "Please ensure your passwords are correct.");
                                }
                                else
                                    DisplayAlert("Empty Field", "Please Confrim your password");
                            }
                            else
                                DisplayAlert("Empty Field", "Please enter a password");
                        }
                        else
                        {
                            DisplayAlert("Empty Field", "Please enter your surname");
                        }
                    }
                    else {
                        DisplayAlert("Empty Field", "Please enter your name");
                    }
                }
                catch (FormatException)
                {
                    DisplayAlert("Invalid Email", "Please enter a valid email address");
                }
            }
        }

        public static void registerSuccessfull()
        {
            Application.Current.MainPage = new HomePage();
        }

        public void DisplayAlert(string title, string msg)
        {
            (Application.Current as App).MainPage.DisplayAlert(title, msg, "OK");
        }
    }
}