﻿using System;
using System.Net;
using System.Threading.Tasks;
using Foundation;
using Media.Plugin;
using Media.Plugin.Abstractions;
using UIKit;

namespace FlashCardsPort.iOS
{
	public partial class MainMenu : UIViewController
	{
		public string id;
		public MainMenu(IntPtr handle) : base(handle)
		{

		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			NavigationItem.HidesBackButton = true;

			//NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem("1", UIBarButtonItemStyle.Done, (sender, e) => { }),true);
			//= new UIBarButtonItem("", UIBarButtonItemStyle.Plain, null);
			//NavigationController.NavigationBarHidden = true;
			// Perform any additional setup after loading the view, typically from a nib.

		}



		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

	}
}

