using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace FlashCardsPort.iOS
{
	class DecksTVS : UITableViewSource
	{
		List<Decks_item> decks;

		public DecksTVS(List<Decks_item> decks)
		{
			this.decks = decks;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (DeckCell) tableView.DequeueReusableCell("cell_id", indexPath);
			var deck = decks[indexPath.Row];
			cell.UpdateCell(deck);
			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return decks.Count;
		}
	}
}