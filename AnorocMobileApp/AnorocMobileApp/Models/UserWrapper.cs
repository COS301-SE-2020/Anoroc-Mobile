namespace AnorocMobileApp.Models
{
    internal class UserWrapper
    {
        public string email;
        public string firstName;
        public string userSurname;
        public bool loggedInAnoroc;
        public bool loggedInFacebook;
        public bool loggedInGoogle;
        public bool carrierStatus;

        public UserWrapper(string email, string firstName, string userSurname, bool loggedInAnoroc, bool loggedInFacebook, bool loggedInGoogle, bool carrierStatus)
        {
            this.email = email;
            this.firstName = firstName;
            this.userSurname = userSurname;
            this.loggedInAnoroc = loggedInAnoroc;
            this.loggedInFacebook = loggedInFacebook;
            this.loggedInGoogle = loggedInGoogle;
            this.carrierStatus = carrierStatus;
        }
    }
}