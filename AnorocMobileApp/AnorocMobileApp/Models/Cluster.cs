namespace AnorocMobileApp.Models
{
    /// <summary>
    /// Main class for clusters
    /// Does not have a list of locations.
    /// </summary>
    class Cluster
    {
        public int Pin_Count { get; set; }
        public int Carrier_Pin_Count { get; set; }
        public double Cluster_Radius { get; set; }
        public Location Center_Pin { get; set; }
    }
}
