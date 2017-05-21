using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace FlashCardsPort.iOS
{
	public partial class Cards : UIViewController
	{
		public string Title_deck, Id_deck;
		BaseData bd = new BaseData();
		int edit_card_index;
		private List<Cards_item> cards = new List<Cards_item>
			{
			new Cards_item{
					Word = "fo",
					Translate = "ffff"
				},
			new Cards_item{
					Word = "fol",
					Translate = "ezzz"
				}
			};
		public Cards(IntPtr handle) : base(handle)
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
						bd.Delete_card(cards[indexPath.Row].Id_deck, cards[indexPath.Row].Word);
						cards.RemoveAt(indexPath.Row);
						CardsTableView.ReloadData();
					}
					if (b.ButtonIndex.ToString() == "1")
					{
						Console.WriteLine("Редактировать");
						var other = Storyboard.InstantiateViewController("ViewAddCard") as ViewAddCard;
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
			Title = Title_deck;
			bd.Cards_list(Id_deck, Title_deck);
			cards = bd.ci;
			CardsTableView.Source = new CardsTVS(cards);
			CardsTableView.Delegate = new CardsDelegate(cards);
			var longpress = new UILongPressGestureRecognizer(LongPressMethod);
			CardsTableView.AddGestureRecognizer(longpress);
			//CardsTableView.Source = new CardsTVS(bd.ci);
			//CardsTableView.Delegate = new CardsDelegate(bd.ci);
		}
		[Action("UnwindCard:")]
		public void UnwindCard(UIStoryboardSegue segue)
		{
			var svc = (ViewAddCard)segue.SourceViewController;
			var word = svc.word;
			var translate = svc.translate;
			if (svc.flag == "edit")
			{
				bd.Update_cards(Id_deck, cards[edit_card_index].Word, word, cards[edit_card_index].Translate, translate);
				cards.RemoveAt(edit_card_index);
			}
			else
			{
				bd.Add_card(Id_deck, word, translate);
			}
			cards.Add(new Cards_item { Word = word, Translate = translate, Title_deck = Title_deck, Id_deck = Id_deck});
			CardsTableView.ReloadData();
		}
	}
}
