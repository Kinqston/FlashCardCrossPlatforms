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
using System.Net;

namespace FlashCardsPort.Droid
{
    [Activity(Label = "Колоды")]
    public class Decks_admin : Activity
    {
        static BaseData bd = new BaseData();
        TextView email;
        ListView list_deck;
        Button ok,edit_item,delete_item;
        EditText input;
        public string delete_title, deck_cost, delete_deck_id, edit_deck_id;
        public string title_text;
        public ArrayAdapter<string> adapter, adapter_deck_cost, adapter_deck_id;
        CheckBox cost;
        EditText title;
        LayoutInflater inflater;
        public Dialog dialog;
        bool deck = false;
        bool create_deck = true;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            bd.connection();
            
            SetContentView(Resource.Layout.decks);
            list_deck = FindViewById<ListView>(Resource.Id.list);
            list_deck.ItemLongClick += delete_edit_item;
            list_deck.ItemClick += action_item;
            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayHomeAsUpEnabled(true);
            actionBar.SetDisplayShowHomeEnabled(false);
            List_deck();
            // email.Text = "1";
            //  }
        }
        private void action_item(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(Cards_deck));
            intent.PutExtra("title_deck", adapter.GetItem(e.Position));
            intent.PutExtra("id_deck", adapter_deck_id.GetItem(e.Position));
            StartActivity(intent);
        }
        void delete_edit_item(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            delete_title = adapter.GetItem(e.Position);
            delete_deck_id = adapter_deck_id.GetItem(e.Position);
            edit_deck_id = adapter_deck_id.GetItem(e.Position);
            deck_cost = adapter_deck_cost.GetItem(e.Position);

            Console.WriteLine(deck_cost+ "                         ");

            LayoutInflater layoutInflater = LayoutInflater.From(this);
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            var view = layoutInflater.Inflate(Resource.Layout.edit_delete_item, null);
            //  input = new EditText(this);
            edit_item = (Button)view.FindViewById(Resource.Id.edit_item);
            delete_item = (Button)view.FindViewById(Resource.Id.delete_item);
            delete_item.Click += delete_item_click;
            edit_item.Click += edit_item_click;
            alert.SetView(view);
            dialog = alert.Create();
            dialog.Show();
        }

        private void edit_item_click(object sender, EventArgs e)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            var view = layoutInflater.Inflate(Resource.Layout.dialog_add_deck_admin, null);
            title = (EditText)view.FindViewById(Resource.Id.title_deck);
            cost = (CheckBox)view.FindViewById(Resource.Id.Cost_deck);
            title.Text = delete_title;
            if(deck_cost == "True")
            {
                cost.Checked = true;
            }
            else
            {
                cost.Checked = false;
            }
            alert.SetPositiveButton("Далее", HandlePositiveButtonClickEdit);
            alert.SetNegativeButton("Отмена", HandleNegativeButtonClick);
            alert.SetView(view);
            dialog = alert.Create();
            dialog.Show();
        }

        private void HandlePositiveButtonClickEdit(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(Add_card_admin));
            // указываем первым параметром ключ, а второе значение
            // по ключу мы будем получать значение с Intent
            intent.PutExtra("function", "Edit");
            intent.PutExtra("id_deck", edit_deck_id);
            intent.PutExtra("title_old", delete_title);
            intent.PutExtra("title", title.Text);
            intent.PutExtra("cost_old", deck_cost);
            intent.PutExtra("cost", cost.Checked.ToString());
            // показываем новое Activity
            StartActivity(intent);
        }

        private void delete_item_click(object sender, EventArgs e)
        {
            bd.delete_item_list(delete_deck_id);        
            dialog.Hide();
            List_deck();
        }
        protected void onRestart()
        {
            base.OnRestart();
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.main, menu);
            return true;
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    var intent = new Intent(this, typeof(Main_menu_admin));
                    StartActivity(intent);
                    break;
                case Resource.Id.item1:
                    deck = false;
                    create_deck = true;

                    LayoutInflater layoutInflater = LayoutInflater.From(this);
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    var view = layoutInflater.Inflate(Resource.Layout.dialog_add_deck_admin, null);
                    //  input = new EditText(this);
                    title = (EditText)view.FindViewById(Resource.Id.title_deck);
                    cost = (CheckBox)view.FindViewById(Resource.Id.Cost_deck);
                    alert.SetPositiveButton("Далее", HandlePositiveButtonClick);
                    alert.SetNegativeButton("Отмена", HandleNegativeButtonClick);
                    alert.SetView(view);
                    dialog = alert.Create();
                    dialog.Show();

                    //var intent2 = new Intent(this, typeof(Add_deck_admin));
                    //StartActivity(intent2);


                    //FragmentTransaction transaction = FragmentManager.BeginTransaction();
                    //  Add_deck_admin add_deck_admin = new Add_deck_admin();
                    // add_deck_admin.Show(transaction, "add_deck");
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
            dialog.Hide();
        }
        private void HandlePositiveButtonClick(object sender, EventArgs e)
        {
            //bd.Add_deck(title.Text, cost.Text);
            //List_deck();
            for (int i = 0; i < adapter.Count; i++)
                if (title.Text.ToLower() == adapter.GetItem(i).ToLower())
                {
                    deck = true;
                }
            if (deck == true)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Создание колоды");
                alert.SetMessage("Колода с таким названием существует, создать еще одну?");
                alert.SetPositiveButton("Создать", (senderAlert, args) =>
                {
                    Create_deck();
                });
                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                { 
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
            else
            {
                Create_deck();
            }
        }
        public void Create_deck()
        {
            Add_card_admin add_card = new Add_card_admin();
            add_card.title = title.Text;
            add_card.cost = cost.Text;
            Intent intent = new Intent(this, typeof(Add_card_admin));
            // указываем первым параметром ключ, а второе значение
            // по ключу мы будем получать значение с Intent
            intent.PutExtra("function", "Create");
            intent.PutExtra("title_old", delete_title);
            intent.PutExtra("title", title.Text);
            intent.PutExtra("cost_old", deck_cost);
            intent.PutExtra("cost", cost.Checked.ToString());
            // показываем новое Activity
            StartActivity(intent);
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
        
    

