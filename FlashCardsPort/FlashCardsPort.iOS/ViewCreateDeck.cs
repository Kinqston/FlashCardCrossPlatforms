using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace FlashCardsPort.iOS
{
    public partial class ViewCreateDeck : UIViewController
    {
		public string title_deck;
		BaseData bd = new BaseData();
		public string cost_deck, id_deck;
		public List<Cards_item> cards = new List<Cards_item>();
        public ViewCreateDeck (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title_deck.Text = title_deck;
			Cost_deck.Text = cost_deck;
			if (id_deck != null)
			{
				bd.Cards_list(id_deck, title_deck);
				cards = bd.ci;
			}
			/*Console.WriteLine(id_deck);
			if (id_deck != null) 
			{
				cards.Add(new Cards_item { Word = "bear", Translate = "med", Title_deck = "fol", Id_deck="1"});
				cards.Add(new Cards_item { Word = "be", Translate = "m", Title_deck = "fol", Id_deck="1"});
			}*/

		}
		public override void PrepareForSegue(UIStoryboardSegue segue, Foundation.NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			var CreateCardsController = segue.DestinationViewController as ViewCreateCard;
			CreateCardsController.title_deck = Title_deck.Text;
			CreateCardsController.cost_deck = Cost_deck.Text;
			CreateCardsController.cards = cards;
			CreateCardsController.id_deck = id_deck;
		}
    }
}