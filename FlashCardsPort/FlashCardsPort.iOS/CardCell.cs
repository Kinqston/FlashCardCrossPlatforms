using Foundation;
using System;
using UIKit;

namespace FlashCardsPort.iOS
{
	public partial class CardCell : UITableViewCell
	{
		public CardCell(IntPtr handle) : base(handle)
		{
		}
		internal void UpdateCell(Cards_item card)
		{
			Word.Text = card.Word;
			Translate.Text = card.Translate;
			//Image.Image=this.FromUrl("http://graversp.beget.tech/file_3df01dde-d732-46ca-9689-200eee18b7cf.jpg");
			//UIImage imagesoursce = UIImage.LoadFromData(data);
			//Image.Image = imagesoursce;
			if (card.Image != null)
			{
				Image.Image = FromUrl("http://graversp.beget.tech/" + card.Image);
			}
			//	var url = new NSUrl("http://graversp.beget.tech/file_3df01dde-d732-46ca-9689-200eee18b7cf.jpg");
			//var data = NSData.FromUrl("http://graversp.beget.tech/file_3df01dde-d732-46ca-9689-200eee18b7cf.jpg");
			//UIImage imagesoursce = UIImage.LoadFromData(data);
			//Image.Image = imagesoursce;
		}

		UIImage FromUrl(string uri)
		{
			using (var url = new NSUrl(uri))
			using (var data = NSData.FromUrl(url))
				return UIImage.LoadFromData(data);
		}
	}
}