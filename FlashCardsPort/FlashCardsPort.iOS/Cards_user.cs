using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using Media.Plugin.Abstractions;

namespace FlashCardsPort.iOS
{
	public partial class Cards_user : UIViewController
	{
		public string Title_deck, Id_deck;
		int edit_card_index;
		private string pathToDatabase;
		private List<Cards_item> cards = new List<Cards_item>();
		private CardLocal cards_copy;
		/*private List<Cards_item> cards = new List<Cards_item>
			{
			new Cards_item{
					Word = "fo",
					Translate = "ffff"
				},
			new Cards_item{
					Word = "fol",
					Translate = "ezzz"
				}
			};*/
		public Cards_user(IntPtr handle) : base(handle)
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
						//bd.Delete_card(cards[indexPath.Row].Id_deck, cards[indexPath.Row].Word);
						using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
						{
							var query = connection.Table<CardLocal>();
							connection.Delete(new CardLocal()
							{id = Convert.ToInt32(cards[indexPath.Row].Id)});
						}
						cards.RemoveAt(indexPath.Row);
						CardsTableView.ReloadData();
					}
					if (b.ButtonIndex.ToString() == "1")
					{
						Console.WriteLine("Редактировать");
						var other = Storyboard.InstantiateViewController("ViewAddCard_user") as ViewAddCard_user;
						other.word = cards[indexPath.Row].Word;
						other.translate = cards[indexPath.Row].Translate;
						other.image_edit = cards[indexPath.Row].Image;
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
			var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			pathToDatabase = System.IO.Path.Combine(documentsFolder, "FlashCards_Database.db");
			Title = Title_deck;
			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				var query = connection.Table<CardLocal>();
				foreach (CardLocal card in query)
				{
					if (card.id_deck == Convert.ToInt32(Id_deck))
					{
						cards.Add(new Cards_item {Id = card.id, Word = card.word, Translate = card.translate, Id_deck = card.id_deck.ToString(), Image = card.image});
					}
					//TableView.ReloadData();
				}
			}
			//bd.Cards_list(Id_deck, Title_deck);
			//cards = bd.ci;
			CardsTableView.Source = new CardsTVS_user(cards);
			//CardsTableView.Delegate = new CardsDelegate(cards);
			var longpress = new UILongPressGestureRecognizer(LongPressMethod);
			CardsTableView.AddGestureRecognizer(longpress);
			//CardsTableView.Source = new CardsTVS(bd.ci);
			//CardsTableView.Delegate = new CardsDelegate(bd.ci);
		}
		[Action("UnwindCard:")]
		public void UnwindCard(UIStoryboardSegue segue)
		{
			var svc = (ViewAddCard_user)segue.SourceViewController;
			var word = svc.word;
			var translate = svc.translate;
			var filename = svc.filename;
			if ((word != null) && (translate != null))
				Сreate_card(svc.flag, word, translate, filename);

		}
		public void Alert(string flag, string word, string translate)
		{
			var textInputAlertController = UIAlertController.Create("Повторная карточка", "Такая карточка уже существует, создать еще одну?", UIAlertControllerStyle.Alert);
			//Add Actions
			var cancelAction = UIAlertAction.Create("Отмена", UIAlertActionStyle.Cancel, alertAction => Console.WriteLine("Cancel was Pressed"));
			//var okayAction = UIAlertAction.Create("Ок", UIAlertActionStyle.Default, alertAction => Сreate_card(flag, word, translate));
			textInputAlertController.AddAction(cancelAction);
			//textInputAlertController.AddAction(okayAction);
			//Present Alert
			PresentViewController(textInputAlertController, true, null);
		}
		public void Сreate_card(string flag, string word, string translate, string filename)
		{
			if (flag == "edit")
			{
				//bd.Update_cards(Id_deck, cards[edit_card_index].Word, word, cards[edit_card_index].Translate, translate, filename);
				cards_copy = new CardLocal();
				using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
				{
					cards_copy = connection.Get<CardLocal>(cards[edit_card_index].Id);
					if (filename == " ")
					{
						connection.Update(new CardLocal() { id = cards[edit_card_index].Id, id_deck = Convert.ToInt32(Id_deck), word = word, translate = translate, image = null, archive_card = cards_copy.archive_card, count_repeat = cards_copy.count_repeat });
					}
					else
					{
						connection.Update(new CardLocal() { id = cards[edit_card_index].Id, id_deck = Convert.ToInt32(Id_deck), word = word, translate = translate, image = filename, archive_card = cards_copy.archive_card, count_repeat = cards_copy.count_repeat });
					}
				}
				cards.RemoveAt(edit_card_index);
			}
			else
			{
				using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
				{
					if (filename == " ")
					{
						connection.Insert(new CardLocal() { id_deck = Convert.ToInt32(Id_deck), word = word, translate = translate, image = null, archive_card = 0, count_repeat = 0 });
					}
					else
					{
						connection.Insert(new CardLocal() { id_deck = Convert.ToInt32(Id_deck), word = word, translate = translate, image = filename, archive_card = 0, count_repeat = 0 });;
					}
				}

			}
			if (filename == " ")
			{
				cards.Add(new Cards_item { Word = word, Translate = translate, Title_deck = Title_deck, Id_deck = Id_deck, Image = null });
			}
			else
			{
				cards.Add(new Cards_item { Word = word, Translate = translate, Title_deck = Title_deck, Id_deck = Id_deck, Image = filename });
			}
			CardsTableView.ReloadData();
		}

		partial void UIBarButtonItem2085_Activated(UIBarButtonItem sender)
		{
			var other = Storyboard.InstantiateViewController("ViewAddCard_user") as ViewAddCard_user;
			other.cards = cards;
			NavigationController.PushViewController(other, true);
		}
	}
}
