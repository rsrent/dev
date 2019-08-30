using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class Floor
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int? TranslationID { get; set; }
    }

    public static class FloorDto
    {

        public static dynamic Basic(this Floor f)
        {
            return new
            {
                f.ID,
                f.Description,
            };
        }
    }
}
