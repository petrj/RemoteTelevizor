using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using CommunityToolkit.Mvvm.Messaging;
using Google.Android.Material.Snackbar;
using LoggerService;
using RemoteTelevizor.ViewModels;
using static Android.Provider.CalendarContract;

namespace RemoteTelevizor.MAUI
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private static Android.Widget.Toast _instance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            MessagingCenter.Subscribe<string>(this, RemoteDeviceViewModel.MSG_ToastMessage, (message) =>
            {
                ShowToastMessage(message);
            });
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

                        var view = FindViewById(Android.Resource.Id.Content);

                        snackBar = Snackbar.Make(view, message, Snackbar.LengthLong);

                        textView = snackBar.View.FindViewById<TextView>(Resource.Id.snackbar_text);
                    }
                    else
                    {
                        // using Toast

                        tView.Background.SetColorFilter(Android.Graphics.Color.Gray, PorterDuff.Mode.SrcIn); //Gets the actual oval background of the Toast then sets the color filter
                        textView = (TextView)tView.FindViewById(Android.Resource.Id.Message);
                        //textView.SetTypeface(Typeface.DefaultBold, TypefaceStyle.Bold);
                    }

                    var minTextSize = textView.TextSize; // 16

                    textView.SetTextColor(Android.Graphics.Color.Black);

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
                    Console.WriteLine(ex.ToString());
                }
            });
        }
    }
}
