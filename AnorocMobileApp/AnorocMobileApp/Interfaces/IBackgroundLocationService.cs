namespace AnorocMobileApp.Interfaces
{
    public interface IBackgroundLocationService
    {
        bool isTracking();
        void Start_Tracking();
        void Stop_Tracking();
        void Run_TrackAsync();
    }
}
