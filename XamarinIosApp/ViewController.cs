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
            this.sceneView = new ARSCNView
            {
                AutoenablesDefaultLighting = true
            };

            this.View.AddSubview(this.sceneView);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.sceneView.Frame = this.View.Frame;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            this.sceneView.Session.Run(new ARWorldTrackingConfiguration
            {
                AutoFocusEnabled = true,
                PlaneDetection = ARPlaneDetection.Horizontal,
                LightEstimationEnabled = true,
                WorldAlignment = ARWorldAlignment.GravityAndHeading
            }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);

            var size = 0.08f;
            var colour = UIColor.Green;
            var topRowY = 0.1f;
            var bottomRowY = 0f;

            this.sceneView.Scene.RootNode.AddChildNode(new PlaneNode(size, colour, new SCNVector3(-0.2f, topRowY, 0), 0));
            this.sceneView.Scene.RootNode.AddChildNode(new PlaneNode(size, colour, new SCNVector3(-0.1f, topRowY, 0), 0.004f));
            this.sceneView.Scene.RootNode.AddChildNode(new PlaneNode(size, colour, new SCNVector3(0, topRowY, 0), 0.008f));
            this.sceneView.Scene.RootNode.AddChildNode(new PlaneNode(size, colour, new SCNVector3(0.1f, topRowY, 0), 0.012f));
            this.sceneView.Scene.RootNode.AddChildNode(new PlaneNode(size, colour, new SCNVector3(0.2f, topRowY, 0), 0.016f));

            this.sceneView.Scene.RootNode.AddChildNode(new PlaneNode(size, colour, new SCNVector3(-0.2f, bottomRowY, 0), 0.020f));
            this.sceneView.Scene.RootNode.AddChildNode(new PlaneNode(size, colour, new SCNVector3(-0.1f, bottomRowY, 0), 0.024f));
            this.sceneView.Scene.RootNode.AddChildNode(new PlaneNode(size, colour, new SCNVector3(0f, bottomRowY, 0), 0.028f));
            this.sceneView.Scene.RootNode.AddChildNode(new PlaneNode(size, colour, new SCNVector3(0.1f, bottomRowY, 0), 0.032f));
            this.sceneView.Scene.RootNode.AddChildNode(new PlaneNode(size, colour, new SCNVector3(0.2f, bottomRowY, 0), 0.036f));

            // A circle would effectively be a square with corner radius of half the width (0.08f) so a corner radius of 0.04f
        }
        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            this.sceneView.Session.Pause();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
    }

    public class PlaneNode : SCNNode
    {
        public PlaneNode(float size, UIColor color, SCNVector3 position, float cornerRadius)
        {
            var rootNode = new SCNNode
            {
                Geometry = CreateGeometry(size, color, cornerRadius),
                Position = position
            };

            AddChildNode(rootNode);
        }

        private static SCNGeometry CreateGeometry(float size, UIColor color, float cornerRadius)
        {
            var material = new SCNMaterial();
            material.Diffuse.Contents = color;

            var geometry = SCNPlane.Create(size, size);
            geometry.Materials = new[] { material };
            geometry.CornerRadius = cornerRadius;

            return geometry;
        }
    }
}
