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
using Android.Util;
using Android.Webkit;
using Java.IO;
using Android.Provider;
using Android.Graphics;
using Java.Sql;
using System.Runtime.Remoting.Contexts;
using System.Net;

namespace FlashCardsPort.Droid
{
    [Activity(Label = "Cards")]
    public class Cards_deck : Activity
    {
        static BaseData bd = new BaseData();
        WebClient myWebClient = new WebClient();
        public String filename;
        private List<Card> new_card;
        private List<Card> cards;
        private CustomAdapter adapter;
        public ArrayAdapter<string> adapter2;
        public ArrayAdapter<string> adapter3;
        Intent Camera_intent, Galery_intent, Crop_intent;
        Button edit_item, delete_item;
        Android.Net.Uri uri;
        ImageView imageview;
        File file;
        ListView list_card;
        EditText word_card, translate_card;
        Button Camera, Galery;
        public string cards_word, cards_translate;
        LayoutInflater inflater;
        public Dialog dialog, dialog1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.cards_deck);
            bd.connection();
            list_card = FindViewById<ListView>(Resource.Id.list_cards);
            list_card.ItemLongClick += delete_edit_card;
            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayHomeAsUpEnabled(true);
            this.Title = Intent.GetStringExtra("title_deck");
            List_card();
        }
        private void delete_edit_card(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            cards_word = adapter2.GetItem(e.Position);
            cards_translate = adapter3.GetItem(e.Position);
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            var view = layoutInflater.Inflate(Resource.Layout.edit_delete_item, null);
            //  input = new EditText(this);
            edit_item = (Button)view.FindViewById(Resource.Id.edit_item);
            delete_item = (Button)view.FindViewById(Resource.Id.delete_item);
            delete_item.Click += delete_item_click;
            edit_item.Click += edit_item_click;
            alert.SetView(view);
            dialog1 = alert.Create();
            dialog1.Show();
            //LayoutInflater layoutInflater = LayoutInflater.From(this);
            //AlertDialog.Builder alert = new AlertDialog.Builder(this);
            //var view = layoutInflater.Inflate(Resource.Layout.dialog_add_cards_admin, null);
            //word_card = (EditText)view.FindViewById(Resource.Id.word_card);
            //word_card.Text = cards_word;
            //translate_card = (EditText)view.FindViewById(Resource.Id.translate_card);
            //translate_card.Text = cards_translate;
            //imageview = (ImageView)view.FindViewById(Resource.Id.icon_card);
            //Camera = (Button)view.FindViewById(Resource.Id.Camera);
            //Galery = (Button)view.FindViewById(Resource.Id.Galery);
            //Galery.Click += Galery_open;
            //Camera.Click += Camera_open;

            //alert.SetPositiveButton("Добавить", HandlePositiveButtonClick);
            //alert.SetNegativeButton("Отмена", HandleNegativeButtonClick);
            //alert.SetView(view);
            //Dialog dialog = alert.Create();
            //dialog.Show();
        }
        private void edit_item_click(object sender, EventArgs e)
        {
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            var view = layoutInflater.Inflate(Resource.Layout.add_card_admin, null);
            word_card = (EditText)view.FindViewById(Resource.Id.word_card);
            translate_card = (EditText)view.FindViewById(Resource.Id.translate_card);
            imageview = (ImageView)view.FindViewById(Resource.Id.icon_card);

            word_card.Text = cards_word;
            translate_card.Text = cards_translate;

            Camera = (Button)view.FindViewById(Resource.Id.Camera);
            Galery = (Button)view.FindViewById(Resource.Id.Galery);
            Galery.Click += Galery_open;
            Camera.Click += Camera_open;

            alert.SetPositiveButton("Добавить", Change);
            alert.SetNegativeButton("Отмена", HandleNegativeButtonClick);
            alert.SetView(view);
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void Change(object sender, DialogClickEventArgs e)
        {
            bd.Update_cards(Intent.GetStringExtra("id_deck"), cards_word, word_card.Text, cards_translate, translate_card.Text);
            cards = new List<Card>();
            bd.Cards_list(Intent.GetStringExtra("id_deck"));
            for (int i = 0; i < bd.items_card_title.Count; i++)
            {
                cards.Add(new Card(bd.items_card_title[i], bd.items_card_translate[i]));
            }
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
            adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_title);
            adapter3 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_translate);
            list_card.Adapter = adapter;
            dialog1.Hide();
        }

        private void delete_item_click(object sender, EventArgs e)
        {
            bd.Delete_card(Intent.GetStringExtra("id_deck"), cards_word);
            dialog1.Hide();
            List_card();
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
                    var intent = new Intent(this, typeof(Decks_admin));
                    StartActivity(intent);
                    break;
                case Resource.Id.item1:
                    LayoutInflater layoutInflater = LayoutInflater.From(this);
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    var view = layoutInflater.Inflate(Resource.Layout.add_card_admin, null);
                    word_card = (EditText)view.FindViewById(Resource.Id.word_card);
                    translate_card = (EditText)view.FindViewById(Resource.Id.translate_card);
                    imageview = (ImageView)view.FindViewById(Resource.Id.icon_card);
                    Camera = (Button)view.FindViewById(Resource.Id.Camera);
                    Galery = (Button)view.FindViewById(Resource.Id.Galery);
                    Galery.Click += Galery_open;
                    Camera.Click += Camera_open;

                    alert.SetPositiveButton("Добавить", HandlePositiveButtonClick);
                    alert.SetNegativeButton("Отмена", HandleNegativeButtonClick);
                    alert.SetView(view);
                    Dialog dialog = alert.Create();
                    dialog.Show();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        private void Camera_open(object sender, EventArgs e)
        {
            filename = "file_" + Guid.NewGuid().ToString() + ".jpg";
            Camera_intent = new Intent(MediaStore.ActionImageCapture);
            file = new File(Android.OS.Environment.ExternalStorageDirectory, filename);
            uri = Android.Net.Uri.FromFile(file);

            Camera_intent.PutExtra(MediaStore.ExtraOutput, uri);
            Camera_intent.PutExtra("return-data", true);
            StartActivityForResult(Camera_intent, 0);
        }
        private void Galery_open(object sender, EventArgs e)
        {
            Galery_intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
            StartActivityForResult(Intent.CreateChooser(Galery_intent, "Select images"), 2);
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if ((requestCode == 0 && resultCode == Result.Ok))
            {
                CropImage();
            }
            else if (requestCode == 2)
            {
                if (data != null)
                {
                    uri = data.Data;
                    CropImage();
                }

            }
            else if (requestCode == 1)
            {
                if (data != null)
                {
                    Bundle bundle = data.Extras;
                    Bitmap bitmap = (Bitmap)bundle.GetParcelable("data");
                    System.Console.WriteLine("FFFF" + bitmap);

                }
            }
        }
        private void CropImage()
        {
            try
            {
                Crop_intent = new Intent("com.android.camera.action.CROP");
                Crop_intent.SetDataAndType(uri, "image/*");

                Crop_intent.PutExtra("crop", "true");
                Crop_intent.PutExtra("outputX", 128);
                Crop_intent.PutExtra("outputY", 128);
                Crop_intent.PutExtra("aspectX", 3);
                Crop_intent.PutExtra("aspectY", 4);
                Crop_intent.PutExtra("scaleUpIfNeeded", true);
                Crop_intent.PutExtra("return-data", true);

                StartActivityForResult(Crop_intent, 1);

            }
            catch (ActivityNotFoundException ex)
            {

            }
        }
        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {

        }
        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            cards.Add(new Card(word_card.Text, translate_card.Text));
            bd.items_card_title.Add(word_card.Text);
            bd.items_card_translate.Add(translate_card.Text);
            new_card = new List<Card>();
            new_card.Add(new Card(word_card.Text, translate_card.Text));
            bd.Add_deck_cards(Intent.GetStringExtra("id_deck"), new_card);
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
            bd.Cards_list(Intent.GetStringExtra("id_deck"));
            adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_title);
            adapter3 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_translate);
            list_card.Adapter = adapter;
        }
        public void List_card()
        {
            cards = new List<Card>();
            bd.Cards_list(Intent.GetStringExtra("id_deck"));
            for (int i = 0; i < bd.items_card_title.Count; i++)
            {
                cards.Add(new Card(bd.items_card_title[i], bd.items_card_translate[i]));
            }
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
            adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_title);
            adapter3 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_translate);
            list_card.Adapter = adapter;
        }
    }
}