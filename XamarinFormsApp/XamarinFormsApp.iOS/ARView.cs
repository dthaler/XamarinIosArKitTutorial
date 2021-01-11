using ARKit;
using CoreGraphics;
using Foundation;
using System;
using System.Drawing;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

// Compare https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/custom-renderer/view

[assembly: ExportRenderer(typeof(XamarinFormsApp.ScanView), typeof(XamarinFormsApp.iOS.ARView))]
namespace XamarinFormsApp.iOS
{
    [Register("ARView")]
    public class ARView : ViewRenderer<ScanView, ARSCNView>
    {
        // ViewRenderer -> 
        // ARSCNView -> SCNView -> UIView
        private readonly ARSCNView sceneView;

        public ARView() : base()
        {
            this.sceneView = new ARSCNView();
            SetNativeControl(sceneView);
            this.sceneView.Frame = this.Frame;

            this.sceneView.Session.Run(new ARWorldTrackingConfiguration
            {
                AutoFocusEnabled = true,
                LightEstimationEnabled = true,
                WorldAlignment = ARWorldAlignment.Gravity
            }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ScanView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                // Unsubscribe
#if false
                sceneView.Tapped -= OnCameraPreviewTapped;
#endif
            }
            if (e.NewElement != null)
            {
#if false
                if (Control == null)
                {
                }

                // Subscribe
                sceneView.Tapped += OnCameraPreviewTapped;
#endif
            }
        }
    }
}