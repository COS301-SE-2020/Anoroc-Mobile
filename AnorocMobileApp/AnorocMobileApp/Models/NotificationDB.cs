using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Models
{
    public class NotificationDB
    {
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get;
            set;
        }
        public string Body
        {
            get;
            set;
        }
    }
}
