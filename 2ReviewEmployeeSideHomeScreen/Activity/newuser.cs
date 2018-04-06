using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace _2ReviewEmployeeSideHomeScreen.Activity
{
    [Activity(Label = "2Review", Theme = "@style/Theme.AppCompat.Light.NoActionBar",ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class newuser : AppCompatActivity
    {
         
        Spinner spinner, spinner_category;
        TextView textviewjoiningdate;
        EditText edittextuserfname, edittextusermname, edittextuserlname, edittextusernumber, edittextuseremail, edittextuseraddress, edittextusercity, edittextuserpincode, edittextuserstate, edittextusercountry, radiogroupgender, edittextuserpassword, edittextconfirmpassword, edittext_employeeimage;

        Button buttonadd;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

          
            SetContentView(Resource.Layout.newuser);
            buttonadd = FindViewById<Button>(Resource.Id.buttonadd);
            edittextuserfname = FindViewById<EditText>(Resource.Id.edittextuserfname);
            edittextusermname = FindViewById<EditText>(Resource.Id.edittextusermname);
            edittextuserlname = FindViewById<EditText>(Resource.Id.edittextuserlname);
            edittextusernumber = FindViewById<EditText>(Resource.Id.edittextusernumber);
            edittextuseremail = FindViewById<EditText>(Resource.Id.edittextuseremail);
            edittextuseraddress = FindViewById<EditText>(Resource.Id.edittextuseraddress);
            edittextusercity = FindViewById<EditText>(Resource.Id.edittextusercity);
            edittextuserpincode = FindViewById<EditText>(Resource.Id.edittextuserpincode);
            edittextuserstate = FindViewById<EditText>(Resource.Id.edittextuserstate);
            edittextusercountry = FindViewById<EditText>(Resource.Id.edittextusercountry);
            textviewjoiningdate = FindViewById<TextView>(Resource.Id.textviewjoiningdate);
            edittextuserpassword = FindViewById<EditText>(Resource.Id.edittextuserpassword);
            edittextconfirmpassword = FindViewById<EditText>(Resource.Id.edittextconfirmpassword);


            spinner = FindViewById<Spinner>(Resource.Id.spinner);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.category, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            //Joining date validation
            textviewjoiningdate.Click += delegate (object sender, EventArgs e)
            {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(this, OnDateSet, today.Year, today.Month - 1, today.Day);
                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();
            };

            buttonadd.Click += delegate (object sender, EventArgs e)
            {
                //first name validation
                if (string.IsNullOrWhiteSpace(edittextuserfname.Text))
                {
                    edittextuserfname.Error = "First Name cannot be Blank";
                }
                else
                {
                    if (edittextuserfname.Text.Length < 3)
                    {
                        edittextuserfname.Error = "First name can not be less then 3 characters";
                    }
                    else
                    {
                        edittextuserfname.Error = null;
                    }
                }
                //middle name validation
                if (string.IsNullOrWhiteSpace(edittextusermname.Text))
                {
                    edittextusermname.Error = "Middle Name cannot be Blank";
                }
                else
                {
                    if (edittextusermname.Text.Length < 3)
                    {
                        edittextusermname.Error = "Middle name can not be less then 3 characters";
                    }
                    else
                    {
                        edittextusermname.Error = null;
                    }
                }

                //last name validation
                if (string.IsNullOrWhiteSpace(edittextuserlname.Text))
                {
                    edittextuserlname.Error = "Last Name cannot be Blank";
                }
                else
                {
                    if (edittextuserlname.Text.Length < 3)
                    {
                        edittextuserlname.Error = "Last name can not be less then 3 characters";
                    }
                    else
                    {
                        edittextuserlname.Error = null;
                    }
                }

                //Number Validation
                string inputnumber = edittextusernumber.Text.ToString();
                var number = isValidMobile(inputnumber);
                if (string.IsNullOrWhiteSpace(inputnumber))
                {
                    edittextusernumber.Error = "Enter the Number.!";
                }
                else
                {
                    if (inputnumber.Length < 10)
                    {
                        edittextusernumber.Error = "Please enter valid number";

                    }
                    else
                    {
                        edittextusernumber.Error = null;
                    }

                }

                //Email Validation

                string inputemail = edittextuseremail.Text.ToString();
                var emailvalidate = isValidEmail(inputemail);

                if (string.IsNullOrWhiteSpace(inputemail))
                {
                    edittextuseremail.Error = "Enter the Email.!";
                }
                else
                {
                    if (emailvalidate == false)
                    {
                        edittextuseremail.Error = "Please enter valid Email";
                    }
                    else
                    {
                        edittextuseremail.Error = null;
                    }
                }

                //Address validation
                if (string.IsNullOrWhiteSpace(edittextuseraddress.Text))
                {
                    edittextuseraddress.Error = " Address cannot be Blank";
                }
                else
                {
                    if (edittextuseraddress.Text.Length < 3)
                    {
                        edittextuseraddress.Error = "Address can not be less then 3 characters";
                    }
                    else
                    {
                        edittextuseraddress.Error = null;
                    }
                }

                //city validation
                if (string.IsNullOrWhiteSpace(edittextusercity.Text))
                {
                    edittextusercity.Error = "City cannot be Blank";
                }
                else
                {
                    edittextusercity.Error = null;
                }

                //pincode validation 
                if (string.IsNullOrWhiteSpace(edittextuserpincode.Text))
                {
                    edittextuserpincode.Error = "Pincode cannot be Blank";
                }
                else
                {
                    if (edittextuserpincode.Text.Length == 6)
                    {
                        edittextuserpincode.Error = null;

                    }
                    else
                    {
                        edittextuserpincode.Error = "Enter valid pincode";
                    }
                }

                //state validation
                if (string.IsNullOrWhiteSpace(edittextuserstate.Text))
                {
                    edittextuserstate.Error = "State cannot be Blank";
                }
                else
                {
                    edittextuserstate.Error = null;
                }

                //country validation
                if (string.IsNullOrWhiteSpace(edittextusercountry.Text))
                {
                    edittextusercountry.Error = "Country cannot be Blank";
                }
                else
                {
                    edittextusercountry.Error = null;
                }

                //joining date validation
                if (string.IsNullOrWhiteSpace(textviewjoiningdate.Text))
                {
                    textviewjoiningdate.Error = "Joining date cannot be Blank";
                }
                else
                {
                    textviewjoiningdate.Error = null;
                }

                //password validation
                if (string.IsNullOrWhiteSpace(edittextuserpassword.Text))
                {
                    edittextuserpassword.Error = "password cannot be Blank";
                }
                else
                {
                    edittextuserpassword.Error = null;
                }

                //confirm password validation
                if (string.IsNullOrWhiteSpace(edittextconfirmpassword.Text))
                {
                    edittextconfirmpassword.Error = "Confirm password cannot be Blank";
                }
                else
                {
                    if (string.Compare(edittextuserpassword.Text, edittextconfirmpassword.Text) != 0)
                    {
                        edittextconfirmpassword.Error = "password and confirm password doesn't match.!";
                    }
                    else
                    {
                        edittextconfirmpassword.Error = null;
                    }

                }
            };
        }
        private bool isValidMobile(String edittextusernumber)
        {
            return Android.Util.Patterns.Phone.Matcher(edittextusernumber).Matches();
        }

        public bool isValidEmail(string edittextuseremail)
        {
            return Android.Util.Patterns.EmailAddress.Matcher(edittextuseremail).Matches();
        }

        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            textviewjoiningdate.Text = e.Date.ToLongDateString();
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
        }
    }
}