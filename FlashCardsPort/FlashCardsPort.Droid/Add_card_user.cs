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
    [Activity(Label = "Добавление карт", Icon = null)]
    public class Add_card_user : Activity
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
        private List<Card> cards, cards_orig;
        private List<String> cards_word_create_deck;
        private List<String> cards_translate_create_deck;
        private List<Android.Net.Uri> cards_image_create_deck;
        private Bitmap cards_image_create_deck_edit;
        private CustomAdapterOffline adapter;
        private CustomAdapterAddCard adapterAddCard;
        public ArrayAdapter<string> adapter2;
        public ArrayAdapter<string> adapter3;
        public ArrayAdapter<Android.Net.Uri> adapter4;
        public string cards_word, cards_translate, cards_image, cards_id;
        ListView list_card;
        Button Camera, Galery;
        Button edit_item, delete_item;
        Button ok_repeat_card, cancel_repeat_card;
        IBlob blob;
        public Dialog dialog, dialog1, dialog3;
        public Bitmap cards_image_bitmap;
        bool create_card = true;
        int new_card_id = 0;
        int action_card;
        private string pathToDatabase;
        private List<DeckLocal> decks;
        private DeckLocal decks_copy;
        private CardLocal card_copy;
        Button Cancel_create_card, Save_create_card;
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
            decks = new List<DeckLocal>();
            cards_image = null;
            cards_image_bitmap = null;
            create_card = true;
            var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            pathToDatabase = System.IO.Path.Combine(documentsFolder, "FlashCards_Database.db");
            adapter = new CustomAdapterOffline(this, Resource.Layout.Custom_layout, cards);
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
            cards_id = adapter.cards[e.Position].Id;
            action_card = e.Position;
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
            create_card = true;
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            var view = layoutInflater.Inflate(Resource.Layout.add_card_admin, null);
            word_card = (EditText)view.FindViewById(Resource.Id.word_card);
            translate_card = (EditText)view.FindViewById(Resource.Id.translate_card);
            imageview = (ImageView)view.FindViewById(Resource.Id.icon_card);
            Cancel_create_card = (Button)view.FindViewById(Resource.Id.Cancel_create_card);
            Save_create_card = (Button)view.FindViewById(Resource.Id.Save_create_card);

            word_card.Text = cards_word;
            translate_card.Text = cards_translate;                
            imageview.SetImageBitmap(cards_image_bitmap);

            Camera = (Button)view.FindViewById(Resource.Id.Camera);
            Galery = (Button)view.FindViewById(Resource.Id.Galery);
            Galery.Click += Galery_open;
            Camera.Click += Camera_open;
            Cancel_create_card.Click += Cancel_edit;
            Save_create_card.Click += Save_change;

            alert.SetView(view);
            dialog3 = alert.Create();
            dialog3.Show();  
        }

        private void Cancel_edit(object sender, EventArgs e)
        {
            dialog3.Hide();
            dialog1.Hide();
        }

        private void Save_change(object sender, EventArgs e)
        {
            for (int i = 0; i < adapter.Count; i++)
                if ((word_card.Text == adapter.cards[i].Word) && (translate_card.Text == adapter.cards[i].Translate))
                {
                    if (action_card != i)
                        create_card = false;
                }
            if (create_card == true)
            {
                Change_card();
            }
            else
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                var view = layoutInflater.Inflate(Resource.Layout.repeat_card, null);
                ok_repeat_card = (Button)view.FindViewById(Resource.Id.Ok_repeat_card);
                cancel_repeat_card = (Button)view.FindViewById(Resource.Id.Cancel_repeat_card);

                ok_repeat_card.Click += Repeat_Create_card;
                cancel_repeat_card.Click += Cancel_repeat_card;
                alert.SetView(view);
                dialog = alert.Create();
                dialog.Show();
                create_card = true;
            }
        }

        private void Cancel_repeat_card(object sender, EventArgs e)
        {
            dialog.Hide();
        }

        private void Repeat_Create_card(object sender, EventArgs e)
        {
            Change_card();
            dialog.Hide();
            dialog3.Hide();
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
            for (int i = 0; i < adapter.Count; i++)
                if ((word_card.Text == adapter.cards[i].Word) && (translate_card.Text == adapter.cards[i].Translate))          // Проверить на повтор карточки
                {
                    if(action_card!=i)
                        create_card = false;
                }
            if (create_card == true)
            {
                Change_card();
            }
            else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Создание карточки");
                alert.SetMessage("Такая карточка уже существует, создать еще одну?");
                alert.SetPositiveButton("Создать", (senderAlert, args) =>
                {
                    Change_card();
                });
                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                {
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }
        public void Change_card()
        {
            if (ImagePath != null)
            {
                //ftpfullpath = "ftp://graversp.beget.tech/public_html/" + filename;
                //FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                //ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                //ftp.KeepAlive = true;
                //ftp.UseBinary = true;
                //ftp.Method = WebRequestMethods.Ftp.UploadFile;
                ////Android.Net.Uri url = Android.Net.Uri.Parse(ImagePath);

                imageview.SetImageURI(uri);
                //System.IO.FileStream fs = System.IO.File.OpenRead(ImagePath);
                //byte[] buffer = new byte[fs.Length];
                //fs.Read(buffer, 0, buffer.Length);
                //fs.Close();
                //System.IO.Stream ftpstream = ftp.GetRequestStream();
                //ftpstream.Write(buffer, 0, buffer.Length);
                //ftpstream.Close();
                //ftpstream.Flush();
            }
            else
            {
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
            cards.Add(new Card(cards_id, word_card.Text, translate_card.Text, ImagePath, bitmap));
            adapter = new CustomAdapterOffline(this, Resource.Layout.Custom_layout, cards);
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
                if(cards[i].Id == cards_id)
                {
                    cards.RemoveAt(i);
                }
            }
            adapter = new CustomAdapterOffline(this, Resource.Layout.Custom_layout, cards);
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
                    var intent = new Intent(this, typeof(Decks_user));
                    StartActivity(intent);
                    break;
                case Resource.Id.item1:
                    cards_image = null;
                    cards_image_bitmap = null;
                    create_card = true;
                    ImagePath = null;
                    new_card_id++;
                    LayoutInflater layoutInflater = LayoutInflater.From(this);
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    var view = layoutInflater.Inflate(Resource.Layout.add_card_admin, null);
                    word_card = (EditText) view.FindViewById(Resource.Id.word_card);
                    translate_card = (EditText) view.FindViewById(Resource.Id.translate_card);
                    imageview = (ImageView)view.FindViewById(Resource.Id.icon_card);
                    Camera = (Button)view.FindViewById(Resource.Id.Camera);
                    Galery = (Button)view.FindViewById(Resource.Id.Galery);
                    Cancel_create_card = (Button)view.FindViewById(Resource.Id.Cancel_create_card);
                    Save_create_card = (Button)view.FindViewById(Resource.Id.Save_create_card);
                    Galery.Click += Galery_open;
                    Camera.Click += Camera_open;

                    Cancel_create_card.Click += Cancel_create;
                    Save_create_card.Click += Save_create;
                    alert.SetView(view);
                    dialog3 = alert.Create();
                    dialog3.Show();
                    break;
                case Resource.Id.done:
                    if (Intent.GetStringExtra("function") == "Edit")
                    {
                        //bd.delete_item_list(Intent.GetStringExtra("id_deck"));
                        //bd.Add_deck(Intent.GetStringExtra("title"), Intent.GetStringExtra("cost"));
                        // bd.New_deck_id(Intent.GetStringExtra("title"), Intent.GetStringExtra("cost"));
                        // bd.Add_deck_cards(Intent.GetStringExtra("title"), cards);
                        //bd.Update_deck(Intent.GetStringExtra("id_deck"), Intent.GetStringExtra("title"), Intent.GetStringExtra("cost"));       !!!!
                        //bd.Delete_all_card(Intent.GetStringExtra("id_deck"));                                                                     !!!!
                        //bd.Add_deck_cards(Intent.GetStringExtra("id_deck"), cards);             
                        decks_copy = new DeckLocal(); 
                        using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
                        {
                            decks_copy = connection.Get<DeckLocal>(Convert.ToInt32(Intent.GetStringExtra("id_deck")));
                            connection.Update(new DeckLocal() { id = Convert.ToInt32(Intent.GetStringExtra("id_deck")), title = Intent.GetStringExtra("title"), acrive_deck = 0});
                            foreach (Card card in cards_orig)
                            {
                                connection.Delete(new CardLocal() { id = Convert.ToInt32(card.Id)});
                            }
                            foreach (Card card in cards)
                            {
                                connection.Insert(new CardLocal() {id = Convert.ToInt32(card.Id), id_deck = Convert.ToInt32(Intent.GetStringExtra("id_deck")), word = card.Word, translate = card.Translate, image = card.Image, archive_card = 0, count_repeat = 0});
                            }
                        }
                    }
                    if (Intent.GetStringExtra("function") == "Create")
                    {
                        using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
                        {
                            var query = connection.Table<DeckLocal>();
                            string new_id;
                            connection.Insert(new DeckLocal() {title = Intent.GetStringExtra("title"), acrive_deck=0});
                            foreach (DeckLocal deck in query)
                            {
                                decks.Add(deck);
                            }
                            foreach (Card card in cards)
                            {
                                connection.Insert(new CardLocal() { id_deck = decks[decks.Count-1].id, word = card.Word, translate = card.Translate, image = card.Image, archive_card = 0, count_repeat = 0 });
                            }
                        }
                        //bd.Add_deck(Intent.GetStringExtra("title"), Intent.GetStringExtra("cost"));                                                   !!1
                        //bd.New_deck_id(Intent.GetStringExtra("title"), Intent.GetStringExtra("cost"));                                        !!!!
                        //bd.Add_deck_cards(bd.new_id_deck, cards);                                                                                     !!!!
                    }
                    StartActivity(typeof(Decks_user));
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void Save_create(object sender, EventArgs e)
        {
            for (int i = 0; i < adapter.Count; i++)
                if ((word_card.Text == adapter.cards[i].Word) && (translate_card.Text == adapter.cards[i].Translate))
                    create_card = false;
            if (create_card == true)
            {
                Create_card();
                dialog3.Hide();
            }
            else
            {
                LayoutInflater layoutInflater = LayoutInflater.From(this);
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                var view = layoutInflater.Inflate(Resource.Layout.repeat_card, null);
                ok_repeat_card = (Button)view.FindViewById(Resource.Id.Ok_repeat_card);
                cancel_repeat_card = (Button)view.FindViewById(Resource.Id.Cancel_repeat_card);

                ok_repeat_card.Click += Repeat_Create_card_new;
                cancel_repeat_card.Click += Cancel_repeat_card;
                alert.SetView(view);
                dialog = alert.Create();
                dialog.Show();
                create_card = true;
            }
        }

        private void Repeat_Create_card_new(object sender, EventArgs e)
        {
            Create_card();
            dialog.Hide();
            dialog3.Hide();
        }

        private void Cancel_create(object sender, EventArgs e)
        {
            dialog3.Hide();
        }

        private void Camera_open(object sender, EventArgs e)
        {
            Camera_image = true;
            Camera_intent = new Intent(MediaStore.ActionImageCapture);
            filename = "file_" + Guid.NewGuid().ToString() + ".jpg";
            File folder = new File(Android.OS.Environment.ExternalStorageDirectory + File.Separator + "FlashCard_image");
            if (!folder.Exists())
            {
                folder.Mkdirs();
            }
            file = new File(folder,filename);
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
        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
            
        }
        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            for (int i = 0; i < adapter.Count; i++)
                if ((word_card.Text == adapter.cards[i].Word) && (translate_card.Text == adapter.cards[i].Translate))    // Проверить карточку на повтор
                    create_card = false;           
            if (create_card == true)
            {
                 Create_card();                
            }
            else {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Создание карточки");
                alert.SetMessage("Такая карточка уже существует, создать еще одну?");
                alert.SetPositiveButton("Создать", (senderAlert, args) =>
                {
                    Create_card();
                });
                alert.SetNegativeButton("Отмена", (senderAlert, args) =>
                {
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }
        public void Create_card()
        {
            if (ImagePath != null)
            {
                //ftpfullpath = "ftp://graversp.beget.tech/public_html/" + filename;
                //FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                //ftp.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                //ftp.KeepAlive = true;
                //ftp.UseBinary = true;
                //ftp.Method = WebRequestMethods.Ftp.UploadFile;
                ////Android.Net.Uri url = Android.Net.Uri.Parse(ImagePath);

                imageview.SetImageURI(uri);
                //System.IO.FileStream fs = System.IO.File.OpenRead(ImagePath);
                //byte[] buffer = new byte[fs.Length];
                //fs.Read(buffer, 0, buffer.Length);
                //fs.Close();
                //System.IO.Stream ftpstream = ftp.GetRequestStream();
                //ftpstream.Write(buffer, 0, buffer.Length);
                //ftpstream.Close();
                //ftpstream.Flush();
                ////using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
                ////{
                ////    connection.Insert(new CardLocal() {id_deck = 0, word = word_card.Text, translate = translate_card.Text, image = ImagePath, archive_card = 0, count_repeat = 0});
                ////}
                cards.Add(new Card(new_card_id.ToString(), word_card.Text, translate_card.Text, ImagePath, bitmap));
            }
            else
            {
                ImagePath = cards_image;
                if (cards_image == null)
                {
                    ImagePath = null;
                }
                cards.Add(new Card(new_card_id.ToString(), word_card.Text, translate_card.Text, ImagePath, null));
            }
            adapter = new CustomAdapterOffline(this, Resource.Layout.Custom_layout, cards);
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
            File folder = new File(Android.OS.Environment.ExternalStorageDirectory + File.Separator + "FlashCard_image");
            if (!folder.Exists())
            {
                folder.Mkdirs();
            }
            var Path = folder.Path;
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
            cards_orig = new List<Card>();
            using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
            {
                var query = connection.Table<CardLocal>();
                foreach (CardLocal card in query)
                {
                    if (card.id_deck == Convert.ToInt32(Intent.GetStringExtra("id_deck")))
                    {
                        cards.Add(new Card(card.id.ToString(), card.word, card.translate, card.image, null));
                        cards_orig.Add(new Card(card.id.ToString(), card.word, card.translate, card.image, null));
                    }
                }
            }
            //    bd.Cards_list(Intent.GetStringExtra("id_deck"));
            //    for (int i = 0; i < bd.items_card_title.Count; i++)
            //    {
            //        cards.Add(new Card(bd.items_card_id[i], bd.items_card_title[i], bd.items_card_translate[i], bd.items_card_image[i], bd.bitmap[i]));
            //    }
            //using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
            //{
            //    var query = connection.Table<DeckLocal>();

            //    foreach (DeckLocal deck in query)
            //    {
            //        decks_title.Add(deck.title);
            //        decks_id.Add(deck.id.ToString());
            //        //TableView.ReloadData();
            //    }
            //}
            adapter = new CustomAdapterOffline(this, Resource.Layout.Custom_layout, cards);
           // adapterAddCard = new CustomAdapterAddCard(this, Resource.Layout.Custom_layout, cards);
           // adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_title);
         //   adapter3 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_translate);
           // adapter4 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_image);
            list_card.Adapter = adapter;
        } 
    }
}