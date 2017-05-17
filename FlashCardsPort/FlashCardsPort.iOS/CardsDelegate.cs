using System.Collections.Generic;
using Foundation;
using UIKit;

namespace FlashCardsPort.iOS
{
	class CardsDelegate : UITableViewDelegate
	{
		List<Cards_item> cards;

		public CardsDelegate(List<Cards_item> cards)
		{
			this.cards = cards;
		}
		public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
		{
			var action = UITableViewRowAction.Create(UITableViewRowActionStyle.Default, "Удалить", (arg1, arg2) =>{
				var cell = (CardCell)tableView.DequeueReusableCell("cell_id", indexPath);
				var card = cards[indexPath.Row];
				Delete(card.Id_deck,card.Word);
				cards.RemoveAt(indexPath.Row);
				tableView.ReloadData();
		});
		var action2 = UITableViewRowAction.Create(UITableViewRowActionStyle.Normal, "Редактировать", (arg1, arg2) =>{
				var cell = (CardCell)tableView.DequeueReusableCell("cell_id", indexPath);
			tableView.ReloadData();
		});
		//action2.BackgroundColor = UIColor.FromPatternImage(image);
		return new UITableViewRowAction[] { action, action2 };
		}
		void Delete(string id_deck, string word)
		{
			BaseData bd = new BaseData();
			bd.Delete_card(id_deck, word);
		}
	}
}