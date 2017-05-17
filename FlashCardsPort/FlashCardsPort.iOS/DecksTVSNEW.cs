using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace FlashCardsPort.iOS
{
	class DecksTVSNEW : UITableViewSource
	{
		List<string> decks;

		public DecksTVSNEW(List<string> decks)
		{
			this.decks = decks;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = new UITableViewCell (UITableViewCellStyle.Default, "");
			cell.TextLabel.Text = decks[indexPath.Row];
			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return decks.Count;
		}
	}
}