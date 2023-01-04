using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;

namespace KFC.Droid
{
    [Activity(Theme = "@style/MySplashScreen.Splash", Icon = "@drawable/hamburger", MainLauncher = true, NoHistory = true)]
    public class SplashScreen : AppCompatActivity
    {
        static readonly string TAG = "X:" + nameof(Droid.SplashScreen);

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            SimulateStartup();
        }

        // Simulates background work that happens behind the splash screen
         private void SimulateStartup()
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        public override void OnBackPressed() { }


    }
}