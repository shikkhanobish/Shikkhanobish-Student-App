using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.LocalNotification;
using Xamarin.Forms.Vonage.Android;
using ImageCircle.Forms.Plugin.Droid;
using ShikkhanobishStudentApp.Model;
using Plugin.LocalNotification.Platform.Droid;

namespace ShikkhanobishStudentApp.Droid
{
    [Activity(Label = "ShikkhanobishStudentApp", Icon = "@mipmap/shikcon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);          
            PlatformVonage.Init(this);
            NotificationCenter.CreateNotificationChannel(new Func<Plugin.LocalNotification.Platform.Droid.NotificationChannelRequestBuilder, Plugin.LocalNotification.Platform.Droid.NotificationChannelRequest>((x) => {
                x.WithBadges(true);
                x.WithImportance(NotificationImportance.Max);
                x.WithVibration(true);
                x.WithDescription("GG Noob");
                x.WithChannelId("ShikkhanobishTeacher");
                x.WithLockScreenVisibility(NotificationVisibility.Public);
                return new Plugin.LocalNotification.Platform.Droid.NotificationChannelRequest { Description = "asdasdas", Importance = NotificationImportance.Max, Id = "ShikkhanobishTeacher" };
            }));
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            XF.Material.Droid.Material.Init(this, savedInstanceState);
            FormsControls.Droid.Main.Init(this);
            ImageCircleRenderer.Init();
            LoadApplication(new App());
            Window.SetStatusBarColor ( Android.Graphics.Color.Rgb (74, 123, 223));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

      
    }
}