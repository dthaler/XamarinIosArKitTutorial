using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(XamarinFormsApp.iOS.ARAppImpl))]
namespace XamarinFormsApp.iOS
{
    public class ARAppImpl : IARApp
    {
        public void LaunchAR()
        {
            // This is in native code; invoke the native UI
            var viewController = new ViewController();

#if false
            // Modally run the view controller.
            UIApplication.SharedApplication.KeyWindow.RootViewController.
              PresentViewController(viewController, true, null);
#else
            // Replace the view controller.
            UIApplication.SharedApplication.KeyWindow.RootViewController = viewController;
#endif
        }
    }
}