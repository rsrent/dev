using System;
using Newtonsoft.Json.Converters;

namespace Rent.Models
{
    public class DartDateTimeConverter : IsoDateTimeConverter
    {
        public DartDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.ffffff";
            //DateTimeFormat = "yyyy-MM-ddTHH:mm:ssK";
        }
    }
}
