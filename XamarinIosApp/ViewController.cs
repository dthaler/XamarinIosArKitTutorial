using ARKit;
using AVFoundation;
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
                MaximumNumberOfTrackedImages = 1

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

                var width = detectedImage.PhysicalSize.Width;
                var length = detectedImage.PhysicalSize.Height;
                var planeNode = new PlaneNode(width, length, new SCNVector3(0, 0, 0), UIColor.Blue);

                float angle = (float)(-Math.PI / 2);
                planeNode.EulerAngles = new SCNVector3(angle, 0, 0);

                node.AddChildNode(planeNode);
            }
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
        public PlaneNode(nfloat width, nfloat length, SCNVector3 position, UIColor colour)
        {
            var rootNode = new SCNNode
            {
                Geometry = CreateGeometry(width, length, colour),
                Position = position
            };

            AddChildNode(rootNode);
        }

        private static SCNGeometry CreateGeometry(nfloat width, nfloat length, UIColor colour)
        {
            var material = new SCNMaterial();
            material.Diffuse.Contents = colour;
            material.DoubleSided = false;

            var geometry = SCNPlane.Create(width, length);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }
}
