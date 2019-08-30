using System;
namespace Rent.Models.General
{
    public class Document
    {
        public int ID { get; set; }
        public int DocumentFolderID { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }

        public virtual DocumentFolder DocumentFolder { get; set; }
    }
}
