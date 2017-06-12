using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.IO;

namespace FlashCardsPort.iOS
{
	public partial class ViewCreateDeck_user : UIViewController
	{
		public string title_deck;
		private string pathToDatabase;
		public string id_deck;
		public List<Cards_item> cards = new List<Cards_item>();
		public List<Cards_item> cards_orig = new List<Cards_item>();
		public ViewCreateDeck_user(IntPtr handle) : base (handle)
        {
		}
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title_deck.Text = title_deck;
			if (id_deck != null)
			{
				var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");
				using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
				{
					var query = connection.Table<CardLocal>();
					foreach (CardLocal card in query)
					{
						if (card.id_deck == Convert.ToInt32(id_deck))
						{
							cards.Add(new Cards_item {Id = card.id, Word = card.word, Translate = card.translate, Id_deck = card.id_deck.ToString(), Image = card.image});
							cards_orig.Add(new Cards_item {Id = card.id, Word = card.word, Translate = card.translate, Id_deck = card.id_deck.ToString(), Image = card.image});
							//cards.Add(new Cards_item(card.id.ToString(), card.word, card.translate, card.image, null));
							//cards_orig.Add(new Card(card.id.ToString(), card.word, card.translate, card.image, null));
						}
					}
				}
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
			var CreateCardsController = segue.DestinationViewController as ViewCreateCard_user;
			CreateCardsController.title_deck = Title_deck.Text;
			CreateCardsController.cards = cards;
			CreateCardsController.cards_orig = cards_orig;
			CreateCardsController.id_deck = id_deck;
		}
	}
}