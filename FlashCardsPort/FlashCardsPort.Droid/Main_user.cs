
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UIKit;
using Foundation;


using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace FlashCardsPort.Droid
{
    [Activity(Label = "Главное меню")]
    public class Main_user : Activity
    {
        Button teaching_button;
        Button decks;
        Button archive;
        Button exit;
        Button user_property;
        Button add;
        Button shop;
        public string id_user = null;
		private string pathToDatabase;
        private int idProperty;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.mian_user);

            var prefs = Application.Context.GetSharedPreferences("FC", FileCreationMode.Private);
            var somePref = prefs.GetString("Id", null);
            id_user = somePref;

            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayShowHomeEnabled(false);

            teaching_button = FindViewById<Button>(Resource.Id.teaching_button);
            //add = FindViewById<Button>(Resource.Id.add);
            archive = FindViewById<Button>(Resource.Id.archive_button);
            user_property = FindViewById<Button>(Resource.Id.property_button);
            decks = FindViewById<Button>(Resource.Id.decks);
            shop = FindViewById<Button>(Resource.Id.shop);
            if (id_user == null)
            {
                shop.Enabled = false;
                shop.SetBackgroundColor(Color.Gray);
            }
            exit = FindViewById<Button>(Resource.Id.exit_button);
            var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			pathToDatabase = System.IO.Path.Combine(documentsFolder, "FlashCards_Database.db");

			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				connection.CreateTable<DeckLocal>();
				connection.CreateTable<CardLocal>();
				connection.CreateTable<PropertyUser>();
				idProperty = new int();
				idProperty = 0;
				var query = connection.Table<PropertyUser>();
				foreach (PropertyUser proper in query) // првоерка, созданы ли настройки для пользователя
				{
					if (proper.id == 1)
					{
						idProperty = 1; // если созданы возвращает true
					}
				}
				if (idProperty == 0) // проверка, нужно ли создать настройки пользователя
				{
					connection.Insert(new PropertyUser() { id = 1, number_of_repetition = 3, side_card = 0 }); //если их нет то создаем
					idProperty = 1;
				}
			}
            teaching_button.Click += teaching_button_Click;
            //add.Click += add_click;
            archive.Click += Archive_Click;
            user_property.Click += User_Property_Click;
            decks.Click += Decks_Click;
            shop.Click += Shop;
            exit.Click += Exit_Click;
        }

        private void Shop(object sender, EventArgs e)
        {
            StartActivity(typeof(Shop_user));
        }

       /* private void add_click(object sender, EventArgs e)
        {
            using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
            {
                connection.Insert(new DeckLocal()
                {
                    id = 1,
                    title = "Животные",
                    acrive_deck = 0
				});
				connection.Insert(new DeckLocal()
				{
					id = 2,
					title = "Растения",
					acrive_deck = 0
				});
				connection.Insert(new DeckLocal()
				{
					id = 3,
					title = "Насекомые",
					acrive_deck = 0
				});

                connection.Insert(new CardLocal()
                {
                    id = 1,
                    id_deck = 1,
                    word = "Lion",
                    translate = "Лев",
                    image = "",
                    count_repeat = 0,
                    archive_card = 0
				});
				connection.Insert(new CardLocal()
				{
					id = 2,
					id_deck = 2,
					word = "Grass",
					translate = "Трава",
					image = "",
					count_repeat = 0,
					archive_card = 0
				});
				connection.Insert(new CardLocal()
				{
					id = 3,
					id_deck = 2,
					word = "Tree",
					translate = "Дерево",
					image = "",
					count_repeat = 0,
					archive_card = 0
				});

				connection.Insert(new CardLocal()
				{
					id = 4,
					id_deck = 3,
					word = "Bug",
					translate = "Жук",
					image = "",
					count_repeat = 0,
					archive_card = 0
				});
				connection.Insert(new CardLocal()
				{
					id = 5,
					id_deck = 1,
					word = "Tiger",
					translate = "Тигр",
					image = "",
					count_repeat = 0,
					archive_card = 0
				});
				connection.Insert(new CardLocal()
				{
					id = 6,
					id_deck = 3,
					word = "Spider",
					translate = "Паук",
					image = "",
					count_repeat = 0,
					archive_card = 0
				});

				connection.Insert(new CardLocal()
				{
					id = 7,
					id_deck = 2,
					word = "Bush",
					translate = "Куст",
					image = "",
					count_repeat = 0,
					archive_card = 0
				});
				connection.Insert(new CardLocal()
				{
					id = 8,
					id_deck = 1,
					word = "Bear",
					translate = "Медведь",
					image = "",
					count_repeat = 0,
					archive_card = 0
				});
				connection.Insert(new CardLocal()
				{
					id = 9,
					id_deck = 1,
					word = "Fox",
					translate = "Лиса",
					image = "",
					count_repeat = 0,
					archive_card = 0
				});
            }     
        } */

        private void teaching_button_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Teaching_decks));
        }

        void Archive_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Archive_decks));
        }

        void User_Property_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Property_user_view));
        }

        void Decks_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(Decks_user));
        }

        void Exit_Click(object sender, EventArgs e)
        {
            var prefs = Application.Context.GetSharedPreferences("FC", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString("Id", null);
            prefEditor.Commit();
            var intent = new Intent(this, typeof(MainActivity));
			StartActivity(intent);
        }
    }
}
