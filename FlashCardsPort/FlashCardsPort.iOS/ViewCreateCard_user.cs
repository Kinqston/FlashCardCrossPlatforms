using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace FlashCardsPort.iOS
{
	public partial class ViewCreateCard_user : UIViewController
	{
		public string title_deck;
		public string id_deck;
		public string Flag;
		int edit_card_index;
		public List<Cards_item> cards;
		public List<Cards_item> cards_orig;
		private List<DeckLocal> decks;
		private DeckLocal decks_copy;
		private CardLocal card_copy;
		private string pathToDatabase;
		public ViewCreateCard_user(IntPtr handle) : base (handle)
        {
		}

		void LongPressMethod(UILongPressGestureRecognizer gestureRecognizer)
		{
			if (gestureRecognizer.State == UIGestureRecognizerState.Began)
			{
				var p = gestureRecognizer.LocationInView(CardsTableView);
				var indexPath = CardsTableView.IndexPathForRowAtPoint(p);
				var selectCategory = new UIActionSheet(null, null, "Отмена", "Удалить", "Редактировать");
				selectCategory.Clicked += delegate (object a, UIButtonEventArgs b)
				{
					if (b.ButtonIndex.ToString() == "0")
					{
						Console.WriteLine("Удалить");
						cards.RemoveAt(indexPath.Row);
						CardsTableView.ReloadData();
					}
					if (b.ButtonIndex.ToString() == "1")
					{
						Console.WriteLine("Редактировать");
						var other = Storyboard.InstantiateViewController("ViewAddCard_user") as ViewAddCard_user;
						other.word = cards[indexPath.Row].Word;
						other.translate = cards[indexPath.Row].Translate;
						other.flag = "edit";
						edit_card_index = indexPath.Row;
						NavigationController.PushViewController(other, true);
					}
					if (b.ButtonIndex.ToString() == "2")
					{
						Console.WriteLine("Отмена");
					}
				};
				selectCategory.ShowInView(this.View);

			}
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = title_deck;
			var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			pathToDatabase = System.IO.Path.Combine(documentsFolder, "FlashCards_Database.db");
			CardsTableView.Source = new CardsTVS_user(cards);
			//CardsTableView.Delegate = new CardsDelegate(cards);
			var longpress = new UILongPressGestureRecognizer(LongPressMethod);
			CardsTableView.AddGestureRecognizer(longpress);
		}
		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			if (segue.Identifier == "Create_card")
			{
				var CreateAddController = segue.DestinationViewController as ViewAddCard_user;
				//CreateAddController.cards = cards;
				CreateAddController.title_deck = title_deck;
				CreateAddController.cards = cards;
			}
			else
			{
				if (id_deck == null)
				{
					Flag = "none";
					decks = new List<DeckLocal>();
					using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
					{
						var query = connection.Table<DeckLocal>();
						string new_id;
						connection.Insert(new DeckLocal() { title = title_deck, acrive_deck = 0 });
						foreach (DeckLocal deck in query)
						{
							decks.Add(deck);
						}
						foreach (Cards_item card in cards)
						{
							connection.Insert(new CardLocal() { id_deck = decks[decks.Count - 1].id, word = card.Word, translate = card.Translate, image = card.Image, archive_card = 0, count_repeat = 0 });
						}
						id_deck = decks[decks.Count - 1].id.ToString();
					}
					//bd.Add_deck(title_deck, free_deck);
					//bd.New_deck_id(title_deck, free_deck);
					//id_deck = bd.new_id_deck;
					//bd.Add_deck_cards(bd.new_id_deck, cards);
				}
				else
				{
					Flag = "yes";
					decks_copy = new DeckLocal(); 
                    using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
                    {
                        decks_copy = connection.Get<DeckLocal>(Convert.ToInt32(id_deck));
                        connection.Update(new DeckLocal() { id = Convert.ToInt32(id_deck), title = title_deck, acrive_deck = 0});
                        foreach (Cards_item card in cards_orig)
                        {
                            connection.Delete(new CardLocal() { id = Convert.ToInt32(card.Id)});
                        }
                        foreach (Cards_item card in cards)
                        {
                            connection.Insert(new CardLocal() { id = Convert.ToInt32(card.Id), id_deck = Convert.ToInt32(id_deck), word = card.Word, translate = card.Translate, image = card.Image, archive_card = 0, count_repeat = 0});
                        }
                    }
					//bd.Update_deck(id_deck, title_deck, free_deck);
					//bd.Delete_all_card(id_deck);
					//bd.Add_deck_cards(id_deck, cards);

				}

			}
		}
		[Action("UnwindCard:")]
		public void UnwindCard(UIStoryboardSegue segue)
		{
			var svc = (ViewAddCard_user)segue.SourceViewController;
			var word = svc.word;
			var translate = svc.translate;
			var filename = svc.filename;
			if ((word != null) && (translate != null))
			{
				if (svc.flag == "edit")
				{
					cards.RemoveAt(edit_card_index);
				}
				if (filename == " ")
				{
					cards.Add(new Cards_item { Word = word, Translate = translate, Title_deck = title_deck, Image = null });
				}
				else
				{
					cards.Add(new Cards_item { Word = word, Translate = translate, Title_deck = title_deck, Image = filename });
				}
				CardsTableView.ReloadData();
			}
		}
	}
}