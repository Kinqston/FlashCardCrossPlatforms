using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.IO;

namespace FlashCardsPort.iOS
{
    public partial class ArchiveCardsTableViewController : UITableViewController
    {
		private string pathToDatabase;
		private List<CardLocal> cards;

		public string Name_Deck
		{
			get;
			set;
		}

		public int Id_deck
		{
			get;
			set;
		}


        public ArchiveCardsTableViewController (IntPtr handle) : base (handle)
        {
			cards = new List<CardLocal>();		
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");
			Title = Name_Deck;

			var ac = UIAlertController.Create("","Нажмите на карточку и она вернется в колоду для её изучения.", UIAlertControllerStyle.Alert);
			//ac.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (action) => this.DismissViewController(true, null)));
			ac.AddAction(UIAlertAction.Create("Я понял", UIAlertActionStyle.Default, null));
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			cards = new List<CardLocal>();

			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				var query = connection.Table<CardLocal>();

				foreach (CardLocal card in query)
				{
					if ((card.archive_card == 1) && (card.id_deck == Id_deck))
					cards.Add(card);
					TableView.ReloadData();
				}
			}
		}
		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return cards.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell("archiveCardCell");
			var data = cards[indexPath.Row];

			cell.TextLabel.Text = data.word;
			cell.DetailTextLabel.Text = data.translate;

			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			tableView.DeselectRow(indexPath, true);
			if (cards.Count != 0)
			{
			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
				{
					connection.Update(new CardLocal()
					{
						id = cards[indexPath.Row].id,
						id_deck = cards[indexPath.Row].id_deck,
						word = cards[indexPath.Row].word,
						translate = cards[indexPath.Row].translate,
						archive_card = 0, 
						count_repeat = 0  
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
					cards.RemoveAt(indexPath.Row);
					TableView.ReloadData();
				}	
			}
			else
			{
				var ac = UIAlertController.Create("Архив пуст!", "Пройдите обучение, после можете вернуть есть хотите повторить выученные слова.", UIAlertControllerStyle.Alert);
				//ac.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (action) => this.DismissViewController(true, null)));
				ac.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (UIAlertAction obj) =>
				{
					DismissViewController(true, null);
				}));
				PresentViewController(ac, true, null);	
			}
		}
    }
}