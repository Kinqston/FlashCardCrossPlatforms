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
    [Register ("ViewRegisterController")]
    partial class ViewRegisterController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField email { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField pass { get; set; }

        [Action ("UIButton1907_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIButton1907_TouchUpInside (UIKit.UIButton sender);

        [Action ("UIButton1916_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIButton1916_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (email != null) {
                email.Dispose ();
                email = null;
            }

            if (pass != null) {
                pass.Dispose ();
                pass = null;
            }
        }
    }
}