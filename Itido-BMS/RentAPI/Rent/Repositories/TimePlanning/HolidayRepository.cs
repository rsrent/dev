using Rent.Data;
using System.Collections.Generic;
using Rent.Models.TimePlanning;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Rent.Repositories.TimePlanning
{

    public class HolidayRepository
    {
        private readonly RentContext _rentContext;
        public HolidayRepository(RentContext rentContext)
        {
            _rentContext = rentContext;
        }

        public IEnumerable<Holiday> GetAllHolidaysForCountryCode(int requester, string countryCode)
        {
            return _rentContext.Holiday.Where(h => h.CountryCode.Equals(countryCode));
        }

    }
}