using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.App;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace _2ReviewEmployeeSideHomeScreen.Activity
{
    [Activity(Label = "2Review", MainLauncher = true, Icon = "@drawable/ICON", Theme = "@style/Theme.AppCompat.Light.NoActionBar", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashscreenActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.SplashScreenEmployee);


        }
        protected override void OnResume()
        {

            base.OnResume();
            Task startupWork = new Task(async () => { await SimulateStartupAsync(); });
            startupWork.Start();
        }

        private async Task SimulateStartupAsync()
        {
            await Task.Delay(4000); // Simulate a bit of startup work.
            StartActivity(new Intent(this, typeof(LoginEmployee)));
            Finish();
        }
    }
}