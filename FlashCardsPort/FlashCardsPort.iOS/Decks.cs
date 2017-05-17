using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace FlashCardsPort.iOS
{
	public partial class Decks : UIViewController
	{
		BaseData bd = new BaseData();
		private List<Decks_item> decks = new List<Decks_item>
			{
				new Decks_item{
					Title = "f"
				},
				new Decks_item{
					Title = "fol"
				}
			};
		public Decks(IntPtr handle) : base(handle)
		{
		}




		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			bd.Decks_list();

			DecksTableView.Source = new DecksTVS(bd.di);
			//DecksTableView.Source = new DecksTVS(bd.di);
			////DecksTableView.Source = new DecksTVSNEW(decks);
			//DecksTableView.Delegate = new DecksDelegate(bd.di);
			DecksTableView.Delegate = new DecksDelegate(bd.di);
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
				var CardsController = segue.DestinationViewController as Cards;
				CardsController.Title_deck = bd.di[indexPath.Row].Title;
				CardsController.Id_deck = bd.di[indexPath.Row].Id;
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
			var svc = (ViewCreateCard)segue.SourceViewController;
			var title = svc.title_deck;
			var cost = svc.cost_deck;
			var id_deck = svc.id_deck;
			bd.di.Add(new Decks_item {Title = title, Cost = cost, Id=id_deck});
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

