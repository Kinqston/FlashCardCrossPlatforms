using System;
using SQLite;

namespace FlashCardsPort.iOS
{
	public class CardLocal
	{
		[PrimaryKey, AutoIncrement]
		public int id
		{
			get;
			set;
		}
		[Indexed]
		public int id_deck
		{
			get;
			set;
		}
		public string word
		{
			get;
			set;
		}
		public string translate
		{
			get;
			set;
		}
		public string image
		{
			get;
			set;
		}
		public int count_repeat
		{
			get;
			set;
		}
		public int archive_card
		{
			get;
			set;
		}
	}
}
