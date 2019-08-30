using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RentApp.Shared.Models.Document
{
    public abstract class Document
    {
        public int ID { get; set; }
        public string Title { get; set; }
    }
}
