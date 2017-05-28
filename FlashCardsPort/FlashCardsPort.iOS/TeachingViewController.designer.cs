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
    [Register ("TeachingViewController")]
    partial class TeachingViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NotRememberButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton rememberButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TextTeachingButton { get; set; }

        [Action ("NotRememberButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void NotRememberButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("RememberButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RememberButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("TextTeachingButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TextTeachingButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (NotRememberButton != null) {
                NotRememberButton.Dispose ();
                NotRememberButton = null;
            }

            if (rememberButton != null) {
                rememberButton.Dispose ();
                rememberButton = null;
            }

            if (TextTeachingButton != null) {
                TextTeachingButton.Dispose ();
                TextTeachingButton = null;
            }
        }
    }
}