using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;


namespace FlashCardsPort.Droid
{
    [Activity(Label = "Магазин")]
    public class Shop_user : Activity
    {




		static BaseData bd = new BaseData();
        ListView shop_list;
        List<string> list;
        List<Deck> decks;
		//private string pathToDatabase;
		//private List<CardLocal> cards;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.shop_user);
			bd.connection();
            shop_list = FindViewById<ListView>(Resource.Id.shop_list);
        //    shop_list.ItemClick += Shop_List_ItemClick;
			ActionBar actionBar = ActionBar;
			actionBar.SetDisplayShowHomeEnabled(false);
			actionBar.SetDisplayHomeAsUpEnabled(true);
            bd.Decks_list();
            decks = new List<Deck>();
            //bd.Cards_list("1");

            list = new List<string>();
            //list.AddRange(new string[] {"2", "213", "123123","111"});

            //Shop_adapter adapter = new Shop_adapter(this, list);
            for (int i = 0; i < bd.items_deck.Count; i++)
            {
                decks.Add(new Deck(bd.items_deck_id[i], bd.items_deck[i], Convert.ToBoolean(bd.items_deck_cost[i])));
            }
            Shop_adapter adapter = new Shop_adapter(this, decks);
            shop_list.Adapter = adapter;


           // Shop_adapter adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_deck);
			//adapter_deck_id = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_deck_id);
			//adapter_deck_cost = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, bd.items_deck_cost);
			//list_deck.Adapter = adapter;
			// Create your application here
		//	ArchiveCardAdapter adapter = new ArchiveCardAdapter(this, cards);
		//	archiveCardListView.Adapter = adapter;
		//	archiveCardListView.ItemClick += ArchiveCardListView_ItemClick;
		//	adapter.NotifyDataSetChanged();
        }

      //  void Shop_List_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
      //  {
      //
      //  }

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
                    var intent = new Intent(this, typeof(Main_user));
					StartActivity(intent);
					break;
			}
			return base.OnOptionsItemSelected(item);
		}
    }
}
