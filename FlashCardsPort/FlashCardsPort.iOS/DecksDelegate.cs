using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace FlashCardsPort.iOS
{
	class DecksDelegate : UITableViewDelegate
	{
	List<Decks_item> decks;
		UIImage image = UIImage.FromBundle("image");
		public DecksDelegate(List<Decks_item> decks)
		{
			this.decks = decks;
		}
		public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
		{
			var action = UITableViewRowAction.Create(UITableViewRowActionStyle.Default, "Удалить", (arg1, arg2) => {
				var cell = (DeckCell) tableView.DequeueReusableCell("cell_id", indexPath);
				var deck = decks[indexPath.Row];
				Delete(deck.Id);
				decks.RemoveAt(indexPath.Row);
				tableView.ReloadData();
				//tableView.DeleteRows(1, UITableViewRowAnimation.None);

				//tableView.DeleteRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.None);
			});
			var action2 = UITableViewRowAction.Create(UITableViewRowActionStyle.Normal, "Редактировать", (arg1, arg2) =>
			{
				var cell = (DeckCell)tableView.DequeueReusableCell("cell_id", indexPath);
				UIStoryboardSegue segue;
				tableView.ReloadData();
				});
			//action2.BackgroundColor = UIColor.FromPatternImage();

			//action2.BackgroundColor = UIColor.FromPatternImage(image);
			return new UITableViewRowAction[] { action, action2 };
		}


		void Delete(string id)
		{
			BaseData bd = new BaseData();
			bd.delete_item_list(id);
		}
}
}