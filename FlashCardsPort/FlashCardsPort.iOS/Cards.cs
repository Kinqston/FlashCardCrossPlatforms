using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace FlashCardsPort.iOS
{
	public partial class Cards : UIViewController
	{
		public string Title_deck, Id_deck;
		BaseData bd = new BaseData();
		private List<Cards_item> cards = new List<Cards_item>
			{
			new Cards_item{
					Word = "fo",
					Translate = "ffff"
				},
			new Cards_item{
					Word = "fol",
					Translate = "ezzz"
				}
			};
		public Cards(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = Title_deck;
			bd.Cards_list(Id_deck, Title_deck);
			CardsTableView.Source = new CardsTVS(bd.ci);
			CardsTableView.Delegate = new CardsDelegate(bd.ci);

			//CardsTableView.Source = new CardsTVS(bd.ci);
			//CardsTableView.Delegate = new CardsDelegate(bd.ci);
		}
	}
}
