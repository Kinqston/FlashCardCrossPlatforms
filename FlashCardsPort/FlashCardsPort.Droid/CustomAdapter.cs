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
using Java.Util;

namespace FlashCardsPort.Droid
{
    class CustomAdapter : ArrayAdapter
    {
        private Context c;
        private List<Card> cards;
        private int resourse;
        private LayoutInflater inflater;
        public CustomAdapter(Context context,int resourse, List<Card> objects):base(context, resourse, objects)
        {
            this.c = context;
            this.resourse = resourse;
            this.cards = objects;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (inflater == null)
            {
                inflater = (LayoutInflater) c.GetSystemService(Context.LayoutInflaterService);
            }
            if(convertView == null)
            {
                convertView = inflater.Inflate(resourse, parent, false);
            }
            MyHolder holder = new MyHolder(convertView);
            holder.word.Text = cards[position].Word;
            holder.translate.Text = cards[position].Translate;
            return convertView;
        }
    }
}