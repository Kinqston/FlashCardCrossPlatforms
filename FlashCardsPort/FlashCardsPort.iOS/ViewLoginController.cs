using Foundation;
using System;
using UIKit;

namespace FlashCardsPort.iOS
{
    public partial class ViewLoginController : UIViewController
    {
        public ViewLoginController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
		//	this.NavigationController.PushViewController(ViewMainMenuUsers, true);
		}
    }
}