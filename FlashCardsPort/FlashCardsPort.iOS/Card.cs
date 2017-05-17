using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlashCardsPort.IOS
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