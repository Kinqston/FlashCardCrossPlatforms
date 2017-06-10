
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
    [Activity(Label = "Archive_decks")]
    public class Archive_decks : Activity
    {
		private string pathToDatabase;
		ListView acrhive_deck_list_view;
		List<string> decks_title;
		List<int> decks_id;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.archive_decks);
            var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");

            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.SetDisplayHomeAsUpEnabled(true);

            Title = "Архив";
			decks_title = new List<string>();
			decks_id = new List<int>();
            acrhive_deck_list_view = FindViewById<ListView>(Resource.Id.acrhive_deck_list_view);
			
			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				var query = connection.Table<DeckLocal>();

				foreach (DeckLocal deck in query)
				{
					if (deck.acrive_deck != 0)
					{
						decks_title.Add(deck.title);
						decks_id.Add(deck.id);
					}
				}
			}
			if (decks_title.Count == 0)
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
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, decks_title);
            acrhive_deck_list_view.Adapter = adapter;
            acrhive_deck_list_view.ItemClick += Acrhive_Deck_List_View_ItemClick;
            adapter.NotifyDataSetChanged();
		}

        void Acrhive_Deck_List_View_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(Archive_card));
			intent.PutExtra("deck_title", decks_title[e.Position]);
			intent.PutExtra("deck_id", decks_id[e.Position]);
			StartActivity(intent);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    var intent = new Intent(this, typeof(Main_user));
                    StartActivity(intent);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}
