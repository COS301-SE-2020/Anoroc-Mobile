using AnorocMobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace AnorocMobileApp.Services
{
    /// <summary>
    /// This class interacts with the API server on behalf of the User class
    /// </summary>
    class SignUpService
    {
        HttpClient http;
        
        public SignUpService()
        {

        }
        // TODO:
        // Link to our API
        /// <summary>
        /// Function to asynchronously log the user in
        /// </summary>
        /// <param name="password">Password used to authenticate sign up</param>
        /// <returns></returns>
        public async Task<bool> registerUserAsync(string password)
        {
            http = new HttpClient();
            http.BaseAddress = new Uri(Constants.AnorocURI);

            // Have to make seporate call to Facebook API to get the email address for the 

            string jsonData = "{" + $"'userEmail':'{User.Email}', 'username':'{User.FirstName}', 'surname':'{User.UserSurname}', 'password':'{password}'" + "}";

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await http.PostAsync("/foo/login", content);

            // this result string should be something like: "{"Token":"rgh2ghgdsfds"}"
            Response result = JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
            if (result.Token.Equals("error"))
                return false;
            else
                return true;
        }
    }
    //We need to put this class in it's own file!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    class Response
    {
        public string Token { get; set; }
    }
}

