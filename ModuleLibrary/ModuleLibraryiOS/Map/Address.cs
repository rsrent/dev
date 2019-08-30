using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreLocation;
using ModuleLibraryShared.Services;

namespace ModuleLibraryiOS.Map
{
    public class Address
    {

        public Position position { get; set; }
        public string address { get; set; }

        public class AddressInfo{
			public string tekst { get; set; }
			public string forslagstekst { get; set; }
			public Data data { get; set; }
			public Data adgangspunkt { get; set; }
        }

        public class Data {
            public string id { get; set; }
            public string vejnavn { get; set; }
            public string husnr { get; set; }
            public string postnr { get; set; }
            public string postnrnavn { get; set; }
            public string href { get; set; }
            public Position position { get; set; }
        }

        public class Position {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public Position(double lat, double lon) {
                Latitude = lat;
                Longitude = lon;
            }

            public Position(CLLocationCoordinate2D coordinates) {
                Latitude = coordinates.Latitude;
                Longitude = coordinates.Longitude;
            }

            public CLLocationCoordinate2D ToCLLocationCoordinate2D() {
                return new CLLocationCoordinate2D(Latitude, Longitude);
            }
        }

        public static async Task<List<Address.AddressInfo>> SearchAddress(string searchText) 
        {
            return await new HttpCall.CallManager<List<Address.AddressInfo>>().Call(HttpCall.CallType.Get, "https://dawa.aws.dk/autocomplete?q=" + searchText);
        }

        public static async Task<Position> GetAddressPosition(Address.AddressInfo address)
		{
			try
			{
                System.Net.Http.HttpResponseMessage response = await HttpCall.GetHttpClient().GetAsync(new Uri(address.data.href));

				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
                    var substrings = System.Text.RegularExpressions.Regex.Split(result, "koordinater");

                    var lat = substrings[1].Substring(29, 10);
                    var lon = substrings[1].Substring(11, 10);

                    System.Diagnostics.Debug.WriteLine(lat + " - " + lon);

                    return new Position(double.Parse(lat, System.Globalization.CultureInfo.InvariantCulture),
                                        double.Parse(lon, System.Globalization.CultureInfo.InvariantCulture));
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
            return null;
		}
    }
}
