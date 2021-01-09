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
                AutoenablesDefaultLighting = true,
                Delegate = new SceneViewDelegate()
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

            var detectionImages = ARReferenceImage.GetReferenceImagesInGroup("AR Resources", null);

            this.sceneView.Session.Run(new ARWorldTrackingConfiguration
            {
                AutoFocusEnabled = true,
                PlaneDetection = ARPlaneDetection.Horizontal | ARPlaneDetection.Vertical,
                LightEstimationEnabled = true,
                WorldAlignment = ARWorldAlignment.GravityAndHeading,
                DetectionImages = detectionImages,
                MaximumNumberOfTrackedImages = 10

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
        }
    }

    public class SceneViewDelegate : ARSCNViewDelegate
    {
        public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            if (anchor is ARImageAnchor imageAnchor)
            {
                var detectedImage = imageAnchor.ReferenceImage;

                // Add video node
                NSUrl videoUrl = new NSUrl("https://archive.org/download/BigBuckBunny_328/BigBuckBunny_512kb.mp4");
                var videoNode = new SKVideoNode(videoUrl);
                //var videoNode = new SKVideoNode("Videos/leasing-intro.mp4");
                videoNode.YScale = -1;
                videoNode.Play();

                var videoScene = new SKScene();
                videoScene.Size = new CoreGraphics.CGSize(416, 240);
                videoScene.ScaleMode = SKSceneScaleMode.AspectFill;
                videoNode.Position = new CoreGraphics.CGPoint(videoScene.Size.Width / 2, videoScene.Size.Height / 2);
                videoScene.AddChild(videoNode);

                var width = detectedImage.PhysicalSize.Width;
                var length = detectedImage.PhysicalSize.Height;
                var planeNode = new PlaneNode(width, length, new SCNVector3(0, 0, 0), videoScene);

                // Euler angles?
                float angle = (float)(-Math.PI / 2);
                planeNode.EulerAngles = new SCNVector3(angle, 0, 0);

                node.AddChildNode(planeNode);
            }
        }

        public static SCNNode CreateModelNodeFromFile(string filePath, string rootNodeName)
        {
            var sceneFromFile = SCNScene.FromFile(filePath);

            var model = sceneFromFile.RootNode.FindChildNode(rootNodeName, true);

            model.Scale = new SCNVector3(0.01f, 0.2f, 0.01f);
            model.Position = new SCNVector3(0, 0.2f, 0);

            return model;
        }

        public override void DidRemoveNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            if (anchor is ARPlaneAnchor planeAnchor)
            {
            }
        }

        public override void DidUpdateNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            if (anchor is ARPlaneAnchor planeAnchor)
            {
            }
        }
    }

    public class PlaneNode : SCNNode
    {
        public PlaneNode(nfloat width, nfloat length, SCNVector3 position, SKScene videoScene)
        {
            var rootNode = new SCNNode
            {
                Geometry = CreateGeometry(width, length, videoScene),
                Position = position
            };

            AddChildNode(rootNode);
        }

        private static SCNGeometry CreateGeometry(nfloat width, nfloat length, SKScene videoScene)
        {
            var material = new SCNMaterial();
            material.Diffuse.Contents = videoScene;
            material.DoubleSided = false;

            var geometry = SCNPlane.Create(width, length);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}
