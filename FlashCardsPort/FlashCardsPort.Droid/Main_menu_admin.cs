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
    [Activity(Label = "Главное меню")]
    public class Main_menu_admin : Activity
    {
        Button Edit_deck;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_menu_admin);
            Edit_deck = FindViewById<Button>(Resource.Id.dashboard_btn_edit);
            Edit_deck.Click += Edit_Deck;
            // Create your application here
        }

        private void Edit_Deck(object sender, EventArgs e)
        {
            StartActivity(typeof(Decks_admin));
        }
    }
}