using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using LoggerService;

namespace RemoteTelevizor.Droid
{
    [Activity(Label = "RemoteTelevizor", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, Exported = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private ILoggingService _loggingService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _loggingService = new NLogLoggingService(GetType().Assembly, "RemoteTelevizor.Droid");

            _loggingService.Info("Starting activity");

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(_loggingService));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}