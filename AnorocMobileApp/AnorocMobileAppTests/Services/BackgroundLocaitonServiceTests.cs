using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnorocMobileApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using AnorocMobileApp.Interfaces;
using Xamarin.Forms;


namespace AnorocMobileApp.Services.Tests
{
    [TestClass()]
    public class BackgroundLocaitonServiceTests
    {
        
        [TestMethod()]
        public void Start_TrackingTest()
        {
            IBackgroundLocationService BackgroundLocationService = new BackgroundLocaitonService();

            MessagingCenter.Subscribe<StartBackgroundLocationTracking>(this, "StartBackgroundLocationTracking", message =>
            {
                Assert.IsTrue(BackgroundLocationService.isTracking());
            });
            BackgroundLocationService.Start_Tracking();
        }

        [TestMethod()]
        public void Stop_TrackingTest()
        {
            IBackgroundLocationService BackgroundLocationService = new BackgroundLocaitonService();

            MessagingCenter.Subscribe<StopBackgroundLocationTrackingMessage>(this, "StopBackgroundLocationTrackingMessage", message =>
            {
                Assert.IsFalse(BackgroundLocationService.isTracking());
            });
            BackgroundLocationService.Stop_Tracking();
        }
    }
}