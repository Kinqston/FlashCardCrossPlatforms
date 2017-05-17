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

namespace FlashCardsPort.Droid
{
    class Card
    {
        private String word;
        private String translate;
        public Card(string word, string translate)
        {
            this.word = word;
            this.translate = translate;
        }
        public string Word
        {
            get { return word; }
        }
        public string Translate
        {
            get { return translate; }
        }
    }
}