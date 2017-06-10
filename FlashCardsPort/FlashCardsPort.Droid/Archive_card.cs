
using System;
using System.Collections.Generic;
using System.IO;
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
    [Activity(Label = "Archive_card")]
    public class Archive_card : Activity
    {
        ListView archiveCardListView;
        ListView archiveDeckListView;
        List<string> list;
		private int Id_deck;
		private string Name_Deck;
        private string pathToDatabase;
		private List<CardLocal> cards;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.archive_cards);
			var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");
            Id_deck = Intent.GetIntExtra("deck_id", 0);
            Name_Deck = Intent.GetStringExtra("deck_title");
            Title = Name_Deck;
            list = new List<string>();
            list.AddRange(new string[]{"asd", "dasd", "asdasd", "asdd", "asdasd", "d", "ddd", "aa", "ad"});
            archiveCardListView = FindViewById<ListView>(Resource.Id.archiveCardListView);
 
            cards = new List<CardLocal>();
			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				var query = connection.Table<CardLocal>();

				foreach (CardLocal card in query)
				{
					if ((card.archive_card == 1) && (card.id_deck == Id_deck))
						cards.Add(card);
				}
			}

            ArchiveCardAdapter adapter = new ArchiveCardAdapter(this, cards);
            archiveCardListView.Adapter = adapter;
            archiveCardListView.ItemClick += ArchiveCardListView_ItemClick;
            adapter.NotifyDataSetChanged();


        }

        void ArchiveCardListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {

			if (cards.Count != 0)
			{
				using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
				{
					connection.Update(new CardLocal()
					{
                        id = cards[e.Position].id,
						id_deck = cards[e.Position].id_deck,
						word = cards[e.Position].word,
						translate = cards[e.Position].translate,
						archive_card = 0,
						count_repeat = 0,
                        image = cards[e.Position].image
					});
					if (cards.Count == 1)
					{
						connection.Update(new DeckLocal()
						{
							id = Id_deck,
							title = Name_Deck,
							acrive_deck = 0
						});
					}
					cards.RemoveAt(e.Position);
					ArchiveCardAdapter adapter = new ArchiveCardAdapter(this, cards);
					archiveCardListView.Adapter = adapter;
				}
			}
			else
			{
				AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
				alertDialog.SetTitle("Архив пуст!");
				alertDialog.SetMessage("Пройдите обучение, после можете вернуть есть хотите повторить выученные слова.");
				alertDialog.SetNeutralButton("OK", delegate
				{
					alertDialog.Dispose();
				});
				alertDialog.Show();
			}
        }
    }
}
