using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class TranslationEntry
    {
        public int ID { get; set; }
        public int TranslationID { get; set; }
        public string Text { get; set; }
        public Language? Language { get; set; }
    }

    public enum Language
    {
        DA, EN, DE
    }
}
