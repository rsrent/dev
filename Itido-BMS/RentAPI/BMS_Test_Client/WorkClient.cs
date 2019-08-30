using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BMS_Test_Client.Extensions;

namespace BMS_Test_Client
{
    public class WorkClient
    {
        private readonly ClientController _client;

        public WorkClient(ClientController client)
        {
            _client = client;
        }
        public async Task Run()
        {
            await GetWorkTest();

            //var id = await CreateForLoaction();
            //await AddContract(id);
            //await GetWorkOfSignedInUser();
            //await GetWorkOfUser();
            //await RegisterWorkNormal();
            await GetWork();
        }

        public async Task GetWork()
        {
            var res = await _client.GetMany("Work/GetWork");
        }

        public async Task<int> CreateForLoaction()
        {
            var work = new
            {
                LocationID = 1,
                Note = "A",
                Date = "2019-08-20",
                Modifications = 0,
                StartTimeMins = 120,
                EndTimeMins = 320,
                BreakMins = 0,
                IsVisible = true,
                Enabled = true,

            };


            var res = await _client.PostId("Work/Create/1", work);
            return res;
        }

        public async Task AddContract(int id)
        {
            var res = await _client.PutNoContent("Work/AddContract/" + id + "/3");
        }

        public async Task AddReplacer()
        {
            var res = await _client.PutNoContent("Work/AddReplacer/42/2");
        }


        public async Task GetWorkTest()
        {
            var res = await _client.GetMany("Work/GetOfProjectItem/19069/2019-08-25 19:17:04.240570/2019-09-26 19:17:04.240601");
            res.Print();
        }

        public async Task GetWorkOfSignedInUser()
        {
            var res = await _client.GetMany("Work/GetWorkOfSignedInUser");
            res.Print();
        }

        public async Task GetWorkOfUser()
        {
            var res = await _client.GetMany("Work/GetWorkOfUser/177/2018-01-01/2020-01-01");
            res.Print();
        }

        public async Task RegisterWorkNormal()
        {
            var res = await _client.PostId("Work/RegisterWork/46");
            Console.WriteLine(res);
        }


        public async Task GetTest()
        {
            var path = "Work/GetOfProjectItem/13892/2019-08-18 09:50:03.035704/2019-09-19 09:50:03.036113";
            var res = await _client.GetMany(path);
            res.Print();
        }
    }
}