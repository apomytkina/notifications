using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using HSE_Notification.Models;
using System.Collections.Generic;
using System;
using Android.Content;

namespace HSE_Notification
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        List<Bus> listOfBuses;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void BusesAdapter_NotificationClick(object sender, BusAdapterClickEventArgs e)
        {
            var bus = listOfBuses[e.Position];

            string departurePlace;

            if (bus.DeparturePlace == "Dubki")
            {
                departurePlace = "Дубки";
            }
            else if (bus.DeparturePlace == "Odintsovo")
            {
                departurePlace = "Одинцово";
            }
            else
            {
                departurePlace = "Славянский бульвар";
            }

            Android.Support.V7.App.AlertDialog.Builder NotificationAlert = new Android.Support.V7.App.AlertDialog.Builder(this);

            NotificationAlert.SetMessage("Напомнить про автобус за 10 минут до его отправления");
            NotificationAlert.SetTitle("Поставить уведомление");

            NotificationAlert.SetPositiveButton("Ok", (alert, args) =>
            {
                StartAlarm(bus.DepartureTime, departurePlace);
            });

            NotificationAlert.SetNegativeButton("Cancel", (alert, args) =>
            {
                NotificationAlert.Dispose();
            });

            NotificationAlert.Show();
        }

        void StartAlarm(DateTime time, string departurePlace)
        {
            var alarmIntent = new Intent(this, typeof(AlarmReceiver));
            alarmIntent.PutExtra("title", "Напоминание");
            alarmIntent.PutExtra("message", $"Автобус от станции {departurePlace} отправится в {time.ToString("HH:mm")}.");

            var pending = PendingIntent.GetBroadcast(this, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            var alarmManager = GetSystemService(AlarmService).JavaCast<AlarmManager>();

            if (time.Ticks - DateTime.Now.AddMinutes(10).Ticks < 0 && time.Ticks - DateTime.Now.Ticks >= 0)
            {
                alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime(), pending);
            }
            else if (time.Ticks - DateTime.Now.AddMinutes(10).Ticks > 0)
            {
                long miliseconds = (time.Ticks - DateTime.Now.AddMinutes(10).Ticks) / 10000;
                alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + miliseconds, pending);
                //alarmManager.Cancel(pending);
            }
        }

    }
}