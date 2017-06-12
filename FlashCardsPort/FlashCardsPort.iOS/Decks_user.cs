using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.IO;

namespace FlashCardsPort.iOS
{
	public partial class Decks_user : UIViewController
	{
		int edit_deck_index;
		private string pathToDatabase;
		private List<Decks_item> decks;
		/*private List<Decks_item> decks = new List<Decks_item>
			{
				new Decks_item{
					Title = "f",
					Id = "1",
					Cost = "0"
				},
				new Decks_item{
					Title = "fol",
					Id = "2",
					Cost = "10"
				}
			};*/
		public Decks_user(IntPtr handle) : base(handle)
		{

		}

		void LongPressMethod(UILongPressGestureRecognizer gestureRecognizer)
		{
			if (gestureRecognizer.State == UIGestureRecognizerState.Began)
			{
				var p = gestureRecognizer.LocationInView(DecksTableView);
				var indexPath = DecksTableView.IndexPathForRowAtPoint(p);
				var selectCategory = new UIActionSheet(null, null, "Отмена", "Удалить", "Редактировать");
				selectCategory.Clicked += delegate (object a, UIButtonEventArgs b)
				{
					if (b.ButtonIndex.ToString() == "0")
					{
						Console.WriteLine("Удалить");
						using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
						{
							var query = connection.Table<CardLocal>();
							connection.Delete(new DeckLocal()
							{ id = Convert.ToInt32(decks[indexPath.Row].Id) });
							foreach (CardLocal card in query)
							{
								if (card.id_deck == Convert.ToInt32(decks[indexPath.Row].Id))
								{
									connection.Delete(new CardLocal()
									{ id_deck = Convert.ToInt32(decks[indexPath.Row].Id)});
								}
							}
						}
						decks.RemoveAt(indexPath.Row);
						DecksTableView.ReloadData();
					}
					if (b.ButtonIndex.ToString() == "1")
					{
						Console.WriteLine("Редактировать");
						var other = Storyboard.InstantiateViewController("ViewCreateDeck_user") as ViewCreateDeck_user;
						other.title_deck = decks[indexPath.Row].Title;
						other.id_deck = decks[indexPath.Row].Id;
						edit_deck_index = indexPath.Row;
						NavigationController.PushViewController(other, true);
					}
					if (b.ButtonIndex.ToString() == "2")
					{
						Console.WriteLine("Отмена");
					}
				};
				selectCategory.ShowInView(this.View);
				DecksTableView.ReloadData();
			}
		}


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");
			decks = new List<Decks_item>();
			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				var query = connection.Table<DeckLocal>();
				foreach (DeckLocal deck in query)
				{
					decks.Add(new Decks_item { Id = deck.id.ToString(), Title = deck.title});
				}
			}
			DecksTableView.Source = new DecksTVS(decks);
			//DecksTableView.Source = new DecksTVS(decks);
			////DecksTableView.Source = new DecksTVSNEW(decks);
			//DecksTableView.Delegate = new DecksDelegate(bd.di);
			//DecksTableView.Delegate = new DecksDelegate(decks);
			var longpress = new UILongPressGestureRecognizer(LongPressMethod);
			DecksTableView.AddGestureRecognizer(longpress);
			//UITableView _table;
			//BaseData bd = new BaseData();
			//bd.Decks_list();
			//string[] data = new string[] {"Ez","Lemon","Pz"};
			//_table = new UITableView
			//{
			//	Frame = new CoreGraphics.CGRect(0, 0, View.Bounds.Width, View.Bounds.Height),
			//	Source = new TableSource(data)                        
			//};
			//View.Add(_table);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			if (segue.Identifier == "Cards")
			{
				var indexPath = DecksTableView.IndexPathForSelectedRow;
				DecksTableView.DeselectRow(indexPath, true);
				var CardsController = segue.DestinationViewController as Cards_user;
				CardsController.Title_deck = decks[indexPath.Row].Title;
				CardsController.Id_deck = decks[indexPath.Row].Id;
			}
			/*if (segue.Identifier == "Add_deck")
			{
				var indexPath = DecksTableView.IndexPathForSelectedRow;
				DecksTableView.DeselectRow(indexPath, true);
				var CreateDeck = segue.DestinationViewController as ViewCreateDeck;
				CreateDeck.id_deck = bd.di[indexPath.Row].Id;
			}*/
			//CardsController.Title_deck = decks[indexPath.Row].Title;
		}
		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		[Action("ViewDecks:")]
		public void ViewDecks(UIStoryboardSegue segue)
		{
			var svc = (ViewCreateCard_user)segue.SourceViewController;
			var title = svc.title_deck;
			var id_deck = svc.id_deck;
			if (svc.Flag == "yes")
			{
				decks.RemoveAt(edit_deck_index);
			}
			decks.Add(new Decks_item { Title = title, Id = id_deck });
			DecksTableView.ReloadData();
		}
		partial void UIBarButtonItem897_Activated(UIBarButtonItem sender)
		{
			//var alert = UIAlertController.Create("Создать колоду", "Введите название колоды и цену", UIAlertControllerStyle.Alert);
			//alert.
			UIAlertView alert = new UIAlertView("Add deck", "title and cost", null, "ok", null);
			alert.AlertViewStyle = UIAlertViewStyle.LoginAndPasswordInput;
			alert.Add(new UIPickerView());
			alert.Show();
		}
	}
}