using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;

namespace _2ReviewEmployeeSideHomeScreen.Activity
{
    [Activity(Label = "2Review" , Theme = "@style/Theme.AppCompat.Light.NoActionBar",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginEmployee : AppCompatActivity
    {
        EditText editTextUsername, editTextPassword;
        TextInputLayout textInputLayoutUsername, textInputLayoutPassword;
        Button buttonLogin;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.LoginEmployee);
            findAllView();
            buttonLogin.Click += check_validation;
            
        }

        private void findAllView()
        {
            buttonLogin = FindViewById<Button>(Resource.Id.loginButton);
            editTextUsername = FindViewById<EditText>(Resource.Id.userNameEditText);
            editTextPassword = FindViewById<EditText>(Resource.Id.passwordEditText);
            textInputLayoutPassword = FindViewById<TextInputLayout>(Resource.Id.userNameTextInputLayout);
            textInputLayoutUsername = FindViewById<TextInputLayout>(Resource.Id.passwordTextInputLayout);
        }
        private void check_validation(object sender, EventArgs e)
        {
            if (editTextUsername.Text.Contains("abc@1rivet.com"))
            {
                if (editTextPassword.Text.Equals("123456"))
                {
                    Toast.MakeText(this, "Login", ToastLength.Short).Show();
                    editTextUsername.Text = "";
                    editTextPassword.Text = "";
                    StartActivity(new Intent(this, typeof(Navigation)));
                    Finish();
                }
                else if (editTextUsername.Text.Length != 0 || editTextUsername.Text.Length < 6)
                {
                    //editTextUsername.SetError("Enter Valid Username", null);
                    Toast.MakeText(this, "Invalid Login", ToastLength.Short).Show();
                }
                else if (editTextUsername.Text.Length == 0)
                {
                    //  editTextUsername.SetError("Invalid credential", null);
                }
            }
            else if (editTextUsername.Text.Length != 0 || editTextUsername.Text.Length < 12)
            {
                //editTextUsername.SetError("Invalid credential", null);
                Toast.MakeText(this, "Invalid Login", ToastLength.Short).Show();
            }
            else if (editTextUsername.Text.Length == 0)
            {
                //editTextUsername.SetError("Invalid credential", null);
            }
         
        }
    }
}