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
    [Activity(Label = "Decks")]
    public class Add_deck_admin : Activity
    {
        public List<String> items_cart_word;
        public List<String> items_cart_translate;

        private CustomAdapter adapter;
        private List<Card> cards;
        private ListView list;
        private List<Card> list_cards;
       
        static BaseData bd = new BaseData();
        TextView email;
        ListView list_card;
        Button save,add_card,cancel;
        EditText input;
        EditText cost, word_card, translate_card;
        EditText title;
        LayoutInflater inflater;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.dialog_add_deck_admin);

            title = FindViewById<EditText>(Resource.Id.title_deck);
            list_card = FindViewById<ListView>(Resource.Id.list);
           // cost = FindViewById<EditText>(Resource.Id.Cost_deck);
            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetDisplayHomeAsUpEnabled(true);

            items_cart_word = new List<String>();
            items_cart_translate = new List<String>();

            list_cards = new List<Card>();

            list = FindViewById<ListView>(Resource.Id.list);
            list.SetScrollContainer(false);

            cancel.Click += Cancel;
            save.Click += Save_deck;
            add_card.Click += Add_card;
            // email.Text = "1";
            //  }
        }

        private void Cancel(object sender, EventArgs e)
        {
            StartActivity(typeof(Decks_admin));
        }

        private void Save_deck(object sender, EventArgs e)
        {
            bd.Add_deck(title.Text,cost.Text);
            bd.Add_deck_cards(title.Text, list_cards);
            StartActivity(typeof(Decks_admin));
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    var intent = new Intent(this, typeof(Decks_admin));
                    StartActivity(intent);
                    break;               
            }
            return base.OnOptionsItemSelected(item);
        }
        //private List<Card> GetCards()
        //{
        //    cards = new List<Card>()
        //    {
        //        new Card("1","2"),
        //        new Card("1","3")
        //    };
        //    return cards; 
        //}

        private void Add_card(object sender, EventArgs e)
        {
            // bd.User_Registration(txtemail.Text,txtpass.Text);
            // StartActivity(typeof(Decks_admin));

            LayoutInflater layoutInflater = LayoutInflater.From(this);
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            var view = layoutInflater.Inflate(Resource.Layout.add_card_admin, null);
            //  input = new EditText(this);
            word_card = (EditText)view.FindViewById(Resource.Id.title_deck);
          //  translate_card = (EditText)view.FindViewById(Resource.Id.Cost_deck);
            alert.SetPositiveButton("Добавить", HandlePositiveButtonClick);
            alert.SetNegativeButton("Отмена", HandleNegativeButtonClick);
            alert.SetView(view);
            Dialog dialog = alert.Create();
            dialog.Show();       
        }

        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
            
        }

        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            
            list_cards.Add(new Card(word_card.Text, translate_card.Text));            
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, list_cards);
            list.Adapter = adapter;
            list.SetScrollContainer(false);
        }
        public void List_deck()
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items_cart_word);
            list_card.Adapter = adapter;
        }
    }
}