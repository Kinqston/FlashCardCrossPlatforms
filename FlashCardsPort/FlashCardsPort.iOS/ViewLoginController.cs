using Foundation;
using System;
using UIKit;
using ToastIOS;

namespace FlashCardsPort.iOS
{
	public partial class ViewLoginController : UIViewController
    {
		BaseData bd = new BaseData();
		String id;
        public ViewLoginController (IntPtr handle) : base (handle)
        {
        }
		public override void ViewDidLoad()
		{
			NavigationItem.HidesBackButton = true;
			base.ViewDidLoad();
		//	this.NavigationController.PushViewController(ViewMainMenuUsers, true);
		}


		partial void UIButton558_TouchUpInside(UIButton sender)
		{
			id = bd.Login(email.Text, pass.Text);
			if (id == "false")
			{

var other = Storyboard.InstantiateViewController("MainViewController") as MainViewController;
NavigationController.PushViewController(other, true);
				//Toast.MakeText("Не правильная почта или пароль").Show();
			}
			else
			{
				var other = Storyboard.InstantiateViewController("MainMenu") as MainMenu;
				NavigationController.PushViewController(other, true);
			}

		}

		partial void UIButton1868_TouchUpInside(UIButton sender)
		{
			var other = Storyboard.InstantiateViewController("ViewRegisterController") as ViewRegisterController;
			NavigationController.PushViewController(other, true);
		}
		[Action("ViewLogin:")]
		public void ViewLogin(UIStoryboardSegue segue)
		{

		}
	}
}