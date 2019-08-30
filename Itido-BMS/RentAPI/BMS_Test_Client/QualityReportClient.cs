using System;
using System.Threading.Tasks;

namespace BMS_Test_Client
{
    public class QualityReportClient
    {
        private readonly ClientController _client;

        public QualityReportClient(ClientController client)
        {
            _client = client;
        }
        public async Task Run()
        {
            await Create();
            //await Update();
            //await CreateAWorkContractForUser();
            //await UpdateWithLongerPeriod();
            //await UpdateWithShorterPeriod();
            //await AddUserToWorkContract();
            // await RemoveUserFromWorkContract();
            //await UpdateWithLongerPeriodUserHasAbsence();
        }

        private async Task Create()
        {
            var res = await _client.PostId("QualityReports/CreateForProjectItem/8749", null);
            Console.WriteLine("Result: " + res);
        }
    }
}
