using System;
using System.Collections.Generic;
using Rent.Models.TimePlanning;
using Rent.Data;

namespace Rent.Repositories.TimePlanning

{

    public abstract class IHolidayCalculator
    {
         public abstract List<DateTime> GetDatesOfHolidays(List<Holiday> holidays, DateTime from, DateTime to);

         public static IHolidayCalculator CreateHolidayCalculator(string countryCode)
         {
             if(countryCode.Equals("DK"))
             {
                 return new DanishHolidayCalculator();
             }
             else
             {
                 return null;
             }
         }


    }
}