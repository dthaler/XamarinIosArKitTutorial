using ARKit;
using AVFoundation;
using CoreGraphics;
using Foundation;
using SceneKit;
using SpriteKit;
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
                LightEstimationEnabled = true,
                WorldAlignment = ARWorldAlignment.Gravity
            }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);

            //var videoNode = new SKVideoNode("Videos/big-buck-bunny-wide.mp4");
            NSUrl videoUrl = new NSUrl("https://archive.org/download/BigBuckBunny_328/BigBuckBunny_512kb.mp4");
            var videoNode = new SKVideoNode(videoUrl);

            // Without this the video will be inverted upside down and back to front
            videoNode.YScale = -1;
            videoNode.Play();

            var videoScene = new SKScene();
            videoScene.Size = new CoreGraphics.CGSize(416, 240);
            videoScene.ScaleMode = SKSceneScaleMode.AspectFill;
            videoNode.Position = new CoreGraphics.CGPoint(videoScene.Size.Width / 2, videoScene.Size.Height / 2);
            videoScene.AddChild(videoNode);

            // These are set to be the same aspect ratio as the video itself (1.77 in this case)
            var width = 0.5f;
            float length = (float)((width * videoScene.Size.Height) / videoScene.Size.Width);
            var planeNode = new PlaneNode(width, length, new SCNVector3(0, 0, -0.5f), videoScene);

            this.sceneView.Scene.RootNode.AddChildNode(planeNode);
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
        public PlaneNode(float width, float length, SCNVector3 position, SKScene videoScene)
        {
            var rootNode = new SCNNode
            {
                Geometry = CreateGeometry(width, length, videoScene),
                Position = position
            };

            AddChildNode(rootNode);
        }

        private static SCNGeometry CreateGeometry(float width, float length, SKScene videoScene)
        {
            var material = new SCNMaterial();
            material.Diffuse.Contents = videoScene;
            material.DoubleSided = true;

            var geometry = SCNPlane.Create(width, length);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}
