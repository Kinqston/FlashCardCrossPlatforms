
using System;
using System.Collections.Generic;
using System.IO;
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

    [Activity(Label = "Property_user_view")]
    public class Property_user_view : Activity
    {
		Button countRepeat;
		Button sideCard;
        private string pathToDatabase;
        PropertyUser property;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.property_user);


			ActionBar actionBar = ActionBar;
			actionBar.SetDisplayHomeAsUpEnabled(true);
			actionBar.SetDisplayShowHomeEnabled(false);
            Title = "Пользовательские настройки";
            countRepeat = FindViewById<Button>(Resource.Id.countRepeatButton);
            sideCard = FindViewById<Button>(Resource.Id.sideCardButton);
			var documentsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");
            countRepeat.Click += CountRepeat_Click;
            sideCard.Click += SideCard_Click;
            property = new PropertyUser();
			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				property = connection.Get<PropertyUser>(1);
			}
        }
       

        void CountRepeat_Click(object sender, EventArgs e)
        {
            
            NumberPicker countRepeatPicker = new NumberPicker(this);
            countRepeatPicker.MinValue = 1;
            countRepeatPicker.MaxValue = 20;
            countRepeatPicker.Value = property.number_of_repetition;
            AlertDialog.Builder builder = new AlertDialog.Builder(this).SetView(countRepeatPicker);
            builder.SetTitle("Выберите количество повторений");
            builder.SetPositiveButton(Resource.String.dialog_ok, (s, a) => 
            {
				using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
				{
                    connection.Update(new PropertyUser() { id = 1, number_of_repetition = countRepeatPicker.Value, side_card = property.side_card });
                    property.number_of_repetition = countRepeatPicker.Value;
                }
			}); 
            builder.Show();
        }

        void SideCard_Click(object sender, EventArgs e)
        {
            ToggleButton sideCardPicker = new ToggleButton(this);
            if (property.side_card == 1)
            {
				sideCardPicker.Checked = false;
				sideCardPicker.Text = "Русское слово";
            }
            else
            {
                sideCardPicker.Checked = true;
                sideCardPicker.Text = "Иностранное слово";
            }
            
            sideCardPicker.TextOn = "Иностранное слово";
            sideCardPicker.TextOff = "Русское слово";
            AlertDialog.Builder builder = new AlertDialog.Builder(this).SetView(sideCardPicker);
			builder.SetTitle("Выберите сторону карточки");
            builder.SetPositiveButton(Resource.String.dialog_ok, (s, a) => 
            {
				using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
				{
                    if (sideCardPicker.Checked)
                    {
                        connection.Update(new PropertyUser() { id = 1, number_of_repetition = property.number_of_repetition, side_card = 0 });
                        property.side_card = 0;
                    }
                    else
                    {
                        connection.Update(new PropertyUser() { id = 1, number_of_repetition = property.number_of_repetition, side_card = 1 });
                        property.side_card = 1;
                    }
				}   
            });
			builder.Show();
        }
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
