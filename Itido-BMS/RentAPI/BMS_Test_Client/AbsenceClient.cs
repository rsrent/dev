using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;

namespace BMS_Test_Client
{
    public class AbsenceClient
    {
        private readonly ClientController _client;
        public AbsenceClient(ClientController client)
        {
            _client = client;
        }

        public async Task Run()
        {
            //await GetOne();
            //await GetOfUser();
           // await Create();
           //await Update();
           //await Reply();
            //await Create();
            //var locations = await _client.Get("Location");
            //locations.Print();
            //await UpdateAbsenceToBeLonger();
            //await CreateAbsenceWhichOverlapsWork();
            await CreateAbsenceCheckWorkRegi();
        }

        public async Task CreateAbsenceCheckWorkRegi()
        {
              var absence = new
            {
                UserID = 177,
                AbsenceReasonID = 2 ,
                Comment = "A",
                Description = "A" ,
                CanRespondToApprovalState = false,
                CreatorName = "Tob" ,
                ApprovalState = 1,
                From = "2019-08-09",
                To = "2019-09-10",
                IsRequest = false,

            };
            var res = await _client.PostId("Absence/Create/177/2/false", absence);
            Console.WriteLine(res);
        }


        public async Task UpdateAbsenceToBeLonger()
        {
            var absence = new
            {
                ID = 53,
                UserID = 177,
                AbsenceReasonID = 2 ,
                Comment = "A",
                Description = "A" ,
                CanRespondToApprovalState = false,
                CreatorName = "Tob" ,
                ApprovalState = 1,
                From = "2019-11-10",
                To = "2019-11-30",
                IsRequest = false,
            };
            var res = await _client.PutNoContent("Absence/Update", absence);
            Console.WriteLine(res);
        }

        public async Task CreateAbsenceWhichOverlapsWork()
        {
            var absence = new
            {
                UserID = 177,
                AbsenceReasonID = 2 ,
                Comment = "A",
                Description = "A" ,
                CanRespondToApprovalState = false,
                CreatorName = "Tob" ,
                ApprovalState = 1,
                From = "2019-10-15",
                To = "2019-10-25",
                IsRequest = false,
            };
            var res = await _client.PostId("Absence/Create/177/2/false", absence);
            Console.WriteLine(res);
        }

        public async Task GetOne()
        {
            var res = await _client.Get("Absence/33");
            Console.WriteLine(res.Values.ToString());
        }

        public async Task GetOfUser()
        {
            var res = await _client.GetMany("Absence/GetAllOfUser/2");
            res.Print();
        }

        public async Task Create()
        {
            var absence = new
            {
                UserID = 177,
                AbsenceReasonID = 2 ,
                Comment = "A",
                Description = "A" ,
                CanRespondToApprovalState = false,
                CreatorName = "Tob" ,
                ApprovalState = 1,
                From = "2019-12-08",
                To = "2019-12-18",
                IsRequest = true,

            };
            var res = await _client.PostId("Absence/Create/2/2/true", absence);
            Console.WriteLine(res);
        }

        public async Task Update()
        {
              var absence = new
            {
                ID = 51,
                UserID = 177,
                AbsenceReasonID = 2 ,
                Comment = "A",
                Description = "A" ,
                CanRespondToApprovalState = false,
                CreatorName = "Tob" ,
                ApprovalState = 1,
                From = "2019-12-08",
                To = "2019-12-20",
                IsRequest = true,

            };
            await _client.PutNoContent("Absence/Update", absence);

        }

        public async Task Reply()
        {
            await _client.PutNoContent("Absence/Reply/51/true");


        }
    }
}
