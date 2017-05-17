using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace FlashCardsPort.iOS
{
	class CardsTVS : UITableViewSource
	{
		List<Cards_item> cards;

		public CardsTVS(List<Cards_item> cards)
		{
			this.cards = cards;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (CardCell)tableView.DequeueReusableCell("cell_id", indexPath);
			var card = cards[indexPath.Row];
			cell.UpdateCell(card);
			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return cards.Count;
		}
	}
}