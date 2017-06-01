using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace FlashCardsPort.iOS
{
    public partial class ViewCreateCard : UIViewController
    {
		public string title_deck;
		public string cost_deck, id_deck;
		public string Flag;
		int edit_card_index;
		public List<Cards_item> cards;
		BaseData bd = new BaseData();
        public ViewCreateCard (IntPtr handle) : base (handle)
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
			Title = title_deck;
			CardsTableView.Source = new CardsTVS(cards);
			CardsTableView.Delegate = new CardsDelegate(cards);
			var longpress = new UILongPressGestureRecognizer(LongPressMethod);
			CardsTableView.AddGestureRecognizer(longpress);
		}
		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			if (segue.Identifier == "Create_card")
			{
				var CreateAddController = segue.DestinationViewController as ViewAddCard;
				//CreateAddController.cards = cards;
				CreateAddController.title_deck = title_deck;
				CreateAddController.cards = cards;
			}
			else {
				if (id_deck == null)
				{
					Flag = "none";
					bd.Add_deck(title_deck, cost_deck);
					bd.New_deck_id(title_deck, cost_deck);
					id_deck = bd.new_id_deck;
					bd.Add_deck_cards(bd.new_id_deck, cards);
				}
				else 
				{
					Flag = "yes";
					bd.Update_deck(id_deck, title_deck, cost_deck);
					bd.Delete_all_card(id_deck);
					bd.Add_deck_cards(id_deck, cards);

				}

			}
		}
		[Action("UnwindCard:")]
		public void UnwindCard(UIStoryboardSegue segue)
		{
			var svc = (ViewAddCard)segue.SourceViewController;
			var word = svc.word;
			var translate = svc.translate;
			var filename = svc.filename;
			if (svc.flag == "edit")
			{
				cards.RemoveAt(edit_card_index);
			}
			cards.Add(new Cards_item { Word = word, Translate = translate, Title_deck = title_deck, Image = filename});
			CardsTableView.ReloadData();
		}
	}
}