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
using Java.Util;
using Android.Graphics;
using System.Net;
using System.IO;

namespace FlashCardsPort.Droid
{
    class Shop_adapter : BaseAdapter<Deck>
    {
        // private List<string> list;
        //private Context context;
        /*public List<Deck> decks;
		private Context c;
		public Android.Net.Uri uri;
		private int resourse;
		private LayoutInflater inflater;*/
        private string filename;
        static BaseData bd = new BaseData(); 
        private List<Deck> list;
		private Context context;
        private List<Card> cards;
        private string pathToDatabase;
        public int flag = 0;


		string ftpHost = "ftp.billions-consult.ru";
		//  string ftpUser = "u688865617.kinkston";
		//  string ftpPassword = "kinkston";
		string ftpUser = "graversp_fc";
		string ftpPassword = "{*545S7e";
		
		bool Camera_image = false;
		
		string ftpfullpath;
		string ImagePath;
		Bitmap bitmap;
		public string title;
		public string cost;
		public Android.Net.Uri uri;
		ImageView imageview;
		EditText word_card, translate_card;
		Intent Camera_intent, Galery_intent, Crop_intent;
		const int RequestPermissionCode = 1;

		private List<String> cards_word_create_deck;
		private List<String> cards_translate_create_deck;
		private List<Android.Net.Uri> cards_image_create_deck;
		private Bitmap image_bitmap, cards_image_create_deck_edit;
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

		public Dialog dialog, dialog1, dialog3;
		public Bitmap cards_image_bitmap;
		bool create_card = true;
		int new_card_id = 0;
		int action_card;
	
		private List<DeckLocal> decks;
		private DeckLocal decks_copy;
		private CardLocal card_copy;
		Button Cancel_create_card, Save_create_card;



        public Shop_adapter(Context context, List<Deck> list)
		{
			this.list = list;
			this.context = context;
		}
		public override int Count
		{
			get
			{
				return list.Count;
			}
		}

        public override Deck this[int position]
		{
			get
			{
				return list[position];
			}
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
            View view = convertView;
			if (view == null)
			{
                view = LayoutInflater.From(context).Inflate(Resource.Layout.shop_user, null, false);
                TextView deck_title = view.FindViewById<TextView>(Resource.Id.shop_deck_title);
                ImageButton donwload_deck = view.FindViewById<ImageButton>(Resource.Id.shop_icon);
                if (list[position].free == true)
                {
				    donwload_deck.SetImageResource(Resource.Drawable.ic_file_download_black_24dp);
                }
                else
                {
                    donwload_deck.SetImageResource(Resource.Drawable.ic_monetization_on_black_24dp);    
                }

                deck_title.Text = list[position].title;

                donwload_deck.Click += (sender, e) =>
                {
                    if (list[position].free == false)
                    {
                        Toast.MakeText(context, "Функция недоступна", ToastLength.Long).Show();
                    }
                    else
                    {
                        flag = 0;
                        cards = new List<Card>();
                        bd.Cards_list(Convert.ToString(list[position].id));
                        for (int i = 0; i < bd.items_card_title.Count; i++)
                        {
                            cards.Add(new Card(bd.items_card_id[i], bd.items_card_title[i], bd.items_card_translate[i], bd.items_card_image[i], bd.bitmap[i]));
                        }
                        var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                        pathToDatabase = System.IO.Path.Combine(documentsFolder, "FlashCards_Database.db");

                        using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
                        {
                            var query = connection.Table<DeckLocal>();

                            foreach (DeckLocal deck in query)
                            {
                                if (deck.title == list[position].title)
                                {
                                    flag = 1;
                                    for (int i = 0; i < cards.Count; i++)
                                    {
                                        image_bitmap = GetImageBitmapFromUrl("http://graversp.beget.tech/" + cards[i].image);
                                        SavePicture(image_bitmap);

                                        connection.Insert(new CardLocal()
                                        {
                                            id_deck = deck.id,
                                            word = cards[i].word,
                                            translate = cards[i].translate,
                                            image = ImagePath,
                                            archive_card = 0,
                                            count_repeat = 0
                                        });
                                    }
                                }
                            }
                            if (flag == 0)
                            {
                                connection.Insert(new DeckLocal()
                                {
                                    title = list[position].title,
                                    acrive_deck = 0
                                });
                                foreach (DeckLocal deck in query)
                                {
                                    if (deck.title == list[position].title)
                                    {
                                        for (int i = 0; i < cards.Count; i++)
                                        {
                                            image_bitmap = GetImageBitmapFromUrl("http://graversp.beget.tech/" + cards[i].image);
                                            SavePicture(image_bitmap);

                                            connection.Insert(new CardLocal()
                                            {
                                                id_deck = deck.id,
                                                word = cards[i].word,
                                                translate = cards[i].translate,
                                                image = ImagePath,
                                                archive_card = 0,
                                                count_repeat = 0
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                };	
			}
			return view;
		}

		public override long GetItemId(int position)
		{
			return position;
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
        public void SavePicture(Bitmap bitmap)
		{
			filename = "file_" + Guid.NewGuid().ToString() + ".jpg";
            Java.IO.File folder = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "FlashCard_image");
			if (!folder.Exists())
			{
				folder.Mkdirs();
			}
			var Path = folder.Path;
			var filePath = System.IO.Path.Combine(Path, filename);
			ImagePath = filePath;
			var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create);
			bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
			stream.Close();
		}
    }
}
