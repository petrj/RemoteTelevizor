using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.AppCompat.App;

namespace RemoteTelevizor.Droid
{
    [Activity(Name= "net.petrjanousek.RemoteTelevizor.SplashActivity", Label = "Remote Televizor", Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true, Exported = true)]
    [IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { Intent.CategoryLauncher, Intent.CategoryLeanbackLauncher })]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            new Task(() => { StartMainActivity(); }).Start();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        private async void StartMainActivity ()
        {
            StartActivity(new Intent(Application.Context, typeof (MainActivity)));
        }
    }
}