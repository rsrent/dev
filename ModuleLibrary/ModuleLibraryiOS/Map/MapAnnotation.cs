using System;
using CoreLocation;
using MapKit;

namespace ModuleLibraryiOS.Map
{
    public class MapAnnotation : MKAnnotation
    {
		CLLocationCoordinate2D coord;
		string title, subtitle;

		public override CLLocationCoordinate2D Coordinate { get { return coord; } }
		public override void SetCoordinate(CLLocationCoordinate2D value)
		{
			coord = value;
		}
		public override string Title { get { return title; } }
		public override string Subtitle { get { return subtitle; } }
		public MapAnnotation(CLLocationCoordinate2D coordinate, string title, string subtitle)
		{
			this.coord = coordinate;
			this.title = title;
			this.subtitle = subtitle;
		}
    }
}
