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
    [Register ("ViewCreateDeck_user")]
    partial class ViewCreateDeck_user
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Title_deck { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Title_deck != null) {
                Title_deck.Dispose ();
                Title_deck = null;
            }
        }
    }
}