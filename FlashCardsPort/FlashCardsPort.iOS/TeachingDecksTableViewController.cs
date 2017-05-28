using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.IO;

namespace FlashCardsPort.iOS
{
	public partial class TeachingDecksTableViewController : UITableViewController
	{
		private string pathToDatabase;
		private List<DeckLocal> decks;

		public TeachingDecksTableViewController(IntPtr handle) : base(handle)
		{
			decks = new List<DeckLocal>();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NavigationItem.BackBarButtonItem = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, null, null);;
			var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");
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
					decks.Add(deck);
					TableView.ReloadData();
				}
			}
		}

		public override nint RowsInSection(UITableView tableView, nint section)
		{
			return decks.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell("teachingDeck");
			var data = decks[indexPath.Row];
			cell.TextLabel.Text = data.title;
			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var deckText = GetCell(tableView, indexPath).TextLabel.Text;
			//	new UIAlertView("Название:", deckText, null, "123!", null).Show();
			tableView.DeselectRow(indexPath, true);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			var indexPath = TableView.IndexPathForSelectedRow;
			var tvc = segue.DestinationViewController as TeachingViewController;
			tvc.Name_Deck = decks[indexPath.Row].title;
			tvc.Id_deck = decks[indexPath.Row].id;
		}
	}
}


		/*	[Action("BackToTeachingDeck:")]
			public void BackToTeachingDeck(UIStoryboardSegue segue)
			{
				var svc = (TeachingViewController)segue.SourceViewController;
				var fuck = svc.Name_Deck;

				Console.WriteLine(fuck);
			} */

