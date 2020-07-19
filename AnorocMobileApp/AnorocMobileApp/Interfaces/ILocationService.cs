using AnorocMobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnorocMobileApp.Interfaces
{
    public interface ILocationService
    {
        void Send_Locaiton_ServerAsync(Location location);
    }
}
