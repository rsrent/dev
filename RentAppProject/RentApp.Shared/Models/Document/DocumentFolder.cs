using System;
using System.Collections.Generic;

namespace RentApp.Shared.Models.Document
{
    public class DocumentFolder : Document
    {
        public ICollection<Document> Documents { get; set; }
    }
}
