using Foundation;
using System;
using UIKit;
using System.IO;
using System.Collections.Generic;

namespace FlashCardsPort.iOS
{
	
    public partial class MainViewController : UIViewController
    {
		private string pathToDatabase;
		private int idProperty;
        public MainViewController (IntPtr handle) : base (handle)
        {

        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");
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
			NavigationItem.BackBarButtonItem = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, null, null);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
		}

		partial void TeachingButton_TouchUpInside(UIButton sender)
		{
			
		}


			
	}
}