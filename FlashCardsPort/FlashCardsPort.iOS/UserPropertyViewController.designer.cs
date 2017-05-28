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
    [Register ("UserPropertyViewController")]
    partial class UserPropertyViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem backBarButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView CountRepeatPickerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem saveBarButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView SideCardPickerVIew { get; set; }

        [Action ("BackBarButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void BackBarButton_Activated (UIKit.UIBarButtonItem sender);

        [Action ("SaveBarButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SaveBarButton_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (backBarButton != null) {
                backBarButton.Dispose ();
                backBarButton = null;
            }

            if (CountRepeatPickerView != null) {
                CountRepeatPickerView.Dispose ();
                CountRepeatPickerView = null;
            }

            if (saveBarButton != null) {
                saveBarButton.Dispose ();
                saveBarButton = null;
            }

            if (SideCardPickerVIew != null) {
                SideCardPickerVIew.Dispose ();
                SideCardPickerVIew = null;
            }
        }
    }
}