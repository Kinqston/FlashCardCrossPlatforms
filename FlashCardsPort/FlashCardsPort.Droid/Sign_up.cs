using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;

namespace FlashCardsPort.Droid
{
    [Activity(Label = "Регистрация")]
    public class Sign_up : Activity
    {
        static BaseData bd = new BaseData();
        String Id_user;
        TextView txtlog, login, forgot_password;
        EditText txtpass, txtemail;
        Button Register;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.sign_up);
            // Create your application here
            Register = FindViewById<Button>(Resource.Id.signup_btn_register);
            txtemail = FindViewById<EditText>(Resource.Id.signup_email);
            txtpass = FindViewById<EditText>(Resource.Id.signup_password);
            login = FindViewById<TextView>(Resource.Id.signup_btn_login);
            forgot_password = FindViewById<TextView>(Resource.Id.signup_btn_forgot_password);

            login.Click += Login;
            Register.Click += Register_user;
            forgot_password.Click += Forgot_Password;

        }

        private void Forgot_Password(object sender, EventArgs e)
        {
            StartActivity(typeof(Forgot_password));
        }

        private void Register_user(object sender, EventArgs e)
        {
            InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            bd.User_Registration(txtemail.Text, txtpass.Text);
            Id_user = bd.Login(txtemail.Text, txtpass.Text);
            StartActivity(typeof(Main_menu_admin));    
        }

        private void Login(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}