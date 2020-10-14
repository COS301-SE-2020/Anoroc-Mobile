using System;

namespace AnorocMobileApp.Services
{
    public interface NotificationServices
    {
        void CreateNotification(String title, String message);

        void notificationToDB(Models.NotificationDB notificationObj);
    }
}
