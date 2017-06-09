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
    class Card
    {
        public String word;
        public String translate;
        public String image;
        public String id;
        public Bitmap bitmap_image;
        public Card(string id, string word, string translate, String image, Bitmap bitmap_image)
        {
            this.id = id;
            this.word = word;
            this.translate = translate;
            this.image = image;
            this.bitmap_image = bitmap_image;
        }
        public string Word
        {
            get { return word; }
        }
        public string Translate
        {
            get { return translate; }
        }
        public String Image
        {
            get { return image; }
        }
        public Bitmap Bitmap_image
        {
            get { return bitmap_image; }
        }
        public String Id
        {
            get { return id; }
        }
    }
}