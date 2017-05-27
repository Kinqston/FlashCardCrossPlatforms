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
using Java.Net;
using Android.Database;
using System.Drawing;

namespace FlashCardsPort.Droid
{
    [Activity(Label = "���������� ����", Icon = null)]
    public class Add_card_admin : Activity
    {
        string ftpHost = "ftp.billions-consult.ru";
      //  string ftpUser = "u688865617.kinkston";
      //  string ftpPassword = "kinkston";
        string ftpUser = "graversp_fc";
        string ftpPassword = "{*545S7e";
        OutputStream fOut = null;

        bool Camera_image = false;
        string filename;
        string ftpfullpath;

        string ImagePath;
        Bitmap bitmap;
        public string title;
        public string cost;
        public Android.Net.Uri uri;
        ImageView imageview;
        EditText word_card, translate_card;
        File file;
        Intent Camera_intent, Galery_intent, Crop_intent;
        const int RequestPermissionCode = 1;
        static BaseData bd = new BaseData();
        private List<Card> cards;
        private List<String> cards_word_create_deck;
        private List<String> cards_translate_create_deck;
        private List<Android.Net.Uri> cards_image_create_deck;
        private Bitmap cards_image_create_deck_edit;
        private CustomAdapter adapter;
        private CustomAdapterAddCard adapterAddCard;
        public ArrayAdapter<string> adapter2;
        public ArrayAdapter<string> adapter3;
        public ArrayAdapter<Android.Net.Uri> adapter4;
        public string cards_word, cards_translate, cards_image;
        ListView list_card;
        Button Camera, Galery;
        Button edit_item, delete_item;
        IBlob blob;
        public Dialog dialog, dialog1;
        public Bitmap cards_image_bitmap;

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
            if (Intent.GetStringExtra("function") == "Edit")
            {
                List_card();
            }           
            // Create your application here
        }

        private void Change_card(object sender, AdapterView.ItemLongClickEventArgs e)
        {
          //  cards_word = adapter.cards[e.Position].Word;
          //  cards_translate = adapter.cards[e.Position].Translate;
            cards_word = adapter.cards[e.Position].Word;
            cards_translate = adapter.cards[e.Position].Translate;
            cards_image = adapter.cards[e.Position].Image;
            cards_image_bitmap = adapter.cards[e.Position].Bitmap_image;
            // cards_image_create_deck_edit = adapter.cards[e.Position].image;
            //  cards_translate = adapter3.GetItem(e.Position);
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
            imageview.SetImageBitmap(cards_image_bitmap);

            Camera = (Button)view.FindViewById(Resource.Id.Camera);
            Galery = (Button)view.FindViewById(Resource.Id.Galery);
            Galery.Click += Galery_open;
            Camera.Click += Camera_open;

            alert.SetPositiveButton("��������", Change);
            alert.SetNegativeButton("������", HandleNegativeButtonClick);
            alert.SetView(view);
            Dialog dialog = alert.Create();
            dialog.Show();  
        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
        private void Change(object sender, DialogClickEventArgs e)
        {
            if (ImagePath != null)
            {
                ftpfullpath = "ftp://graversp.beget.tech/public_html/" + filename;
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                //Android.Net.Uri url = Android.Net.Uri.Parse(ImagePath);

                imageview.SetImageURI(uri);
                System.IO.FileStream fs = System.IO.File.OpenRead(ImagePath);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                System.IO.Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
                ftpstream.Flush();
            }
            else {
                filename = cards_image;
                bitmap = cards_image_bitmap;
            }
            for (int i = 0; i < cards.Count(); i++)
            {
                if (cards[i].Word == cards_word && cards[i].Translate == cards_translate)
                {
                    cards.RemoveAt(i);
                }
            }
            cards.Add(new Card(word_card.Text, translate_card.Text, filename, bitmap));
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
           // adapterAddCard = new CustomAdapterAddCard(this, Resource.Layout.Custom_layout, cards);
            list_card.Adapter = adapter;
            uri = null;
            bitmap = null;
            dialog1.Hide();
        }
        private void delete_item_click(object sender, EventArgs e)
        {
            dialog1.Hide();
            for (int i = 0; i < cards.Count(); i++)
            {
                if(cards[i].Word == cards_word)
                {
                    cards.RemoveAt(i);
                }
            }
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
           // list_card.Adapter = adapter;
          //  adapterAddCard = new CustomAdapterAddCard(this, Resource.Layout.Custom_layout, cards);
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

                    alert.SetPositiveButton("��������", HandlePositiveButtonClick);
                    alert.SetNegativeButton("������", HandleNegativeButtonClick);
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


                        //ftpfullpath = "ftp://ftp.billions-consult.ru/flashcards/" + filename;
                        //FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                        //ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                        //ftp.KeepAlive = true;
                        //ftp.UseBinary = true;
                        //ftp.Method = WebRequestMethods.Ftp.UploadFile;
                        ////Android.Net.Uri url = Android.Net.Uri.Parse(ImagePath);

                        //imageview.SetImageURI(uri);
                        //System.IO.FileStream fs = System.IO.File.OpenRead(ImagePath);
                        //byte[] buffer = new byte[fs.Length];
                        //fs.Read(buffer, 0, buffer.Length);
                        //fs.Close();
                        //System.IO.Stream ftpstream = ftp.GetRequestStream();
                        //ftpstream.Write(buffer, 0, buffer.Length);
                        //ftpstream.Close();
                        //ftpstream.Flush();
                    }
                    StartActivity(typeof(Decks_admin));
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        private void Camera_open(object sender, EventArgs e)
        {
            Camera_image = true;
            Camera_intent = new Intent(MediaStore.ActionImageCapture);
            filename = "file_" + Guid.NewGuid().ToString() + ".jpg";
            file = new File(Android.OS.Environment.ExternalStorageDirectory,filename);
            ImagePath = file.Path;
            uri = Android.Net.Uri.FromFile(file);
          //  System.Console.WriteLine(uri);
           // System.Console.WriteLine(file);
            Camera_intent.PutExtra(MediaStore.ExtraOutput, uri);
            Camera_intent.PutExtra("return-data", true);
            StartActivityForResult(Camera_intent, 0);
        }
        private void Galery_open(object sender, EventArgs e)
        {
            Galery_intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
           // ImagePath = GetPathToImage(Galery_intent.Data);
            filename = "file_" + Guid.NewGuid().ToString() + ".jpg";
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
                    ImagePath = GetPathToImage(data.Data);
                    CropImage();
                   // imageview.SetImageURI(uri);
                }

            }
            else if (requestCode==1)
            {
                if(data!= null)
                {
                    Bundle bundle = data.Extras;
                    bitmap = (Bitmap)bundle.GetParcelable("data");
                    SavePicture(bitmap);
                    imageview.SetImageBitmap(bitmap);
                }
            }    
        }
        private string GetPathToImage(Android.Net.Uri uri)
        {
            string doc_id = "";
            using (var c1 = ContentResolver.Query(uri, null, null, null, null))
            {
                c1.MoveToFirst();
                String document_id = c1.GetString(0);
                doc_id = document_id.Substring(document_id.LastIndexOf(":") + 1);
            }

            string path = null;

            // The projection contains the columns we want to return in our query.
            string selection = Android.Provider.MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
            using (var cursor = ManagedQuery(Android.Provider.MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { doc_id }, null))
            {
                if (cursor == null) return path;
                var columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
                cursor.MoveToFirst();
                path = cursor.GetString(columnIndex);
            }
            return path;
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
            if (ImagePath != null)
            {
                ftpfullpath = "ftp://graversp.beget.tech/public_html/" + filename;
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                //Android.Net.Uri url = Android.Net.Uri.Parse(ImagePath);

                imageview.SetImageURI(uri);
                System.IO.FileStream fs = System.IO.File.OpenRead(ImagePath);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();
                System.IO.Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
                ftpstream.Flush();
                cards.Add(new Card(word_card.Text, translate_card.Text, filename, bitmap));
            }
            else
            {
                filename = cards_image;
                cards.Add(new Card(word_card.Text, translate_card.Text, filename, null));
            }      
            //cards_word_create_deck.Add(word_card.Text);
            //cards_translate_create_deck.Add(translate_card.Text);
            //cards_image_create_deck.Add(uri);
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
           // adapterAddCard = new CustomAdapterAddCard(this, Resource.Layout.Custom_layout, cards);
            //adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, cards_word_create_deck);
            //adapter3 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, cards_translate_create_deck);
            //  adapter4 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, cards_image_create_deck);
            list_card.Adapter = adapter;
            uri = null;
            bitmap = null;
        }
        private void SavePicture(Bitmap bitmap)
        {
            if (Camera_image == true)
            {
                File file = new File(ImagePath);
                file.Delete();
            }
            filename = "file_" + Guid.NewGuid().ToString() + ".jpg";
            var Path = Android.OS.Environment.ExternalStorageDirectory.ToString();
            var filePath = System.IO.Path.Combine(Path, filename);
            ImagePath = filePath;
            var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
            stream.Close();
            Camera_image = false;
           // filename = "file_" + Guid.NewGuid().ToString() + ".jpg";
           // file = new File(Path, filename);

        }
        public void List_card()
        {
            cards = new List<Card>();
            bd.Cards_list(Intent.GetStringExtra("id_deck"));
            for (int i = 0; i < bd.items_card_title.Count; i++)
            {
                cards.Add(new Card(bd.items_card_title[i], bd.items_card_translate[i], bd.items_card_image[i], bd.bitmap[i]));
            }
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
           // adapterAddCard = new CustomAdapterAddCard(this, Resource.Layout.Custom_layout, cards);
            adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_title);
            adapter3 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_translate);
           // adapter4 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_image);
            list_card.Adapter = adapter;
        }
        
    }
}