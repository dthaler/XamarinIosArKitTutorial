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
                AutoenablesDefaultLighting = true,
                DebugOptions =
                ARSCNDebugOptions.ShowFeaturePoints | ARSCNDebugOptions.ShowWorldOrigin
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
                WorldAlignment = ARWorldAlignment.Gravity
            }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);

            // Lesson: Add text to scene
            var textNode = new TextNode("Hello Universe", UIColor.Orange)
            {
                Position = new SCNVector3(0, -0.6f, 0) // Any lower than this and it doesn't show!?
            };

            this.sceneView.Scene.RootNode.AddChildNode(textNode);
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

    public class TextNode : SCNNode
    {
        public TextNode(string text, UIColor color)
        {
            var rootNode = new SCNNode
            {
                Geometry = CreateGeometry(text, color),
                Position = new SCNVector3(0, 0, 0)
            };

            AddChildNode(rootNode);
        }

        private static SCNGeometry CreateGeometry(string text, UIColor color)
        {
            var geometry = SCNText.Create(text, 0.01f);
            geometry.Font = UIFont.FromName("Courier", 0.5f);
            geometry.Flatness = 0;
            geometry.FirstMaterial.DoubleSided = true;
            geometry.FirstMaterial.Diffuse.Contents = color;
            geometry.FirstMaterial.Specular.Contents = UIColor.Blue;
            return geometry;
        }
    }
}
