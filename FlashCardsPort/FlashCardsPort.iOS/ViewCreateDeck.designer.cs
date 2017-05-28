// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace FlashCardsPort.iOS
{
    [Register ("ViewCreateDeck")]
    partial class ViewCreateDeck
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Cost_deck { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Title_deck { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Cost_deck != null) {
                Cost_deck.Dispose ();
                Cost_deck = null;
            }

            if (Title_deck != null) {
                Title_deck.Dispose ();
                Title_deck = null;
            }
        }
    }
}