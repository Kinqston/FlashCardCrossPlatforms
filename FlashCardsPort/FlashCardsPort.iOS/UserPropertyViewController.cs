using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Diagnostics.Contracts;

namespace FlashCardsPort.iOS
{
    public partial class UserPropertyViewController : UIViewController
    {
		private string pathToDatabase;
		private string countRepeatSave;
		private string sideCardSave;
		private PropertyUser property;

		public UserPropertyViewController (IntPtr handle) : base (handle)
		{
			
		}

		partial void BackBarButton_Activated(UIBarButtonItem sender)
		{
			DismissViewController(true, null);
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var repeat = new List<string>
			{
				"1","2","3","4","5","6","7","8","9"
			};
			var sideCardList = new List<string>
			{
				"Слово", "Перевод"
			};
			var repeatViewModel = new RepeatViewModal(repeat);
			var sideCardViewModel = new RepeatViewModal(sideCardList);
			repeatViewModel.repeatChanged += (sender, e) =>
			{
				countRepeatSave = repeatViewModel.SelectedNum;
				//UserPropertyTextLabel.Text = repeatViewModel.SelectedNum;
			};
			sideCardViewModel.repeatChanged += (sender, e) =>
			{
				sideCardSave = sideCardViewModel.SelectedNum;
			};
			CountRepeatPickerView.Model = repeatViewModel;
			SideCardPickerVIew.Model = sideCardViewModel;

			property = new PropertyUser();
			var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			pathToDatabase = Path.Combine(documentsFolder, "FlashCards_Database.db");
			using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				property = connection.Get<PropertyUser>(1);
				//repeatViewModel.SelectedNum = property.number_of_repetition;
				// property.number_of_repetition;
				// property.side_card;
			}
		}

		/*partial void SaveUserPropertyButton_TouchUpInside(UIButton sender)
		{
		using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				var numberOfProperty = Int32.Parse(UserPropertyTextLabel.Text); // из строки преобразовываем в int
				var sideCard = Int32.Parse(sideCardTextField.Text);
				connection.Update(new PropertyUser() { id = 1, number_of_repetition = numberOfProperty, side_card = sideCard});
			}
		}*/

		partial void SaveBarButton_Activated(UIBarButtonItem sender)
		{
		using (var connection = new SQLite.SQLiteConnection(pathToDatabase))
			{
				var numberOfProperty = Int32.Parse(countRepeatSave);
				var sideOfProperty = 0;
				if (sideCardSave == "Слово")
				{
					sideOfProperty = 0;
				}
				else
				{
					sideOfProperty = 1;
				}
				connection.Update(new PropertyUser() { id = 1, number_of_repetition = numberOfProperty, side_card = sideOfProperty});
				DismissViewController(true, null);
			}	
		}
	}

}