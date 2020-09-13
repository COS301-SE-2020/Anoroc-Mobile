using System;
namespace AnorocMobileApp.Models
{
    public class Area
    {

        public string Country { get; set; }//nCountryName
        public string Province { get; set; } //AdminArea
        public string Suburb { get; set; }//nLocality
        public string City { get; set; }
        public Area()
        {
        }

        public Area(string Count, string Prov, string city ,string Sub)
        {
            Country = Count;
            Province = Prov;
            Suburb = Sub;
            City = city;
        }
    }
}
