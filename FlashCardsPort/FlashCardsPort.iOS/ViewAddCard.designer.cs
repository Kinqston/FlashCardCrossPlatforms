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
    [Register ("ViewAddCard")]
    partial class ViewAddCard
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Add { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Camera { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Gallery { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView image { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Translate { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Word { get; set; }

        [Action ("UIButton1780_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIButton1780_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (Add != null) {
                Add.Dispose ();
                Add = null;
            }

            if (Camera != null) {
                Camera.Dispose ();
                Camera = null;
            }

            if (Gallery != null) {
                Gallery.Dispose ();
                Gallery = null;
            }

            if (image != null) {
                image.Dispose ();
                image = null;
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