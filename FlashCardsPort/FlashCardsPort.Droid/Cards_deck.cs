﻿using System;
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
        string ftpUser = "graversp_fc";
        string ftpPassword = "{*545S7e";
        string ftpfullpath;
        private List<Card> new_card;
        private List<Card> cards;
        private CustomAdapter adapter;
        public ArrayAdapter<string> adapter2;
        public ArrayAdapter<string> adapter3;
        Intent Camera_intent, Galery_intent, Crop_intent;
        Button edit_item, delete_item;
        Android.Net.Uri uri;
        string ImagePath;
        bool Camera_image;
        ImageView imageview;
        TextView Header_card;
        File file;
        ListView list_card;
        EditText word_card, translate_card;
        Button Camera, Galery;
        Button Cancel_create_card, Save_create_card;
        Button ok_repeat_card, cancel_repeat_card;
        public string cards_word, cards_translate, cards_image, cards_id;
        public Bitmap cards_image_bitmap;
        LayoutInflater inflater;
        public Dialog dialog, dialog1, dialog3;
        public Bitmap cards_bitmap_image;
        public Bitmap bitmap;
        bool create_card = true;
        int action_card;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.cards_deck);
            bd.connection();
            list_card = FindViewById<ListView>(Resource.Id.list_cards);
            list_card.ItemLongClick += delete_edit_card;
            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayHomeAsUpEnabled(true);
            actionBar.SetDisplayShowHomeEnabled(false);
            this.Title = Intent.GetStringExtra("title_deck");
            List_card();
        }
        private void delete_edit_card(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            cards_id = adapter.cards[e.Position].Id;
            cards_word = adapter.cards[e.Position].Word;
            cards_translate = adapter.cards[e.Position].Translate;
            cards_image = adapter.cards[e.Position].Image;
            action_card = e.Position;
            cards_bitmap_image = adapter.cards[e.Position].Bitmap_image;
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
            create_card = true;
            LayoutInflater layoutInflater = LayoutInflater.From(this);
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            var view = layoutInflater.Inflate(Resource.Layout.add_card_admin, null);
            word_card = (EditText)view.FindViewById(Resource.Id.word_card);
            translate_card = (EditText)view.FindViewById(Resource.Id.translate_card);
            imageview = (ImageView)view.FindViewById(Resource.Id.icon_card);
            Cancel_create_card = (Button)view.FindViewById(Resource.Id.Cancel_create_card);
            Save_create_card = (Button)view.FindViewById(Resource.Id.Save_create_card);
            Header_card = (TextView)view.FindViewById(Resource.Id.Card_1);

            Header_card.Text = "Редактирование";

            word_card.Text = cards_word;
            translate_card.Text = cards_translate;
            imageview.SetImageBitmap(cards_bitmap_image);

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
            for(int i=0;i< adapter2.Count; i++)
                if((word_card.Text == adapter2.GetItem(i))&&(translate_card.Text == adapter3.GetItem(i)))
                {
                    if(action_card!=i)
                        create_card = false;
                }
            if (create_card == true)
            {
                Update_card();
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

        public void Update_card()
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
            else
            {
                filename = cards_image;
                bitmap = cards_bitmap_image;
            }
            bd.Update_cards(cards_id, Intent.GetStringExtra("id_deck"), cards_word, word_card.Text, cards_translate, translate_card.Text, filename);
            for (int i = 0; i < cards.Count(); i++)
            {
                if (cards[i].Word == cards_word && cards[i].Translate == cards_translate)
                {
                    cards.RemoveAt(i);
                }
            }
            cards.Add(new Card(cards_id, word_card.Text, translate_card.Text, filename, bitmap));
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
            adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_title);
            adapter3 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_translate);
            list_card.Adapter = adapter;
            dialog1.Hide();
        }
        private void delete_item_click(object sender, EventArgs e)
        {
            bd.Delete_card(Intent.GetStringExtra("id_deck"), cards_word, cards_id);
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
                    cards_image = null;
                    cards_bitmap_image = null;
                    create_card = true;
                    ImagePath = null;
                    LayoutInflater layoutInflater = LayoutInflater.From(this);
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    var view = layoutInflater.Inflate(Resource.Layout.add_card_admin, null);
                    word_card = (EditText)view.FindViewById(Resource.Id.word_card);
                    translate_card = (EditText)view.FindViewById(Resource.Id.translate_card);
                    imageview = (ImageView)view.FindViewById(Resource.Id.icon_card);
                    Cancel_create_card = (Button)view.FindViewById(Resource.Id.Cancel_create_card);
                    Save_create_card = (Button)view.FindViewById(Resource.Id.Save_create_card);
                    Header_card = (TextView)view.FindViewById(Resource.Id.Card_1);

                    Header_card.Text = "Создание";
                    Camera = (Button)view.FindViewById(Resource.Id.Camera);
                    Galery = (Button)view.FindViewById(Resource.Id.Galery);
                    Galery.Click += Galery_open;
                    Camera.Click += Camera_open;
                    Cancel_create_card.Click += Cancel_create;
                    Save_create_card.Click += Save_create;
                    alert.SetView(view);
                    dialog3 = alert.Create();
                    dialog3.Show();
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
                dialog3.Hide();
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

        private void Change_card()
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
            cards.Add(new Card(cards_id, word_card.Text, translate_card.Text, filename, bitmap));
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
            // adapterAddCard = new CustomAdapterAddCard(this, Resource.Layout.Custom_layout, cards);
            list_card.Adapter = adapter;
            uri = null;
            bitmap = null;
            dialog1.Hide();
        }

        private void Cancel_create(object sender, EventArgs e)
        {
            dialog3.Hide();
        }

        private void Camera_open(object sender, EventArgs e)
        {
            Camera_image = true;
            filename = "file_" + Guid.NewGuid().ToString() + ".jpg";
            Camera_intent = new Intent(MediaStore.ActionImageCapture);
            File folder = new File(Android.OS.Environment.ExternalStorageDirectory + File.Separator + "FlashCard_image");
            if (!folder.Exists())
            {
                folder.Mkdirs();
            }
            file = new File(folder, filename);
            uri = Android.Net.Uri.FromFile(file);
            ImagePath = file.Path;
            Camera_intent.PutExtra(MediaStore.ExtraOutput, uri);
            Camera_intent.PutExtra("return-data", true);
            StartActivityForResult(Camera_intent, 0);
        }
        private void Galery_open(object sender, EventArgs e)
        {
            Galery_intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
            filename = "file_" + Guid.NewGuid().ToString() + ".jpg";
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
                    ImagePath = GetPathToImage(data.Data);
                    CropImage();
                }

            }
            else if (requestCode == 1)
            {
                if (data != null)
                {
                    Bundle bundle = data.Extras;
                    bitmap = (Bitmap)bundle.GetParcelable("data");
                    SavePicture(bitmap);
                    imageview.SetImageBitmap(bitmap);

                }
            }
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
            catch (ActivityNotFoundException ex)
            {

            }
        }
        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {

        }
        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            for (int i = 0; i < adapter.Count; i++)
                if ((word_card.Text == adapter.cards[i].Word) && (translate_card.Text == adapter.cards[i].Translate))
                    create_card = false;

            if (create_card == true)
            {
                Create_card();
            }
            else
            {
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
            else
            {
                filename = null;
                bitmap = null;
            }
            cards.Add(new Card(null, word_card.Text, translate_card.Text, filename, bitmap));
            bd.items_card_title.Add(word_card.Text);
            bd.items_card_translate.Add(translate_card.Text);
            new_card = new List<Card>();
            new_card.Add(new Card(null,word_card.Text, translate_card.Text, filename, bitmap));
            bd.Add_deck_cards(Intent.GetStringExtra("id_deck"), new_card);
            bd.Cards_list(Intent.GetStringExtra("id_deck"));
            cards = new List<Card>();
            for (int i = 0; i < bd.items_card_title.Count; i++)
            {
                cards.Add(new Card(bd.items_card_id[i], bd.items_card_title[i], bd.items_card_translate[i], bd.items_card_image[i], bd.bitmap[i]));
            }
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);        
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
                cards.Add(new Card(bd.items_card_id[i], bd.items_card_title[i], bd.items_card_translate[i], bd.items_card_image[i], bd.bitmap[i]));
            }
            adapter = new CustomAdapter(this, Resource.Layout.Custom_layout, cards);
            adapter2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_title);
            adapter3 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_card_translate);
            list_card.Adapter = adapter;
        }
    }
}