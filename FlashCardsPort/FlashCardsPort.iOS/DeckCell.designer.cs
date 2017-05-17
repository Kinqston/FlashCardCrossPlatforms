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
    [Register ("DeckCell")]
    partial class DeckCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DeckTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DeckTitle != null) {
                DeckTitle.Dispose ();
                DeckTitle = null;
            }
        }
    }
}