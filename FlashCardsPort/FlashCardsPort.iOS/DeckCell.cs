using Foundation;
using System;
using UIKit;

namespace FlashCardsPort.iOS
{
	public partial class DeckCell : UITableViewCell
    {
        public DeckCell (IntPtr handle) : base (handle)
        {
        }

		internal void UpdateCell(Decks_item deck)
		{
			DeckTitle.Text = deck.Title;
		}
	}
}