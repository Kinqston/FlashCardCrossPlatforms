
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
    [Activity(Label = "Teaching")]
    public class Teaching : Activity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.teaching);

            Title = Intent.GetStringExtra("deck_title");
            
            int deck_id = Intent.GetIntExtra("deck_id", 0);
            Console.WriteLine(deck_id);
            // Create your application here
        }
    }
}
