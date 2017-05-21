using Foundation;
using System;
using UIKit;

namespace FlashCardsPort.iOS
{
    public partial class ViewRegisterController : UIViewController
    {
		public System.IntPtr intPtr;
		ViewRegisterController register;
		UIViewController myview;
        public ViewRegisterController (IntPtr handle) : base (handle)
        {
			intPtr = handle+1;
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NavigationItem.HidesBackButton = true;
		}

		partial void UIButton1907_TouchUpInside(UIButton sender)
		{

			//UIStoryboard board = UIStoryboard.FromName("Main", null);
			//ViewController ctrl = (ViewController)board.InstantiateViewController("MainMenu");
			//register.NavigationController.PushViewController (ctrl, true)
			var other = Storyboard.InstantiateViewController("MainMenu") as MainMenu;
			NavigationController.PushViewController(other, true);
		}

		partial void UIButton1916_TouchUpInside(UIButton sender)
		{
			var other = Storyboard.InstantiateViewController("ViewLoginController") as ViewLoginController;
			NavigationController.PushViewController(other, true);
		}
	}
}