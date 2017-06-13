using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace FlashCardsPort.Droid
{
    class ArchiveCardAdapter : BaseAdapter<CardLocal>
    {
        private List<CardLocal> list;
        private Context context;

        public ArchiveCardAdapter(Context context, List<CardLocal> list)
        {
            this.list = list;
            this.context = context;
        }

        public override int Count
        {
            get
            {
                return list.Count;
            }
        }

        public override CardLocal this[int position]
        {
            get
            {
                return list[position];
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null){
                view = LayoutInflater.From(context).Inflate(Resource.Layout.archive_cards, null, false);
                TextView word = view.FindViewById<TextView>(Resource.Id.wordArchivetextView);
                TextView translate = view.FindViewById<TextView>(Resource.Id.translateArchiveRextView);

                word.Text = list[position].word;
                translate.Text = list[position].translate;

            }
            return view;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}
