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
using System.Net.Mail;

namespace FlashCardsPort.Droid
{
    [Activity(Label = "Восстановление пароля")]
    public class Forgot_password : Activity
    {
        EditText txtemail;
        Button Forgot_btn_reset;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.forgot_password);
            Forgot_btn_reset = FindViewById<Button>(Resource.Id.forgot_btn_reset);
            txtemail = FindViewById<EditText>(Resource.Id.forgot_email);
            Forgot_btn_reset.Click += Reset;
            // Create your application here
        }

        private void Reset(object sender, EventArgs e)
        {
           
        }
    }
}