using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;
using System.Dynamic;

namespace BMS_Test_Client
{
    public class WorkContractClient
    {
        //Test 1 
        //Create a workcontract for a user
        //Check if all works in period has been created

        //Test 2 
        //
        private readonly ClientController _client;

        public WorkContractClient(ClientController client)
        {
            _client = client;
        }
        public async Task Run()
        {
            //await Create();
            //await Update();
            //await CreateAWorkContractForUser();
            //await UpdateWithLongerPeriod();
            //await UpdateWithShorterPeriod();
            //await AddUserToWorkContract();
           // await RemoveUserFromWorkContract();
           //await UpdateWithLongerPeriodUserHasAbsence();
        }


        //Test 1
        public async Task CreateAWorkContractForUser()
        {
            var holidays = new List<dynamic> {
                new {
                    HolidayName = "ChristAscension",
                    HolidayCountryCode = "DK",
                },
                new {
                    HolidayName = "ChristmasDay",
                    HolidayCountryCode = "DK"
                },
                 new {
                    HolidayName = "NewyearsEve",
                    HolidayCountryCode = "DK"
                }
           };

            var days = new List<dynamic> {
                new {
                    DayOfWeek = 3,
                    StartTimeMins = 540,
                    EndTimeMins = 1020,
                    BreakMins = 30,
                }
            };

            var workContract = new 
            {
                LocationID = 2,
                Note = "1",
                FromDate = "2019-10-16" ,
                ToDate = "2019-10-25",
                IsVisible = true,
                WorkHolidays = holidays,
                WorkDays = days,
            };
            var res = await _client.PostId("WorkContract/Create/2", workContract);
            Console.WriteLine(res);
        }

        public async Task UpdateWithLongerPeriod()
        {
             var holidays = new List<dynamic> {
                new {
                    HolidayName = "ChristAscension",
                    HolidayCountryCode = "DK",
                },
           };
            var days = new List<dynamic> {
                new {
                    DayOfWeek = 2,
                    StartTimeMins = 540,
                    EndTimeMins = 1020,
                    BreakMins = 30,
                }
            };
             var workContract = new 
            {
                ID = 21,
                LocationID = 2,
                Note = "1",
                FromDate = "2019-10-16" ,
                ToDate = "2019-11-10",
                IsVisible = true,
                WorkHolidays = holidays,
                WorkDays = days,
            };
            var res = await _client.PutNoContent("WorkContract/Update", workContract);
            Console.WriteLine(res);
        }

        public async Task UpdateWithShorterPeriod()
        {
            var holidays = new List<dynamic> {
                new {
                    HolidayName = "ChristAscension",
                    HolidayCountryCode = "DK",
                },
           };
            var days = new List<dynamic> {
                new {
                    DayOfWeek = 2,
                    StartTimeMins = 540,
                    EndTimeMins = 1020,
                    BreakMins = 30,
                }
            };
            var workContract = new 
            {
                ID = 21,
                LocationID = 2,
                Note = "1",
                FromDate = "2019-10-16" ,
                ToDate = "2019-10-29",
                IsVisible = true,
                WorkHolidays = holidays,
                WorkDays = days,
            };
            var res = await _client.PutNoContent("WorkContract/Update", workContract);
            Console.WriteLine(res);
        }

        public async Task AddUserToWorkContract()
        {
            var res = await _client.PutNoContent("WorkContract/AddContract/21/3");
            Console.WriteLine(res);

        }

        public async Task RemoveUserFromWorkContract()
        {
            var res = await _client.PutNoContent("WorkContract/RemoveContract/21");
            Console.WriteLine(res);
        }

        
        public async Task UpdateWithLongerPeriodUserHasAbsence()
        {
              var holidays = new List<dynamic> {
                new {
                    HolidayName = "ChristAscension",
                    HolidayCountryCode = "DK",
                },
           };
            var days = new List<dynamic> {
                new {
                    DayOfWeek = 2,
                    StartTimeMins = 540,
                    EndTimeMins = 1020,
                    BreakMins = 30,
                }
            };
            var workContract = new 
            {
                ID = 21,
                LocationID = 2,
                Note = "1",
                FromDate = "2019-10-16" ,
                ToDate = "2019-11-29",
                ContractID = 3,
                IsVisible = true,
                WorkHolidays = holidays,
                WorkDays = days,
            };
            var res = await _client.PutNoContent("WorkContract/Update", workContract);
            Console.WriteLine(res);

        }

        public async Task Create()
        {
            var holidays = new List<dynamic> {
                new {
                    HolidayName = "ChristAscension",
                    HolidayCountryCode = "DK",
                },
           };
            var days = new List<dynamic> {
                new {
                    DayOfWeek = 0,
                    StartTimeMins = 540,
                    EndTimeMins = 1020,
                    BreakMins = 30,
                    
                }
            };
            var workContract = new 
            {
                LocationID = 1,
                Note = "1",
                FromDate = "2019-08-16" ,
                ToDate = "2019-08-25",
                IsVisible = true,
                WorkHolidays = holidays,
                WorkDays = days,
            };
            var res = await _client.PostId("WorkContract/Create/1", workContract);
            Console.WriteLine(res);
            
        }

        public async Task Update() 
        {
             var holidays = new List<dynamic> {
                new {
                    HolidayName = "ChristAscension",
                    HolidayCountryCode = "DK",
                },
           };
            var days = new List<dynamic> {
                new {
                    DayOfWeek = 0,
                    StartTimeMins = 540,
                    EndTimeMins = 1020,
                    BreakMins = 30,
                    
                }
            };
            var workContract = new 
            {
                ID = 17,
                LocationID = 1,
                Note = "1",
                FromDate = "2019-08-20" ,
                ToDate = "2019-09-10",
                IsVisible = true,
                WorkHolidays = holidays,
                WorkDays = days,
                
            };
            var res = await _client.PutNoContent("WorkContract/Update", workContract);


        }


    }
}