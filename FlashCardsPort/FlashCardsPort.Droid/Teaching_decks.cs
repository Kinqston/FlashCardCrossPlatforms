
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
    [Activity(Label = "Teaching_decks")]



    public class Teaching_decks : Activity
    {
        ListView teachingDecksListView;
        //List<string> decks;
        List<string> decks_title;
		List<int> decks_id;

		private string pathToDatabase;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.teacjing_decks);
            Title = "Выберите колоду";
            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayShowHomeEnabled(false);
            actionBar.SetDisplayHomeAsUpEnabled(true);
            decks_title = new List<string>();
            decks_id = new List<int>();
            teachingDecksListView = FindViewById<ListView>(Resource.Id.TeachingDecksListView);
            //decks = new List<Dec>();

            //decks.AddRange(new string[] { "wdaw", "123", "dsawdaw", "1231232131321321" });
			var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");

			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				var query = connection.Table<DeckLocal>();

				foreach (DeckLocal deck in query)
				{
                    if (deck.acrive_deck == 0){
						decks_title.Add(deck.title);
						decks_id.Add(deck.id);    
                    }

					//TableView.ReloadData();
				}
            }
            if (decks_id.Count == 0){
				AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
				alertDialog.SetTitle("Нет колод!");
				alertDialog.SetMessage("Создайте новую колоду, загрузите с магазина или разархивируйте из выученных уже.");
				alertDialog.SetNeutralButton("OK", delegate
				{
                    var intent = new Intent(this, typeof(Main_user));
					StartActivity(intent);
					alertDialog.Dispose();
				});
				alertDialog.Show();
            }
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, decks_title);
            teachingDecksListView.Adapter = adapter;
            teachingDecksListView.ItemClick += TeachingDecksListView_Click;
        }


        void TeachingDecksListView_Click(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(Teaching));
            intent.PutExtra("deck_title", decks_title[e.Position]);
            intent.PutExtra("deck_id", decks_id[e.Position]);
            StartActivity(intent);
           // Console.WriteLine("Нажато" + e.Position + "id = " + decks_id[e.Position]);
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
