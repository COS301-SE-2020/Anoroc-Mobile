using System;
namespace AnorocMobileApp.Models
{
    public class Area
    {

        public string Country { get; set; }//nCountryName
        public string Province { get; set; } //AdminArea
        public string Suburb { get; set; }//nLocality

        public Area()
        {
        }

        public Area(String Count, String Prov, String Sub)
        {
            Country = Count;
            Province = Prov;
            Suburb = Sub;
        }
    }
}
