using System;
using SQLite;

namespace FlashCardsPort.Droid
{
	public class DeckLocal
	{
		[PrimaryKey, AutoIncrement]
		public int id
		{
			get;
			set;
		}
		public string title
		{
			get;
			set;
		}
		public int acrive_deck
		{
			get;
			set;
		}

	}
}
