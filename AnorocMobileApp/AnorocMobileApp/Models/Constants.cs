using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models
{
    public class Constants
    {
        public static bool IsDev = true;
        public static readonly string GoogleClientID = "1069091943659-cnv80cif7vrod2be9lmfu9a1ft01pq1n.apps.googleusercontent.com";
        public static readonly string GoogleClientSecret = "Sz5VEBHUU4o4QsNGBobe4FHt";
        public static readonly string FaceBookDevelpmentHash = "ga0RGNYHvNM5d0SLGQfpQWAPGJ8=";
    }

}


/*
 *  keystore: codesummore2020
 *  
 *  Generating Realse Hash Key (Facebook): 
 *  > keytool -exportcert -alias YOUR_RELEASE_KEY_ALIAS -keystore YOUR_RELEASE_KEY_PATH | openssl sha1 -binary | openssl base64
 */
