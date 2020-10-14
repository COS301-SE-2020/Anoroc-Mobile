using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models
{
    public class NotificationServer
    {
        public string AccessToken { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        //public int Risk { get; set; }

        public DateTime Created { get; set; }

        public int Risk { get; set; }
    }
}
