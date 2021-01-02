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
                | ARSCNDebugOptions.ShowWorldOrigin,
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

            this.sceneView.Session.Run(new ARWorldTrackingConfiguration
            {
                AutoFocusEnabled = true,
                PlaneDetection = ARPlaneDetection.Horizontal | ARPlaneDetection.Vertical,
                LightEstimationEnabled = true,
                WorldAlignment = ARWorldAlignment.GravityAndHeading
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

    internal class PlaneNode : SCNNode
    {
        private readonly SCNPlane planeGeometry;

        public PlaneNode(ARPlaneAnchor planeAnchor, UIColor colour)
        {
            Geometry = (planeGeometry = CreateGeometry(planeAnchor, colour));
        }

        public void Update(ARPlaneAnchor planeAnchor)
        {
            planeGeometry.Width = planeAnchor.Extent.X;
            planeGeometry.Height = planeAnchor.Extent.Z;

            Position = new SCNVector3(
                planeAnchor.Center.X,
                planeAnchor.Center.Y,
                planeAnchor.Center.Z);
        }

        private static SCNPlane CreateGeometry(ARPlaneAnchor planeAnchor, UIColor colour)
        {
            var material = new SCNMaterial();
            material.Diffuse.Contents = colour;
            material.DoubleSided = true;
            material.Transparency = 0.8f;

            var geometry = SCNPlane.Create(planeAnchor.Extent.X, planeAnchor.Extent.Z);
            geometry.Materials = new[] { material };

            return geometry;
        }
    }

    public class SceneViewDelegate : ARSCNViewDelegate
    {
        private readonly IDictionary<NSUuid, PlaneNode> planeNodes = new Dictionary<NSUuid, PlaneNode>();

        public override void DidAddNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            if (anchor is ARPlaneAnchor planeAnchor)
            {
                UIColor colour;

                if (planeAnchor.Alignment == ARPlaneAnchorAlignment.Vertical)
                {
                    colour = UIColor.Red;
                }
                else
                {
                    colour = UIColor.Blue;
                }

                var planeNode = new PlaneNode(planeAnchor, colour);
                var angle = (float)(-Math.PI / 2);
                planeNode.EulerAngles = new SCNVector3(angle, 0, 0);

                node.AddChildNode(planeNode);
                this.planeNodes.Add(anchor.Identifier, planeNode);
            }
        }

        public override void DidRemoveNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            if (anchor is ARPlaneAnchor planeAnchor)
            {
                this.planeNodes[anchor.Identifier].RemoveFromParentNode();
                this.planeNodes.Remove(anchor.Identifier);
            }
        }

        public override void DidUpdateNode(ISCNSceneRenderer renderer, SCNNode node, ARAnchor anchor)
        {
            if (anchor is ARPlaneAnchor planeAnchor)
            {
                this.planeNodes[anchor.Identifier].Update(planeAnchor);
            }
        }
    }
}
