using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using LoggerService;
using Xamarin.Forms;
using Android.Widget;
using Google.Android.Material.Snackbar;
using Plugin.CurrentActivity;
using Android.Graphics;
using Xamarin.Essentials;
using RemoteTelevizor.ViewModels;
using AndroidX.Lifecycle;
using RemoteTelevizor.Models;

namespace RemoteTelevizor.Droid
{
    [Activity(Label = "RemoteTelevizor", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, Exported = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private ILoggingService _loggingService;
        private static Android.Widget.Toast _instance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _loggingService = new NLogLoggingService(GetType().Assembly, "RemoteTelevizor.Droid");

            _loggingService.Info("Starting activity");

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);

            MessagingCenter.Subscribe<string>(this, RemoteDeviceViewModel.MSG_ToastMessage, (message) =>
            {
                ShowToastMessage(message);
            });

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var appData = new RemoteTelevizorAppData();

#if DEBUG

            var connections = appData.Connections;

            var TVConnection = appData.GetConnectionByIP("10.0.0.18");
            if (TVConnection == null)
            {
                TVConnection = new RemoteDeviceConnection()
                {
                    IP = "10.0.0.18",
                    Port = 49152,
                    Name = "TV",
                    SecurityKey = "OnlineTelevizor"
                };

                connections.Add(TVConnection);

                appData.Connections = connections;
            }

            var tabletConnection = appData.GetConnectionByIP("10.0.0.231");
            if (tabletConnection == null)
            {
                tabletConnection = new RemoteDeviceConnection()
                {
                    IP = "10.0.0.231",
                    Port = 49152,
                    Name = "Lenovo Tablet",
                    SecurityKey = "OnlineTelevizor"
                };

                connections.Add(tabletConnection);
                appData.Connections = connections;
            }

            appData.LastConnectionIP = "10.0.0.231";
#endif



            LoadApplication(new App(_loggingService, appData));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void ShowToastMessage(string message, int fontZoomFactor = 0)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    _instance?.Cancel();
                    _instance = Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short);

                    TextView textView;
                    Snackbar snackBar = null;

                    var tView = _instance.View;
                    if (tView == null)
                    {
                        // Since Android 11, custom toast is deprecated - using snackbar instead:

                        Activity activity = CrossCurrentActivity.Current.Activity;
                        var view = activity.FindViewById(Android.Resource.Id.Content);

                        snackBar = Snackbar.Make(view, message, Snackbar.LengthLong);

                        textView = snackBar.View.FindViewById<TextView>(Resource.Id.snackbar_text);
                    }
                    else
                    {
                        // using Toast

                        tView.Background.SetColorFilter(Android.Graphics.Color.Gray, PorterDuff.Mode.SrcIn); //Gets the actual oval background of the Toast then sets the color filter
                        textView = (TextView)tView.FindViewById(Android.Resource.Id.Message);
                        textView.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
                    }

                    var minTextSize = textView.TextSize; // 16

                    textView.SetTextColor(Android.Graphics.Color.White);

                    var screenHeightRate = 0;

                    //configuration font size:

                    //Normal = 0,
                    //AboveNormal = 1,
                    //Big = 2,
                    //Biger = 3,
                    //VeryBig = 4,
                    //Huge = 5

                    if (DeviceDisplay.MainDisplayInfo.Height < DeviceDisplay.MainDisplayInfo.Width)
                    {
                        // Landscape

                        screenHeightRate = Convert.ToInt32(DeviceDisplay.MainDisplayInfo.Height / 16.0);
                        textView.SetMaxLines(5);
                    }
                    else
                    {
                        // Portrait

                        screenHeightRate = Convert.ToInt32(DeviceDisplay.MainDisplayInfo.Height / 32.0);
                        textView.SetMaxLines(5);
                    }

                    var fontSizeRange = screenHeightRate - minTextSize;
                    var fontSizePerValue = fontSizeRange / 5;

                    var fontSize = minTextSize + fontZoomFactor * fontSizePerValue;

                    textView.SetTextSize(Android.Util.ComplexUnitType.Px, Convert.ToSingle(fontSize));

                    if (snackBar != null)
                    {
                        snackBar.Show();
                    }
                    else
                    {
                        _instance.Show();
                    }
                }
                catch (Exception ex)
                {
                    _loggingService.Error(ex);
                }
            });
        }

    }
}