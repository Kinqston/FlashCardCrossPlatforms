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
using System.IO;

namespace FlashCardsPort.Droid
{
    [Activity(Label = "������")]
    public class Decks_user : Activity
    {
        static BaseData bd = new BaseData();
        TextView email;
        ListView list_deck;
        List<string> decks_title;
        List<string> decks_id;
        Button ok,edit_item,delete_item;
        EditText input;
        Button cancel, next;
        int action_deck;
        public string delete_title, deck_cost, delete_deck_id, edit_deck_id;
        public string title_text;
        public ArrayAdapter<string> adapter, adapter_deck_cost, adapter_deck_id;
        EditText cost;
        EditText title;
        LayoutInflater inflater;
        Button ok_repeat_deck, cancel_repeat_deck;
        public Dialog dialog, dialog3;
        bool deck = true;
        bool create_deck = true;
        private string pathToDatabase;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.decks);
            bd.connection();
            list_deck = FindViewById<ListView>(Resource.Id.list);
            list_deck.ItemLongClick += delete_edit_item;
            list_deck.ItemClick += action_item;
            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayHomeAsUpEnabled(true);
            actionBar.SetDisplayShowHomeEnabled(false);
            var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");

            List_deck();


        }
        private void action_item(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(Cards_deck_user));
            intent.PutExtra("title_deck", adapter.GetItem(e.Position));
            intent.PutExtra("id_deck", adapter_deck_id.GetItem(e.Position));
            StartActivity(intent);
        }
        void delete_edit_item(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            delete_title = adapter.GetItem(e.Position);
            delete_deck_id = adapter_deck_id.GetItem(e.Position);
            edit_deck_id = adapter_deck_id.GetItem(e.Position);
            action_deck = e.Position;
            deck = true;
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
            var view = layoutInflater.Inflate(Resource.Layout.dialog_add_deck_user, null);
            title = (EditText)view.FindViewById(Resource.Id.title_deck_user);          
            title.Text = delete_title;
            cancel = (Button)view.FindViewById(Resource.Id.Cancel);
            next = (Button)view.FindViewById(Resource.Id.Ok);
            cancel.Click += Cancel_edit;
            next.Click += Next_edit;
            alert.SetView(view);
            dialog3 = alert.Create();
            dialog3.Show();
        }

        private void Cancel_edit(object sender, EventArgs e)
        {
            dialog3.Hide();
            dialog.Hide();
        }

        private void Next_edit(object sender, EventArgs e)
        {
            for (int i = 0; i < adapter.Count; i++)
                if (title.Text.ToLower() == adapter.GetItem(i).ToLower())
                {
                    if (action_deck != i)
                    {
                        deck = false;
                    }
                }
            if (deck == false)
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                var view = layoutInflater.Inflate(Resource.Layout.repeat_deck, null);
                ok_repeat_deck = (Button)view.FindViewById(Resource.Id.Ok_repeat);
                cancel_repeat_deck = (Button)view.FindViewById(Resource.Id.Cancel_repeat);

                ok_repeat_deck.Click += Repeat_Create_deck;
                cancel_repeat_deck.Click += Cancel_repeat_deck;
                alert.SetView(view);
                dialog = alert.Create();
                dialog.Show();
                deck = true;
            }
            else
            {
                Edit();
            }
        }

        private void Repeat_Create_deck(object sender, EventArgs e)
        {
            Edit();
        }

        private void Edit()
        {
            Intent intent = new Intent(this, typeof(Add_card_user));
            intent.PutExtra("function", "Edit");
            intent.PutExtra("id_deck", edit_deck_id);
            intent.PutExtra("title_old", delete_title);
            intent.PutExtra("title", title.Text);
            StartActivity(intent);
        }

        private void HandlePositiveButtonClickEdit(object sender, DialogClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(Add_card_user));
            intent.PutExtra("function", "Edit");
            intent.PutExtra("id_deck", edit_deck_id);
            intent.PutExtra("title_old", delete_title);
            intent.PutExtra("title", title.Text);
            StartActivity(intent);
        }

        private void delete_item_click(object sender, EventArgs e)
        {     
            using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
            {
                var query = connection.Table<CardLocal>();
                connection.Delete(new DeckLocal()
                {id = Convert.ToInt32(delete_deck_id)});
                foreach (CardLocal card in query)
                {
                    if (card.id_deck == Convert.ToInt32(delete_deck_id))
                    {
                        connection.Delete(new CardLocal()
                        {id_deck = Convert.ToInt32(delete_deck_id)});
                    }
                }
            }
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
                    var intent = new Intent(this, typeof(Main_user));
                    StartActivity(intent);
                    break;
                case Resource.Id.item1:
                    deck = true;
                    create_deck = true;

                    LayoutInflater layoutInflater = LayoutInflater.From(this);
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    var view = layoutInflater.Inflate(Resource.Layout.dialog_add_deck_user, null);
                    //  input = new EditText(this);
                    title = (EditText)view.FindViewById(Resource.Id.title_deck_user);
                    cancel = (Button)view.FindViewById(Resource.Id.Cancel);
                    next = (Button)view.FindViewById(Resource.Id.Ok);
                    cancel.Click += Cancel;
                    next.Click += Next;
                    alert.SetView(view);
                    dialog3 = alert.Create();
                    dialog3.Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void Next(object sender, EventArgs e)
        {
            for (int i = 0; i < adapter.Count; i++)
                if (title.Text.ToLower() == adapter.GetItem(i).ToLower())
                {
                    deck = false;
                }
            if (deck == false)
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                var view = layoutInflater.Inflate(Resource.Layout.repeat_deck, null);
                ok_repeat_deck = (Button)view.FindViewById(Resource.Id.Ok_repeat);
                cancel_repeat_deck = (Button)view.FindViewById(Resource.Id.Cancel_repeat);

                ok_repeat_deck.Click += Repeat_Create_deck_new;
                cancel_repeat_deck.Click += Cancel_repeat_deck;
                alert.SetView(view);
                dialog = alert.Create();
                dialog.Show();
                deck = true;
            }
            else
            {
                Create_deck();
            }
        }

        private void Cancel_repeat_deck(object sender, EventArgs e)
        {
            dialog.Hide();
        }

        private void Repeat_Create_deck_new(object sender, EventArgs e)
        {
            Create_deck();
        }

        private void Cancel(object sender, EventArgs e)
        {
            dialog3.Hide();
        }

        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
            //    
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
                alert.SetTitle("�������� ������");
                alert.SetMessage("������ � ����� ��������� ����������, ������� ��� ����?");
                alert.SetPositiveButton("�������", (senderAlert, args) =>
                {
                    Create_deck();
                });
                alert.SetNegativeButton("������", (senderAlert, args) =>
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
            Intent intent = new Intent(this, typeof(Add_card_user));
            // ��������� ������ ���������� ����, � ������ ��������
            // �� ����� �� ����� �������� �������� � Intent
            intent.PutExtra("function", "Create");
            intent.PutExtra("title", title.Text);
            // ���������� ����� Activity
            StartActivity(intent);
        }
        public void List_deck()
        {
            decks_title = new List<string>();
            decks_id = new List<string>();
            using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
            {
                var query = connection.Table<DeckLocal>();

                foreach (DeckLocal deck in query)
                {
                    decks_title.Add(deck.title);
                    decks_id.Add(deck.id.ToString());
                    //TableView.ReloadData();
                }
            }
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, decks_title);
            adapter_deck_id = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, decks_id);
            list_deck.Adapter = adapter;     
        }
    }
}
        
    

