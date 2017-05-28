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
    [Register ("MainViewController")]
    partial class MainViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton teachingButton { get; set; }

        [Action ("TeachingButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TeachingButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (teachingButton != null) {
                teachingButton.Dispose ();
                teachingButton = null;
            }
        }
    }
}