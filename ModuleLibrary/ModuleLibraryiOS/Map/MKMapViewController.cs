using Foundation;
using MapKit;
using System;
using UIKit;

namespace ModuleLibraryiOS.Map
{
    public partial class MKMapViewController : MKMapView
    {
        public MKMapViewController (IntPtr handle) : base (handle)
        {
        }

		public override MKAnnotationView ViewForAnnotation(IMKAnnotation annotation)
		{
			var pinView = DequeueReusableAnnotation("colorpin") as MKPinAnnotationView;
			if (pinView == null)
			{
				pinView = new MKPinAnnotationView(annotation, "colorpin")
				{
					CanShowCallout = true,
				};
			}
			//pinView.Image = new UIImage("faceTest.jpg");



			
            else
            {
                pinView.Annotation = annotation;
            }
            var color = MKPinAnnotationColor.Green;
			/*
            var place = viewModel.Places.First(p => p.Latitude == annotation.Coordinate.Latitude && p.Longitude == annotation.Coordinate.Longitude && p.Name == annotation.GetTitle());
            if (place.Stars < 3.5)
                color = MKPinAnnotationColor.Red;
            else if (place.Stars < 4.3)
                color = MKPinAnnotationColor.Purple;*/

			pinView.PinColor = MKPinAnnotationColor.Purple;


            var btn = new UIButton(new CoreGraphics.CGRect(200, 200, 50, 50));
            btn.SetTitle("TEST", UIControlState.Normal);

            var test = new MKAnnotationView();
            test.Add(btn);

			return test;
		}
    }
}