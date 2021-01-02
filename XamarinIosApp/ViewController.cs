using ARKit;
using Foundation;
using SceneKit;
using System;
using System.Linq;
using UIKit;

namespace XamarinIosApp
{
    public partial class ViewController : UIViewController
    {
        private readonly ARSCNView sceneView;

        public ViewController(IntPtr handle) : base(handle)
        {
            // Lesson: Create Blank Scene
            this.sceneView = new ARSCNView();
            this.View.AddSubview(this.sceneView);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            this.sceneView.Frame = this.View.Frame;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            this.sceneView.Session.Run(new ARWorldTrackingConfiguration
            {
                AutoFocusEnabled = true,
                LightEstimationEnabled = true,
                WorldAlignment = ARWorldAlignment.Gravity
            }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            this.sceneView.Session.Pause();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}
