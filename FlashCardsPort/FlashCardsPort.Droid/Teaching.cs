﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using UIKit;

namespace FlashCardsPort.Droid
{
	[Activity(Label = "Teaching")]
	public class Teaching : Activity
	{
		Button remember;
		Button not_remember;
		Button translate;
		private string pathToDatabase;
		private PropertyUser property;
		private int Id_deck;
		private string Name_Deck;
        ImageView teachingImageView;
		private List<CardLocal> cards;
		private CardLocal currentCard;
		private int sideCard = 0;
		
		public int numberWord = 0;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.teaching);
            Id_deck = Intent.GetIntExtra("deck_id", 0);
            Name_Deck = Intent.GetStringExtra("deck_title");    
            Title = Name_Deck;
            ActionBar actionBar = ActionBar;
            actionBar.SetDisplayHomeAsUpEnabled(true);
            actionBar.SetDisplayShowHomeEnabled(false);
            not_remember = FindViewById<Button>(Resource.Id.not_remember_button);
			remember = FindViewById<Button>(Resource.Id.remember_button);
			translate = FindViewById<Button>(Resource.Id.translate_button);
            teachingImageView = FindViewById<ImageView>(Resource.Id.teachingImageView);

            var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");

			property = new PropertyUser();
			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				property = connection.Get<PropertyUser>(1);
			}
			sideCard = property.side_card;

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
				translate.Text = "";
				teachingImageView.SetImageDrawable(null);
				AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
				alertDialog.SetTitle("В колоде пусто!");
				alertDialog.SetMessage("В этой калоде нет не выученных карточек.");
				alertDialog.SetNeutralButton("OK", delegate
				{
					alertDialog.Dispose();
				});
				alertDialog.Show();
			}
			else if (sideCard == 0)
			{
				translate.Text = cards[0].word;
				if (cards[0].image != null)
				{
					Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(cards[0].image));
					teachingImageView.SetImageURI(uri);
				}
                else
                {
					teachingImageView.SetImageDrawable(null);
                }
			}
			else if (sideCard == 1)
			{
				translate.Text = cards[0].translate;
				if (cards[0].image != null)
				{
					Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(cards[0].image));
					teachingImageView.SetImageURI(uri);
				}
                else
                {
					teachingImageView.SetImageDrawable(null);
                }
				//TextTeachingButton.SetTitle(cards[0].translate, UIControlState.Normal);
			}



			translate.Click += Translate_Click;
			remember.Click += Remember_Click;
			not_remember.Click += Not_Remember_Click;


		}

		void Translate_Click(object sender, EventArgs e)
		{
			if (cards.Count != 0)
			{
				if (translate.Text == cards[numberWord].word)
				{
					translate.Text = cards[numberWord].translate;
					//TextTeachingButton.SetTitle(cards[numberWord].translate, UIControlState.Normal);
				}
				else
				{
					translate.Text = cards[numberWord].word;
					//TextTeachingButton.SetTitle(cards[numberWord].word, UIControlState.Normal);
				}
			}
            else
            {
				AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
				alertDialog.SetTitle("В колоде пусто!");
				alertDialog.SetMessage("В этой калоде больше не осталось не выученных карточек." +
									   " Зайдите в архив если вы хотите повторить ранее выученые карточки.");
				alertDialog.SetNeutralButton("OK", delegate
				{
					alertDialog.Dispose();
				});
				alertDialog.Show();
            }
		}

		void Remember_Click(object sender, EventArgs e)
		{
            Console.WriteLine(cards.Count);
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
                            count_repeat = currentCard.count_repeat,
                            image = currentCard.image
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
							count_repeat = 0,
                            image = currentCard.image
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
						translate.Text = "";
                        teachingImageView.SetImageDrawable(null);
						AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
						alertDialog.SetTitle("Поздравляем!");
						alertDialog.SetMessage("Вы выучили все карточки этой колоды!");
						alertDialog.SetNeutralButton("OK", delegate
						{
                            var intent = new Intent(this, typeof(Teaching_decks));
							StartActivity(intent);
							alertDialog.Dispose();
						});
						alertDialog.Show();
					}
                    else if (sideCard == 0)
					{
						if (numberWord >= cards.Count)
						{
							numberWord = 0;
							translate.Text = cards[0].word;
							if (cards[0].image != null)
							{
								Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(cards[0].image));
								teachingImageView.SetImageURI(uri);
							}
                            else
                            {
								teachingImageView.SetImageDrawable(null);
                            }

						}
						else
						{
							translate.Text = cards[numberWord].word;
							if (cards[numberWord].image != null)
							{
								Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(cards[numberWord].image));
								teachingImageView.SetImageURI(uri);
							}
                            else
                            {
								teachingImageView.SetImageDrawable(null);
                            }
						}
					}
					else if (sideCard == 1)
					{
						if (numberWord >= cards.Count)
						{
							numberWord = 0;
							translate.Text = cards[0].translate;
							if (cards[0].image != null)
							{
								Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(cards[0].image));
								teachingImageView.SetImageURI(uri);
							}
                            else
                            {
								teachingImageView.SetImageDrawable(null);
                            }
						}
						else
						{
							translate.Text = cards[numberWord].translate;
							if (cards[numberWord].image != null)
							{
								Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(cards[numberWord].image));
								teachingImageView.SetImageURI(uri);
							}
                            else
                            {
								teachingImageView.SetImageDrawable(null);
                            }
						}
					}
				}
			}
			else
			{
				AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
				alertDialog.SetTitle("В колоде пусто!");
				alertDialog.SetMessage("В этой калоде больше не осталось не выученных карточек.");
				alertDialog.SetNeutralButton("OK", delegate
				{
                    var intent = new Intent(this, typeof(Teaching_decks));
					StartActivity(intent);
					alertDialog.Dispose();
				});
				alertDialog.Show();
			}

		}

		void Not_Remember_Click(object sender, EventArgs e)
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
						count_repeat = 0,
                        image = currentCard.image
					});
					numberWord++;
				}
				if (sideCard == 0)
				{
					if (numberWord >= cards.Count)
					{
						numberWord = 0;
						translate.Text = cards[0].word;
                        if (cards[0].image != null)
						{
                            Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(cards[0].image));
                            teachingImageView.SetImageURI(uri);
						}
                        else
                        {
							teachingImageView.SetImageDrawable(null);
                        }
					}
					else
					{
						translate.Text = cards[numberWord].word;
						if (cards[numberWord].image != null)
						{
							Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(cards[numberWord].image));
							teachingImageView.SetImageURI(uri);
						}
                        else
                        {
							teachingImageView.SetImageDrawable(null);
                        }
                    }
				}
				else if (sideCard == 1)
				{
					if (numberWord >= cards.Count)
					{
						numberWord = 0;
						translate.Text = cards[0].translate;
						if (cards[0].image != null)
						{
							Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(cards[0].image));
							teachingImageView.SetImageURI(uri);
						}
                        else
                        {
							teachingImageView.SetImageDrawable(null);
                        }
					}
					else
					{
						translate.Text = cards[numberWord].translate;
						if (cards[numberWord].image != null)
						{
							Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(cards[numberWord].image));
							teachingImageView.SetImageURI(uri);
						}
                        else
                        {
							teachingImageView.SetImageDrawable(null);   
                        }
					}
				}
			}
			else
			{
				translate.Text = "";
				teachingImageView.SetImageDrawable(null);
				AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
				alertDialog.SetTitle("В колоде пусто!");
				alertDialog.SetMessage("В этой кoлоде больше не осталось не выученных карточек." +
									   " Зайдите в архив если вы хотите повторить ранее выученые карточки.");
				alertDialog.SetNeutralButton("OK", delegate
				{
                    var intent = new Intent(this, typeof(Teaching_decks));
					StartActivity(intent);
					alertDialog.Dispose();
				});
				alertDialog.Show();
			}
		}
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    var intent = new Intent(this, typeof(Teaching_decks));
                    StartActivity(intent);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}

