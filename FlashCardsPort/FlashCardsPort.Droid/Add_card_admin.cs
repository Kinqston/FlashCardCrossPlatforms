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

namespace FlashCardsPort.Droid
{
    [Activity(Label = "Добавление карт",Icon = null)]
    public class Add_card_admin : Activity
    {
        public string title;
        public string cost;
        Android.Net.Uri uri;
        ImageView imageview;
        EditText word_card, translate_card;
        File file;
        Intent Camera_intent, Galery_intent, Crop_intent;
        const int RequestPermissionCode = 1;
        static BaseData bd = new BaseData();
        private List<Card> cards;
        private CustomAdapter adapter;
        public ArrayAdapter<string> adapter2;
        public ArrayAdapter<string> adapter3;
        public string cards_word, cards_translate;
        ListView list_card;
        Button Camera, Galery;
        Button edit_item, delete_item;
        IBlob blob;
        public Dialog dialog, dialog1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.dialog_add_cards_admin);
            list_card = FindViewById<ListView>(Resource.Id.list_card);
            Decks_admin da = new Decks_admin();
            // Console.WriteLine("FFFFFFFFFFFF"+ Intent.GetStringExtra("title"));
            this.Title = Intent.GetStringExtra("title");
            list_card.ItemLongClick += Change_card;
            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.SetDisplayHomeAsUpEnabled(true);
            cards = new List<Card>();
            List_card();
            // Create your application here
        }

        private void Change_card(object sender, AdapterView.ItemLongClickEventArgs e)
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
            for (int i = 0; i < bd.items_card_title.Count; i++)
            {
                if (cards[i].Word == cards_word && cards[i].Translate == cards_translate)
                {
                    cards.RemoveAt(i);                   
                }
            }
            cards.Add(new Card(word_card.Text, translate_card.Text));
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
            list_card.Adapter = adapter;
            dialog1.Hide();
        }

        private void delete_item_click(object sender, EventArgs e)
        {
            bd.Delete_card(Intent.GetStringExtra("id_deck"), cards_word);
            dialog1.Hide();
            for (int i = 0; i < bd.items_card_title.Count; i++)
            {
                if(cards[i].Word == cards_word)
                {
                    cards.RemoveAt(i);
                }
            }
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
            list_card.Adapter = adapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.deck_admin_actionbar, menu);
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
                    word_card = (EditText) view.FindViewById(Resource.Id.word_card);
                    translate_card = (EditText) view.FindViewById(Resource.Id.translate_card);
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
                case Resource.Id.done:
                    if (Intent.GetStringExtra("function") == "Edit")
                    {
                        //bd.delete_item_list(Intent.GetStringExtra("id_deck"));
                        //bd.Add_deck(Intent.GetStringExtra("title"), Intent.GetStringExtra("cost"));
                        // bd.New_deck_id(Intent.GetStringExtra("title"), Intent.GetStringExtra("cost"));
                        // bd.Add_deck_cards(Intent.GetStringExtra("title"), cards);
                        bd.Update_deck(Intent.GetStringExtra("id_deck"), Intent.GetStringExtra("title"), Intent.GetStringExtra("cost"));
                        bd.Delete_all_card(Intent.GetStringExtra("id_deck"));
                        bd.Add_deck_cards(Intent.GetStringExtra("id_deck"), cards);
                    }
                    if (Intent.GetStringExtra("function") == "Create")
                    {
                        bd.Add_deck(Intent.GetStringExtra("title"), Intent.GetStringExtra("cost"));
                        bd.New_deck_id(Intent.GetStringExtra("title"), Intent.GetStringExtra("cost"));
                        bd.Add_deck_cards(bd.new_id_deck, cards);
                    }
                    StartActivity(typeof(Decks_admin));
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void Camera_open(object sender, EventArgs e)
        {
            Camera_intent = new Intent(MediaStore.ActionImageCapture);
            file = new File(Android.OS.Environment.ExternalStorageDirectory, "file_" + Guid.NewGuid().ToString() + ".jpg"); 
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
            if((requestCode == 0 && resultCode == Result.Ok))
            {
                CropImage();
            }
            else if (requestCode == 2){
                if (data != null)
                {
                    uri = data.Data;
                    CropImage();
                }

            }
            else if (requestCode==1)
            {
                if(data!= null)
                {
                    Bundle bundle = data.Extras;
                    Bitmap bitmap = (Bitmap)bundle.GetParcelable("data");
                    System.Console.WriteLine("FFFF"+bitmap);
                           
                }
            }
        }

        private void CropImage()
        {
            try
            {
                Crop_intent = new Intent("com.android.camera.action.CROP");
                Crop_intent.SetDataAndType(uri, "image/*");

                Crop_intent.PutExtra("crop","true");
                Crop_intent.PutExtra("outputX", 128);
                Crop_intent.PutExtra("outputY", 128);
                Crop_intent.PutExtra("aspectX", 3);
                Crop_intent.PutExtra("aspectY", 4);
                Crop_intent.PutExtra("scaleUpIfNeeded", true);
                Crop_intent.PutExtra("return-data", true);

                StartActivityForResult(Crop_intent, 1);

            }
            catch(ActivityNotFoundException ex)
            {

            }
        }

        //public string convertToBase64(Bitmap bitmap)
        //{
        //    ByteArrayOutputStream os = new ByteArrayOutputStream();
        //    bitmap.Compress(Bitmap.CompressFormat.Png, 100, os);
        //    byte[] byteArray = os.ToByteArray();
        //    return Base64.EncodeToString(byteArray, 0);
        //}

        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
            
        }

        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            cards.Add(new Card(word_card.Text, translate_card.Text));
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
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