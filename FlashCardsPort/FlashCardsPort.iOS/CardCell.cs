using Foundation;
using System;
using UIKit;

namespace FlashCardsPort.iOS
{
    public partial class CardCell : UITableViewCell
    {
        public CardCell (IntPtr handle) : base (handle)
        {
        }
		internal void UpdateCell(Cards_item card)
		{
			Word.Text = card.Word;
			Translate.Text = card.Translate;
		}
    }
}