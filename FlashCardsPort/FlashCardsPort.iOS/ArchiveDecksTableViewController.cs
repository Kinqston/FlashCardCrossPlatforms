using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.IO;

namespace FlashCardsPort.iOS
{
    public partial class ArchiveDecksTableViewController : UITableViewController
    {
		private string pathToDatabase;
		private List<DeckLocal> decks;

        public ArchiveDecksTableViewController (IntPtr handle) : base (handle)
        {
			decks = new List<DeckLocal>();
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");
			DismissViewController(true, null);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			decks = new List<DeckLocal>();

			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				var query = connection.Table<DeckLocal>();

				foreach (DeckLocal deck in query)
 				{
					if (deck.acrive_deck != 0)
					{
						decks.Add(deck);
					}
				}
			}
			if (decks.Count == 0)
			{
				var ac = UIAlertController.Create("Архив пуст!", "Пройдите обучение, после можете вернуть есть хотите повторить выученные слова.", UIAlertControllerStyle.Alert);
				//ac.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (action) => this.DismissViewController(true, null)));
				ac.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (UIAlertAction obj) =>
				{
					DismissViewController(true, null);
				}));
				PresentViewController(ac, true, null);
			}
			TableView.ReloadData();	
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return decks.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell("archiveDeckCell");
			var data = decks[indexPath.Row];
			cell.TextLabel.Text = data.title;
			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var deckText = GetCell(tableView, indexPath).TextLabel.Text;
			tableView.DeselectRow(indexPath, true);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			var indexPath = TableView.IndexPathForSelectedRow;
			var tvc = segue.DestinationViewController as ArchiveCardsTableViewController;
			tvc.Name_Deck = decks[indexPath.Row].title;
			tvc.Id_deck = decks[indexPath.Row].id;
		}
	}
}