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
    [Register ("Cards")]
    partial class Cards
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem Add_one_card { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView CardsTableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Add_one_card != null) {
                Add_one_card.Dispose ();
                Add_one_card = null;
            }

            if (CardsTableView != null) {
                CardsTableView.Dispose ();
                CardsTableView = null;
            }
        }
    }
}