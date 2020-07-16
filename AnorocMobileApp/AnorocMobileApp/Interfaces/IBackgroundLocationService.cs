using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Interfaces
{
    public interface IBackgroundLocationService
    {
        void Start_Tracking();
        void Stop_Tracking();
    }
}
