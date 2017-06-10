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
using MySql.Data.MySqlClient;
using FlashCardsPort;
using System.Runtime.Remoting.Contexts;
using Android.Webkit;
using Xamarin.InAppBilling;
using Xamarin.InAppBilling.Utilities;

namespace FlashCardsPort.Droid
{
    [Activity(Label = "Колоды")]
    public class Decks_shop : Activity
    {
        static BaseData bd = new BaseData();
        TextView email;
        ListView list_deck;
        Button ok,edit_item,delete_item;
        EditText input;
        public string delete_title, deck_cost, delete_deck_id, edit_deck_id;
        public string title_text;
        public ArrayAdapter<string> adapter, adapter_deck_cost, adapter_deck_id;
        EditText cost;
        EditText title;
        LayoutInflater inflater;
        public Dialog dialog;
        bool deck = false;
        bool create_deck = true;
        private InAppBillingServiceConnection _serviceConnection;
        public string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAizTzBEUo7bLR5G5I4qO+/0pSGQFyGzW9J6vVd7SIG84xXY2pspnvxZzN3nLdY0czdUuDwuF4iPVwBihXAGGFxejmf6Qcg3OlIFtUTpvgrC/NcdFFSeGeZO1gZqhi3d8PKTSVdbL1WAI9M9mDRSSwFhAHoBUEjSM1uG0Ys1rMHsSVvumkq32HFx3vNLMqAj6jRl21khfK6IcXCRCU++2ql+TZsn0O6BXWfcEef0tY6sKf/TM0c47YXZLku+JVq9tiKUxmniukDBIJaI+My+1syIWFVI9N3lbjdpaUnXK0oAunfCgvPr80DA62Z2BE+h9aixTtPsW7vLX8+GEcOkJCtwIDAQAB";
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            bd.connection();
           
            SetContentView(Resource.Layout.decks_shop);
            list_deck = FindViewById<ListView>(Resource.Id.list);
            list_deck.ItemClick += action_item;
            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayHomeAsUpEnabled(true);
            actionBar.SetDisplayShowHomeEnabled(false);
            List_deck();
        }
        private void action_item(object sender, AdapterView.ItemClickEventArgs e)
        {

            // Create a new connection to the Google Play Service
            _serviceConnection = new InAppBillingServiceConnection(this, publicKey);
            _serviceConnection.OnConnected += () => { };
            // Load available products and any purchases
            // Attempt to connect to the service
            _serviceConnection.Connect();
           // _serviceConnection.BillingHandler.BuyProduct(deck);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            _serviceConnection.BillingHandler.HandleActivityResult(requestCode, resultCode, data);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    var intent = new Intent(this, typeof(Main_user));
                    StartActivity(intent);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void List_deck()
        {
            bd.Decks_list();
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_deck);
            adapter_deck_id = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_deck_id);
            adapter_deck_cost = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_deck_cost);
            list_deck.Adapter = adapter;
        }
    }
}
        
    

