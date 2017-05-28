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
    [Register ("CardCell")]
    partial class CardCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView Image { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Translate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Word { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Image != null) {
                Image.Dispose ();
                Image = null;
            }

            if (Translate != null) {
                Translate.Dispose ();
                Translate = null;
            }

            if (Word != null) {
                Word.Dispose ();
                Word = null;
            }
        }
    }
}