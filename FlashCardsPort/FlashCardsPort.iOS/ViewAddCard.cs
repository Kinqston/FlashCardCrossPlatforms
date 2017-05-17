using Foundation;
using System;
using UIKit;
using System.Collections.Generic;

namespace FlashCardsPort.iOS
{
    public partial class ViewAddCard : UIViewController
    {
		public string word, translate, title_deck;
		public List<Cards_item> cards;
        public ViewAddCard (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Title = title_deck;
		}


		partial void UIButton1780_TouchUpInside(UIButton sender)
		{
			word = Word.Text;
			translate = Translate.Text;
		}
	}
}