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
    class MyHolder
    {
        public TextView word;
        public TextView translate;
        public MyHolder(View itemView)
        {
            word = itemView.FindViewById<TextView>(Resource.Id.word);
            translate = itemView.FindViewById<TextView>(Resource.Id.translate);
        }
    }
}