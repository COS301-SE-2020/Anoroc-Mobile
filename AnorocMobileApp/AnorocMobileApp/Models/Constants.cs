using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models
{
    public class Constants
    {
        public static bool IsDev = true;
        public static readonly string GoogleClientID = "googleID on google project .apps.googleusercontent.com";
        public static readonly string GoogleClientSecret = "google secret on google project";
       //public static readonly string FaceBookDevelpmentHash = "ga0RGNYHvNM5d0SLGQfpQWAPGJ8=";
        public static readonly string FacebookAppID = "985395151878298";
    }

}


/*
 *  keystore: codesummore2020
 *  
 *  Generating Realse Hash Key (Facebook): 
 *  > keytool -exportcert -alias YOUR_RELEASE_KEY_ALIAS -keystore YOUR_RELEASE_KEY_PATH | openssl sha1 -binary | openssl base64
 */
