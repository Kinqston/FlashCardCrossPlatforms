using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using FlashCardsPort;


namespace FlashCardsPort.Droid
{
	[Activity (Label = "Авторизация", MainLauncher = true, Icon = "@drawable/tick")]
	public class MainActivity : Activity
	{
        int count = 1;
        static BaseData bd = new BaseData();
        TextView txtlog, sign_up, forgot_password;
        EditText txtpass, txtemail;
        Button login;
        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            // Get our button from the layout resource,
            // and attach an event to it
            bd.connection();
            login = FindViewById<Button> (Resource.Id.login_btn_login);
            txtemail = FindViewById<EditText>(Resource.Id.login_email);
            txtpass = FindViewById<EditText>(Resource.Id.login_password);
            sign_up = FindViewById<TextView>(Resource.Id.login_btn_signup);
            forgot_password = FindViewById<TextView>(Resource.Id.login_btn_forgot_password);

            forgot_password.Click += Forgot_pass;
            sign_up.Click += Sign_up_user;
            login.Click += Register_user;
		}

        private void Forgot_pass(object sender, EventArgs e)
        {
            StartActivity(typeof(Forgot_password));
        }

        private void Sign_up_user(object sender, EventArgs e)
        {
            StartActivity(typeof(Sign_up));
        }

        private void Register_user(object sender, EventArgs e)
        {           
           // bd.User_Registration(txtemail.Text,txtpass.Text);
            StartActivity(typeof(Main_menu_admin));
        }
    }
}


