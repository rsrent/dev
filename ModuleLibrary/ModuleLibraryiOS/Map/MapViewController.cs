using Foundation;
using System;
using UIKit;
using CoreLocation;
using MapKit;
using ObjCRuntime;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryiOS.ViewControllerInstanciater;

namespace ModuleLibraryiOS.Map
{
    public partial class MapViewController : UIViewController
    {
        public MapViewController (IntPtr handle) : base (handle) {  }

        Action<Address> DoneAction;
        Address displayAddress;
        Address selectedAddress;
        MapType mapType;

        CLLocationManager locationManager;
        CLGeocoder geoCoder;

        public enum MapType {
            Display, Search, Cell
        }

        public static void Start(UIView container, UIViewController viewController, MapType mapType, Action<Address> doneAction = null, Address displayAddress = null)
		{
			var chatStoryboard = UIStoryboard.FromName("Map", null);
			var newView = chatStoryboard.InstantiateViewController("MapViewController") as MapViewController;
            newView.ParseInfo(mapType, doneAction, displayAddress);
            //TODO
            Instanciate.Start(container, viewController, newView);
		}

        private void ParseInfo(MapType type, Action<Address> doneAction = null, Address address = null) {
            mapType = type;
            DoneAction = doneAction;
            displayAddress = address;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            geoCoder = new CLGeocoder();
			locationManager = new CLLocationManager();
			locationManager.RequestWhenInUseAuthorization();
            Map.ShowsUserLocation = true;
            if(displayAddress == null) {
                GoToPosition(locationManager.Location.Coordinate);
                SetSelectedAddress(locationManager.Location.Coordinate);
            }
            else {
                SetSelectedAddress(displayAddress);
                GoToPosition(displayAddress);
            }

            if(mapType == MapType.Display || mapType == MapType.Cell) 
            {
                SelectButton.Hidden = true;
                SearchBar.Hidden = true;
                if(displayAddress != null) PlacePin(displayAddress);
            } 
            else if(mapType == MapType.Search)
            {
				Map.AddGestureRecognizer(new UITapGestureRecognizer(MapTapped));

				TableContainer.Hidden = true;
				var source = new Table.TableSource<Address.AddressInfo>((table, row, obj) => {
                    return Table.TableFunctions.InstanciateCell<Table.TextCell>(table, Table.TextCell.Key, (cell) => { cell.UpdateCell(obj.forslagstekst); });
				}, out var opdateList, (row, obj) => {
					AddressSelected(obj);
				});
				opdateList.Invoke(new List<Address.AddressInfo>());
                var reloadTable = Table.TableViewController.Start(this, TableContainer).Initialize(source, (table, view) => {
					table.RegisterNibForCellReuse(Table.TextCell.Nib, Table.TextCell.Key);
                }).GetRefreshFunction();
				EventHandler<UISearchBarTextChangedEventArgs> textChanged = async (sender, e) => {
					if (SearchBar.Text.Length > 0)
					{
						opdateList.Invoke(await Address.SearchAddress(SearchBar.Text));
						TableContainer.Hidden = false;
						reloadTable.Invoke(true);
					}
					else TableContainer.Hidden = true;
				};
				SearchBar.TextChanged += textChanged;

				SearchBar.SearchButtonClicked += (sender, e) => {
					SearchBar.ResignFirstResponder();
				};
				SelectButton.Hidden = DoneAction == null;
				SelectButton.TouchUpInside += (sender, e) => {
					DoneAction.Invoke(selectedAddress);
				};
            }

            if(mapType == MapType.Cell) {
                MapHeightConstraint.Constant = 300;
                MapHeightConstraint.Active = true;
            }
            else MapHeightConstraint.Active = false;
        }

		private void SetSelectedAddress(Address address)
		{
            try
			{
    			selectedAddress = address;
    			SearchBar.Text = selectedAddress.address;
    			RemovePins();
    			PlacePin(selectedAddress);
            }
			catch (Exception exc) { }
		}
        private async void SetSelectedAddress(CLLocationCoordinate2D coords) {
            try{
				var addressArray = await GetAddressFromPosition(coords);
				SetSelectedAddress(new Address { position = new Address.Position(coords), address = addressArray[0] });
            } catch(Exception exc) {}
        }
        private async void SetSelectedAddress(Address.AddressInfo addressInfo)
		{
			try
			{
				var pos = await GetPositionFromAddress(addressInfo);
				var addressArray = await GetAddressFromPosition(pos);
				SetSelectedAddress(new Address { position = new Address.Position(pos), address = addressArray[0] });
			}
			catch (Exception exc) { }
		}

        private async Task AddressSelected(Address.AddressInfo address) {
			SearchBar.Text = address.forslagstekst;
			TableContainer.Hidden = true;
            var coords = await GetPositionFromAddress(address);
			GoToPosition(coords);
            SetSelectedAddress(coords);
        }

		protected async void MapTapped(UIGestureRecognizer sender) {
            if (mapType == MapType.Cell) Start(null, this, MapType.Search);
            SearchBar.ResignFirstResponder();
			CLLocationCoordinate2D coords = Map.ConvertPoint(sender.LocationInView(Map), Map);
            var addressArray = await GetAddressFromPosition(coords);
            SetSelectedAddress(coords);
        }

        private async Task<CLLocationCoordinate2D> GetPositionFromAddress(Address.AddressInfo address) {
            var pos = await Address.GetAddressPosition(address);
            return new CLLocationCoordinate2D(pos.Latitude, pos.Longitude);
        }

        private async Task<string[]> GetAddressFromPosition(CLLocationCoordinate2D position) {
			var location = new CLLocation(position.Latitude, position.Longitude);

			var placemarks = await geoCoder.ReverseGeocodeLocationAsync(location);
			string[] address = new string[4];
			foreach (var placemark in placemarks)
			{
				var splitString = placemark.ToString().Split(',', '@');
				int counter = 0;
				foreach (var split in splitString)
				{
					if (counter > 0 && counter < 4)
					{
						address[0] += split.TrimEnd();
						address[counter] = split.Trim();
					}
					counter++;
				}
			}
			return address;
        }

		private void GoToPosition(CLLocationCoordinate2D coords) {
			var span = new MKCoordinateSpan(MilesToLatitudeDegrees(2), MilesToLongitudeDegrees(2, coords.Latitude));
			Map.Region = new MKCoordinateRegion(coords, span);
		}
		private void GoToPosition(CLLocation coords) { GoToPosition(new CLLocationCoordinate2D(coords.Coordinate.Latitude, coords.Coordinate.Longitude));}
        private void GoToPosition(Address address) { GoToPosition(new CLLocationCoordinate2D(address.position.Latitude, address.position.Longitude));}

        private void RemovePins() { Map.RemoveAnnotations(Map.Annotations); }

		private void PlacePin(Address address)
		{
			var annotation = new MapAnnotation(address.position.ToCLLocationCoordinate2D(), address.address, address.address);
			Map.ViewForAnnotation(annotation);
			Map.AddAnnotation(annotation);
            Map.ShowAnnotations(new IMKAnnotation[]{annotation }, true);
		}

		public static double MilesToLatitudeDegrees(double miles)
		{
			double earthRadius = 3960.0; //in miles
			double radiansToDegrees = 180.0 / Math.PI;
			return (miles / earthRadius) * radiansToDegrees;
		}

		public static double MilesToLongitudeDegrees(double miles, double atLatitude)
		{
			double earthRadius = 3960.0; //in miles
			double degreesToRadians = Math.PI / 180.0;
			double radiansToDegrees = 180.0 / Math.PI;
			//derive the earth's radius at theat point in latitude
			double radiusAtLatitude = earthRadius * Math.Cos(atLatitude * degreesToRadians);
			return (miles / radiusAtLatitude) * radiansToDegrees;
		}
    }
}