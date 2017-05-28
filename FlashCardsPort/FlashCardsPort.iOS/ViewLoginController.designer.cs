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
    [Register ("ViewLoginController")]
    partial class ViewLoginController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField email { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField pass { get; set; }

        [Action ("UIButton1868_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIButton1868_TouchUpInside (UIKit.UIButton sender);

        [Action ("UIButton558_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void UIButton558_TouchUpInside (UIKit.UIButton sender);

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