using System;
using System.Collections.Generic;
using Rent.Helpers;

namespace Rent.DTOs
{
    public abstract class DocumentDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int? ParentFolderID { get; set; }
        public DocumentDTO Parent { get; set; }

        public class Folder : DocumentDTO 
        {
            public string Type = "Folder";
            public List<DocumentDTO> Documents { get; set; }
            public bool Removable { get; set; }
            public bool Editable { get; set; }
        }

        public class Item : DocumentDTO
        {
            public string Type = "Item";
            public string DocumentLocation { get; set; }
        }
    }
}
