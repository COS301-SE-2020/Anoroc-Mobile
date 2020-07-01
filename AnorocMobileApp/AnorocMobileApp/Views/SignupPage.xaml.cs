using AnorocMobileApp.Models;
using System;
using System.Net.Mail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AnorocMobileApp.Views
{
    /// <summary>
    /// Class to manage Sign Up
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignupPage : ContentPage
    {
        User user;
        /// <summary>
        /// Constructor to Load Sign Up page
        /// </summary>
        public SignupPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Navigation to home screen
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event Arguments</param>
        private void btn_Back_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        // TODO:
        // Change Alerts to red highlights of the entry missing
        /// <summary>
        /// Managing and using user provided input to sign up and create a new account
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event Argumants</param>
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
                                        User.FirstName = UsersName.Text;
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
        /// <summary>
        /// Manage a successful registration of new user
        /// </summary>
        public static void registerSuccessfull()
        {
            Application.Current.MainPage = new HomePage();
        }
        /// <summary>
        /// Simple Notification that can inform the user on the register activity
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="msg">Message that needs to be displayed to the user</param>
        public void DisplayAlert(string title, string msg)
        {
            (Application.Current as App).MainPage.DisplayAlert(title, msg, "OK");
        }
    }
}