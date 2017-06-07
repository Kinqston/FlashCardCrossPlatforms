﻿using System;
using SQLite;

namespace FlashCardsPort.Droid
{
	public class PropertyUser
	{
		[PrimaryKey]
		public int id
		{
			get;
			set;
		}
		public int number_of_repetition
		{
			get;
			set;
		}
		public int side_card
		{
			get;
			set;
		}
	}
}
