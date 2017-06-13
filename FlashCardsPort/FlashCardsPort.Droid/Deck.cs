using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace FlashCardsPort.Droid
{
    class Deck
    {
        public String id;
        public String title;
        public Boolean free;

        public Deck(string id, string title, Boolean free)
        {
            this.id = id;
            this.title = title;
            this.free = free;
        }
        public string Title
        {
            get { return title; }
        }
        public String Id
        {
            get { return id; }
        }
        public Boolean Free
		{
			get { return free; }
		}

	}
}