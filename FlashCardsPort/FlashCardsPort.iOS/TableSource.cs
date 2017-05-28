using System;
using Foundation;
using UIKit;
namespace FlashCardsPort.iOS
{
	public partial class TableSource : UITableViewSource
	{
		string[] tableitems;
		string cellIdentifer = "TableCell";
		public TableSource(string[] items)
		{
			tableitems = items;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifer);
			if (cell == null)
				cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifer);
			cell.TextLabel.Text = tableitems[indexPath.Row];
			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return tableitems.Length;
		}
	}
}
