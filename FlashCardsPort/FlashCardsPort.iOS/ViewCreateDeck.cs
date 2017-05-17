using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace FlashCardsPort.iOS
{
    public partial class ViewCreateDeck : UIViewController
    {
		public string title_deck;
		public string cost_deck, id_deck;
		public List<Cards_item> cards = new List<Cards_item>();
        public ViewCreateDeck (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		}
		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			var CreateCardsController = segue.DestinationViewController as ViewCreateCard;
			CreateCardsController.title_deck = Title_deck.Text;
			CreateCardsController.cost_deck = Cost_deck.Text;
			CreateCardsController.cards = cards;
		}
    }
}