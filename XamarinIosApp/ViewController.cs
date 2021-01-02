using ARKit;
using CoreGraphics;
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

        private SCNNode cubeNode;

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
                LightEstimationEnabled = true,
                WorldAlignment = ARWorldAlignment.Gravity
            }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);

            cubeNode = new CubeNode(1f, UIColor.Yellow, new SCNVector3(0, 0, 3f));

            this.sceneView.Scene.RootNode.AddChildNode(cubeNode);

            UIButton myButton = new UIButton(UIButtonType.System);
            myButton.Frame = new CGRect(100, 50, 200, 75);
            myButton.SetTitle("Change to Red", UIControlState.Normal);
            myButton.BackgroundColor = UIColor.Green;
            myButton.Layer.CornerRadius = 5f;
            myButton.TouchUpInside += (sender, e) => {

                var material = new SCNMaterial();
                material.Diffuse.Contents = UIColor.Red;

                cubeNode.ChildNodes[0].Geometry.Materials = new SCNMaterial[] { material };
            };

            this.View.Add(myButton);
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
