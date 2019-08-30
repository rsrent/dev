using System;
namespace Rent.Models.TimePlanning
{
    public class ProjectRole
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool HasAllPermissions { get; set; }
    }
}
