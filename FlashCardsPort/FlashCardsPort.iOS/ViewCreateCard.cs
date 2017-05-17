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
		public List<Cards_item> cards;
		BaseData bd = new BaseData();
        public ViewCreateCard (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = title_deck;
			CardsTableView.Source = new CardsTVS(cards);
			CardsTableView.Delegate = new CardsDelegate(cards);
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
				bd.Add_deck(title_deck,cost_deck);
				bd.New_deck_id(title_deck,cost_deck);
				id_deck = bd.new_id_deck;
				bd.Add_deck_cards(bd.new_id_deck, cards);
			}
		}
		[Action("UnwindCard:")]
		public void UnwindCard(UIStoryboardSegue segue)
		{
			var svc = (ViewAddCard)segue.SourceViewController;
			var word = svc.word;
			var translate = svc.translate;
			cards.Add(new Cards_item {Word = word, Translate = translate, Title_deck = title_deck});
			CardsTableView.ReloadData();
		}
	}
}