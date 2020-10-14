using AnorocMobileApp.Helpers;

namespace AnorocMobileApp.Models
{
    public class Constants
    {

        /*public static bool IsDev = true;
        public static readonly string GoogleClientID = "googleID on google project .apps.googleusercontent.com";
        public static readonly string GoogleClientSecret = "google secret on google project";
       
        public static readonly string FacebookAppID = "985395151878298";*/
        public static readonly string googleAPIKey = "//KEY HERE//";
        public static readonly string clientID = $"{googleAPIKey}.apps.googleusercontent.com";
        public static readonly string redirectUrl = $"com.googleusercontent.apps.{googleAPIKey}:/oauth2redirect";
        public static readonly string AnorocURI = Secrets.baseEndpoint;
         public static bool IsDev = true;
        public static readonly string GoogleClientID = "googleID on google project .apps.googleusercontent.com";
        public static readonly string GoogleClientSecret = "google secret on google project";
       
        public static readonly string FacebookAppID = "985395151878298";

        public static readonly string GooglePlacesApiAutoCompleteUrl =
            "https://maps.googleapis.com/maps/api/place/autocomplete/json?key={0}&input={1}&components=country:za";

        public static readonly string AzureFuzzySearchUrl =
            "https://atlas.microsoft.com/search/fuzzy/json?subscription-key={1}&api-version=1.0&query={0}&limit=5&countrySet=ZA&language=En&minFuzzyLevel=1&maxFuzzyLevel=2&typeahead=true";
    }

}
