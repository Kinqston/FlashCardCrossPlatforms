using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.IO;

namespace FlashCardsPort.iOS
{
	public partial class TeachingViewController : UIViewController
	{
		private string pathToDatabase;
		private List<CardLocal> cards;
		private CardLocal currentCard;
		private PropertyUser property;
		private int sideCard = 0;
		private int SideCardInButton = 0;
		public int numberWord = 0;

		public string Name_Deck
		{
			get;
			set;
		}

		public int Id_deck
		{
			get;
			set;
		}

		public TeachingViewController(IntPtr handle) : base(handle)
		{
			cards = new List<CardLocal>();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");
			property = new PropertyUser();

			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				property = connection.Get<PropertyUser>(1);
			}
			sideCard = property.side_card;
			Title = Name_Deck;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			//TextTeachingButton.TitleLabel.Text = "qweqwe";
			cards = new List<CardLocal>();
			currentCard = new CardLocal();
			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				var query = connection.Table<CardLocal>();

				foreach (CardLocal card in query)
				{
					if ((card.id_deck == Id_deck) && (card.archive_card != 1))
					{
						cards.Add(card);
					}
				}
			}
			if (cards.Count == 0)
			{
				TextTeachingButton.SetTitle("", UIControlState.Normal);
				//WordLabel.Text = "";
				//TranslateLabel.Text = "";
				var ac = UIAlertController.Create("В колоде пусто!", "В этой калоде нет не выученных карточек.", UIAlertControllerStyle.Alert);
				//ac.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (action) => this.DismissViewController(true, null)));
				ac.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (UIAlertAction obj) =>
				{
					DismissViewController(true, null);
				}));
				PresentViewController(ac, true, null);
			}
			else if (sideCard == 0)
			{
				TextTeachingButton.SetTitle(cards[0].word, UIControlState.Normal);
			}
			else if (sideCard == 1)
			{
				TextTeachingButton.SetTitle(cards[0].translate, UIControlState.Normal);
			}

		}
		 
	
		partial void RememberButton_TouchUpInside(UIButton sender)
		{
			if (cards.Count != 0)
			{
				using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
				{
					currentCard = connection.Get<CardLocal>(cards[numberWord].id); //получаем из бд карту по её id;
					currentCard.count_repeat++;
					if (currentCard.count_repeat < property.number_of_repetition)
					{
						connection.Update(new CardLocal
						{
							id = currentCard.id,
							id_deck = currentCard.id_deck,
							word = currentCard.word,
							translate = currentCard.translate,
							archive_card = currentCard.archive_card,
							count_repeat = currentCard.count_repeat
						});
						numberWord++;
					}
					else
					{
						connection.Update(new CardLocal
						{
							id = currentCard.id,
							id_deck = currentCard.id_deck,
							word = currentCard.word,
							translate = currentCard.translate,
							archive_card = 1,
							count_repeat = 0
						});
						connection.Update(new DeckLocal 
						{
							id = Id_deck,
							title = Name_Deck,
							acrive_deck = 1
						});
						cards.RemoveAt(numberWord);

					}

					if (cards.Count == 0)
					{
						TextTeachingButton.SetTitle("", UIControlState.Normal);
						//WordLabel.Text = "";
						//TranslateLabel.Text = "";
					}
					else if (sideCard == 0)
					{
						if  (numberWord >= cards.Count)
						{
							numberWord = 0;
							TextTeachingButton.SetTitle(cards[0].word, UIControlState.Normal);;
						}
						else
						{
							TextTeachingButton.SetTitle(cards[numberWord].word, UIControlState.Normal);
						}
					}
					else if (sideCard == 1)
					{
						if (numberWord >= cards.Count)
						{
							numberWord = 0;
							TextTeachingButton.SetTitle(cards[0].translate, UIControlState.Normal);
						}
						else
						{
							TextTeachingButton.SetTitle(cards[numberWord].translate, UIControlState.Normal);
						}	
					}
				}
			}
			else
			{
				var ac = UIAlertController.Create("В колоде пусто!", "В этой калоде больше не осталось не выученных карточек. Зайдите в архив если вы хотите повторить ранее выученые карточки.", UIAlertControllerStyle.Alert);
				//ac.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (action) => this.DismissViewController(true, null)));
				ac.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, (UIAlertAction obj) =>
				{
					DismissViewController(true, null);
				}));
				PresentViewController(ac, true, null);
			}
		}
		partial void NotRememberButton_TouchUpInside(UIButton sender) //button не помню
		{
			if (cards.Count != 0)
			{
				using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
				{
					currentCard = connection.Get<CardLocal>(cards[numberWord].id); //получаем из бд карту по её id
					currentCard.count_repeat = 0;
					connection.Update(new CardLocal
					{
						id = currentCard.id,
						id_deck = currentCard.id_deck,
						word = currentCard.word,
						translate = currentCard.translate,
						archive_card = currentCard.archive_card,
						count_repeat = 0
					});
					numberWord++;
				}
				if (sideCard == 0)
				{
					if (numberWord >= cards.Count)
					{
						numberWord = 0;
						TextTeachingButton.SetTitle(cards[0].word, UIControlState.Normal);
					}
					else
					{
						TextTeachingButton.SetTitle(cards[numberWord].word, UIControlState.Normal);
					}
				}
				else if (sideCard == 1)
				{
					if (numberWord >= cards.Count)
					{
						numberWord = 0;
						TextTeachingButton.SetTitle(cards[0].translate, UIControlState.Normal);
					}
					else
					{
						TextTeachingButton.SetTitle(cards[numberWord].translate, UIControlState.Normal);
					}	
				}
			}
			else
			{
				Console.WriteLine("Нет картонок");
			}
		}

		partial void TextTeachingButton_TouchUpInside(UIButton sender)
		{
			if (cards.Count != 0)
			{
				if (TextTeachingButton.TitleLabel.Text == cards[numberWord].word)
				{
					TextTeachingButton.SetTitle(cards[numberWord].translate, UIControlState.Normal);
				}
				else
				{
					TextTeachingButton.SetTitle(cards[numberWord].word, UIControlState.Normal);
				}
			}
		}
	}
}