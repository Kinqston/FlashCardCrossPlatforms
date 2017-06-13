using System;
using System.Collections.Generic;
using System.IO;
using Android.Content;
using Android.Views;
using Android.Widget;
using SQLite;

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
        static BaseData bd = new BaseData(); 
        private List<Deck> list;
		private Context context;
        private List<Card> cards;
        private string pathToDatabase;
        private int a;
        public int flag = 0;

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
               
                deck_title.Text = list[position].title;

                donwload_deck.Click += (sender, e) => 
                {
                    flag = 0;
					cards = new List<Card>();
                    bd.Cards_list(Convert.ToString(list[position].id));
					for (int i = 0; i < bd.items_card_title.Count; i++)
					{
						cards.Add(new Card(bd.items_card_id[i], bd.items_card_title[i], bd.items_card_translate[i], bd.items_card_image[i], bd.bitmap[i]));
					}
					var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
					pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");

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
									connection.Insert(new CardLocal()
									{
                                            id_deck = deck.id,
											word = cards[i].word,
											translate = cards[i].translate,
											image = "",
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
                                        connection.Insert(new CardLocal()
                                        {
                                            id_deck = deck.id,
                                            word = cards[i].word,
                                            translate = cards[i].translate,
                                            image = "",
                                            archive_card = 0,
                                            count_repeat = 0
                                        });
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
	}
}
