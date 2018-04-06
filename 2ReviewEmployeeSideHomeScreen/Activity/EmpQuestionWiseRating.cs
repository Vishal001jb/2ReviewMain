using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace _2ReviewEmployeeSideHomeScreen.Activity
{
    [Activity(Label = "EmpQuestionWiseRating" , MainLauncher = true, Theme = "@style/Theme.AppCompat.Light.NoActionBar", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class EmpQuestionWiseRating : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EmpQuestionWiseRating);
            // Create your application here
        }
    }
}