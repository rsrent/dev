using Rent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    public class TranslationEntryDTO
    {
        public string Text { get; set; }
        public Language? Language { get; set; }
    }
}
