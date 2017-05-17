// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace FlashCardsPort.iOS
{
    [Register ("Decks")]
    partial class Decks
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView DecksTableView { get; set; }

        [Action ("UIBarButtonItem897_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIBarButtonItem897_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (DecksTableView != null) {
                DecksTableView.Dispose ();
                DecksTableView = null;
            }
        }
    }
}