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
    [Register ("ViewCreateCard")]
    partial class ViewCreateCard
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView CardsTableView { get; set; }

        [Action ("UIBarButtonItem1338_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIBarButtonItem1338_Activated (UIKit.UIBarButtonItem sender);

        [Action ("UIBarButtonItem1339_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIBarButtonItem1339_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (CardsTableView != null) {
                CardsTableView.Dispose ();
                CardsTableView = null;
            }
        }
    }
}