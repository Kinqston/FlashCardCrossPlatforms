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
        Button Edit_deck, exit;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_menu_admin);
            Edit_deck = FindViewById<Button>(Resource.Id.dashboard_btn_edit);
            exit = FindViewById<Button>(Resource.Id.dashboard_btn_exit);
            Edit_deck.Click += Edit_Deck;
            exit.Click += Exit;
            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayShowHomeEnabled(false);
            // Create your application here
        }

        private void Exit(object sender, EventArgs e)
        {
            var prefs = Application.Context.GetSharedPreferences("FC", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString("Id", null);
            prefEditor.Commit();
            StartActivity(typeof(MainActivity));
        }

        private void Edit_Deck(object sender, EventArgs e)
        {
            StartActivity(typeof(Decks_admin));
        }
    }
}