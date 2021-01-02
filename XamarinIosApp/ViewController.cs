using ARKit;
using Foundation;
using SceneKit;
using System;
using System.Collections.Generic;
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
                AutoenablesDefaultLighting = true,
                DebugOptions = ARSCNDebugOptions.ShowFeaturePoints
                | ARSCNDebugOptions.ShowWorldOrigin
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

            var size = 0.05f;

            this.sceneView.Scene.RootNode.AddChildNode(new CubeNode(size, UIColor.Green, new SCNVector3(-0.1f, 0.1f, 0)));
            this.sceneView.Scene.RootNode.AddChildNode(new CubeNode(size, UIColor.Green, new SCNVector3(0, 0.1f, 0)));
            this.sceneView.Scene.RootNode.AddChildNode(new CubeNode(size, UIColor.Green, new SCNVector3(0.1f, 0.1f, 0)));

            this.sceneView.Scene.RootNode.AddChildNode(new CubeNode(size, UIColor.Green, new SCNVector3(-0.1f, 0, 0)));
            this.sceneView.Scene.RootNode.AddChildNode(new CubeNode(size, UIColor.Green, new SCNVector3(0, 0, 0)));
            this.sceneView.Scene.RootNode.AddChildNode(new CubeNode(size, UIColor.Green, new SCNVector3(0.1f, 0, 0)));

            this.sceneView.Scene.RootNode.AddChildNode(new CubeNode(size, UIColor.Green, new SCNVector3(-0.1f, -0.1f, 0)));
            this.sceneView.Scene.RootNode.AddChildNode(new CubeNode(size, UIColor.Green, new SCNVector3(0, -0.1f, 0)));
            this.sceneView.Scene.RootNode.AddChildNode(new CubeNode(size, UIColor.Green, new SCNVector3(0.1f, -0.1f, 0)));
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            if (touches.AnyObject is UITouch touch)
            {
                var point = touch.LocationInView(this.sceneView);

                var hitTestOptions = new SCNHitTestOptions();

                var hits = this.sceneView.HitTest(point, hitTestOptions);
                var hit = hits.FirstOrDefault();

                if (hit == null)
                    return;

                var node = hit.Node;

                if (node == null)
                    return;

                var redMaterial = new SCNMaterial();
                redMaterial.Diffuse.Contents = UIColor.Red;

                node.Geometry.Materials = new SCNMaterial[] { redMaterial };
            }
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

    public class CubeNode : SCNNode
    {
        public CubeNode(float size, UIColor color, SCNVector3 position)
        {
            var rootNode = new SCNNode
            {
                Geometry = CreateGeometry(size, color),
                Position = position
            };

            AddChildNode(rootNode);
        }

        private static SCNGeometry CreateGeometry(float size, UIColor color)
        {
            var material = new SCNMaterial();
            material.Diffuse.Contents = color;

            var geometry = SCNBox.Create(size, size, size, 0);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}
