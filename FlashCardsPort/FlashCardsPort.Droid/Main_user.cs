
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

namespace FlashCardsPort.Droid
{
    [Activity(Label = "Main_user")]
    public class Main_user : Activity
    {
        Button teaching_button;
        Button decks;
        Button shop;
        Button archive;
        Button exit;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.mian_user);

            teaching_button = FindViewById<Button>(Resource.Id.teaching_button);




            teaching_button.Click += teaching_button_Click;
            // Create your application here
        }

        private void teaching_button_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Teaching_decks));
        }
    }
}
