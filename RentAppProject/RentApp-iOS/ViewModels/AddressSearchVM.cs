using System;
using ModuleLibraryiOS.Map;
namespace RentApp.ViewModels
{
    public class AddressSearchVM
    {
        public Action<Address.AddressInfo> ItemSeletedAction { get; set; }
        public string Address { get; set; }
    }
}
