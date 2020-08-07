using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;

using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using AnorocMobileApp.Interfaces;
using AnorocMobileApp.Services;
using Xamarin.Forms;

namespace AnorocMobileApp.Droid.Resources.services
{
    [Service]
    public class BackgroundLocationAndroidService : Service
    {
        CancellationTokenSource _cts;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

     
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            _ = Task.Run(() =>
              {
                  try
                  {
                      var backgroundLocationService = App.IoCContainer.GetInstance<IBackgroundLocationService>();
                      backgroundLocationService.Run_TrackAsync();
                      BackgroundLocationService.Tracking = true;
                  }
                  catch (System.OperationCanceledException)
                  {

                  }
                  finally
                  {
                      if (_cts.IsCancellationRequested)
                      {
                          var message = new CancelMessage();
                          Device.BeginInvokeOnMainThread(() =>
                          {
                              MessagingCenter.Send(message, "CancelMessage");
                          });
                      }
                  }
              }, _cts.Token);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if(_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();
                _cts.Cancel();
            }
            base.OnDestroy();
        }
    }
}