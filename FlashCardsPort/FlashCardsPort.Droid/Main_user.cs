
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
        Button create_deck;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.mian_user);
            create_deck = FindViewById<Button>(Resource.Id.decks);
            create_deck.Click += Create_deck;
            // Create your application here
        }

        private void Create_deck(object sender, EventArgs e)
        {
            StartActivity(typeof(Decks_user));
        }
    }
}
